using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using beRemote.Core.CommandLineInterface;
using beRemote.Core.Common.Helper.CLI;
using beRemote.Core.Common.LogSystem;
using beRemote.Core.Common.Helper;
using System.IO;
using beRemote.Core.Common.PluginBase;
using beRemote.Core.Common.Vpn;
using beRemote.Core.Definitions.Classes;
using beRemote.Core.Exceptions;
using beRemote.Core.Exceptions.Kernel;
using beRemote.Core.Exceptions.Plugin;
using beRemote.Core.InterComm;
using beRemote.Core.KernelHelper;
using beRemote.Core.ProtocolSystem.ProtocolBase;
using System.Threading;
using beRemote.Core.StorageSystem.StorageBase;
using beRemote.GUI.Notification;
using System.Diagnostics;

namespace beRemote.Core
{
    public class Kernel
    {
        public delegate void KernelReadyEventHandler(object sender, KernelReadyEventArgs e);
        public delegate void KernelFailedEventHandler(object sender, KernelFailedEventArgs e);
        public delegate void KernelReceivedNewConnectionEventHandler(object sender, long connid);
        
        public static event KernelReadyEventHandler OnKernelReady;
        public static event KernelFailedEventHandler OnKernelFailed;
        public static event KernelReceivedNewConnectionEventHandler OnNewConnectionTriggered;

        private const String LoggerContext = "Kernel";

        public static Guid KernelInstanceGuid = Guid.NewGuid();

        private static MEF.MEFLoader _mefLoaderInstance;

        private static CliOptions _parsedOptions;

        public static CliOptions ParsedOptions
        {
            get
            {
                if (_parsedOptions == null)
                    _parsedOptions = new CliOptions();

                return _parsedOptions;
            }
        }

        public static CommServer InterCommServer;

        public static void InitiateCore()
        {
            InitiateCore(false);
        }

        private static void InitiateCore(Boolean databaseOnly)
        {
            // Set the failed handler in order to catch also AlreadyRunning exceptions
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            OnKernelFailed += Kernel_OnKernelFailed;
            // At first we need to check if other beremote instances are running, if so we do some stuff...
            //var exists = ExistsMultipleBeremoteInstances();
            //if (exists)
            //{
            //    var client = new CommClient();
            //    client.ShowNotification("beRemote is already running. Trying to focus it now...");
            //    client.FocusMainWindow();

            //    throw new ApplicationAlreadyRunningException();
            //}
            
            // Starting the logging facility
            InitiateLogger();

            // Starting the inter communication wcf service for communication accross multiple beremote instances (cli support only)
            try
            {
                InterCommServer = new CommServer(KernelInstanceGuid);
                InterCommServer.Start();
            }
            catch (System.ServiceModel.AddressAlreadyInUseException ex)
            {
                var client = new CommClient();
                client.ShowNotification("beRemote is already running. Trying to focus it now...");
                client.FocusMainWindow();

                //TriggerNewConnection();

                var exc = new ApplicationAlreadyRunningException();

                OnKernelFailed(null, new KernelFailedEventArgs(exc));

                return;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString(), 0);
            }
         

            try
            {
                //TODO: Remove this after update releases. Only for compatiblity purposes while old versions are in use
                var databasePath = System.Environment.ExpandEnvironmentVariables(Helper.GetApplicationConfiguration().GetValue("database", "dbpath"));
                var newDbPath = new FileInfo(databasePath);
                var compatOldDbPath = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\" + newDbPath.Name);

                if (compatOldDbPath.Exists && false == newDbPath.Exists)
                {
                    Logger.Warning("Database was present in executable folder. Copying it over to config file target.");

                    compatOldDbPath.CopyTo(newDbPath.FullName);
                }

                // possible override of database
                if (ParsedOptions != null)
                {
                    if (ParsedOptions.ContainsParameter("db-interface"))
                    {
                        var option = ParsedOptions.GetParameter("db-interface");

                        var localConfig = Helper.GetApplicationConfiguration();

                        Logger.Info("Overriding default database interface");

                        localConfig.SetValue("database", "dbinterface", option.Value, true);
                    }

                    if (ParsedOptions.ContainsParameter("db-path"))
                    {
                        var option = ParsedOptions.GetParameter("db-path");

                        var localConfig = Helper.GetApplicationConfiguration();

                        Logger.Info("Overriding default database path, now using: " + option.Value);

                        localConfig.SetValue("database", "dbpath", option.Value, true);
                    }
                }

                StorageCore.Core.InitDatabaseGuid(false);

            }
            catch { }

