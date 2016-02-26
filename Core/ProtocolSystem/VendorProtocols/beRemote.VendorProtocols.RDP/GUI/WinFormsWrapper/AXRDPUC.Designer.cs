namespace beRemote.VendorProtocols.RDP.GUI.WinFormsWrapper
{
    partial class AXRDPUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AXRDPUC));
            this.rdpControl = new AxMSTSCLib.AxMsTscAxNotSafeForScripting();
            ((System.ComponentModel.ISupportInitialize)(this.rdpControl)).BeginInit();
            this.SuspendLayout();
            // 
            // rdpControl
            // 
            this.rdpControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdpControl.Enabled = true;
            this.rdpControl.Location = new System.Drawing.Point(0, 0);
            this.rdpControl.Name = "rdpControl";
            this.rdpControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("rdpControl.OcxState")));
            this.rdpControl.Size = new System.Drawing.Size(842, 420);
            this.rdpControl.TabIndex = 0;
            // 
            // AXRDPUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.rdpControl);
            this.Name = "AXRDPUC";
            this.Size = new System.Drawing.Size(842, 420);
            ((System.ComponentModel.ISupportInitialize)(this.rdpControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public AxMSTSCLib.AxMsTscAxNotSafeForScripting rdpControl;

    }
}
