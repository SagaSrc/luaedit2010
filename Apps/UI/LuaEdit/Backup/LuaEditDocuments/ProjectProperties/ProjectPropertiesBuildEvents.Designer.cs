namespace LuaEdit.Documents.ProjectProperties
{
    partial class ProjectPropertiesBuildEvents
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.luaEditHeaderSeparator2 = new LuaEdit.Controls.LuaEditHeaderSeparator();
            this.luaEditHeaderSeparator1 = new LuaEdit.Controls.LuaEditHeaderSeparator();
            this.txtPreBuildEventCmdLine = new System.Windows.Forms.TextBox();
            this.btnEditPreBuild = new System.Windows.Forms.Button();
            this.btnEditPostBuild = new System.Windows.Forms.Button();
            this.txtPostBuildEventCmdLine = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboRunPostBuildEvent = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // luaEditHeaderSeparator2
            // 
            this.luaEditHeaderSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.luaEditHeaderSeparator2.Location = new System.Drawing.Point(7, 8);
            this.luaEditHeaderSeparator2.Name = "luaEditHeaderSeparator2";
            this.luaEditHeaderSeparator2.SeparatorColor = System.Drawing.SystemColors.ControlDark;
            this.luaEditHeaderSeparator2.Size = new System.Drawing.Size(437, 22);
            this.luaEditHeaderSeparator2.TabIndex = 14;
            this.luaEditHeaderSeparator2.Title = "Pre-build Event Command Line";
            // 
            // luaEditHeaderSeparator1
            // 
            this.luaEditHeaderSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.luaEditHeaderSeparator1.Location = new System.Drawing.Point(7, 173);
            this.luaEditHeaderSeparator1.Name = "luaEditHeaderSeparator1";
            this.luaEditHeaderSeparator1.SeparatorColor = System.Drawing.SystemColors.ControlDark;
            this.luaEditHeaderSeparator1.Size = new System.Drawing.Size(437, 22);
            this.luaEditHeaderSeparator1.TabIndex = 15;
            this.luaEditHeaderSeparator1.Title = "Post-build Event Command Line";
            // 
            // txtPreBuildEventCmdLine
            // 
            this.txtPreBuildEventCmdLine.Location = new System.Drawing.Point(26, 35);
            this.txtPreBuildEventCmdLine.Multiline = true;
            this.txtPreBuildEventCmdLine.Name = "txtPreBuildEventCmdLine";
            this.txtPreBuildEventCmdLine.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPreBuildEventCmdLine.Size = new System.Drawing.Size(388, 96);
            this.txtPreBuildEventCmdLine.TabIndex = 16;
            this.txtPreBuildEventCmdLine.WordWrap = false;
            this.txtPreBuildEventCmdLine.Leave += new System.EventHandler(this.txtPreBuildEventCmdLine_Leave);
            this.txtPreBuildEventCmdLine.Enter += new System.EventHandler(this.txtPreBuildEventCmdLine_Enter);
            // 
            // btnEditPreBuild
            // 
            this.btnEditPreBuild.Location = new System.Drawing.Point(304, 137);
            this.btnEditPreBuild.Name = "btnEditPreBuild";
            this.btnEditPreBuild.Size = new System.Drawing.Size(110, 23);
            this.btnEditPreBuild.TabIndex = 17;
            this.btnEditPreBuild.Text = "Edit Pre-build...";
            this.btnEditPreBuild.UseVisualStyleBackColor = true;
            // 
            // btnEditPostBuild
            // 
            this.btnEditPostBuild.Location = new System.Drawing.Point(304, 305);
            this.btnEditPostBuild.Name = "btnEditPostBuild";
            this.btnEditPostBuild.Size = new System.Drawing.Size(110, 23);
            this.btnEditPostBuild.TabIndex = 19;
            this.btnEditPostBuild.Text = "Edit Post-build...";
            this.btnEditPostBuild.UseVisualStyleBackColor = true;
            // 
            // txtPostBuildEventCmdLine
            // 
            this.txtPostBuildEventCmdLine.Location = new System.Drawing.Point(26, 203);
            this.txtPostBuildEventCmdLine.Multiline = true;
            this.txtPostBuildEventCmdLine.Name = "txtPostBuildEventCmdLine";
            this.txtPostBuildEventCmdLine.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPostBuildEventCmdLine.Size = new System.Drawing.Size(388, 96);
            this.txtPostBuildEventCmdLine.TabIndex = 18;
            this.txtPostBuildEventCmdLine.WordWrap = false;
            this.txtPostBuildEventCmdLine.Leave += new System.EventHandler(this.txtPostBuildEventCmdLine_Leave);
            this.txtPostBuildEventCmdLine.Enter += new System.EventHandler(this.txtPostBuildEventCmdLine_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 333);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Run the post build event:";
            // 
            // cboRunPostBuildEvent
            // 
            this.cboRunPostBuildEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRunPostBuildEvent.FormattingEnabled = true;
            this.cboRunPostBuildEvent.Items.AddRange(new object[] {
            "Always",
            "On successful build"});
            this.cboRunPostBuildEvent.Location = new System.Drawing.Point(26, 349);
            this.cboRunPostBuildEvent.Name = "cboRunPostBuildEvent";
            this.cboRunPostBuildEvent.Size = new System.Drawing.Size(387, 21);
            this.cboRunPostBuildEvent.TabIndex = 21;
            this.cboRunPostBuildEvent.SelectedValueChanged += new System.EventHandler(this.cboRunPostBuildEvent_SelectedValueChanged);
            // 
            // ProjectPropertiesBuildEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.cboRunPostBuildEvent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEditPostBuild);
            this.Controls.Add(this.txtPostBuildEventCmdLine);
            this.Controls.Add(this.btnEditPreBuild);
            this.Controls.Add(this.txtPreBuildEventCmdLine);
            this.Controls.Add(this.luaEditHeaderSeparator1);
            this.Controls.Add(this.luaEditHeaderSeparator2);
            this.Name = "ProjectPropertiesBuildEvents";
            this.Size = new System.Drawing.Size(450, 388);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LuaEdit.Controls.LuaEditHeaderSeparator luaEditHeaderSeparator2;
        private LuaEdit.Controls.LuaEditHeaderSeparator luaEditHeaderSeparator1;
        private System.Windows.Forms.TextBox txtPreBuildEventCmdLine;
        private System.Windows.Forms.Button btnEditPreBuild;
        private System.Windows.Forms.Button btnEditPostBuild;
        private System.Windows.Forms.TextBox txtPostBuildEventCmdLine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboRunPostBuildEvent;
    }
}
