﻿#pragma checksum "..\..\..\..\..\StatusBar\Stopwatch\SbStopwatch.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "76985DD5C85B7ACE3F8014BCA7D12CEE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
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
using System.Windows.Shell;
using beRemote.Core.Definitions.Classes;


namespace beRemote.GUI.StatusBar.Stopwatch {
    
    
    /// <summary>
    /// SbStopwatch
    /// </summary>
    public partial class SbStopwatch : beRemote.Core.Definitions.Classes.StatusBarBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 59 "..\..\..\..\..\StatusBar\Stopwatch\SbStopwatch.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbStopwatch;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/beRemote.GUI;component/statusbar/stopwatch/sbstopwatch.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\StatusBar\Stopwatch\SbStopwatch.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cmbStopwatch = ((System.Windows.Controls.ComboBox)(target));
            
            #line 58 "..\..\..\..\..\StatusBar\Stopwatch\SbStopwatch.xaml"
            this.cmbStopwatch.KeyUp += new System.Windows.Input.KeyEventHandler(this.ComboBox_KeyUp);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 63 "..\..\..\..\..\StatusBar\Stopwatch\SbStopwatch.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemClear_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 64 "..\..\..\..\..\StatusBar\Stopwatch\SbStopwatch.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MenuItemCopy_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

