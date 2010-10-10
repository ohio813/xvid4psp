﻿//<SnippetCodeBehind>
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Automation;
using System.Globalization;
using System.Diagnostics;

namespace MyUserControl
{
    public partial class NumericUpDown : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// Initializes a new instance of the NumericUpDownControl.
        /// </summary>
        public NumericUpDown()
        {
            InitializeComponent();

            //возможно всё это не нужно
            //InitializeCommands();
            //// Listen to MouseLeftButtonDown event to determine if slide should move focus to itself
            //EventManager.RegisterClassHandler(typeof(NumericUpDown),
            //    Mouse.MouseDownEvent, new MouseButtonEventHandler(NumericUpDown.OnMouseLeftButtonDown), true);
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
            //updateValueString();
        }

        private bool _IsAction = false;
        public bool IsAction
        {
            get { return _IsAction; }
            set { _IsAction = value; }
        }

        #region Properties
        #region Value
        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set
            {
                if (value > Maximum ||
                    value < Minimum)
                    IsAction = true;
                SetValue(ValueProperty, value); 
            }
        }

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultValue,
                    new PropertyChangedCallback(OnValueChanged),
                    new CoerceValueCallback(CoerceValue)
                )
            );

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)obj;

            decimal oldValue = (decimal)args.OldValue;
            decimal newValue = (decimal)args.NewValue;

            #region Fire Automation events
            NumericUpDownAutomationPeer peer = UIElementAutomationPeer.FromElement(control) as NumericUpDownAutomationPeer;
            if (peer != null)
            {
                peer.RaiseValueChangedEvent(oldValue, newValue);
            }
            #endregion

            RoutedPropertyChangedEventArgs<decimal> e = new RoutedPropertyChangedEventArgs<decimal>(
                oldValue, newValue, ValueChangedEvent);

            control.OnValueChanged(e);

            control.updateValueString();
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
        {
            
            RaiseEvent(args);
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            decimal newValue = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;

            newValue = Math.Max(control.Minimum, Math.Min(control.Maximum, newValue));
            newValue = Decimal.Round(newValue, control.DecimalPlaces);

            return newValue;
        }
        #endregion
        #region Minimum
        public decimal Minimum
        {
            get { return (decimal)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultMinValue,
                    new PropertyChangedCallback(OnMinimumChanged), new CoerceValueCallback(CoerceMinimum)
                )
            );

        private static void OnMinimumChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            element.CoerceValue(MaximumProperty);
            element.CoerceValue(ValueProperty);
        }
        private static object CoerceMinimum(DependencyObject element, object value)
        {
            decimal minimum = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;
            return Decimal.Round(minimum, control.DecimalPlaces);
        }
        #endregion
        #region Maximum
        public decimal Maximum
        {
            get { return (decimal)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultMaxValue,
                    new PropertyChangedCallback(OnMaximumChanged),
                    new CoerceValueCallback(CoerceMaximum)
                )
            );

        private static void OnMaximumChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            element.CoerceValue(ValueProperty);
        }

        private static object CoerceMaximum(DependencyObject element, object value)
        {
            NumericUpDown control = (NumericUpDown)element;
            decimal newMaximum = (decimal)value;
            return Decimal.Round(Math.Max(newMaximum, control.Minimum), control.DecimalPlaces);
        }
        #endregion
        #region Change
        public decimal Change
        {
            get { return (decimal)GetValue(ChangeProperty); }
            set { SetValue(ChangeProperty, value); }
        }

        public static readonly DependencyProperty ChangeProperty =
            DependencyProperty.Register(
                "Change", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultChange, new PropertyChangedCallback(OnChangeChanged), new CoerceValueCallback(CoerceChange)),
            new ValidateValueCallback(ValidateChange)
            );

        private static bool ValidateChange(object value)
        {
            decimal change = (decimal)value;
            return change > 0;
        }

        private static void OnChangeChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {

        }

        private static object CoerceChange(DependencyObject element, object value)
        {
            decimal newChange = (decimal)value;
            NumericUpDown control = (NumericUpDown)element;

            decimal coercedNewChange = Decimal.Round(newChange, control.DecimalPlaces);

            //If Change is .1 and DecimalPlaces is changed from 1 to 0, we want Change to go to 1, not 0.
            //Put another way, Change should always be rounded to DecimalPlaces, but never smaller than the 
            //previous Change
            if (coercedNewChange < newChange)
            {
                coercedNewChange = smallestForDecimalPlaces(control.DecimalPlaces);
            }

            return coercedNewChange;
        }

        private static decimal smallestForDecimalPlaces(int decimalPlaces)
        {
            if (decimalPlaces < 0)
            {
                throw new ArgumentException("decimalPlaces");
            }

            decimal d = 1;

            for (int i = 0; i < decimalPlaces; i++)
            {
                d /= 10;
            }

            return d;
        }

        #endregion
        #region DecimalPlaces
        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); }
        }

        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(
                "DecimalPlaces", typeof(int), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(DefaultDecimalPlaces,
                    new PropertyChangedCallback(OnDecimalPlacesChanged)
                ), new ValidateValueCallback(ValidateDecimalPlaces)
            );

        private static void OnDecimalPlacesChanged(DependencyObject element, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)element;
            control.CoerceValue(ChangeProperty);
            control.CoerceValue(MinimumProperty);
            control.CoerceValue(MaximumProperty);
            control.CoerceValue(ValueProperty);
            control.updateValueString();
        }

        private static bool ValidateDecimalPlaces(object value)
        {
            int decimalPlaces = (int)value;
            return decimalPlaces >= 0;
        }

        #endregion

        #region ValueString
        public string ValueString
        {
            get
            {
                return (string)GetValue(ValueStringProperty);
            }
        }

        private static readonly DependencyPropertyKey ValueStringPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("ValueString", typeof(string), typeof(NumericUpDown), new PropertyMetadata());

        public static readonly DependencyProperty ValueStringProperty = ValueStringPropertyKey.DependencyProperty;

        private void updateValueString()
        {
            _numberFormatInfo.NumberDecimalDigits = this.DecimalPlaces;
            string newValueString = this.Value.ToString("f", _numberFormatInfo);
            this.SetValue(ValueStringPropertyKey, newValueString);
        }
        private NumberFormatInfo _numberFormatInfo = new NumberFormatInfo();
        #endregion

        #endregion

        #region Events
        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(NumericUpDown));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
        #endregion

        #region Commands

        public static RoutedCommand IncreaseCommand
        {
            get
            {
                return _increaseCommand;
            }
        }
        public static RoutedCommand DecreaseCommand
        {
            get
            {
                return _decreaseCommand;
            }
        }

        private static void InitializeCommands()
        {
            _increaseCommand = new RoutedCommand("IncreaseCommand", typeof(NumericUpDown));
            CommandManager.RegisterClassCommandBinding(typeof(NumericUpDown), new CommandBinding(_increaseCommand, OnIncreaseCommand));
            CommandManager.RegisterClassInputBinding(typeof(NumericUpDown), new InputBinding(_increaseCommand, new KeyGesture(Key.Up)));

            _decreaseCommand = new RoutedCommand("DecreaseCommand", typeof(NumericUpDown));
            CommandManager.RegisterClassCommandBinding(typeof(NumericUpDown), new CommandBinding(_decreaseCommand, OnDecreaseCommand));
            CommandManager.RegisterClassInputBinding(typeof(NumericUpDown), new InputBinding(_decreaseCommand, new KeyGesture(Key.Down)));
        }

        private static void OnIncreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;
            if (control != null)
            {
                control.OnIncrease();
            }
        }
        private static void OnDecreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;
            if (control != null)
            {
                control.OnDecrease();
            }
        }

        protected virtual void OnIncrease()
        {
            this.Value += Change;
        }
        protected virtual void OnDecrease()
        {
            this.Value -= Change;
        }

        private static RoutedCommand _increaseCommand;
        private static RoutedCommand _decreaseCommand;
        #endregion

        #region Automation
        //<SnippetOnCreateAutomationPeer>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new NumericUpDownAutomationPeer(this);
        }
        //</SnippetOnCreateAutomationPeer>
        #endregion

        /// <summary>
        /// This is a class handler for MouseLeftButtonDown event.
        /// The purpose of this handle is to move input focus to NumericUpDown when user pressed
        /// mouse left button on any part of slider that is not focusable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NumericUpDown control = (NumericUpDown)sender;

            // When someone click on a part in the NumericUpDown and it's not focusable
            // NumericUpDown needs to take the focus in order to process keyboard correctly
            if (!control.IsKeyboardFocusWithin)
            {
                e.Handled = control.Focus() || e.Handled;
            }
        }

        private const decimal DefaultMinValue = 0,
            DefaultValue = DefaultMinValue,
            DefaultMaxValue = 100,
            DefaultChange = 1;
        private const int DefaultDecimalPlaces = 0;

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            IsAction = true;
            Value+= Change;
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            IsAction = true;
            Value-= Change;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox text = (TextBox)sender;
            string value = text.Text;

            if (text.IsFocused)
            {
                IsAction = true;
            }

            try
            {
                if (Value.ToString(_numberFormatInfo) != value)
                {
                    int intvalue;

                    //Убираем всё лишнее
                    if (this.DecimalPlaces == 0)
                        value = value.Replace(".", "").Replace(",", "");
                    else
                        value = value.TrimEnd(new char[] { '.', ',' });

                    if (Int32.TryParse(value, NumberStyles.Integer, null, out intvalue))
                    {
                        decimal dvalue = Convert.ToDecimal(value);
                        if (dvalue > Maximum)
                        {
                            Value = Maximum;
                            text.Text = Maximum.ToString(_numberFormatInfo);
                        }
                        else if (dvalue < Minimum)
                        {
                            Value = Minimum;
                            text.Text = Minimum.ToString(_numberFormatInfo);
                        }
                        else
                        {
                            Value = dvalue;
                            if (this.DecimalPlaces == 0)
                                text.Text = Value.ToString(_numberFormatInfo);
                        }
                    }
                    else
                    {
                        double double_value;
                        string sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                        value = value.Replace(".", sep).Replace(",", sep);

                        if (value.Contains(sep))
                        {
                            //Отрезаем лишнюю десятичную часть; Decimal.Round() - округляет последний символ, не годится
                            value = value.Substring(0, Math.Min(value.Length, value.IndexOf(sep) + this.DecimalPlaces + 1));
                        }

                        if (Double.TryParse(value, out double_value))
                        {
                            decimal dvalue = Convert.ToDecimal(double_value);
                            if (dvalue > Maximum) Value = Maximum;
                            else if (dvalue < Minimum) Value = Minimum;
                            else Value = dvalue;
                            text.Text = Value.ToString(_numberFormatInfo);
                        }
                        else
                        {
                            text.Text = Value.ToString(_numberFormatInfo);
                        }
                    }
                    text.CaretIndex = text.ToString().Length;
                }
            }
            catch
            {
                text.Text = Value.ToString(_numberFormatInfo);
                text.CaretIndex = text.ToString().Length;
            }

            IsAction = false;
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                IsAction = true;
                Value += Change;
                TextBox text = (TextBox)sender;
                text.CaretIndex = text.Text.Length;
            }
            if (e.Key == Key.Down)
            {
                IsAction = true;
                Value -= Change;
                TextBox text = (TextBox)sender;
                text.CaretIndex = text.Text.Length;
            }
        }

        private void TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            IsAction = true;
            Value += (e.Delta / 120) * Change;
            TextBox text = (TextBox)sender;
            text.CaretIndex = Value.ToString().Length;
        }

    }

    public class NumericUpDownAutomationPeer : FrameworkElementAutomationPeer, IRangeValueProvider
    {
        public NumericUpDownAutomationPeer(NumericUpDown control)
            : base(control)
        {
        }

        protected override string GetClassNameCore()
        {
            return "NumericUpDown";
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Spinner;
        }

        //<SnippetGetPattern>
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.RangeValue)
            {
                return this;
            }
            return base.GetPattern(patternInterface);
        }
        //</SnippetGetPattern>

        internal void RaiseValueChangedEvent(decimal oldValue, decimal newValue)
        {
            base.RaisePropertyChangedEvent(RangeValuePatternIdentifiers.ValueProperty,
                (double)oldValue, (double)newValue);
        }

        #region IRangeValueProvider Members

        bool IRangeValueProvider.IsReadOnly
        {
            get
            {
                return !IsEnabled();
            }
        }

        double IRangeValueProvider.LargeChange
        {
            get { return (double)MyOwner.Change; }
        }

        double IRangeValueProvider.Maximum
        {
            get { return (double)MyOwner.Maximum; }
        }

        double IRangeValueProvider.Minimum
        {
            get { return (double)MyOwner.Minimum; }
        }

        void IRangeValueProvider.SetValue(double value)
        {
            if (!IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            decimal val = (decimal)value;
            if (val < MyOwner.Minimum || val > MyOwner.Maximum)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            MyOwner.Value = val;
        }

        double IRangeValueProvider.SmallChange
        {
            get { return (double)MyOwner.Change; }
        }

        double IRangeValueProvider.Value
        {
            get { return (double)MyOwner.Value; }
        }

        #endregion

        private NumericUpDown MyOwner
        {
            get
            {
                return (NumericUpDown)base.Owner;
            }
        }
        //<SnippetClose>
    }
    //</SnippetClose>
}