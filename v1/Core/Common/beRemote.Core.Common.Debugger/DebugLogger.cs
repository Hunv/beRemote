using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace beRemote.Core.Common.Debugger
{
    class DebugLogger : LogSystem.LogHandler.IHandlerBase
    {
        internal GUI.LogViewer view;
        private bool _initiated = false;
        private bool _enabled = true;
        public LogSystem.LogEntryType[] GetHandledTypes()
        {
            return new LogSystem.LogEntryType[] { LogSystem.LogEntryType.Verbose, LogSystem.LogEntryType.Debug, LogSystem.LogEntryType.Exception, LogSystem.LogEntryType.Info, LogSystem.LogEntryType.Warning };
        }

        public bool IsUiHandlerEnabled()
        {
            return _enabled;
        }

        public void SetUiHandlerState(Boolean state)
        {
            _enabled = state;
        }

        public void InitiateLogHandler(beRemote.Core.Common.SimpleSettings.IniFile configuration)
        {
            if (!_initiated)
            {
                view = new GUI.LogViewer();
                _initiated = true;
            }
        }

        public void SendEntry(LogSystem.LogEntry message)
        {
            if (IsUiHandlerEnabled())
            {
                view.WriteLine(message);
            }
        }

        public Type GetClassLock()
        {
            return typeof(DebugLogger);
        }

        #region IHandlerBase Member


        public void PauseLogger()
        {
         
        }

        public void UnpauseLogger()
        {
          
        }

        #endregion


        public void Stop()
        {
            
        }
    }
}
