using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using beRemote.Core.Common.SimpleSettings;
using System.IO;

namespace beRemote.Core.Common.LogSystem.LogHandler
{
    public class LogfileLogger : IHandlerBase
    {
        private bool _rotate = true;
        private bool _rotateonrestart = true;
        private int _maxlines = 500;
        private int _keeplogcount = 2;

        private bool _pause = false;

        private TextWriter _writer;
        private TextReader _reader;

        private int _currentlinecount = 0;
        private string _filename = "beRemote.application.log";
        private string _directory = @"logs\";

        private FileInfo[] _logFiles;

        private List<LogEntry> _pausedQueue;

        public void SendEntry(LogEntry message)
        {
            if (!_pause)
            {
                try
                {
                    if (_writer == null)
                    {
                        _writer = (TextWriter)new StreamWriter(_logFiles[0].FullName, true);
                    }

                    lock (_writer)
                    {
                        _writer.WriteLine(message.Timestamp.ToString("HH:mm:ss.ffffff") + " : " + message.ToString());
                        _writer.Flush();                        
                    }
                }
                catch { }
            }
            else
            {
                if (_pausedQueue == null)
                    _pausedQueue = new List<LogEntry>();

                _pausedQueue.Add(message);
            }
        }

        /// <summary>
        /// Not yet implemented
        /// </summary>
        /// <returns>-</returns>
        public List<LogEntry> GetEntries()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public LogEntryType[] GetHandledTypes()
        {
            return new LogEntryType[] { LogEntryType.Verbose, LogEntryType.Debug, LogEntryType.Info, LogEntryType.Warning, LogEntryType.Exception };
        }


        public void InitiateLogHandler(IniFile configuration)
        {
            try { _rotate = Convert.ToBoolean(configuration.GetValue("logging.logfilelogger", "lfl.rotate")); } catch{}
            try { _rotateonrestart = Convert.ToBoolean(configuration.GetValue("logging.logfilelogger", "lfl.rotateonrestart")); } catch{}
            try { _keeplogcount = Convert.ToInt32(configuration.GetValue("logging.logfilelogger", "lfl.keeplogcount")); } catch{}
            try { _filename = configuration.GetValue("logging.logfilelogger", "lfl.logfilename"); } catch{}
            try { _directory = configuration.GetValue("logging.logfilelogger", "lfl.logpath"); } catch { }
            
            if(_rotate)
                RotateLogFile();
        }

        private void RotateLogFile()
        {
            
            _logFiles = new FileInfo[_keeplogcount +1];
            _logFiles[0] = new FileInfo(_directory + _filename + ".0");
            DirectoryInfo currentLogDir = new DirectoryInfo(_directory);

            if (!currentLogDir.Exists)
            {
                currentLogDir.Create();                
                

                for (int i = 0; i < _keeplogcount; i++)
                {
                    if (i != 0)
                    {
                        _logFiles[i] = null;
                    }
                }
            }
            else
            {
                for (int i = 0; i <= _keeplogcount; i++)
                {
                    FileInfo logFile = new FileInfo(_directory + _filename + "." + (_keeplogcount - i).ToString());
                    if(logFile.Exists)
                    {
                        if ((_keeplogcount - i) == _keeplogcount)
                        {
                            logFile.Delete();
                        }
                        else
                        {
                            String currentFileNoStr = logFile.Name.Substring(logFile.Name.Length - 1, 1);
                            int currentFileNo = 0;
                            if (Int32.TryParse(currentFileNoStr, out currentFileNo))
                            {
                                String newFileName = _directory + _filename + "." + (currentFileNo + 1).ToString();
                                logFile.MoveTo(newFileName);
                                _logFiles[(currentFileNo + 1)] = logFile;
                            }

                            
                        }
                    }
                }
            }

        }


        public Type GetClassLock()
        {
            return typeof(LogfileLogger);
        }

        #region IHandlerBase Member
        
        public void Stop()
        {
            if (_writer != null)
            {
                _pause = true;
                _writer.Flush();
                _writer.Close();
                _writer.Dispose();
                _writer = null;
            }
        }

        public void PauseLogger()
        {
           
                SendEntry(new LogEntry(LogEntryType.Info, "Pausing logfile logger...", "LogSystem"));
                _pause = true;

                if (_writer != null)
                {
                    _writer.Flush();
                    _writer.Close();
                    _writer = null;
                }

                _pausedQueue = new List<LogEntry>();
                
            

        }

        #endregion

        #region IHandlerBase Member


        public void UnpauseLogger()
        {
            SendEntry(new LogEntry(LogEntryType.Info, "Restarting logfilelogger", "LogSystem"));
            _pause = false;

            foreach (LogEntry entry in _pausedQueue)
            {
                SendEntry(entry);
            }
            _pausedQueue = null;
        }

        #endregion


        
    }
}
