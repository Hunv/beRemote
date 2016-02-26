﻿#pragma checksum "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D3410034B93ADAE85FEE4C319419E980"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
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
using beRemote.GUI.Tabs.ManageFilter;


namespace beRemote.GUI.Tabs.ManageFilter {
    
    
    /// <summary>
    /// TabManageFilter
    /// </summary>
    public partial class TabManageFilter : beRemote.Core.Definitions.Classes.TabBase, System.Windows.Markup.IComponentConnector {
        
        
        #line 62 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstFilterOverview;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSetSave;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstFilter;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFilterDelete;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbValue;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbValueProtocol;
        
        #line default
        #line hidden
        
        
        #line 200 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtSave;
        
        #line default
        #line hidden
        
        
        #line 216 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRefreshPreview;
        
        #line default
        #line hidden
        
        
        #line 219 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstPreview;
        
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
            System.Uri resourceLocater = new System.Uri("/beRemote.GUI;component/tabs/managefilter/tabmanagefilter.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
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
            
            #line 11 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            ((beRemote.GUI.Tabs.ManageFilter.TabManageFilter)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ContentTab_Loaded);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            ((beRemote.GUI.Tabs.ManageFilter.TabManageFilter)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.TabBase_Close);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lstFilterOverview = ((System.Windows.Controls.ListBox)(target));
            
            #line 63 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            this.lstFilterOverview.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lstFilterOverview_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 77 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnSetAddNew_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 78 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnSetDelete_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 79 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnSetCopy_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnSetSave = ((System.Windows.Controls.Button)(target));
            
            #line 80 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            this.btnSetSave.Click += new System.Windows.RoutedEventHandler(this.btnSetSave_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.lstFilter = ((System.Windows.Controls.ListBox)(target));
            
            #line 101 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            this.lstFilter.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lstFilter_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnFilterDelete = ((System.Windows.Controls.Button)(target));
            
            #line 108 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            this.btnFilterDelete.Click += new System.Windows.RoutedEventHandler(this.btnFilterDelete_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.cmbValue = ((System.Windows.Controls.ComboBox)(target));
            
            #line 131 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            this.cmbValue.IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.cmbValue_IsVisibleChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.cmbValueProtocol = ((System.Windows.Controls.ComboBox)(target));
            
            #line 150 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            this.cmbValueProtocol.IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.cmbValue_IsVisibleChanged);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 173 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 180 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 187 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            ((System.Windows.Controls.CheckBox)(target)).Click += new System.Windows.RoutedEventHandler(this.CheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.txtSave = ((System.Windows.Controls.TextBox)(target));
            return;
            case 15:
            this.btnRefreshPreview = ((System.Windows.Controls.Button)(target));
            
            #line 216 "..\..\..\..\..\Tabs\ManageFilter\TabManageFilter.xaml"
            this.btnRefreshPreview.Click += new System.Windows.RoutedEventHandler(this.btnRefreshPreview_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.lstPreview = ((System.Windows.Controls.ListBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
