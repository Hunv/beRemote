namespace beRemote.Core.Common.Debugger.GUI
{
    partial class LogViewer
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lvLogEntries = new System.Windows.Forms.ListView();
            this.chDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chContext = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.logEntryContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopyMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdStopLogViewer = new System.Windows.Forms.Button();
            this.cmdStartLogViewer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdContexts = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdClearView = new System.Windows.Forms.Button();
            this.logEntryContext.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvLogEntries
            // 
            this.lvLogEntries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chDateTime,
            this.chType,
            this.chContext,
            this.chMessage});
            this.lvLogEntries.ContextMenuStrip = this.logEntryContext;
            this.lvLogEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLogEntries.FullRowSelect = true;
            this.lvLogEntries.Location = new System.Drawing.Point(3, 16);
            this.lvLogEntries.Name = "lvLogEntries";
            this.lvLogEntries.Size = new System.Drawing.Size(901, 473);
            this.lvLogEntries.TabIndex = 0;
            this.lvLogEntries.UseCompatibleStateImageBehavior = false;
            this.lvLogEntries.View = System.Windows.Forms.View.Details;
            // 
            // chDateTime
            // 
            this.chDateTime.Text = "Time";
            this.chDateTime.Width = 126;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 86;
            // 
            // chContext
            // 
            this.chContext.Text = "Context";
            this.chContext.Width = 115;
            // 
            // chMessage
            // 
            this.chMessage.Text = "Text";
            this.chMessage.Width = 575;
            // 
            // logEntryContext
            // 
            this.logEntryContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyMessages});
            this.logEntryContext.Name = "logEntryContext";
            this.logEntryContext.Size = new System.Drawing.Size(165, 26);
            // 
            // tsmiCopyMessages
            // 
            this.tsmiCopyMessages.Image = global::beRemote.Core.Common.Debugger.Properties.Resources.floppy64;
            this.tsmiCopyMessages.Name = "tsmiCopyMessages";
            this.tsmiCopyMessages.Size = new System.Drawing.Size(164, 22);
            this.tsmiCopyMessages.Text = "Copy message(s)";
            this.tsmiCopyMessages.Click += new System.EventHandler(this.tsmiCopyMessages_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdClearView);
            this.groupBox1.Controls.Add(this.cmdStopLogViewer);
            this.groupBox1.Controls.Add(this.cmdStartLogViewer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmdContexts);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(907, 46);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log view settings";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cmdStopLogViewer
            // 
            this.cmdStopLogViewer.Location = new System.Drawing.Point(119, 16);
            this.cmdStopLogViewer.Name = "cmdStopLogViewer";
            this.cmdStopLogViewer.Size = new System.Drawing.Size(100, 21);
            this.cmdStopLogViewer.TabIndex = 7;
            this.cmdStopLogViewer.Text = "Stop log viewer";
            this.cmdStopLogViewer.UseVisualStyleBackColor = true;
            this.cmdStopLogViewer.Click += new System.EventHandler(this.cmdStopLogViewer_Click);
            // 
            // cmdStartLogViewer
            // 
            this.cmdStartLogViewer.Location = new System.Drawing.Point(13, 16);
            this.cmdStartLogViewer.Name = "cmdStartLogViewer";
            this.cmdStartLogViewer.Size = new System.Drawing.Size(100, 21);
            this.cmdStartLogViewer.TabIndex = 6;
            this.cmdStartLogViewer.Text = "Start log viewer";
            this.cmdStartLogViewer.UseVisualStyleBackColor = true;
            this.cmdStartLogViewer.Click += new System.EventHandler(this.cmdStartLogViewer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(572, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Context";
            // 
            // cmdContexts
            // 
            this.cmdContexts.Enabled = false;
            this.cmdContexts.FormattingEnabled = true;
            this.cmdContexts.Location = new System.Drawing.Point(621, 17);
            this.cmdContexts.Name = "cmdContexts";
            this.cmdContexts.Size = new System.Drawing.Size(280, 21);
            this.cmdContexts.TabIndex = 4;
            this.cmdContexts.SelectedIndexChanged += new System.EventHandler(this.cmdContexts_SelectedIndexChanged);
            this.cmdContexts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cmdContexts_MouseDown);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvLogEntries);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(0, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(907, 492);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // cmdClearView
            // 
            this.cmdClearView.Location = new System.Drawing.Point(225, 16);
            this.cmdClearView.Name = "cmdClearView";
            this.cmdClearView.Size = new System.Drawing.Size(100, 21);
            this.cmdClearView.TabIndex = 8;
            this.cmdClearView.Text = "Clear entries";
            this.cmdClearView.UseVisualStyleBackColor = true;
            this.cmdClearView.Click += new System.EventHandler(this.cmdClearView_Click);
            // 
            // LogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "LogViewer";
            this.Size = new System.Drawing.Size(907, 538);
            this.logEntryContext.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvLogEntries;
        private System.Windows.Forms.ColumnHeader chDateTime;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chContext;
        private System.Windows.Forms.ColumnHeader chMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmdContexts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdStartLogViewer;
        private System.Windows.Forms.Button cmdStopLogViewer;
        private System.Windows.Forms.ContextMenuStrip logEntryContext;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyMessages;
        private System.Windows.Forms.Button cmdClearView;
    }
}
