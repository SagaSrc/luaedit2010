namespace LuaEdit.Forms
{
    partial class BreakpointsDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BreakpointsDlg));
            this.breakpointsToolStrip = new System.Windows.Forms.ToolStrip();
            this.btnDeleteSelected = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteAllBreakpoints = new System.Windows.Forms.ToolStripButton();
            this.btnDisableAllBreakpoints = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGoToSourceCode = new System.Windows.Forms.ToolStripButton();
            this.imlBreakpoints = new System.Windows.Forms.ImageList(this.components);
            this.tlvwBreakpoints = new LuaEdit.Controls.LuaEditTreeListView();
            this.colFile = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colCondition = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colHitCount = new DotNetLib.Controls.TreeListViewColumnHeader();
            this._breakpointsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.gotoSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.conditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hitCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakpointsToolStrip.SuspendLayout();
            this._breakpointsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // breakpointsToolStrip
            // 
            this.breakpointsToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.breakpointsToolStrip.AutoSize = false;
            this.breakpointsToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.breakpointsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.breakpointsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDeleteSelected,
            this.btnDeleteAllBreakpoints,
            this.btnDisableAllBreakpoints,
            this.toolStripSeparator1,
            this.btnGoToSourceCode});
            this.breakpointsToolStrip.Location = new System.Drawing.Point(1, 0);
            this.breakpointsToolStrip.Name = "breakpointsToolStrip";
            this.breakpointsToolStrip.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.breakpointsToolStrip.Size = new System.Drawing.Size(518, 25);
            this.breakpointsToolStrip.TabIndex = 2;
            this.breakpointsToolStrip.Text = "toolStrip1";
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteSelected.Image = global::LuaEdit.Properties.Resources.Delete;
            this.btnDeleteSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteSelected.Text = "Delete Selected Breakpoint(s)";
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // btnDeleteAllBreakpoints
            // 
            this.btnDeleteAllBreakpoints.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteAllBreakpoints.Image = global::LuaEdit.Properties.Resources.DeleteAllBreakpoints;
            this.btnDeleteAllBreakpoints.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteAllBreakpoints.Name = "btnDeleteAllBreakpoints";
            this.btnDeleteAllBreakpoints.Size = new System.Drawing.Size(23, 22);
            this.btnDeleteAllBreakpoints.Text = "Delete All Breakpoints";
            this.btnDeleteAllBreakpoints.Click += new System.EventHandler(this.btnDeleteAllBreakpoints_Click);
            // 
            // btnDisableAllBreakpoints
            // 
            this.btnDisableAllBreakpoints.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDisableAllBreakpoints.Image = global::LuaEdit.Properties.Resources.DisableAllBreakpoints;
            this.btnDisableAllBreakpoints.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDisableAllBreakpoints.Name = "btnDisableAllBreakpoints";
            this.btnDisableAllBreakpoints.Size = new System.Drawing.Size(23, 22);
            this.btnDisableAllBreakpoints.Text = "Disable All Breakpoints";
            this.btnDisableAllBreakpoints.Click += new System.EventHandler(this.btnDisableAllBreakpoints_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnGoToSourceCode
            // 
            this.btnGoToSourceCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnGoToSourceCode.Image = global::LuaEdit.Properties.Resources.GoToSourceCode;
            this.btnGoToSourceCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnGoToSourceCode.Name = "btnGoToSourceCode";
            this.btnGoToSourceCode.Size = new System.Drawing.Size(23, 22);
            this.btnGoToSourceCode.Text = "Go to Source Code";
            this.btnGoToSourceCode.Click += new System.EventHandler(this.btnGoToSourceCode_Click);
            // 
            // imlBreakpoints
            // 
            this.imlBreakpoints.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlBreakpoints.ImageStream")));
            this.imlBreakpoints.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imlBreakpoints.Images.SetKeyName(0, "BreakpointEnabled.bmp");
            this.imlBreakpoints.Images.SetKeyName(1, "BreakpointDisabled.bmp");
            this.imlBreakpoints.Images.SetKeyName(2, "BreakpointConditionedEnabled.bmp");
            this.imlBreakpoints.Images.SetKeyName(3, "BreakpointConditionedDisabled.bmp");
            // 
            // tlvwBreakpoints
            // 
            this.tlvwBreakpoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tlvwBreakpoints.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.tlvwBreakpoints.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvwBreakpoints.CheckBoxes = true;
            this.tlvwBreakpoints.Columns.AddRange(new DotNetLib.Controls.TreeListViewColumnHeader[] {
            this.colFile,
            this.colCondition,
            this.colHitCount});
            this.tlvwBreakpoints.DefaultItemHeight = 16;
            this.tlvwBreakpoints.FullRowSelect = true;
            this.tlvwBreakpoints.HeaderHeight = 16;
            this.tlvwBreakpoints.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvwBreakpoints.HideSelection = false;
            this.tlvwBreakpoints.ItemContextMenu = this._breakpointsContextMenu;
            this.tlvwBreakpoints.Location = new System.Drawing.Point(1, 28);
            this.tlvwBreakpoints.Name = "tlvwBreakpoints";
            this.tlvwBreakpoints.Size = new System.Drawing.Size(502, 271);
            this.tlvwBreakpoints.SmallImageList = this.imlBreakpoints;
            this.tlvwBreakpoints.TabIndex = 3;
            this.tlvwBreakpoints.VisualStyles = false;
            this.tlvwBreakpoints.SelectedItemsChanged += new System.EventHandler(this.tlvwBreakpoints_SelectedItemsChanged);
            this.tlvwBreakpoints.ItemChecked += new DotNetLib.Controls.TreeListViewEventHandler(this.tlvwBreakpoints_ItemChecked);
            this.tlvwBreakpoints.DoubleClick += new System.EventHandler(this.tlvwBreakpoints_DoubleClick);
            // 
            // colFile
            // 
            this.colFile.CustomSortTag = null;
            this.colFile.Tag = null;
            this.colFile.Text = "File";
            this.colFile.Width = 250;
            // 
            // colCondition
            // 
            this.colCondition.CustomSortTag = null;
            this.colCondition.DisplayIndex = 1;
            this.colCondition.Tag = null;
            this.colCondition.Text = "Condition";
            this.colCondition.Width = 125;
            // 
            // colHitCount
            // 
            this.colHitCount.CustomSortTag = null;
            this.colHitCount.DisplayIndex = 2;
            this.colHitCount.Tag = null;
            this.colHitCount.Text = "Hit Count";
            this.colHitCount.Width = 125;
            // 
            // _breakpointsContextMenu
            // 
            this._breakpointsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem1,
            this.gotoSourceToolStripMenuItem,
            this.toolStripMenuItem2,
            this.conditionToolStripMenuItem,
            this.hitCountToolStripMenuItem});
            this._breakpointsContextMenu.Name = "_breakpointsContextMenu";
            this._breakpointsContextMenu.Size = new System.Drawing.Size(178, 104);
            this._breakpointsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this._breakpointsContextMenu_Opening);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Delete;
            this.deleteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(174, 6);
            // 
            // gotoSourceToolStripMenuItem
            // 
            this.gotoSourceToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.GoToSourceCode;
            this.gotoSourceToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.gotoSourceToolStripMenuItem.Name = "gotoSourceToolStripMenuItem";
            this.gotoSourceToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.gotoSourceToolStripMenuItem.Text = "Go To Source Code";
            this.gotoSourceToolStripMenuItem.Click += new System.EventHandler(this.gotoSourceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(174, 6);
            // 
            // conditionToolStripMenuItem
            // 
            this.conditionToolStripMenuItem.Name = "conditionToolStripMenuItem";
            this.conditionToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.conditionToolStripMenuItem.Text = "Condition...";
            this.conditionToolStripMenuItem.Click += new System.EventHandler(this.conditionToolStripMenuItem_Click);
            // 
            // hitCountToolStripMenuItem
            // 
            this.hitCountToolStripMenuItem.Name = "hitCountToolStripMenuItem";
            this.hitCountToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.hitCountToolStripMenuItem.Text = "Hit Count...";
            this.hitCountToolStripMenuItem.Click += new System.EventHandler(this.hitCountToolStripMenuItem_Click);
            // 
            // BreakpointsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 299);
            this.Controls.Add(this.tlvwBreakpoints);
            this.Controls.Add(this.breakpointsToolStrip);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BreakpointsDlg";
            this.TabText = "Breakpoints";
            this.Text = "Breakpoints";
            this.breakpointsToolStrip.ResumeLayout(false);
            this.breakpointsToolStrip.PerformLayout();
            this._breakpointsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip breakpointsToolStrip;
        private LuaEdit.Controls.LuaEditTreeListView tlvwBreakpoints;
        private DotNetLib.Controls.TreeListViewColumnHeader colFile;
        private DotNetLib.Controls.TreeListViewColumnHeader colCondition;
        private System.Windows.Forms.ImageList imlBreakpoints;
        private System.Windows.Forms.ToolStripButton btnDeleteSelected;
        private System.Windows.Forms.ToolStripButton btnDeleteAllBreakpoints;
        private System.Windows.Forms.ToolStripButton btnDisableAllBreakpoints;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnGoToSourceCode;
        private DotNetLib.Controls.TreeListViewColumnHeader colHitCount;
        private System.Windows.Forms.ContextMenuStrip _breakpointsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem gotoSourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem conditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hitCountToolStripMenuItem;
    }
}