﻿#pragma checksum "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9EBD2CFE7E2BE3E0016D34EBD2D07D36"
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


namespace beRemote.GUI_v2.Tabs.TabManageFavorites {
    
    
    /// <summary>
    /// TabManageFavorites
    /// </summary>
    public partial class TabManageFavorites : beRemote.GUI_v2.ViewModel.ViewModelTabBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgQuickies;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUp;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDown;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemove;
        
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
            System.Uri resourceLocater = new System.Uri("/beRemote.GUI_v2;component/tabs/tabmanagefavorites/tabmanagefavorites.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
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
            
            #line 11 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgQuickies = ((System.Windows.Controls.DataGrid)(target));
            
            #line 25 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
            this.dgQuickies.BeginningEdit += new System.EventHandler<System.Windows.Controls.DataGridBeginningEditEventArgs>(this.dgQuickies_BeginningEdit);
            
            #line default
            #line hidden
            
            #line 27 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
            this.dgQuickies.AutoGeneratingColumn += new System.EventHandler<System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs>(this.dgQuickies_AutoGeneratingColumn);
            
            #line default
            #line hidden
            
            #line 28 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
            this.dgQuickies.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgQuickies_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnUp = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
            this.btnUp.Click += new System.Windows.RoutedEventHandler(this.btnUp_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnDown = ((System.Windows.Controls.Button)(target));
            
            #line 51 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
            this.btnDown.Click += new System.Windows.RoutedEventHandler(this.btnDown_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnRemove = ((System.Windows.Controls.Button)(target));
            
            #line 52 "..\..\..\..\..\Tabs\TabManageFavorites\TabManageFavorites.xaml"
            this.btnRemove.Click += new System.Windows.RoutedEventHandler(this.btnRemove_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
