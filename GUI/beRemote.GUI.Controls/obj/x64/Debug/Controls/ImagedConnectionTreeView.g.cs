﻿#pragma checksum "..\..\..\..\Controls\ImagedConnectionTreeView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1F15CA4D1F7D62A37680CD77E8FBC816"
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
using beRemote.GUI.Controls;
using beRemote.GUI.Controls.Classes;


namespace beRemote.GUI.Controls {
    
    
    /// <summary>
    /// ImagedConnectionTreeView
    /// </summary>
    public partial class ImagedConnectionTreeView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 32 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer svScroll;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal beRemote.GUI.Controls.ImagedConnectionTreeViewControl tvConnectionList;
        
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
            System.Uri resourceLocater = new System.Uri("/beRemote.GUI.Controls;component/controls/imagedconnectiontreeview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
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
            
            #line 10 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((beRemote.GUI.Controls.ImagedConnectionTreeView)(target)).PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.UserControl_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((beRemote.GUI.Controls.ImagedConnectionTreeView)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 15 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvDeleteEntryExecute);
            
            #line default
            #line hidden
            
            #line 15 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvDeleteEntryCanExecute);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 16 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvAddFolderExecute);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvAddFolderCanExecute);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 17 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvAddSettingExecute);
            
            #line default
            #line hidden
            
            #line 17 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvAddSettingCanExecute);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 18 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvEditSettingExecute);
            
            #line default
            #line hidden
            
            #line 18 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvEditSettingCanExecute);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 19 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvSetDefaultFolderExecute);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvSetDefaultFolderCanExecute);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 20 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvSetDefaultProtocolExecute);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvSetDefaultProtocolCanExecute);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 21 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvConnectExecute);
            
            #line default
            #line hidden
            
            #line 21 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvConnectCanExecute);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 22 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvSortUpExecute);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvSortUpCanExecute);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 23 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvSortDownExecute);
            
            #line default
            #line hidden
            
            #line 23 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvSortDownCanExecute);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 24 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.tvQuickConnectAddExecute);
            
            #line default
            #line hidden
            
            #line 24 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.tvQuickConnectAddCanExecute);
            
            #line default
            #line hidden
            return;
            case 12:
            this.svScroll = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 13:
            this.tvConnectionList = ((beRemote.GUI.Controls.ImagedConnectionTreeViewControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 14:
            
            #line 55 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            ((System.Windows.Controls.StackPanel)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Item_LeftMouseDown);
            
            #line default
            #line hidden
            break;
            case 15:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.DragOverEvent;
            
            #line 157 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            eventSetter.Handler = new System.Windows.DragEventHandler(this.treeView_DragOver);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.DropEvent;
            
            #line 158 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            eventSetter.Handler = new System.Windows.DragEventHandler(this.treeView_Drop);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseMoveEvent;
            
            #line 159 "..\..\..\..\Controls\ImagedConnectionTreeView.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseEventHandler(this.treeView_MouseMove);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

