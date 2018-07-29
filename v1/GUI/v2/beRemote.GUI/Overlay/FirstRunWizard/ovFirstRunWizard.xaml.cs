using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using beRemote.Core.Common.Helper;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.Overlay.FirstRunWizard
{
    /// <summary>
    /// Interaction logic for ovFirstRunWizard.xaml
    /// </summary>
    public partial class OvFirstRunWizard : INotifyPropertyChanged
    {
        #region Commands
        public CmdBtnBackImpl CmdBtnBack { get; set; }
        public CmdBtnNextImpl CmdBtnNext { get; set; }
        public CmdBtnFinishImpl CmdBtnFinish { get; set; }
        #endregion

        private readonly ResourceDictionary _LangDictionary = new ResourceDictionary();

        public OvFirstRunWizard()
        {
            #region Load Language Dictionary
            var dictionaryFiles = new List<string>
                                           {
                                               "../Overlay/FirstRunWizard/Language/language.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.de-DE.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.es-ES.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.fr-FR.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.it-IT.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.nl-NL.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.pl-PL.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.ru-RU.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.zh-CN.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.cs-CZ.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.ar-SA.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.bg-BG.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.dk-DK.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.el-GR.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.fa-IR.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.fi-FI.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.he-IL.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.hi-IN.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.hr-HR.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.hu-HU.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.ko-KR.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.nn-NO.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.se-SE.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.tr-TR.xaml",
                                               "../Overlay/FirstRunWizard/Language/language.zh-CN.xaml"
                                           };

            foreach (var aLangfile in dictionaryFiles)
                _LangDictionary.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri(aLangfile, UriKind.Relative) });

            #endregion

            CmdBtnBack = new CmdBtnBackImpl();
            CmdBtnNext = new CmdBtnNextImpl();
            CmdBtnFinish = new CmdBtnFinishImpl();

            CmdBtnBack.PrevTab += CmdBtnBack_PrevTab;
            CmdBtnNext.NextTab += CmdBtnNext_WizardFinished;
            CmdBtnFinish.FinishWizard += CmdBtnFinish_FinishWizard;

            InitializeComponent();
        }

        void CmdBtnFinish_FinishWizard(object sender, RoutedEventArgs e)
        {
            //Check, if Superadmin-password is OK
            if (pbSuperadmin.SecurePassword.Length < 3)
            {
                var evArgsPw = new WizardMessageEventArgs();
                //Todo: translate
                evArgsPw.Message = _LangDictionary["MsgSuperadminPasswordShortText"].ToString();
                evArgsPw.MessageImage = MessageBoxImage.Error;
                evArgsPw.Title = _LangDictionary["MsgSuperadminPasswordShortTitle"].ToString();

                OnWizardMessageEvent(evArgsPw);
                return;
            }

            //Create SUperadmin
            var salt1 = Helper.GenerateSalt(512);
            var salt2 = Helper.GenerateSalt(512);
            var saId = StorageCore.Core.AddUser("superadmin", "Superadmin", Helper.GetPasswordHash(pbSuperadmin.SecurePassword, salt1, salt2));
            StorageCore.Core.ModifyUserSuperadmin(saId, true);
            StorageCore.Core.SetUserSalt1(saId, salt1);
            StorageCore.Core.SetUserSalt2(saId, salt2);
            StorageCore.Core.SetUserSalt3(saId, Helper.GenerateSalt(512));

            //Set the Autologin, if SU-Mode is selected
            if (SingleUserModeSelected)
            {
                //Create new user
                var Salt1StaticUser = Helper.GenerateSalt(512);
                var Salt2StaticUser = Helper.GenerateSalt(512);
                var SaIdStaticUser = StorageCore.Core.AddUser("User", "User", Helper.GetPasswordHash(Helper.ConvertToSecureString("0,34,23:alt,sec,yy:MM:dd hh:mm:ss,first"), Salt1StaticUser, Salt2StaticUser));
                StorageCore.Core.SetUserSalt1(SaIdStaticUser, Salt1StaticUser);
                StorageCore.Core.SetUserSalt2(SaIdStaticUser, Salt2StaticUser);
                StorageCore.Core.SetUserSalt3(SaIdStaticUser, Helper.GenerateSalt(512));

                StorageCore.Core.SetSingleUserMode(true, "0,34,23:alt,sec,yy:MM:dd hh:mm:ss,first");
            }
            else
            {
                StorageCore.Core.SetSingleUserMode(false, "");
            }

            var evArgs = new WizardResultEventArgs();
            evArgs.WizardName = "FirstRun";
            evArgs.Result = true;
            OnWizardFinishedEvent(evArgs);
        }

        void CmdBtnNext_WizardFinished(object sender, RoutedEventArgs e)
        {
            SelectedTabIndex++;
        }

        void CmdBtnBack_PrevTab(object sender, RoutedEventArgs e)
        {
            SelectedTabIndex--;
        }


        #region Properties

        #region DatabasePath
        public string DatabasePath
        {
            get { return (string)GetValue(DatabasePathProperty); }
            set { SetValue(DatabasePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DatabasePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatabasePathProperty =
            DependencyProperty.Register("DatabasePath", typeof(string), typeof(OvFirstRunWizard), new PropertyMetadata(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\beRemote"));
        #endregion



        #region SelectedTabIndex
        public int SelectedTabIndex
        {
            get { return (int)GetValue(SelectedTabIndexProperty); }
            set { SetValue(SelectedTabIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTabIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTabIndexProperty =
            DependencyProperty.Register("SelectedTabIndex", typeof(int), typeof(OvFirstRunWizard), new PropertyMetadata(0));
        #endregion


        #region MultiUserModeSelected
        public bool MultiUserModeSelected
        {
            get { return (bool)GetValue(MultiUserModeSelectedProperty); }
            set { SetValue(MultiUserModeSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MultiUserModeSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MultiUserModeSelectedProperty =
            DependencyProperty.Register("MultiUserModeSelected", typeof(bool), typeof(OvFirstRunWizard), new PropertyMetadata(true));
        #endregion



        #region SingleUserModeSelected
        public bool SingleUserModeSelected
        {
            get { return (bool)GetValue(SingleUserModeSelectedProperty); }
            set { SetValue(SingleUserModeSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleUserModeSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleUserModeSelectedProperty =
            DependencyProperty.Register("SingleUserModeSelected", typeof(bool), typeof(OvFirstRunWizard), new PropertyMetadata(false));
        #endregion


        


        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; //To Update Content on the Form

        /// <summary>
        /// Helper for Triggering PropertyChanged
        /// </summary>
        /// <param name="triggerControl">The Name of the Property to update</param>
        private void RaisePropertyChanged(string triggerControl)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(triggerControl));
            }
        }
        #endregion

        #region Evetns

        #region WizardFinished

        public delegate void WizardFinishedEventHandler(object sender, WizardResultEventArgs e);

        public event WizardFinishedEventHandler WizardFinished;

        protected virtual void OnWizardFinishedEvent(WizardResultEventArgs e)
        {
            var Handler = WizardFinished;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region WizardMessage

        public delegate void WizardMessageEventHandler(object sender, WizardMessageEventArgs e);

        public event WizardMessageEventHandler WizardMessage;

        protected virtual void OnWizardMessageEvent(WizardMessageEventArgs e)
        {
            var Handler = WizardMessage;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
        #endregion
    }
}
