namespace ResolutionSwitcher.Gui
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.lsvModes = new System.Windows.Forms.ListView();
            this.colResolution = new System.Windows.Forms.ColumnHeader();
            this.colOrientation = new System.Windows.Forms.ColumnHeader();
            this.colScale = new System.Windows.Forms.ColumnHeader();
            this.label1 = new System.Windows.Forms.Label();
            this.cboResolution = new System.Windows.Forms.ComboBox();
            this.cboOrientation = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboScale = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblRecommended = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lsvModes
            // 
            this.lsvModes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colResolution,
            this.colOrientation,
            this.colScale});
            this.lsvModes.FullRowSelect = true;
            this.lsvModes.GridLines = true;
            this.lsvModes.Location = new System.Drawing.Point(190, 90);
            this.lsvModes.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.lsvModes.Name = "lsvModes";
            this.lsvModes.Size = new System.Drawing.Size(813, 783);
            this.lsvModes.TabIndex = 0;
            this.lsvModes.UseCompatibleStateImageBehavior = false;
            this.lsvModes.View = System.Windows.Forms.View.Details;
            // 
            // colResolution
            // 
            this.colResolution.Text = "Resolution";
            this.colResolution.Width = 240;
            // 
            // colOrientation
            // 
            this.colOrientation.Text = "Orientation";
            this.colOrientation.Width = 120;
            // 
            // colScale
            // 
            this.colScale.Text = "Scale";
            this.colScale.Width = 120;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1029, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Resolution:";
            // 
            // cboResolution
            // 
            this.cboResolution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboResolution.FormattingEnabled = true;
            this.cboResolution.Location = new System.Drawing.Point(1029, 51);
            this.cboResolution.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cboResolution.Name = "cboResolution";
            this.cboResolution.Size = new System.Drawing.Size(299, 32);
            this.cboResolution.TabIndex = 2;
            // 
            // cboOrientation
            // 
            this.cboOrientation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrientation.FormattingEnabled = true;
            this.cboOrientation.Location = new System.Drawing.Point(1029, 123);
            this.cboOrientation.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cboOrientation.Name = "cboOrientation";
            this.cboOrientation.Size = new System.Drawing.Size(299, 32);
            this.cboOrientation.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1029, 90);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Orientation:";
            // 
            // cboScale
            // 
            this.cboScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScale.FormattingEnabled = true;
            this.cboScale.Location = new System.Drawing.Point(1029, 195);
            this.cboScale.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cboScale.Name = "cboScale";
            this.cboScale.Size = new System.Drawing.Size(299, 32);
            this.cboScale.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1029, 162);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Scale";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(11, 13);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 30);
            this.label4.TabIndex = 7;
            this.label4.Text = "Recommended:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(25, 48);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 24);
            this.label5.TabIndex = 8;
            this.label5.Text = "Current:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(25, 90);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(156, 24);
            this.label6.TabIndex = 9;
            this.label6.Text = "Available:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRecommended
            // 
            this.lblRecommended.AutoSize = true;
            this.lblRecommended.Location = new System.Drawing.Point(190, 13);
            this.lblRecommended.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblRecommended.Name = "lblRecommended";
            this.lblRecommended.Size = new System.Drawing.Size(0, 24);
            this.lblRecommended.TabIndex = 10;
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Location = new System.Drawing.Point(190, 48);
            this.lblCurrent.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(0, 24);
            this.lblCurrent.TabIndex = 11;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(1031, 253);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(300, 41);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(1029, 316);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(300, 41);
            this.btnRemove.TabIndex = 13;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(1031, 440);
            this.btnMoveUp.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(300, 41);
            this.btnMoveUp.TabIndex = 14;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(1031, 503);
            this.btnMoveDown.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(300, 41);
            this.btnMoveDown.TabIndex = 15;
            this.btnMoveDown.Text = "MoveDown";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1031, 378);
            this.btnClear.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(300, 41);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 887);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.lblRecommended);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboScale);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboOrientation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboResolution);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lsvModes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Activated += new System.EventHandler(this.SettingsForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView lsvModes;
        private Label label1;
        private ComboBox cboResolution;
        private ComboBox cboOrientation;
        private Label label2;
        private ComboBox cboScale;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label lblRecommended;
        private Label lblCurrent;
        private ColumnHeader colResolution;
        private ColumnHeader colOrientation;
        private ColumnHeader colScale;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnClear;
    }
}