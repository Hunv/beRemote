﻿#pragma checksum "..\..\..\..\..\Controls\TreeView\beTreeView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3F0EB21D55B2FA67218436C883B4E456"
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
using beRemote.GUI.Controls.Items;
using beRemote.GUI.Controls.ViewModel;


namespace beRemote.GUI.Controls {
    
    
    /// <summary>
    /// beTreeView
    /// </summary>
    public partial class beTreeView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 15 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal beRemote.GUI.Controls.beTreeView mainControl;
        
        #line default
        #line hidden
        
        
        #line 212 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal beRemote.GUI.Controls.ConnectionTree tvMain;
        
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
            System.Uri resourceLocater = new System.Uri("/beRemote.GUI.Controls;component/controls/treeview/betreeview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
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
            this.mainControl = ((beRemote.GUI.Controls.beTreeView)(target));
            return;
            case 2:
            
            #line 195 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanAddFolder);
            
            #line default
            #line hidden
            
            #line 195 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.AddFolder);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 196 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanAddConnection);
            
            #line default
            #line hidden
            
            #line 196 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.AddConnection);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 197 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanDelete);
            
            #line default
            #line hidden
            
            #line 197 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.DeleteFolder);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 200 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanEdit);
            
            #line default
            #line hidden
            
            #line 200 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.EditItem);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 201 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanSortUp);
            
            #line default
            #line hidden
            
            #line 201 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.SortUpItem);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 202 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanSortDown);
            
            #line default
            #line hidden
            
            #line 202 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.SortDownItem);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 203 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanAddToFav);
            
            #line default
            #line hidden
            
            #line 203 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.AddToFavItem);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 204 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanDefaultProtocol);
            
            #line default
            #line hidden
            
            #line 204 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.SetDefaultProtocol);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 205 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanDefaultFolder);
            
            #line default
            #line hidden
            
            #line 205 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.SetDefaultFolder);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 206 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).CanExecute += new System.Windows.Input.CanExecuteRoutedEventHandler(this.EvaluateCanConnectWithoutCredentials);
            
            #line default
            #line hidden
            
            #line 206 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            ((System.Windows.Input.CommandBinding)(target)).Executed += new System.Windows.Input.ExecutedRoutedEventHandler(this.ConnectWithoutCredentialsItem);
            
            #line default
            #line hidden
            return;
            case 12:
            this.tvMain = ((beRemote.GUI.Controls.ConnectionTree)(target));
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
            case 13:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.DragOverEvent;
            
            #line 226 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            eventSetter.Handler = new System.Windows.DragEventHandler(this.treeView_DragOver);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.DropEvent;
            
            #line 227 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            eventSetter.Handler = new System.Windows.DragEventHandler(this.treeView_Drop);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseMoveEvent;
            
            #line 228 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseEventHandler(this.treeView_MouseMove);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseDownEvent;
            
            #line 229 "..\..\..\..\..\Controls\TreeView\beTreeView.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.treeView_MouseDown);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

