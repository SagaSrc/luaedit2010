namespace LuaEdit.Forms
{
    partial class SolutionExplorerDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionExplorerDlg));
            this.solutionExplorerToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this._solutionExplorerContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.existingProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.newItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.existingItemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.setAsStartupProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeFromProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeListViewColumnHeader1 = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.solutionExplorerTreeView = new LuaEdit.Controls.LuaEditTreeListView();
            this.solutionExplorerToolStrip.SuspendLayout();
            this._solutionExplorerContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // solutionExplorerToolStrip
            // 
            this.solutionExplorerToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.solutionExplorerToolStrip.AutoSize = false;
            this.solutionExplorerToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.solutionExplorerToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.solutionExplorerToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.solutionExplorerToolStrip.Location = new System.Drawing.Point(-3, 0);
            this.solutionExplorerToolStrip.Name = "solutionExplorerToolStrip";
            this.solutionExplorerToolStrip.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.solutionExplorerToolStrip.Size = new System.Drawing.Size(309, 25);
            this.solutionExplorerToolStrip.TabIndex = 1;
            this.solutionExplorerToolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::LuaEdit.Properties.Resources.SolutionExplorerProperties;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Properties";
            // 
            // _solutionExplorerContextMenu
            // 
            this._solutionExplorerContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripMenuItem5,
            this.addToolStripMenuItem,
            this.toolStripMenuItem1,
            this.setAsStartupProjectToolStripMenuItem,
            this.excludeFromProjectToolStripMenuItem,
            this.toolStripMenuItem2,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.toolStripMenuItem3,
            this.propertiesToolStripMenuItem});
            this._solutionExplorerContextMenu.Name = "_groupContextMenu";
            this._solutionExplorerContextMenu.Size = new System.Drawing.Size(193, 248);
            this._solutionExplorerContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this._solutionExplorerContextMenu_Opening);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Open;
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(189, 6);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.existingProjectToolStripMenuItem,
            this.toolStripMenuItem8,
            this.newItemToolStripMenuItem,
            this.existingItemToolStripMenuItem,
            this.newFolderToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.newProjectToolStripMenuItem.Text = "New Project...";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // existingProjectToolStripMenuItem
            // 
            this.existingProjectToolStripMenuItem.Name = "existingProjectToolStripMenuItem";
            this.existingProjectToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.existingProjectToolStripMenuItem.Text = "Existing Project...";
            this.existingProjectToolStripMenuItem.Click += new System.EventHandler(this.existingProjectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(168, 6);
            // 
            // newItemToolStripMenuItem
            // 
            this.newItemToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.NewItem;
            this.newItemToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.newItemToolStripMenuItem.Name = "newItemToolStripMenuItem";
            this.newItemToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.newItemToolStripMenuItem.Text = "New Item...";
            this.newItemToolStripMenuItem.Click += new System.EventHandler(this.newItemToolStripMenuItem_Click);
            // 
            // existingItemToolStripMenuItem
            // 
            this.existingItemToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.ExistingItem;
            this.existingItemToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.existingItemToolStripMenuItem.Name = "existingItemToolStripMenuItem";
            this.existingItemToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.existingItemToolStripMenuItem.Text = "Existing Item...";
            this.existingItemToolStripMenuItem.Click += new System.EventHandler(this.existingItemToolStripMenuItem_Click);
            // 
            // newFolderToolStripMenuItem
            // 
            this.newFolderToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.NewProjectFolder;
            this.newFolderToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.newFolderToolStripMenuItem.Text = "New Folder";
            this.newFolderToolStripMenuItem.Click += new System.EventHandler(this.newFolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(189, 6);
            // 
            // setAsStartupProjectToolStripMenuItem
            // 
            this.setAsStartupProjectToolStripMenuItem.Name = "setAsStartupProjectToolStripMenuItem";
            this.setAsStartupProjectToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.setAsStartupProjectToolStripMenuItem.Text = "Set as StartUp Project";
            this.setAsStartupProjectToolStripMenuItem.Click += new System.EventHandler(this.setAsStartupProjectToolStripMenuItem_Click);
            // 
            // excludeFromProjectToolStripMenuItem
            // 
            this.excludeFromProjectToolStripMenuItem.Name = "excludeFromProjectToolStripMenuItem";
            this.excludeFromProjectToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.excludeFromProjectToolStripMenuItem.Text = "Exclude From Project";
            this.excludeFromProjectToolStripMenuItem.Click += new System.EventHandler(this.excludeFromProjectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(189, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Cut;
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Copy;
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Paste;
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Delete;
            this.removeToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(189, 6);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.SolutionExplorerProperties;
            this.propertiesToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // treeListViewColumnHeader1
            // 
            this.treeListViewColumnHeader1.CustomSortTag = null;
            this.treeListViewColumnHeader1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeListViewColumnHeader1.SortDataType = DotNetLib.Controls.SortDataType.String;
            this.treeListViewColumnHeader1.Tag = null;
            this.treeListViewColumnHeader1.Text = "Files";
            this.treeListViewColumnHeader1.Width = 25;
            // 
            // solutionExplorerTreeView
            // 
            this.solutionExplorerTreeView.AllowMultiSelect = true;
            this.solutionExplorerTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.solutionExplorerTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.solutionExplorerTreeView.Columns.AddRange(new DotNetLib.Controls.TreeListViewColumnHeader[] {
            this.treeListViewColumnHeader1});
            this.solutionExplorerTreeView.DefaultItemHeight = 16;
            this.solutionExplorerTreeView.FullItemSelect = false;
            this.solutionExplorerTreeView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.solutionExplorerTreeView.ItemContextMenu = this._solutionExplorerContextMenu;
            this.solutionExplorerTreeView.Location = new System.Drawing.Point(1, 29);
            this.solutionExplorerTreeView.Name = "solutionExplorerTreeView";
            this.solutionExplorerTreeView.ShowPlusMinus = true;
            this.solutionExplorerTreeView.ShowTreeLines = true;
            this.solutionExplorerTreeView.Size = new System.Drawing.Size(283, 376);
            this.solutionExplorerTreeView.TabIndex = 2;
            this.solutionExplorerTreeView.VisualStyles = false;
            this.solutionExplorerTreeView.ItemBeforeEdit += new DotNetLib.Controls.TreeListViewBeforeEditEventHandler(this.solutionExplorerTreeView_ItemBeforeEdit);
            this.solutionExplorerTreeView.ItemAfterEdit += new DotNetLib.Controls.TreeListViewAfterEditEventHandler(this.solutionExplorerTreeView_ItemAfterEdit);
            this.solutionExplorerTreeView.DoubleClick += new System.EventHandler(this.solutionExplorerTreeView_DoubleClick);
            // 
            // SolutionExplorerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 405);
            this.Controls.Add(this.solutionExplorerTreeView);
            this.Controls.Add(this.solutionExplorerToolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.ForeColor = System.Drawing.Color.Black;
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SolutionExplorerDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "Solution Explorer";
            this.Text = "Solution Explorer";
            this.solutionExplorerToolStrip.ResumeLayout(false);
            this.solutionExplorerToolStrip.PerformLayout();
            this._solutionExplorerContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip solutionExplorerToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private DotNetLib.Controls.TreeListViewColumnHeader treeListViewColumnHeader1;
        private System.Windows.Forms.ContextMenuStrip _solutionExplorerContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existingItemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem setAsStartupProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excludeFromProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existingProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private LuaEdit.Controls.LuaEditTreeListView solutionExplorerTreeView;
    }
}