            // Deleting tmp folder
            try
            {
                var tmp = new DirectoryInfo(Environment.ExpandEnvironmentVariables("%appdata%\\beRemote\\tmp"));

                if (tmp.Exists)
                    tmp.Delete(true);
            }
            catch (Exception ex)
            {
                new beRemoteException(beRemoteExInfoPackage.MajorInformationPackage,
                    "Problem removing tmp folder of previous instances.", ex);

            }


            if (true == databaseOnly)
            {
                Logger.Log(LogEntryType.Warning, "Kernel initiation is skipped due to database only flag!");

                if (OnKernelReady != null)
                    OnKernelReady(new StackTrace(), new KernelReadyEventArgs());

                return;
            }
            else
            {


                Logger.Log(LogEntryType.Debug, "Initiating beRemote ProtocolSystem thread", LoggerContext);

                AllPluginSystemCaller();


                Logger.Log(LogEntryType.Debug, "... all beRemote System threads are running!", LoggerContext);

                if (OnKernelReady != null)
                    OnKernelReady(new StackTrace(), new KernelReadyEventArgs());
            }

        }

      
        static void Kernel_OnKernelFailed(object sender, KernelFailedEventArgs e)
        {
            OnKernelReady = null;


        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject.GetType().IsSubclassOf(typeof(Exception)))
            {
                if (e.IsTerminating)
                {
                    Logger.Error("Fatal error in execution", 0);
                    Logger.Error(((Exception)e.ExceptionObject).Message, 0);
                    Logger.Error(((Exception)e.ExceptionObject).ToString(), 0);
                }
                else
                {
                    ShowUnhandledExceptionDialog((Exception)e.ExceptionObject);
                }
            }
            else
            {
                
            }

           
        }

        public static void ShowUnhandledExceptionDialog(Exception ex)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var result = new beRemote.Core.Exceptions.UnhandledUIException(
                  beRemoteExInfoPackage.SignificantInformationPackage,
                   "An unhandled UI exception is thrown...", ex).ShowDialog(false);

                switch (result)
                {
                    case UIExceptionWindow.brDialogResult.STOP:
                        Application.Current.Shutdown();
                        break;
                    case UIExceptionWindow.brDialogResult.CONTINUE:
                    default:
                        var result2 = MessageBox.Show("Continuing the execution may lead in corrupted data.\r\nWe highly recommend you to not continue the execution!\r\nRestart beRemote instead!\r\n\r\nAre you sure you want to continue the execution?", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.No);
                        if (result2 != MessageBoxResult.Yes)
                        {


                            Application.Current.Shutdown();



                        }
                        break;
                }
            }), null);
        }

        public static void ShowHandledExceptionDialog(beRemoteException bEx)
        {
            bool closed = false;
            
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (bEx.GetHandlerAction() != null)
                {
                    bEx.GetHandlerAction().Invoke();
                }
                else
                {
                    var ui = new UIExceptionWindow(bEx, false);
                    ui.ShowDialog();
                }
                closed = true;
            }), null);

            while (closed == false)
                Thread.Sleep(500);

        }



        private static bool _loggerInitiated = false;
        public static void InitiateLogger()
        {
            if (_loggerInitiated == false)
            {
                String logLevel = Helper.GetApplicationConfiguration().GetValue("logging.base", "level").ToLower();
                LogEntryType level = LogEntryType.Verbose;

                if (logLevel == LogEntryType.Verbose.ToString().ToLower() || logLevel == LogEntryType.Debug.ToString().ToLower())
                {
                    level = LogEntryType.Verbose;
                }
                else if (logLevel == LogEntryType.Info.ToString().ToLower())
                {
                    level = LogEntryType.Info;
                }
                else if (logLevel == LogEntryType.Warning.ToString().ToLower())
                {
                    level = LogEntryType.Warning;
                }
                else if (logLevel == LogEntryType.Exception.ToString().ToLower())
                {
                    level = LogEntryType.Exception;
                }

                //lfl.logfilename
                String logFile = Helper.GetApplicationConfiguration().GetValue("logging.logfilelogger", "lfl.logpath");
                if (false == logFile.Contains(@"\"))
                    logFile += @"\";

                logFile += Helper.GetApplicationConfiguration().GetValue("logging.logfilelogger", "lfl.logfilename");

                bool logToEvtLog = false;
                bool logToFileLog = true;

                //TODO configurable

                Logger.Init(level, true, logFile, false, logToEvtLog, logToFileLog);

                _loggerInitiated = true;
            }
        }

        private static Boolean pstc_running = false;
        /// <summary>
        /// Loads all plugins that are known to our MEF. :)
        /// </summary>
        private static void AllPluginSystemCaller()
        {
            //Thread.Sleep(10000);

            if (!pstc_running)
            {
                try
                {
                    pstc_running = true;

                    if (_mefLoaderInstance == null)
                        _mefLoaderInstance = new MEF.MEFLoader();

                    _mefLoaderInstance.LoadProtocols();

                    try
                    {
                        _mefLoaderInstance.LoadUIPlugins();
                    }
                    catch (Exception ex)
                    {
                        throw new beRemote.Core.Exceptions.beRemoteException(beRemote.Core.Exceptions.beRemoteExInfoPackage.SignificantInformationPackage, "Problem while loading UI plugins", ex);
                    }


                }
                catch (beRemote.Core.Exceptions.beRemoteException bEx)
                {
                    if (OnKernelFailed != null)
                        OnKernelFailed(new StackTrace(), new KernelFailedEventArgs(bEx));

                }
                catch (Exception ex)
                {
                    if (OnKernelFailed != null)
                        OnKernelFailed(new StackTrace(), new KernelFailedEventArgs(new beRemote.Core.Exceptions.beRemoteException(beRemote.Core.Exceptions.beRemoteExInfoPackage.StopInformationPackage, ex.Message, ex)));
                }
                finally
                {
                    KernelHelper.ModuleState.MEFPluginSystemReady = true;
                }
            }
        }


        public static Boolean IsKernelReady()
        {

            //if (_faultedThreads.Count > 0)
            //{
            //    String message = "Multiple exceptions in stack.\r\n";
            //    foreach (Exception ex in _faultedThreads.Values)
            //    {
            //        message += ex.ToString() + "\r\n";
            //    }

            //    throw new beRemote.Core.Exceptions.Kernel.FaultedThreadException(beRemote.Core.Exceptions.beRemoteExInfoPackage.StopInformationPackage, message);

            //    // TODO: UI Exceptions
            //}

            if (KernelHelper.ModuleState.MEFPluginSystemReady)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Shutdown()
        {
            Logger.Log(LogEntryType.Info, "Shutting down beRemote Kernel and corresponding systems", LoggerContext);

            TrayIcon.TrayIconInstance.Hide();
            try
            {
                if (InterCommServer.Running)
                {
                    InterCommServer.Stop();
                    InterCommServer = null;
                }
            }
            catch { }




        }



        /// <summary>
        /// Returns a list of all available protocols (Only usable, if any protocol is invalid it will not show up in this list)
        /// </summary>
        public static SortedList<String, Protocol> GetAvailableProtocols()
        {
            return _mefLoaderInstance.AvailableProtocols;
        }

        /// <summary>
        /// Returns a list of all available ui plugins
        /// </summary>        
        public static SortedList<String, beRemote.GUI.Plugins.UiPlugin> GetAvailableUIPlugins()
        {
            return _mefLoaderInstance.AvailableUiPlugins;
        }


        public static SortedList<String, Plugin> GetAvailablePlugins()
        {
            var returnList = new SortedList<String, Plugin>();

            foreach (var prot in GetAvailableProtocols())
                returnList.Add(prot.Key, prot.Value);

            foreach (var plugs in GetAvailableUIPlugins())
                returnList.Add(plugs.Key, plugs.Value);

            return returnList;
        }

        public static String GetBaseDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }


        //public static ExceptionSystem.ExceptionBase.ExceptionType ThrowException(Exception ex)
        //{
        //    if (ex.GetType() == typeof(beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions.BERemoteGUIException) ||
        //            ex.GetType().BaseType == typeof(beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions.BERemoteGUIException))
        //    {
        //        ((beRemote.Core.ExceptionSystem.ExceptionBase.Exceptions.BERemoteGUIException)ex).ShowMessageWindow(null);

        //        // TODO implement exception type 

        //        Kernel.Shutdown();

        //        return ExceptionSystem.ExceptionBase.ExceptionType.BLOCKER;
        //    }

        //    return ExceptionSystem.ExceptionBase.ExceptionType.IGNORE;
        //}

        private static String _cachedBuildInfo = String.Empty;
        public static String GetBuildInformation()
        {
            if (_cachedBuildInfo == String.Empty)
            {
                _cachedBuildInfo = "no.build.info";
                if (File.Exists("build.dat"))
                {
                    TextReader tr = (TextReader)new StreamReader("build.dat");
                    String build_string = tr.ReadLine();
                    if (build_string != null || build_string != "")
                        _cachedBuildInfo = build_string;

                    tr.Close();
                    tr.Dispose();
                }
            }
            return _cachedBuildInfo;
        }

        /// <summary>
        /// Used to add debuggable objects from libs that do not have references to the debugger. USE WITH CARE!!!
        /// </summary>
        /// <param name="obj"></param>
        public static void AddDebugObject(String module, Object obj)
        {
            //GetDebugWorker().AddDebugObject(module, obj);
        }


        private static String _lastLoggedInUser = null;
        public static string GetLastLoggedInUser()
        {
            if (_lastLoggedInUser == null)
            {
                try
                {
                    if (System.IO.File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\beRemote\\localconfig.ini"))
                    {
                        System.IO.StreamReader sR = new System.IO.StreamReader(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\beRemote\\localconfig.ini");
                        while (sR.Peek() >= 0)
                        {
                            string line = sR.ReadLine();
                            string[] data = line.Split(new char[1] { '=' }, 2);
                            if (data[0] == "lastuser")
                            {
                                _lastLoggedInUser = data[1];
                                break;
                            }
                        }
                    }
                    else //First start
                    {
                        System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\beRemote");
                        _lastLoggedInUser = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
                    }
                }
                catch (Exception ea)
                {
                    System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\beRemote");
                    _lastLoggedInUser = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
                    Logger.Log(LogEntryType.Warning, "Cannot access " + System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\beRemote\\localconfig.ini", ea);
                    //throw new beRemote.Core.Exceptions.Kernel.KernelException(beRemote.Core.Exceptions.beRemoteExInfoPackage.MajorInformationPackage, "Problem reading last user", (int)KernelEventId.LastUserNotFound, ea);


                }
            }

            return _lastLoggedInUser;

        }

        public static void SetLastUser(string userName)
        {
            try
            {
                System.IO.StreamWriter sW = new System.IO.StreamWriter(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) +
                    "\\beRemote\\localconfig.ini");
                sW.WriteLine("lastuser=" + userName);
                sW.Close();
            }
            catch (Exception ex)
            {
                //throw new beRemote.Core.Exceptions.Kernel.KernelException(beRemote.Core.Exceptions.beRemoteExInfoPackage.MajorInformationPackage, "Problem reading last user", (int)KernelEventId.LastUserNotFound, ea);
                Logger.Log(LogEntryType.Warning, "Cannot access " + System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\beRemote\\localconfig.ini", ex);
            }
        }


        public static void TriggerNewConnection(long connectionsettingId)
        {
            try
            {
                if (OnNewConnectionTriggered != null)
                {
                    //long host_dbConnectionId = StorageCore.Core.GetConnectionSetting(connectionsettingId).getConnectionId();

                    //var host = Host.LoadConnection(host_dbConnectionId);

                    //TriggerNewConnection(host.NewSession(connectionsettingId));
                    OnNewConnectionTriggered(null, connectionsettingId);
                }
                else
                {
                    Logger.Warning("No connection trigger event is available. Try again later");
                }
            }
            catch (Exception ex)
            {
                throw new PluginException(beRemoteExInfoPackage.SignificantInformationPackage, String.Format("Connection with id {0} could not be opened since it does not exist", connectionsettingId));
            }

        }

        //public static void TriggerNewConnection(Session session)
        //{
        //    if (OnNewConnectionTriggered != null)
        //        OnNewConnectionTriggered(null, session);
        //    else
        //    {
        //        Logger.Warning("No connection trigger event is available. Try again later");
        //    }
        //}

        public static void InsertPluginUpdate(FileInfo pluginContainer)
        {
            if (false == pluginContainer.Exists)
                new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                    "Plugin container file not found",
                    new FileNotFoundException("Plugin container file not found", pluginContainer.FullName));

            var baseDirectory = new DirectoryInfo("plugins\\updates");

            if (false == baseDirectory.Exists)
                baseDirectory.Create();

            if (File.Exists(Path.Combine(baseDirectory.FullName, pluginContainer.Name)))
                File.Delete(Path.Combine(baseDirectory.FullName, pluginContainer.Name));

            pluginContainer.MoveTo(Path.Combine(baseDirectory.FullName, pluginContainer.Name));

            try
            {
                _mefLoaderInstance.ValidatePluginContainer(pluginContainer);
            }
            catch (Exception ex)
            {
                throw new beRemoteException(beRemoteExInfoPackage.SignificantInformationPackage,
                    "Plugin validation failed. This pluign is not valid. Please contact the support!", ex);

            }

        }


        /// <summary>
        /// Get UserProxy-Settings (Local or db-stored)
        /// </summary>
        /// <returns></returns>
        public static UserProxySetting GetUserProxySettings()
        {
            UserProxySetting ret = new UserProxySetting();

            string proxyConfigFilePath = System.Environment.SpecialFolder.ApplicationData + "\\beRemote\\proxy.cfg";
            if (File.Exists(proxyConfigFilePath))
            {
                string host = "";
                int port = 0;
                int credentials = 0;
                int flags = 0;

                StreamReader sR = new StreamReader(proxyConfigFilePath, Encoding.Default);
                while (sR.Peek() >= 0)
                {
                    try
                    {
                        string[] line = sR.ReadLine().Split(new char[1] { ':' }, 2);
                        switch (line[0])
                        {
                            case "proxyenabled":
                                if (line[1] == "0")
                                    return (null);
                                break;
                            case "proxyhost":
                                if (line.Length > 1)
                                    host = line[1];
                                break;
                            case "proxyport":
                                port = Convert.ToInt32(line[1]);
                                break;
                            case "proxycredentials":
                                credentials = Convert.ToInt32(line[1]);
                                break;
                            case "proxyflags":
                                flags = Convert.ToInt32(line[1]);
                                break;
                        }
                    }
                    catch (Exception ea)
                    {
                        Logger.Log(LogEntryType.Warning, "Error occured while reading local proxy-settings", ea);
                    }
                }

                //Get flags
                bool bypasslocal = false;
                bool useSystemSettings = false;
                if (flags >= 2)
                {
                    useSystemSettings = true;
                    flags -= 2;
                }
                if (flags == 1)
                {
                    bypasslocal = true;
                }

                if (useSystemSettings == true)
                {
                    //Use System Settings
                    ret.ConfiguredProxy = System.Net.WebProxy.GetDefaultProxy();
                    //wP = System.Net.WebRequest.GetSystemWebProxy(); //Is better, but currently not applicable

                    ret.UseSystemSettings = true;
                }
                else
                {
                    //Use Custom settings
                    UriBuilder wpub = new UriBuilder();

                    wpub.Host = host;
                    wpub.Port = port;

                    ret.ConfiguredProxy = new System.Net.WebProxy(wpub.Uri);
                    ret.ConfiguredProxy.BypassProxyOnLocal = bypasslocal;


                    if (credentials == -1)
                    {
                        ret.ConfiguredProxy.UseDefaultCredentials = true;
                    }
                    else if (credentials > 0)
                    {
                        var uC = StorageCore.Core.GetUserCredentials(StorageCore.Core.GetUserId());
                        ret.ConfiguredProxy.Credentials = new System.Net.NetworkCredential(
                            uC.Username,
                            Helper.DecryptStringFromBytes(
                                uC.Password,
                                Helper.GetHash1(StorageCore.Core.GetUserSalt1()),
                                Encoding.UTF8.GetBytes(StorageCore.Core.GetDatabaseGuid()),
                                StorageCore.Core.GetUserSalt3()),
                            uC.Domain);
                        ret.UserCredentialId = uC.Id;
                    }
                }

            }
            else
            {
                ret = StorageCore.Core.GetUserProxySettings();
            }

            return ret;
        }


        public static void RemovePlugin(string pluginIdentifier)
        {
            if (_mefLoaderInstance.AvailableProtocols.ContainsKey(pluginIdentifier))
            {
                RemovePlugin(_mefLoaderInstance.AvailableProtocols[pluginIdentifier]);

            }

            if (_mefLoaderInstance.AvailableUiPlugins.ContainsKey(pluginIdentifier))
            {
                RemovePlugin(_mefLoaderInstance.AvailableUiPlugins[pluginIdentifier]);
            }




        }

        private static void RemovePlugin(Plugin plugin)
        {
            var assemblyFile = new FileInfo(plugin.GetType().Assembly.Location);

            //var pluginDir = new DirectoryInfo(_baseDirectory);
            var trashDir = new DirectoryInfo("plugins\\trash");

            if (false == trashDir.Exists)
                trashDir.Create();

            assemblyFile.MoveTo(trashDir.FullName + "\\" + assemblyFile.Name);
        }


        //public static void CreateJob()
        //{

        //}
    }
}
