namespace LuaEdit.Forms
{
    partial class DocumentListDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentListDlg));
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.treeListViewColumnHeader1 = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.treeListViewColumnHeader2 = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colFile = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colPath = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.tlvwDocList = new LuaEdit.Controls.LuaEditTreeListView();
            this.SuspendLayout();
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilter.Location = new System.Drawing.Point(9, 214);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(397, 20);
            this.txtFilter.TabIndex = 0;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            this.txtFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilter_KeyDown);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(412, 214);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(412, 243);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // treeListViewColumnHeader1
            // 
            this.treeListViewColumnHeader1.CustomSortTag = null;
            this.treeListViewColumnHeader1.Tag = null;
            this.treeListViewColumnHeader1.Text = "File";
            this.treeListViewColumnHeader1.Width = 150;
            // 
            // treeListViewColumnHeader2
            // 
            this.treeListViewColumnHeader2.CustomSortTag = null;
            this.treeListViewColumnHeader2.Tag = null;
            this.treeListViewColumnHeader2.Text = "Path";
            this.treeListViewColumnHeader2.Width = 325;
            // 
            // colFile
            // 
            this.colFile.CustomSortTag = null;
            this.colFile.MinimumWidth = 30;
            this.colFile.SortDataType = DotNetLib.Controls.SortDataType.String;
            this.colFile.Tag = null;
            this.colFile.Text = "File";
            this.colFile.Width = 160;
            // 
            // colPath
            // 
            this.colPath.CustomSortTag = null;
            this.colPath.DisplayIndex = 1;
            this.colPath.MinimumWidth = 30;
            this.colPath.Tag = null;
            this.colPath.Text = "Path";
            this.colPath.Width = 316;
            // 
            // tlvwDocList
            // 
            this.tlvwDocList.AllowMultiSelect = true;
            this.tlvwDocList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tlvwDocList.AutoResizeColumnIndex = 1;
            this.tlvwDocList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvwDocList.Columns.AddRange(new DotNetLib.Controls.TreeListViewColumnHeader[] {
            this.colFile,
            this.colPath});
            this.tlvwDocList.DefaultItemHeight = 16;
            this.tlvwDocList.FullRowSelect = true;
            this.tlvwDocList.GridLines = ((DotNetLib.Controls.GridLines)((DotNetLib.Controls.GridLines.Horizontal | DotNetLib.Controls.GridLines.Vertical)));
            this.tlvwDocList.HeaderHeight = 16;
            this.tlvwDocList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvwDocList.HideSelection = false;
            this.tlvwDocList.Location = new System.Drawing.Point(9, 9);
            this.tlvwDocList.MinimumSize = new System.Drawing.Size(162, 0);
            this.tlvwDocList.Name = "tlvwDocList";
            this.tlvwDocList.ShowTreeLines = true;
            this.tlvwDocList.Size = new System.Drawing.Size(478, 199);
            this.tlvwDocList.TabIndex = 3;
            this.tlvwDocList.VisualStyles = false;
            this.tlvwDocList.DoubleClick += new System.EventHandler(this.tlvwDocList_DoubleClick);
            // 
            // DocumentListDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(496, 272);
            this.Controls.Add(this.tlvwDocList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtFilter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DocumentListDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Document List";
            this.Load += new System.EventHandler(this.DocumentListDlg_Load);
            this.Shown += new System.EventHandler(this.DocumentListDlg_Shown);
            this.Activated += new System.EventHandler(this.DocumentListDlg_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private DotNetLib.Controls.TreeListViewColumnHeader treeListViewColumnHeader1;
        private DotNetLib.Controls.TreeListViewColumnHeader treeListViewColumnHeader2;
        private LuaEdit.Controls.LuaEditTreeListView tlvwDocList;
        private DotNetLib.Controls.TreeListViewColumnHeader colFile;
        private DotNetLib.Controls.TreeListViewColumnHeader colPath;
    }
}