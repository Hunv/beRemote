using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using beRemote.Core;
using beRemote.Core.Common.LogSystem;
using beRemote.GUI;
using beRemote.GUI.Notification;
using beRemote.GUI.ViewModel.EventArg;
using Xceed.Wpf.AvalonDock;

namespace beRemote.GUI.ViewModel.Command
{
    public class CmdMainWindowLoadedImpl : ICommand, INotifyPropertyChanged
    {
        public bool CanExecute(object sender)
        {
            return (true);
        }

        public void Execute(object sender)
        {
            try
            {
                //if (GetBeRemoteProcesses().Count == 1)
                if (1==1)
                {
                    #region First beRemote instance


                    //new Thread(() => CommServer.Instance.Start()).Start();

                    

                    try
                    {
                        #region Set Applicationicon

                        var iconUri = new Uri("pack://application:,,,/Images/icon.ico", UriKind.RelativeOrAbsolute);

                        //set the Icon without 256x256 for XP and vista?!
                        if (Environment.OSVersion.Version.Major == 5 &&
                            Environment.OSVersion.Version.Minor == 1 || //XP
                            Environment.OSVersion.Version.Major == 6 &&
                            Environment.OSVersion.Version.Minor == 0) //Vista
                            iconUri = new Uri("pack://application:,,,/Images/icon_xp.ico", UriKind.RelativeOrAbsolute);

                        //todo: Set ApplicationIcon
                        //Dispatcher.BeginInvoke(new Action(delegate
                        //{
                        //    Icon = BitmapFrame.Create(iconUri);
                        //}), null);


                        #endregion
                    }
                    catch (Exception ea)
                    {
                        Logger.Log(LogEntryType.Exception, "Error while preparing GUI " + ea);
                        //MessageBox.Show("Error while loading MainWindow or corresponding modules: \r\n" + ea);
                    }

                    //todo set infos
                    //Get last user and Build-Information for the world :)
                    //UiBuildInfo = Kernel.GetBuildInformation();
                    //UiLastUserLoggedIn = Kernel.GetLastLoggedInUser();

                    ////todo parse arguments
                    //Kernel.ParsedOptions.ParseArguments(Environment.GetCommandLineArgs());
                    //new Thread((Kernel.InitiateCore)).Start(App.ParsedOptions);

                    //remove this, when argument parsing is implemented
                    new Thread((Kernel.InitiateCore)).Start();

                    //todo check for updates
                    //CheckForUpdate();

                    #endregion
                }
                //else if (GetBeRemoteProcesses().Count > 1)
                //{
                //    #region next instances

                //    Process current = Process.GetCurrentProcess();
                //    foreach (Process process in GetBeRemoteProcesses())
                //    {
                //        if (process.Id != current.Id)
                //        {
                //            new Thread(() =>
                //            {
                //                App.ParsedOptions.ParseArguments(Environment.GetCommandLineArgs());

                //                if (App.ParsedOptions.ConnectionSettingId != null)
                //                {
                //                    App.InterCommClient.OpenNewConnection(
                //                        long.Parse(App.ParsedOptions.ConnectionSettingId));
                //                }
                //            }).Start();

                //            SetForegroundWindow(process.MainWindowHandle);

                //            _LoadingCanceled = true;

                //        }
                //    }
                //    _LoadingCanceled = true;
                //    Application.Current.Shutdown();

                //    #endregion
                //}
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                var tw = (TextWriter)
                    new StreamWriter(Path.Combine("" + Environment.GetEnvironmentVariable("appdata"), "beRemote",
                        "CRASH_" + DateTime.Now.Millisecond));

                tw.Write(ex.ToString());
            }
            
            OnApplicationLoaded(new MainWindowLoadedEventArgs());
        }

        public event EventHandler CanExecuteChanged;


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

        #region Event

        public delegate void ApplicationLoadedEventHandler(object sender, MainWindowLoadedEventArgs e);

        public event ApplicationLoadedEventHandler ApplicationLoaded;

        protected virtual void OnApplicationLoaded(MainWindowLoadedEventArgs e)
        {
            var Handler = ApplicationLoaded;
            if (Handler != null)
                Handler(this, e);
        }
        #endregion
    }
}
