namespace LuaEdit.Forms
{
    partial class LocalsDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocalsDlg));
            this.tlvwLocals = new LuaEdit.Controls.LuaEditTreeListView();
            this.colName = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colValue = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colType = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.SuspendLayout();
            // 
            // tlvwLocals
            // 
            this.tlvwLocals.AutoResizeColumnIndex = 1;
            this.tlvwLocals.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.tlvwLocals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvwLocals.Columns.AddRange(new DotNetLib.Controls.TreeListViewColumnHeader[] {
            this.colName,
            this.colValue,
            this.colType});
            this.tlvwLocals.DefaultItemHeight = 16;
            this.tlvwLocals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlvwLocals.FullRowSelect = true;
            this.tlvwLocals.GridLines = ((DotNetLib.Controls.GridLines)((DotNetLib.Controls.GridLines.Horizontal | DotNetLib.Controls.GridLines.Vertical)));
            this.tlvwLocals.HeaderHeight = 16;
            this.tlvwLocals.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvwLocals.HideSelection = false;
            this.tlvwLocals.Location = new System.Drawing.Point(0, 0);
            this.tlvwLocals.MinimumSize = new System.Drawing.Size(257, 0);
            this.tlvwLocals.Name = "tlvwLocals";
            this.tlvwLocals.ShowPlusMinus = true;
            this.tlvwLocals.ShowRootTreeLines = true;
            this.tlvwLocals.ShowTreeLines = true;
            this.tlvwLocals.Size = new System.Drawing.Size(427, 262);
            this.tlvwLocals.TabIndex = 0;
            this.tlvwLocals.VisualStyles = false;
            this.tlvwLocals.ItemBeforeEdit += new DotNetLib.Controls.TreeListViewBeforeEditEventHandler(this.tlvwLocals_ItemBeforeEdit);
            this.tlvwLocals.ItemExpanding += new DotNetLib.Controls.TreeListViewCancelEventHandler(this.tlvwLocals_ItemExpanding);
            this.tlvwLocals.ItemAfterEdit += new DotNetLib.Controls.TreeListViewAfterEditEventHandler(this.tlvwLocals_ItemAfterEdit);
            // 
            // colName
            // 
            this.colName.CustomSortTag = null;
            this.colName.Editable = false;
            this.colName.MinimumWidth = 30;
            this.colName.Tag = null;
            this.colName.Text = "Name";
            this.colName.Width = 125;
            // 
            // colValue
            // 
            this.colValue.CustomSortTag = null;
            this.colValue.DisplayIndex = 1;
            this.colValue.MinimumWidth = 30;
            this.colValue.Tag = null;
            this.colValue.Text = "Value";
            this.colValue.Width = 200;
            // 
            // colType
            // 
            this.colType.CustomSortTag = null;
            this.colType.DisplayIndex = 2;
            this.colType.Editable = false;
            this.colType.MinimumWidth = 30;
            this.colType.Tag = null;
            this.colType.Text = "Type";
            this.colType.Width = 100;
            // 
            // LocalsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 262);
            this.Controls.Add(this.tlvwLocals);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LocalsDlg";
            this.TabText = "Locals";
            this.Text = "Locals";
            this.Shown += new System.EventHandler(this.LocalsDlg_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private LuaEdit.Controls.LuaEditTreeListView tlvwLocals;
        private DotNetLib.Controls.TreeListViewColumnHeader colName;
        private DotNetLib.Controls.TreeListViewColumnHeader colValue;
        private DotNetLib.Controls.TreeListViewColumnHeader colType;
    }
}