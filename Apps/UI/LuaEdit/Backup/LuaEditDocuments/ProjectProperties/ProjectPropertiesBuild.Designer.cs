namespace LuaEdit.Documents.ProjectProperties
{
    partial class ProjectPropertiesBuild
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
            this.lblProjectName = new System.Windows.Forms.Label();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.luaEditHeaderSeparator2 = new LuaEdit.Controls.LuaEditHeaderSeparator();
            this.btnBrowseOutputDir = new System.Windows.Forms.Button();
            this.folderBrowserOutputDir = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Location = new System.Drawing.Point(26, 37);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(85, 13);
            this.lblProjectName.TabIndex = 0;
            this.lblProjectName.Text = "Output directory:";
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Location = new System.Drawing.Point(161, 34);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(251, 20);
            this.txtOutputDir.TabIndex = 1;
            this.txtOutputDir.Leave += new System.EventHandler(this.txtOutputDir_Leave);
            this.txtOutputDir.Enter += new System.EventHandler(this.txtOutputDir_Enter);
            // 
            // luaEditHeaderSeparator2
            // 
            this.luaEditHeaderSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.luaEditHeaderSeparator2.Location = new System.Drawing.Point(10, 6);
            this.luaEditHeaderSeparator2.Name = "luaEditHeaderSeparator2";
            this.luaEditHeaderSeparator2.SeparatorColor = System.Drawing.SystemColors.ControlDark;
            this.luaEditHeaderSeparator2.Size = new System.Drawing.Size(435, 22);
            this.luaEditHeaderSeparator2.TabIndex = 13;
            this.luaEditHeaderSeparator2.Title = "Output";
            // 
            // btnBrowseOutputDir
            // 
            this.btnBrowseOutputDir.Location = new System.Drawing.Point(418, 34);
            this.btnBrowseOutputDir.Name = "btnBrowseOutputDir";
            this.btnBrowseOutputDir.Size = new System.Drawing.Size(27, 20);
            this.btnBrowseOutputDir.TabIndex = 20;
            this.btnBrowseOutputDir.Text = "...";
            this.btnBrowseOutputDir.UseVisualStyleBackColor = true;
            this.btnBrowseOutputDir.Click += new System.EventHandler(this.btnBrowseOutputDir_Click);
            // 
            // folderBrowserOutputDir
            // 
            this.folderBrowserOutputDir.Description = "Select the project\'s build output directory";
            this.folderBrowserOutputDir.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // ProjectPropertiesBuild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.btnBrowseOutputDir);
            this.Controls.Add(this.luaEditHeaderSeparator2);
            this.Controls.Add(this.txtOutputDir);
            this.Controls.Add(this.lblProjectName);
            this.Name = "ProjectPropertiesBuild";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(450, 82);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.TextBox txtOutputDir;
        private LuaEdit.Controls.LuaEditHeaderSeparator luaEditHeaderSeparator2;
        private System.Windows.Forms.Button btnBrowseOutputDir;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserOutputDir;
    }
}