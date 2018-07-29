using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace beRemote.Core.Common.Debugger.GUI
{
    public partial class DebugWindow : Form
    {
        //private NotifyIcon _trayIcon;
        private ContextMenuStrip _cmsTrayIcon;
        private ToolStripButton _tsbShow;
        private ToolStripButton _tsbHide;

        private Dictionary<String, TabPage> _contexts = new Dictionary<string, TabPage>();

        private Dictionary<DBGView, TabPage> _tabPages = new Dictionary<DBGView, TabPage>();

        private Boolean _exit = false;

        public DebugWindow()
        {
            InitializeComponent();

            //_trayIcon = new NotifyIcon();

            this.Show();
            this.Hide();

            _cmsTrayIcon = new ContextMenuStrip();

            _tsbShow = new ToolStripButton("Show debugger");
            _tsbShow.Enabled = false;
            _tsbShow.Click += new EventHandler(tsbShow_Click);

            _tsbHide = new ToolStripButton("Hide debugger");
            _tsbHide.Enabled = true;
            _tsbHide.Click += new EventHandler(tsbHide_Click);

            _cmsTrayIcon.Items.Add(_tsbShow);
            _cmsTrayIcon.Items.Add(_tsbHide);

            this.FormClosing += new FormClosingEventHandler(DebugWindow_FormClosing);

            

            _trayIcon.ContextMenuStrip = _cmsTrayIcon;
            _trayIcon.Visible = true;

            _trayIcon.ShowBalloonTip(5000, "beRemote debugger", "beRemote debugger is running", ToolTipIcon.Info);

            
        }


        public void StartLogViewer()
        {
            if (DebugWorker.GetNewDebugWorker()._autoStartLog)
                DebugWorker.GetNewDebugWorker().dbgLogger.view.StartLogViewer();
        }
        public void HideTrayIcon()
        {
            _trayIcon.Visible = false;
        }

        public void ExitUI()
        {
            HideTrayIcon();
            _exit = true;
            this.Close();
        }

        private void AddContext(string p)
        {
            TabPage tpContext = new TabPage(p);
            TabControl tcContext = new TabControl();
            tcContext.Dock = DockStyle.Fill;
            tpContext.Controls.Add(tcContext);         

            _contexts.Add(p.ToLower(), tpContext);

            MethodInvoker inv = delegate
            {
                tcDebugObjects.TabPages.Add(tpContext);
            };
            this.Invoke(inv);
        }

        private TabControl GetContext(String p)
        {
            if (_contexts.ContainsKey(p.ToLower()))
                return (TabControl)_contexts[p.ToLower()].Controls[0];
            else
            {
                AddContext(p);
                return GetContext(p);
            }
        }

        void tsbHide_Click(object sender, EventArgs e)
        {
            _tsbShow.Enabled = true;
            _tsbHide.Enabled = false;

            this.Hide();
        }

        void tsbShow_Click(object sender, EventArgs e)
        {
            _tsbShow.Enabled = false;
            _tsbHide.Enabled = true;

            this.BringToFront();
            this.WindowState = FormWindowState.Maximized;

            this.Show();
        }

        void DebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_exit)
            {
                e.Cancel = true;

                _tsbShow.Enabled = true;
                _tsbHide.Enabled = false;

                this.Hide();
            }
        }

        //public void AddObjectToDebugView(Object dbgObject)
        //{

        //}

        internal void AddTabItem(String context, DBGView view)
        {

            TabControl tcContext = GetContext(context);

            String type = "Debugging";

            if (view._dbgObj != null)
                type = view._dbgObj.GetType().ToString();

            TabPage tp = new TabPage(type);
            tp.Controls.Add(view);

           
            MethodInvoker invoker = delegate
            {
                tcContext.TabPages.Add(tp);

                _tabPages.Add(view, tp);
            };
            this.Invoke(invoker);

            //tcDebugObjects.TabPages.Add(tp);
        }

        private void _trayIcon_Click(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                _tsbShow.Enabled = false;
                _tsbHide.Enabled = true;
            }
            else
            {
                _tsbShow.Enabled = true;
                _tsbHide.Enabled = false;
            }
        }

        internal void RemoveTabItem(string contextName, DBGView dbgView)
        {
            GetContext(contextName).TabPages.Remove(_tabPages[dbgView]);
            _tabPages.Remove(dbgView);
        }

        internal void AddTabItem(string context, LogViewer logViewer)
        {
            TabControl tcContext = GetContext(context);

            String type = "LogViewer";

           

            TabPage tp = new TabPage(type);
            tp.Controls.Add(logViewer);

            logViewer.Dock = DockStyle.Fill;

            tcContext.TabPages.Add(tp);

            //_tabPages.Add(logViewer, tp);
            

        }

    }
}
