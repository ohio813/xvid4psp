﻿#pragma checksum "..\..\..\windows\Filtering.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2C54382DC865DE2DB1F22CB25F3BD91F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace XviD4PSP {
    
    
    /// <summary>
    /// Filtering
    /// </summary>
    public partial class Filtering : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\..\windows\Filtering.xaml"
        internal XviD4PSP.Filtering Window;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\windows\Filtering.xaml"
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\windows\Filtering.xaml"
        internal System.Windows.Controls.Grid grid_main;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\windows\Filtering.xaml"
        internal System.Windows.Controls.TextBox script_box;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\windows\Filtering.xaml"
        internal System.Windows.Controls.Grid grid_buttons;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\windows\Filtering.xaml"
        internal System.Windows.Controls.Button button_cancel;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\windows\Filtering.xaml"
        internal System.Windows.Controls.Button button_ok;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\windows\Filtering.xaml"
        internal System.Windows.Controls.Button button_refresh;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\windows\Filtering.xaml"
        internal System.Windows.Controls.Button button_fullscreen;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\windows\Filtering.xaml"
        internal System.Windows.Controls.Button button_Avsp;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/XviD4PSP;component/windows/filtering.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\windows\Filtering.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Window = ((XviD4PSP.Filtering)(target));
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.grid_main = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.script_box = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.grid_buttons = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.button_cancel = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\windows\Filtering.xaml"
            this.button_cancel.Click += new System.Windows.RoutedEventHandler(this.button_cancel_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.button_ok = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\..\windows\Filtering.xaml"
            this.button_ok.Click += new System.Windows.RoutedEventHandler(this.button_ok_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.button_refresh = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\..\windows\Filtering.xaml"
            this.button_refresh.Click += new System.Windows.RoutedEventHandler(this.button_refresh_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.button_fullscreen = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\windows\Filtering.xaml"
            this.button_fullscreen.Click += new System.Windows.RoutedEventHandler(this.button_fullscreen_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.button_Avsp = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\..\windows\Filtering.xaml"
            this.button_Avsp.Click += new System.Windows.RoutedEventHandler(this.button_avsp_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}