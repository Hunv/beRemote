﻿#pragma checksum "..\..\..\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9B657D6CC611B66F0521518D59C046AB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Fluent;
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
using System.Windows.Interactivity;
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
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Converters;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Themes;
using beRemote.Core.Common.Vpn;
using beRemote.GUI.Controls;
using beRemote.GUI.Converter;
using beRemote.GUI.Overlay.FirstRunWizard;
using beRemote.GUI.StatusBar.UserOnline;
using beRemote.GUI.ViewModel;
using beRemote.GUI.ViewModel.Command;


namespace beRemote.GUI {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal beRemote.GUI.MainWindow beRemote;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal beRemote.GUI.ViewModel.ViewModelMain viewModel;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spLogin;
        
        #line default
        #line hidden
        
        
        #line 135 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbPleaseLogin;
        
        #line default
        #line hidden
        
        
        #line 137 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbLoginUsername;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbLoginPassword;
        
        #line default
        #line hidden
        
        
        #line 142 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pbLoginPassword;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdChangePw;
        
        #line default
        #line hidden
        
        
        #line 191 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pbOld;
        
        #line default
        #line hidden
        
        
        #line 192 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pbNew1;
        
        #line default
        #line hidden
        
        
        #line 193 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pbNew2;
        
        #line default
        #line hidden
        
        
        #line 195 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblOldPw;
        
        #line default
        #line hidden
        
        
        #line 196 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblNewPw;
        
        #line default
        #line hidden
        
        
        #line 197 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblNewPwRep;
        
        #line default
        #line hidden
        
        
        #line 204 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
        #line default
        #line hidden
        
        
        #line 218 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 221 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdCreateAccount;
        
        #line default
        #line hidden
        
        
        #line 241 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCreateAccountUsername;
        
        #line default
        #line hidden
        
        
        #line 244 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCreateAccountDisplayname;
        
        #line default
        #line hidden
        
        
        #line 247 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pbCreateAccountPassword;
        
        #line default
        #line hidden
        
        
        #line 250 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pbCreateAccountPassword2;
        
        #line default
        #line hidden
        
        
        #line 278 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Fluent.RibbonContextualTabGroup tabGroupFilter;
        
        #line default
        #line hidden
        
        
        #line 286 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Fluent.RibbonContextualTabGroup tabGroupVpn;
        
        #line default
        #line hidden
        
        
        #line 749 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.AvalonDock.DockingManager adMainDock;
        
        #line default
        #line hidden
        
        
        #line 818 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal beRemote.GUI.Controls.beTreeView tvConList;
        
        #line default
        #line hidden
        
        
        #line 872 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbFilter;
        
        #line default
        #line hidden
        
        
        #line 891 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtQuickConnect;
        
        #line default
        #line hidden
        
        
        #line 920 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border lblQuickConnect;
        
        #line default
        #line hidden
        
        
        #line 941 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbQuickConnect;
        
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
            System.Uri resourceLocater = new System.Uri("/beRemote.GUI;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
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
            this.beRemote = ((beRemote.GUI.MainWindow)(target));
            
            #line 28 "..\..\..\MainWindow.xaml"
            this.beRemote.SizeChanged += new System.Windows.SizeChangedEventHandler(this.beRemote_SizeChanged);
            
            #line default
            #line hidden
            
            #line 29 "..\..\..\MainWindow.xaml"
            this.beRemote.LocationChanged += new System.EventHandler(this.beRemote_LocationChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.viewModel = ((beRemote.GUI.ViewModel.ViewModelMain)(target));
            return;
            case 3:
            this.spLogin = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.tbPleaseLogin = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.tbLoginUsername = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.tbLoginPassword = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.pbLoginPassword = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 8:
            this.grdChangePw = ((System.Windows.Controls.Grid)(target));
            return;
            case 9:
            this.pbOld = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 10:
            this.pbNew1 = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 11:
            this.pbNew2 = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 12:
            this.lblOldPw = ((System.Windows.Controls.Label)(target));
            return;
            case 13:
            this.lblNewPw = ((System.Windows.Controls.Label)(target));
            return;
            case 14:
            this.lblNewPwRep = ((System.Windows.Controls.Label)(target));
            return;
            case 15:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            return;
            case 16:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            return;
            case 17:
            this.grdCreateAccount = ((System.Windows.Controls.Grid)(target));
            return;
            case 18:
            this.txtCreateAccountUsername = ((System.Windows.Controls.TextBox)(target));
            return;
            case 19:
            this.txtCreateAccountDisplayname = ((System.Windows.Controls.TextBox)(target));
            return;
            case 20:
            this.pbCreateAccountPassword = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 247 "..\..\..\MainWindow.xaml"
            this.pbCreateAccountPassword.PasswordChanged += new System.Windows.RoutedEventHandler(this.pbCreateAccountPassword_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 21:
            this.pbCreateAccountPassword2 = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 250 "..\..\..\MainWindow.xaml"
            this.pbCreateAccountPassword2.PasswordChanged += new System.Windows.RoutedEventHandler(this.pbCreateAccountPassword2_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 22:
            this.tabGroupFilter = ((Fluent.RibbonContextualTabGroup)(target));
            return;
            case 23:
            this.tabGroupVpn = ((Fluent.RibbonContextualTabGroup)(target));
            return;
            case 24:
            this.adMainDock = ((Xceed.Wpf.AvalonDock.DockingManager)(target));
            return;
            case 25:
            this.tvConList = ((beRemote.GUI.Controls.beTreeView)(target));
            return;
            case 26:
            this.cmbFilter = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 27:
            this.txtQuickConnect = ((System.Windows.Controls.TextBox)(target));
            return;
            case 28:
            this.lblQuickConnect = ((System.Windows.Controls.Border)(target));
            return;
            case 29:
            this.cmbQuickConnect = ((System.Windows.Controls.ComboBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

