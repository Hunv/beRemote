﻿#pragma checksum "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3348286DC7C9F1DD1FEB60C7298ACA58"
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


namespace beRemote.GUI.Tabs.ManageFavorites {
    
    
    /// <summary>
    /// TabManageFavorites
    /// </summary>
    public partial class TabManageFavorites : beRemote.Core.Definitions.Classes.TabBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 49 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgQuickies;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnUp;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDown;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
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
            System.Uri resourceLocater = new System.Uri("/beRemote.GUI;component/tabs/managefavorites/tabmanagefavorites.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
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
            
            #line 9 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
            ((beRemote.GUI.Tabs.ManageFavorites.TabManageFavorites)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgQuickies = ((System.Windows.Controls.DataGrid)(target));
            
            #line 61 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
            this.dgQuickies.BeginningEdit += new System.EventHandler<System.Windows.Controls.DataGridBeginningEditEventArgs>(this.dgQuickies_BeginningEdit);
            
            #line default
            #line hidden
            
            #line 63 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
            this.dgQuickies.AutoGeneratingColumn += new System.EventHandler<System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs>(this.dgQuickies_AutoGeneratingColumn);
            
            #line default
            #line hidden
            
            #line 64 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
            this.dgQuickies.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgQuickies_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnUp = ((System.Windows.Controls.Button)(target));
            
            #line 86 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
            this.btnUp.Click += new System.Windows.RoutedEventHandler(this.btnUp_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnDown = ((System.Windows.Controls.Button)(target));
            
            #line 87 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
            this.btnDown.Click += new System.Windows.RoutedEventHandler(this.btnDown_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnRemove = ((System.Windows.Controls.Button)(target));
            
            #line 88 "..\..\..\..\..\Tabs\ManageFavorites\TabManageFavorites.xaml"
            this.btnRemove.Click += new System.Windows.RoutedEventHandler(this.btnRemove_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

