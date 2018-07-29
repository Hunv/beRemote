using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using beRemote.Core;
using beRemote.Core.Common.Helper;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Common.SimpleSettings;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.Exceptions;
using beRemote.Core.Services;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.ViewModel.EventArg;

namespace beRemote.GUI.ViewModel
{
    public class Login : INotifyPropertyChanged
    {
        #region Events
        #region LoginProcessing

        public delegate void LoginProcessingEventHandler(object sender, LoginProcessingEventArgs e);

        public event LoginProcessingEventHandler LoginProcessing;

        protected virtual void OnLoginProcessing(LoginProcessingEventArgs e)
        {
            var Handler = LoginProcessing;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region UserNotExists

        public delegate void UserNotExistsEventHandler(object sender, RoutedEventArgs e);

        public event UserNotExistsEventHandler UserNotExists;

        protected virtual void OnUserNotExists(RoutedEventArgs e)
        {
            var Handler = UserNotExists;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region ShowMessage

        public delegate void ShowMessageEventHandler(object sender, ShowMessageEventArgs e);

        public event ShowMessageEventHandler ShowMessage;

        protected virtual void OnShowMessage(ShowMessageEventArgs e)
        {
            var Handler = ShowMessage;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region LoginFailed
        public delegate void LoginFailedEventHandler(object sender, RoutedEventArgs e);

        public event LoginFailedEventHandler LoginFailed;

        protected virtual void OnLoginFailed(RoutedEventArgs e)
        {
            var Handler = LoginFailed;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion

        #region LoginSuccessful

        public delegate void LoginSuccessfulEventHandler(object sender, LoginSuccessfulEventArgs e);

        public event LoginSuccessfulEventHandler LoginSuccessful;

        protected virtual void OnLoginSuccessful(LoginSuccessfulEventArgs e)
        {
            var Handler = LoginSuccessful;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion

        #region PasswordChangeGridShow

        public delegate void PasswordChangeGridShowEventHandler(object sender, RoutedEventArgs e);

        public event PasswordChangeGridShowEventHandler PasswordChangeGridShow;

        protected virtual void OnPasswordChangeGridShow(RoutedEventArgs e)
        {
            var Handler = PasswordChangeGridShow;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion


        #region PasswordChangeGridHide

        public delegate void PasswordChangeGridHideEventHandler(object sender, RoutedEventArgs e);

        public event PasswordChangeGridHideEventHandler PasswordChangeGridHide;

        protected virtual void OnPasswordChangeGridHide(RoutedEventArgs e)
        {
            var Handler = PasswordChangeGridHide;
            if (Handler != null)
                Handler(this, e);
        }

        #endregion
        #endregion

        #region private variables
        /// <summary>
        /// Contains the values of the local config-file
        /// </summary>
        private readonly Dictionary<string, string> _LocalConfigValues = new Dictionary<string, string>();
        #endregion

        #region private Methods

        /// <summary>
        /// Loads the local configfile and stores them in the private variable _LocalConfigValues
        /// </summary>
        /// <returns>Loading successful?</returns>
        private bool LoadLocalConfig()
        {
            return (LoadLocalConfig(false));
        }

        /// <summary>
        /// Loads the local configfile and stores them in the private variable _LocalConfigValues
        /// </summary>
        /// <param name="reload">If set to true, the configfile will be reloaded, even if it was already loaded</param>
        /// <returns>Loading successful?</returns>
        private bool LoadLocalConfig(bool reload)
        {
            //Check if the config is already loaded and it should not be reloaded
            if (_LocalConfigValues.Count > 0 && reload == false)
            {
                //It is already loaded
                return (true);
            }

            //Set the path for the local config-file
            var localConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\beRemote\\localconfig.ini";

            //If the local configfile not exists, create it
            if (!File.Exists(localConfigPath))
            {
                try
                {
                    var sW = File.CreateText(localConfigPath);
                    sW.Close();
                }
                catch (Exception ea)
                {
                    //Log an Error on failure and return false                    
                    Logger.Log(LogEntryType.Warning, "Local configfile localconfig.ini could not be created.", ea);
                    return (false);
                }
            }

            //Create the stream for the local config file
            var sR = new StreamReader(localConfigPath, Encoding.Default);

            //As long the end of the file is not reached
            while (sR.Peek() >= 0)
            {
                //Read the line and split it in two strings by deviding it by equal-sign
                var line = sR.ReadLine().Split(new[] { '=' }, 2);

                //Add the values to the _LocalConfigValues-Dictionary
                _LocalConfigValues.Add(line[0], line[1]);
            }
            sR.Close();

            return (true);
        }

        /// <summary>
        /// Starts the login-process and verifies, that everything is fine
        /// </summary>
        /// <param name="objParameter"></param>
        private void StartLoginProcess(object objParameter)
        {
            //Check if the parameter is valid
            if (objParameter.GetType() != typeof(object[]))
                throw new InvalidOperationException("The parameter must be an object[]");

            //Convert parameters to the real type
            var arrParameter = objParameter as object[];

            #region Verify Parameters

            if (arrParameter == null)
                throw new InvalidOperationException("The parameters must be an object[]");
            
            if (arrParameter.Length != 2 && arrParameter.Length != 3)
            {
                if (arrParameter[0].GetType() != typeof (string))
                    throw new InvalidOperationException("The first object has to be a string");

                if (arrParameter[1].GetType() != typeof(SecureString))
                    throw new InvalidOperationException("The second object has to be a SecureString");

                if (arrParameter.Length == 3)
                {
                    if (arrParameter[2].GetType() != typeof (int))
                        throw new InvalidOperationException("The third object has to be an integer");
                }
            }
            #endregion

            var username = arrParameter[0].ToString();
            var password = arrParameter[1] as SecureString;

            //Check if there is a delay configured to delay the execution of the Login
            if (arrParameter.Length > 2)
            {
                Thread.Sleep((int)arrParameter[2]);
            }

            //Raise Event to disable the Login-Controls
            OnLoginProcessing(new LoginProcessingEventArgs(true));
            
            //Get the UserId
            long userId = StorageCore.Core.GetUserId(username);

            //Is the user not existing?
            if (userId == 0)
            {
                //User not exists: so cancel here
                OnUserNotExists(null);
                return;
            }

            #region Maintenance Mode

            //If the Database is in MaintenanceMode
            if (IsMaintenanceModeActive)
            {
                //Todo: Fit to translation
                OnShowMessage(
                    new ShowMessageEventArgs(
                        "The beRemote Database is currently in Maintenace-Mode. Please try again later.",
                        "Maintanance Mode",
                        MessageBoxImage.Information));

                return;
            }

            #endregion

            //Is Password correct?
            if (StorageCore.Core.CheckUserPassword(
                userId,
                Helper.GetPasswordHash(password, StorageCore.Core.GetUserSalt1(userId), StorageCore.Core.GetUserSalt2(userId))))
            {
                Logger.Log(LogEntryType.Info, "User " + username + " logged in");

                Kernel.SetLastUser(username);

                //login
                login(username, password);
            }
            else //Password wrong
            {
                Logger.Log(LogEntryType.Info, "Invalid credentials for user " + username);

                OnLoginFailed(null);
            }
        }

        /// <summary>
        /// Sets the user and the password, waits for the Kernel, if it is not ready and finally loads the gui
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void login(String username, SecureString password)
        {
            //Get the expected userId
            var expUserId = StorageCore.Core.GetUserId(username);

            //Set the new Usercredentials
            StorageCore.Core.ChangeUser(username, Helper.GetPasswordHash(password, StorageCore.Core.GetUserSalt1(expUserId), StorageCore.Core.GetUserSalt2(expUserId)));

            //Set the password
            Helper.SetUserPassword(password);

            #region Wait for the kernel and finish login

            if (Kernel.IsKernelReady() == false)
            {
                //Todo: Loading-Animation
                while (Kernel.IsKernelReady() == false) //Wait until the Kernelloading is finished
                {
                    Thread.Sleep(1000);
                }
            }

            #endregion

            LoadLoggedInData();
        }

        /// <summary>
        /// Loads Data for the GUI and Triggers the LoginSuccessful-Event
        /// </summary>
        private void LoadLoggedInData()
        {
            var e = new LoginSuccessfulEventArgs();

            var userSettings = StorageCore.Core.GetUserSettings(); //Get the current user settings
            StorageCore.Core.UserLoggedIn();//Increase Logincounter; Set Lastlogin-time and -machine

            #region Cancel if something went wrong
            if (userSettings == null || userSettings.getId() == 0)
            {
                throw new beRemoteException(beRemoteExInfoPackage.StopInformationPackage, "Can't load usersettings");
            }
            #endregion


            //Get the Visual Settings of the User
            if (userSettings.getVisualSettings() == null)
                userSettings.VisualSettings = StorageCore.Core.GetUserVisuals(userSettings.getId());

            e.UserSettings = userSettings;
            e.IsUserSuperadmin = StorageCore.Core.GetUserSuperadmin();

            OnLoginSuccessful(e);
        }

        #region Methods, called by LoggedIn for Loading GUI

        #endregion

        #endregion

        #region public Methods

        /// <summary>
        /// Gets the user, that will be shown in the username-textbox of the loginscreen on startup
        /// </summary>
        /// <returns></returns>
        public string GetDefaultLoginUsername()
        {
            //Load the local config, if not already done
            LoadLocalConfig();

            //Get the last user, if there is a last user
            if (_LocalConfigValues.ContainsKey("lastuser"))
            {
                return (_LocalConfigValues["lastuser"]);
            }

            //No last user is know, so return an emtpy string
            return (string.Empty);
        }

        /// <summary>
        /// Performce the Login-Precedure
        /// </summary>
        /// <param name="username">The username of the loginform</param>
        /// <param name="password">The password of the loginform</param>
        /// <returns>Login was successfull or not</returns>
        public bool ExecuteLogin(string username, SecureString password)
        {
            new Thread(StartLoginProcess).Start(new object[] {username, password});

            return (true);
        }

        /// <summary>
        /// Creates a new account, if all parameters are matching
        /// </summary>
        /// <param name="username">The username of the new user, that will be used to login and authenticate</param>
        /// <param name="displayname">The name, that will be shown in the GUI</param>
        /// <param name="password">The initial password</param>
        /// <param name="password2">The initial password, again</param>
        /// <returns></returns>
        public bool CreateAccount(string username, string displayname, SecureString password, SecureString password2)
        {
            //Check, if something went wrong, that will result in unwanted errors
            if (password == null || password2 == null)
                throw new InvalidOperationException("the password-parameters must not be null");

            //If the passwords are not matching
            if (!Helper.SecureStringsAreEqual(password, password2))
            {
                //Todo: Translate
                OnShowMessage(new ShowMessageEventArgs("Your passwords are not matching.", "Non-Equal passwords", MessageBoxImage.Error));
                return (false);
            }

            //If there are no new Account allowed
            if (StorageCore.Core.GetSetting("useraccountcreation") != "1")
            {
                //Todo: Translate
                OnShowMessage(new ShowMessageEventArgs("User Account Creation is disabled on this server. Please contact your administrator!", "Account Creation disabled", MessageBoxImage.Information));
                return (false);
            }

            //Check, if a username with at last 3 letters is given
            if (username.Length < 3) 
            {
                //Todo: Translate
                OnShowMessage(new ShowMessageEventArgs("Please enter a username, longer that two letters", "Username to short", MessageBoxImage.Warning));
                return (false);
            }

            //Will be not zero, if the username already exists
            if (StorageCore.Core.GetUserId(username) != 0)
            {
                //Todo: Translate
                OnShowMessage(new ShowMessageEventArgs("This username already exists", "Username invalid", MessageBoxImage.Information));
                return (false);
            }

            //Set Displayname, if not set
            if (displayname == "")
                displayname = username;
            
                
            Logger.Log(LogEntryType.Info, "Creating Username " + username + " (Displayname: " + displayname + ")");

            //Create the useraccount
            var salt1 = Helper.GenerateSalt(512);
            var salt2 = Helper.GenerateSalt(512);
            var userId = StorageCore.Core.AddUser(username, displayname, Helper.GetPasswordHash(password, salt1, salt2));
            StorageCore.Core.SetUserSalt1(userId, salt1);
            StorageCore.Core.SetUserSalt2(userId, salt2);
            StorageCore.Core.SetUserSalt3(userId, Helper.GenerateSalt(512));
            
            //Login the user
            //login(username, password);
            // Using the same impl as when the user clicks the login button on the UI in order to trigger some other actions
            new Thread(StartLoginProcess).Start(new object[] { username, password });


            return (true);
        }

        #endregion

        #region public properties

        private string _LoginUsername;

        /// <summary>
        /// The Username typed into to Loginform
        /// </summary>
        public string LoginUsername
        {
            get
            {   
                //If the Username is not null, it is set and return the stored value
                if (_LoginUsername != null) return _LoginUsername;

                //If the Username is null, it is not set, so load the default username
                //Returns an emtpy string if there is no DefaultLoginUsername and _LoginUsername will not be null anymore
                _LoginUsername = GetDefaultLoginUsername();
                RaisePropertyChanged("LoginUsername");

                return _LoginUsername;
            }
            set
            {
                _LoginUsername = value;
                RaisePropertyChanged("LoginUsername");
            }
        }

        /// <summary>
        /// The Password entered in the Loginform
        /// This property is not stored on any byte in beRemote. Nobody has access to this!
        /// This Property just exists to pervent developers from searching this property anywhere else.
        /// </summary>
        public SecureString LoginPassword
        {
            get { throw new SecurityException("This property is not stored on any byte in beRemote. Nobody has access to this!"); }
        }

        /// <summary>
        /// Is the Database currently in Maintenance Mode?
        /// </summary>
        public bool IsMaintenanceModeActive
        {
            get 
            {
                return (StorageCore.Core.GetSetting("maintmode") == "1");
            }
        }

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
    }
}
