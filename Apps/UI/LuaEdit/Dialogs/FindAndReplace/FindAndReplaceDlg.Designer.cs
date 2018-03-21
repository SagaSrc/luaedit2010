namespace LuaEdit.Forms
{
    partial class FindAndReplaceDlg
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
            this.findAndReplaceToolStrip = new System.Windows.Forms.ToolStrip();
            this.findToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.quickFindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findInFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.replaceToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.quickReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceInFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findAndReplaceToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // findAndReplaceToolStrip
            // 
            this.findAndReplaceToolStrip.AutoSize = false;
            this.findAndReplaceToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.findAndReplaceToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripDropDownButton,
            this.toolStripSeparator1,
            this.replaceToolStripDropDownButton});
            this.findAndReplaceToolStrip.Location = new System.Drawing.Point(0, 0);
            this.findAndReplaceToolStrip.Name = "findAndReplaceToolStrip";
            this.findAndReplaceToolStrip.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.findAndReplaceToolStrip.Size = new System.Drawing.Size(317, 25);
            this.findAndReplaceToolStrip.TabIndex = 2;
            this.findAndReplaceToolStrip.Text = "toolStrip1";
            // 
            // findToolStripDropDownButton
            // 
            this.findToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quickFindToolStripMenuItem,
            this.findInFilesToolStripMenuItem});
            this.findToolStripDropDownButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.findToolStripDropDownButton.Image = global::LuaEdit.Properties.Resources.Find;
            this.findToolStripDropDownButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.findToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.findToolStripDropDownButton.Name = "findToolStripDropDownButton";
            this.findToolStripDropDownButton.Size = new System.Drawing.Size(85, 22);
            this.findToolStripDropDownButton.Text = "Quick Find";
            this.findToolStripDropDownButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // quickFindToolStripMenuItem
            // 
            this.quickFindToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Find;
            this.quickFindToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.quickFindToolStripMenuItem.Name = "quickFindToolStripMenuItem";
            this.quickFindToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.quickFindToolStripMenuItem.ShowShortcutKeys = false;
            this.quickFindToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quickFindToolStripMenuItem.Text = "Quick Find";
            this.quickFindToolStripMenuItem.Click += new System.EventHandler(this.quickFindToolStripMenuItem_Click);
            // 
            // findInFilesToolStripMenuItem
            // 
            this.findInFilesToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.FindInFiles;
            this.findInFilesToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.findInFilesToolStripMenuItem.Name = "findInFilesToolStripMenuItem";
            this.findInFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.F)));
            this.findInFilesToolStripMenuItem.ShowShortcutKeys = false;
            this.findInFilesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.findInFilesToolStripMenuItem.Text = "Find in Files";
            this.findInFilesToolStripMenuItem.Click += new System.EventHandler(this.findInFilesToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // replaceToolStripDropDownButton
            // 
            this.replaceToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quickReplaceToolStripMenuItem,
            this.replaceInFilesToolStripMenuItem});
            this.replaceToolStripDropDownButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.replaceToolStripDropDownButton.Image = global::LuaEdit.Properties.Resources.Repalce;
            this.replaceToolStripDropDownButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.replaceToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.replaceToolStripDropDownButton.Name = "replaceToolStripDropDownButton";
            this.replaceToolStripDropDownButton.Size = new System.Drawing.Size(103, 22);
            this.replaceToolStripDropDownButton.Text = "Quick Replace";
            this.replaceToolStripDropDownButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // quickReplaceToolStripMenuItem
            // 
            this.quickReplaceToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Repalce;
            this.quickReplaceToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.quickReplaceToolStripMenuItem.Name = "quickReplaceToolStripMenuItem";
            this.quickReplaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.quickReplaceToolStripMenuItem.ShowShortcutKeys = false;
            this.quickReplaceToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.quickReplaceToolStripMenuItem.Text = "Quick Replace";
            this.quickReplaceToolStripMenuItem.Click += new System.EventHandler(this.quickReplaceToolStripMenuItem_Click);
            // 
            // replaceInFilesToolStripMenuItem
            // 
            this.replaceInFilesToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.ReplaceInFiles;
            this.replaceInFilesToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.replaceInFilesToolStripMenuItem.Name = "replaceInFilesToolStripMenuItem";
            this.replaceInFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.H)));
            this.replaceInFilesToolStripMenuItem.ShowShortcutKeys = false;
            this.replaceInFilesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.replaceInFilesToolStripMenuItem.Text = "Replace in Files";
            this.replaceInFilesToolStripMenuItem.Click += new System.EventHandler(this.replaceInFilesToolStripMenuItem_Click);
            // 
            // FindAndReplaceDlg
            // 
            this.AllowEndUserDocking = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 396);
            this.Controls.Add(this.findAndReplaceToolStrip);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Float;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.HideOnClose = true;
            this.KeyPreview = true;
            this.Name = "FindAndReplaceDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "Find and Replace";
            this.Text = "Find and Replace";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FindAndReplaceDlg_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindAndReplaceDlg_KeyDown);
            this.findAndReplaceToolStrip.ResumeLayout(false);
            this.findAndReplaceToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip findAndReplaceToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton findToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem quickFindToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findInFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton replaceToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem quickReplaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceInFilesToolStripMenuItem;
    }
}