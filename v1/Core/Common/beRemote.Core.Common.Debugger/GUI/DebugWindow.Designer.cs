namespace beRemote.Core.Common.Debugger.GUI
{
    partial class DebugWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugWindow));
            this.tcDebugObjects = new System.Windows.Forms.TabControl();
            this._trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // tcDebugObjects
            // 
            this.tcDebugObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcDebugObjects.Location = new System.Drawing.Point(0, 0);
            this.tcDebugObjects.Name = "tcDebugObjects";
            this.tcDebugObjects.SelectedIndex = 0;
            this.tcDebugObjects.Size = new System.Drawing.Size(1128, 622);
            this.tcDebugObjects.TabIndex = 0;
            // 
            // _trayIcon
            // 
            this._trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("_trayIcon.Icon")));
            this._trayIcon.Text = "beRemote Debugger";
            this._trayIcon.Visible = true;
            this._trayIcon.Click += new System.EventHandler(this._trayIcon_Click);
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 622);
            this.Controls.Add(this.tcDebugObjects);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "DebugWindow";
            this.Text = "DebugWindow";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcDebugObjects;
        public System.Windows.Forms.NotifyIcon _trayIcon;
    }
}