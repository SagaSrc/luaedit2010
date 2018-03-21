namespace LuaEdit
{
    partial class AboutScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutScreen));
            this.btnClose = new System.Windows.Forms.Button();
            this.lblAssemblyTitle = new System.Windows.Forms.Label();
            this.lblAssemblyVersion = new System.Windows.Forms.Label();
            this.lblAssemblyCopyright = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(460, 368);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblAssemblyTitle
            // 
            this.lblAssemblyTitle.AutoSize = true;
            this.lblAssemblyTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblAssemblyTitle.Location = new System.Drawing.Point(12, 154);
            this.lblAssemblyTitle.Name = "lblAssemblyTitle";
            this.lblAssemblyTitle.Size = new System.Drawing.Size(71, 13);
            this.lblAssemblyTitle.TabIndex = 1;
            this.lblAssemblyTitle.Text = "AssemblyTitle";
            // 
            // lblAssemblyVersion
            // 
            this.lblAssemblyVersion.AutoSize = true;
            this.lblAssemblyVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblAssemblyVersion.Location = new System.Drawing.Point(12, 167);
            this.lblAssemblyVersion.Name = "lblAssemblyVersion";
            this.lblAssemblyVersion.Size = new System.Drawing.Size(86, 13);
            this.lblAssemblyVersion.TabIndex = 2;
            this.lblAssemblyVersion.Text = "AssemblyVersion";
            // 
            // lblAssemblyCopyright
            // 
            this.lblAssemblyCopyright.AutoSize = true;
            this.lblAssemblyCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblAssemblyCopyright.Location = new System.Drawing.Point(12, 180);
            this.lblAssemblyCopyright.Name = "lblAssemblyCopyright";
            this.lblAssemblyCopyright.Size = new System.Drawing.Size(95, 13);
            this.lblAssemblyCopyright.TabIndex = 3;
            this.lblAssemblyCopyright.Text = "AssemblyCopyright";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 193);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "All rights reserved.";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(15, 215);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(520, 147);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // AboutScreen
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(544, 400);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAssemblyCopyright);
            this.Controls.Add(this.lblAssemblyVersion);
            this.Controls.Add(this.lblAssemblyTitle);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About LuaEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblAssemblyTitle;
        private System.Windows.Forms.Label lblAssemblyVersion;
        private System.Windows.Forms.Label lblAssemblyCopyright;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;

    }
}