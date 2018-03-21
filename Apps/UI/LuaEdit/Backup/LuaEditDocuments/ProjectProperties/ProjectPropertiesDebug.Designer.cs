namespace LuaEdit.Documents.ProjectProperties
{
    partial class ProjectPropertiesDebug
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
            this.txtRemoteMachine = new System.Windows.Forms.TextBox();
            this.btnBrowseWorkingDir = new System.Windows.Forms.Button();
            this.txtWorkingDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboStartupFile = new System.Windows.Forms.ComboBox();
            this.chkRemoteMachine = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.folderBrowserWorkingDir = new System.Windows.Forms.FolderBrowserDialog();
            this.radStartProject = new System.Windows.Forms.RadioButton();
            this.radStartExternalProgram = new System.Windows.Forms.RadioButton();
            this.txtExternalProgram = new System.Windows.Forms.TextBox();
            this.btnBrowseExternalProgram = new System.Windows.Forms.Button();
            this.externalProgramBrowser = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCmdLineArgs = new System.Windows.Forms.TextBox();
            this.luaEditHeaderSeparator2 = new LuaEdit.Controls.LuaEditHeaderSeparator();
            this.luaEditHeaderSeparator1 = new LuaEdit.Controls.LuaEditHeaderSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRemoteMachine
            // 
            this.txtRemoteMachine.Enabled = false;
            this.txtRemoteMachine.Location = new System.Drawing.Point(161, 183);
            this.txtRemoteMachine.Name = "txtRemoteMachine";
            this.txtRemoteMachine.Size = new System.Drawing.Size(253, 20);
            this.txtRemoteMachine.TabIndex = 9;
            this.txtRemoteMachine.Leave += new System.EventHandler(this.txtRemoteMachine_Leave);
            this.txtRemoteMachine.Enter += new System.EventHandler(this.txtRemoteMachine_Enter);
            // 
            // btnBrowseWorkingDir
            // 
            this.btnBrowseWorkingDir.Location = new System.Drawing.Point(420, 156);
            this.btnBrowseWorkingDir.Name = "btnBrowseWorkingDir";
            this.btnBrowseWorkingDir.Size = new System.Drawing.Size(27, 20);
            this.btnBrowseWorkingDir.TabIndex = 7;
            this.btnBrowseWorkingDir.Text = "...";
            this.btnBrowseWorkingDir.UseVisualStyleBackColor = true;
            this.btnBrowseWorkingDir.Click += new System.EventHandler(this.btnBrowseWorkingDir_Click);
            // 
            // txtWorkingDir
            // 
            this.txtWorkingDir.Location = new System.Drawing.Point(161, 156);
            this.txtWorkingDir.Name = "txtWorkingDir";
            this.txtWorkingDir.Size = new System.Drawing.Size(253, 20);
            this.txtWorkingDir.TabIndex = 6;
            this.txtWorkingDir.Leave += new System.EventHandler(this.txtWorkingDir_Leave);
            this.txtWorkingDir.Enter += new System.EventHandler(this.txtWorkingDir_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Working Directory:";
            // 
            // cboStartupFile
            // 
            this.cboStartupFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartupFile.FormattingEnabled = true;
            this.cboStartupFile.Location = new System.Drawing.Point(161, 35);
            this.cboStartupFile.Name = "cboStartupFile";
            this.cboStartupFile.Size = new System.Drawing.Size(253, 21);
            this.cboStartupFile.TabIndex = 1;
            this.cboStartupFile.SelectedValueChanged += new System.EventHandler(this.cboStartupFile_SelectedValueChanged);
            // 
            // chkRemoteMachine
            // 
            this.chkRemoteMachine.AutoSize = true;
            this.chkRemoteMachine.Location = new System.Drawing.Point(26, 185);
            this.chkRemoteMachine.Name = "chkRemoteMachine";
            this.chkRemoteMachine.Size = new System.Drawing.Size(129, 17);
            this.chkRemoteMachine.TabIndex = 8;
            this.chkRemoteMachine.Text = "Use Remote Machine";
            this.chkRemoteMachine.UseVisualStyleBackColor = true;
            this.chkRemoteMachine.CheckedChanged += new System.EventHandler(this.chkRemoteMachine_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Port:";
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(161, 209);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(120, 20);
            this.nudPort.TabIndex = 10;
            this.nudPort.Leave += new System.EventHandler(this.nudPort_Leave);
            this.nudPort.Enter += new System.EventHandler(this.nudPort_Enter);
            // 
            // folderBrowserWorkingDir
            // 
            this.folderBrowserWorkingDir.Description = "Select the project\'s working directory";
            this.folderBrowserWorkingDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // radStartProject
            // 
            this.radStartProject.AutoSize = true;
            this.radStartProject.Location = new System.Drawing.Point(26, 36);
            this.radStartProject.Name = "radStartProject";
            this.radStartProject.Size = new System.Drawing.Size(82, 17);
            this.radStartProject.TabIndex = 0;
            this.radStartProject.Text = "Start project";
            this.radStartProject.UseVisualStyleBackColor = true;
            this.radStartProject.CheckedChanged += new System.EventHandler(this.radStartProject_CheckedChanged);
            // 
            // radStartExternalProgram
            // 
            this.radStartExternalProgram.AutoSize = true;
            this.radStartExternalProgram.Location = new System.Drawing.Point(26, 62);
            this.radStartExternalProgram.Name = "radStartExternalProgram";
            this.radStartExternalProgram.Size = new System.Drawing.Size(128, 17);
            this.radStartExternalProgram.TabIndex = 2;
            this.radStartExternalProgram.Text = "Start external program";
            this.radStartExternalProgram.UseVisualStyleBackColor = true;
            this.radStartExternalProgram.CheckedChanged += new System.EventHandler(this.radStartExternalProgram_CheckedChanged);
            // 
            // txtExternalProgram
            // 
            this.txtExternalProgram.Enabled = false;
            this.txtExternalProgram.Location = new System.Drawing.Point(161, 62);
            this.txtExternalProgram.Name = "txtExternalProgram";
            this.txtExternalProgram.Size = new System.Drawing.Size(253, 20);
            this.txtExternalProgram.TabIndex = 3;
            this.txtExternalProgram.Leave += new System.EventHandler(this.txtExternalProgram_Leave);
            this.txtExternalProgram.Enter += new System.EventHandler(this.txtExternalProgram_Enter);
            // 
            // btnBrowseExternalProgram
            // 
            this.btnBrowseExternalProgram.Location = new System.Drawing.Point(420, 63);
            this.btnBrowseExternalProgram.Name = "btnBrowseExternalProgram";
            this.btnBrowseExternalProgram.Size = new System.Drawing.Size(27, 20);
            this.btnBrowseExternalProgram.TabIndex = 4;
            this.btnBrowseExternalProgram.Text = "...";
            this.btnBrowseExternalProgram.UseVisualStyleBackColor = true;
            this.btnBrowseExternalProgram.Click += new System.EventHandler(this.btnBrowseExternalProgram_Click);
            // 
            // externalProgramBrowser
            // 
            this.externalProgramBrowser.DefaultExt = "exe";
            this.externalProgramBrowser.Filter = "All Executable Files (*.exe)|*.exe";
            this.externalProgramBrowser.RestoreDirectory = true;
            this.externalProgramBrowser.Title = "Browse External Program";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Command line arguments:";
            // 
            // txtCmdLineArgs
            // 
            this.txtCmdLineArgs.Location = new System.Drawing.Point(161, 130);
            this.txtCmdLineArgs.Name = "txtCmdLineArgs";
            this.txtCmdLineArgs.Size = new System.Drawing.Size(253, 20);
            this.txtCmdLineArgs.TabIndex = 5;
            this.txtCmdLineArgs.Leave += new System.EventHandler(this.txtCmdLineArgs_Leave);
            this.txtCmdLineArgs.Enter += new System.EventHandler(this.txtCmdLineArgs_Enter);
            // 
            // luaEditHeaderSeparator2
            // 
            this.luaEditHeaderSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.luaEditHeaderSeparator2.Location = new System.Drawing.Point(9, 8);
            this.luaEditHeaderSeparator2.Name = "luaEditHeaderSeparator2";
            this.luaEditHeaderSeparator2.SeparatorColor = System.Drawing.SystemColors.ControlDark;
            this.luaEditHeaderSeparator2.Size = new System.Drawing.Size(435, 22);
            this.luaEditHeaderSeparator2.TabIndex = 12;
            this.luaEditHeaderSeparator2.Title = "Start Action";
            // 
            // luaEditHeaderSeparator1
            // 
            this.luaEditHeaderSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.luaEditHeaderSeparator1.Location = new System.Drawing.Point(9, 97);
            this.luaEditHeaderSeparator1.Name = "luaEditHeaderSeparator1";
            this.luaEditHeaderSeparator1.SeparatorColor = System.Drawing.SystemColors.ControlDark;
            this.luaEditHeaderSeparator1.Size = new System.Drawing.Size(435, 22);
            this.luaEditHeaderSeparator1.TabIndex = 11;
            this.luaEditHeaderSeparator1.Title = "Start Options";
            // 
            // ProjectPropertiesDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.txtCmdLineArgs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowseExternalProgram);
            this.Controls.Add(this.txtExternalProgram);
            this.Controls.Add(this.radStartExternalProgram);
            this.Controls.Add(this.radStartProject);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkRemoteMachine);
            this.Controls.Add(this.luaEditHeaderSeparator2);
            this.Controls.Add(this.luaEditHeaderSeparator1);
            this.Controls.Add(this.btnBrowseWorkingDir);
            this.Controls.Add(this.txtWorkingDir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboStartupFile);
            this.Controls.Add(this.txtRemoteMachine);
            this.Name = "ProjectPropertiesDebug";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(450, 270);
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRemoteMachine;
        private System.Windows.Forms.Button btnBrowseWorkingDir;
        private System.Windows.Forms.TextBox txtWorkingDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboStartupFile;
        private LuaEdit.Controls.LuaEditHeaderSeparator luaEditHeaderSeparator1;
        private LuaEdit.Controls.LuaEditHeaderSeparator luaEditHeaderSeparator2;
        private System.Windows.Forms.CheckBox chkRemoteMachine;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserWorkingDir;
        private System.Windows.Forms.RadioButton radStartProject;
        private System.Windows.Forms.RadioButton radStartExternalProgram;
        private System.Windows.Forms.TextBox txtExternalProgram;
        private System.Windows.Forms.Button btnBrowseExternalProgram;
        private System.Windows.Forms.OpenFileDialog externalProgramBrowser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCmdLineArgs;
    }
}