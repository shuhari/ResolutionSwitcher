namespace ResolutionSwitcher.Gui
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTip = new System.Windows.Forms.Label();
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Resolution Switcher";
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += new System.EventHandler(this.trayIcon_DoubleClick);
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSettings,
            this.mnuSep2,
            this.mnuExit});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(132, 54);
            // 
            // mnuSettings
            // 
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(131, 22);
            this.mnuSettings.Text = "Settings...";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // mnuSep2
            // 
            this.mnuSep2.Name = "mnuSep2";
            this.mnuSep2.Size = new System.Drawing.Size(128, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(131, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // lblTip
            // 
            this.lblTip.BackColor = System.Drawing.Color.Transparent;
            this.lblTip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTip.Location = new System.Drawing.Point(0, 282);
            this.lblTip.Name = "lblTip";
            this.lblTip.Padding = new System.Windows.Forms.Padding(16);
            this.lblTip.Size = new System.Drawing.Size(584, 79);
            this.lblTip.TabIndex = 1;
            this.lblTip.Text = "Keyboard shortcuts:\r\n0: Reset to recommended screen mode\r\n1-9: Set to defined scr" +
    "een mode\r\n";
            this.lblTip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.lblTip);
            this.ForeColor = System.Drawing.Color.LightBlue;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "MainForm";
            this.Text = "Resolution Switcher";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private ToolStripMenuItem mnuSettings;
        private ToolStripSeparator mnuSep2;
        private ToolStripMenuItem mnuExit;
        private Label lblTip;
    }
}