using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using beRemote.Core.Common.LogSystem;
using System.Threading;

namespace beRemote.Core.Common.Debugger.GUI
{
    public partial class LogViewer : UserControl
    {
        List<LogEntry> entries = new List<LogEntry>();

        DebugWorker _parent;

        public LogViewer()
        {
            InitializeComponent();

            
        }

        public void SetParent(DebugWorker parent)
        {
            _parent = parent;
        }

        public void WriteLine(LogEntry message)
        {
            entries.Add(message);

            try
            {
                WriteToView(message);
            }
            catch 
            { 
                // nothing to do since there are only exceptions if we are currently working on the LV
            }
        }

        private void WriteToView(LogEntry message)
        {
            ListViewItem lvi = null;
            lvi = GenerateItem(message);

            if (lvi != null)
            {
                MethodInvoker inv = delegate 
                { 
                    lvLogEntries.Items.Add(lvi);
                    lvLogEntries.EnsureVisible(lvi.Index);
                };
                this.Invoke(inv);
            }
        }

        private ListViewItem GenerateItem(LogEntry message)
        {
            ListViewItem returnItem = new ListViewItem();

            returnItem.Tag = message;

            ListViewItem.ListViewSubItem lvsiType = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem lvsiContext = new ListViewItem.ListViewSubItem();
            ListViewItem.ListViewSubItem lvsiMessage = new ListViewItem.ListViewSubItem();

            returnItem.Text = message.GetTimestamp().ToString();
            lvsiType.Text = message.GetEntryType().ToString();
            lvsiContext.Text = message.GetContext();
            lvsiMessage.Text = message.GetMessage();

            returnItem.SubItems.Add(lvsiType);
            returnItem.SubItems.Add(lvsiContext);
            returnItem.SubItems.Add(lvsiMessage);

            return returnItem;
        }

        private void cmdContexts_MouseDown(object sender, MouseEventArgs e)
        {
            cmdContexts.Items.Clear();

            foreach (String s in Logger.GetLogContexts())
            {
                cmdContexts.Items.Add(s);
            }
        }

        private void cmdContexts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //foreach (ListViewItem lvi in lvLogEntries.Items)
            //{
            //    if(lvi.SubItems[1].Text != cmdContexts.Text)
                    

            //}
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

    

        private void cmdStartLogViewer_Click(object sender, EventArgs e)
        {
            StartLogViewer();
            
        }

        public void StartLogViewer()
        {
            cmdStartLogViewer.Enabled = false;
            cmdStopLogViewer.Enabled = true;

            cmdContexts.Enabled = true;
            groupBox2.Enabled = true;
            cmdStopLogViewer.Enabled = true;

            Logger.AddLogger(this._parent.dbgLogger);

            Logger.Log(LogEntryType.Info, "Log viewer in debugger is now enabled and added to the Log handler system", "Debugger");

            cmdContexts.Items.Clear();

            foreach (String s in Logger.GetLogContexts())
            {
                cmdContexts.Items.Add(s);
            }
        }

        private void cmdGetAllEntries_Click(object sender, EventArgs e)
        {
            //ReloadLogView();
        }

        //private void ReloadLogView()
        //{
        //    Logger.Log(LogEntryType.Info, "Stopping log viewer to get all available entries from the begining");

        //    this._parent.dbgLogger.SetUiHandlerState(false);

        //    lvLogEntries.Items.Clear();
        //    LogEntryType[] types = GetLoggedTypes();
        //    lock(types)
        //    {
        //        List<LogEntry> entries =Logger.GetLoggedEntries(types);
        //        lock (entries)
        //        {
        //            foreach (LogEntry le in entries)
        //            {
        //                WriteToView(le);
        //            }
        //        }
        //    }
        //    Logger.Log(LogEntryType.Info, "Starting log viewer");

        //    this._parent.dbgLogger.SetUiHandlerState(true);
        //}

        private LogEntryType[] GetLoggedTypes()
        {
            List<LogEntryType> list = new List<LogEntryType>();

            return list.ToArray();
        }

        private void cvLogLevel_CheckedChanged(object sender, EventArgs e)
        {
            //ReloadLogView();
        }

        private void cmdStopLogViewer_Click(object sender, EventArgs e)
        {
            Logger.RemoveLogger(this._parent.dbgLogger);

            cmdStopLogViewer.Enabled = false;
            cmdStartLogViewer.Enabled = true;
        }

        private void tsmiCopyMessages_Click(object sender, EventArgs e)
        {
            Logger.Log(LogEntryType.Info, "Copying content of selected log entries to the clipboard", "Debugger");
            Logger.PauseLogger();

            lock (lvLogEntries)
            {
                String copied_messages = "";

                foreach (ListViewItem lvi in lvLogEntries.SelectedItems)
                {                   
                    LogEntry message = (LogEntry)lvi.Tag;

                    copied_messages += message.Timestamp.ToString("HH:mm:ss.ffffff") + " : " + message.ToString() + "\r\n";
                }

                System.Windows.Forms.Clipboard.SetText(copied_messages);
            }

            Logger.UnpauseLogger();
        }

        private void cmdClearView_Click(object sender, EventArgs e)
        {
            lock (lvLogEntries)
            {
                lvLogEntries.Items.Clear();
            }
        }



   

    }
}

