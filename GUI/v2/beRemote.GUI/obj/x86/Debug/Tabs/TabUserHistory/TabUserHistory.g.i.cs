﻿#pragma checksum "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1A404F6269612E14FE8269E91D678D9F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
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
using beRemote.GUI_v2.ViewModel;


namespace beRemote.GUI_v2.Tabs.TabUserHistory {
    
    
    /// <summary>
    /// TabUserHistory
    /// </summary>
    public partial class TabUserHistory : beRemote.GUI_v2.ViewModel.ViewModelTabBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgHistory;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblDisplay;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLast;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNext;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpDate;
        
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
            System.Uri resourceLocater = new System.Uri("/beRemote.GUI_v2;component/tabs/tabuserhistory/tabuserhistory.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 11 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgHistory = ((System.Windows.Controls.DataGrid)(target));
            
            #line 12 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
            this.dgHistory.BeginningEdit += new System.EventHandler<System.Windows.Controls.DataGridBeginningEditEventArgs>(this.dgHistory_BeginningEdit);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
            this.dgHistory.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.dgHistory_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.lblDisplay = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.btnLast = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
            this.btnLast.Click += new System.Windows.RoutedEventHandler(this.btnLast_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnNext = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
            this.btnNext.Click += new System.Windows.RoutedEventHandler(this.btnNext_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.dpDate = ((System.Windows.Controls.DatePicker)(target));
            
            #line 31 "..\..\..\..\..\Tabs\TabUserHistory\TabUserHistory.xaml"
            this.dpDate.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.dpDate_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

