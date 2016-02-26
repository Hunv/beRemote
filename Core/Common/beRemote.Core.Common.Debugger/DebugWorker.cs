using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Common.LogSystem;
using System.Windows.Forms;
namespace beRemote.Core.Common.Debugger
{
    public class DebugWorker
    {
        private Dictionary<Object, GUI.DBGView> _debugObjects;
        private GUI.DebugWindow _frmDebugWindow;
        internal DebugLogger dbgLogger;

        private static String loggerContext = "DebugWorker";
        public bool _autoStartLog = false;
        private static DebugWorker _instance;
        
        public static DebugWorker GetNewDebugWorker()
        {            
            return GetNewDebugWorker(false);
        }

        public static DebugWorker GetNewDebugWorker(bool autostartlivelog)
        {
            
            if (beRemote.Licensing.Decryption.Decryptor.IsLicensed("advanced.debugmode"))
            {
                if (_instance == null)
                    _instance = new DebugWorker();
                else
                    return _instance;

                _instance._autoStartLog = autostartlivelog;

                Logger.Log(LogEntryType.Info, "Initiating beRemote debugger", loggerContext);

                _instance._debugObjects = new Dictionary<object, GUI.DBGView>();
                _instance._frmDebugWindow = new GUI.DebugWindow();

                Logger.Log(LogEntryType.Debug, "Initiated beRemote debugger UI", loggerContext);
                _instance.dbgLogger = new DebugLogger();
                _instance.dbgLogger.InitiateLogHandler(null);
                _instance.dbgLogger.view.SetParent(_instance);

                _instance._frmDebugWindow.AddTabItem("Logging", _instance.dbgLogger.view);

                

                return _instance;
            }
            else
                return null;
        }

        

        public DebugWorker()
        {
           
            
        }

        public void StartUIThread()
        {

            using (_frmDebugWindow)
            {
                _frmDebugWindow.StartLogViewer();
                Application.Run(_frmDebugWindow);
            }
        }

        public void AddDebugObject(String contextName, Object dbgObject)
        {
            // need check if we are running debug mode 
            if (!_debugObjects.ContainsKey(dbgObject))
            {
                Logger.Log(LogEntryType.Info, String.Format("Adding {0} to beRemote debugger...", dbgObject), loggerContext);

                MethodInvoker invoker = delegate
                {
                    GUI.DBGView view = new GUI.DBGView(dbgObject);
                    _debugObjects.Add(dbgObject, view);
                    _frmDebugWindow.AddTabItem(contextName, view);
                };
                _frmDebugWindow.BeginInvoke(invoker);
            }
        }

        public void RemoveDebugObject(String contextName, Object dbgOject)
        {
            _frmDebugWindow.RemoveTabItem(contextName, _debugObjects[dbgOject]);
            _debugObjects.Remove(dbgOject);
        }

        public void StopDebugger()
        {
            MethodInvoker inv = delegate
            {
                _frmDebugWindow.ExitUI();
            };
            _frmDebugWindow.Invoke(inv);
        }

    }
}
