namespace LuaEdit.Controls
{
    partial class LuaEditFolderSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LuaEditFolderSelector));
            this.lblMessage = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboFolderSets = new System.Windows.Forms.ComboBox();
            this.btnApplyFolderSet = new System.Windows.Forms.Button();
            this.btnDeleteFolderSet = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvwAvailableFolders = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.btnMoveUpOneFolder = new System.Windows.Forms.Button();
            this.imlButtons = new System.Windows.Forms.ImageList(this.components);
            this.cboCurrentPath = new System.Windows.Forms.ComboBox();
            this.btnTransferRight = new System.Windows.Forms.Button();
            this.btnTransferLeft = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvwSelectedFolders = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(9, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(500, 32);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Dialog message here... and we must make this message long because we need to see " +
                "what it looks like when long messages are entered here.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Folder set:";
            // 
            // cboFolderSets
            // 
            this.cboFolderSets.FormattingEnabled = true;
            this.cboFolderSets.Location = new System.Drawing.Point(12, 66);
            this.cboFolderSets.Name = "cboFolderSets";
            this.cboFolderSets.Size = new System.Drawing.Size(233, 21);
            this.cboFolderSets.TabIndex = 2;
            this.cboFolderSets.SelectedValueChanged += new System.EventHandler(this.cboFolderSets_SelectedValueChanged);
            this.cboFolderSets.TextChanged += new System.EventHandler(this.cboFolderSets_TextChanged);
            // 
            // btnApplyFolderSet
            // 
            this.btnApplyFolderSet.Location = new System.Drawing.Point(256, 64);
            this.btnApplyFolderSet.Name = "btnApplyFolderSet";
            this.btnApplyFolderSet.Size = new System.Drawing.Size(75, 23);
            this.btnApplyFolderSet.TabIndex = 3;
            this.btnApplyFolderSet.Text = "Apply";
            this.btnApplyFolderSet.UseVisualStyleBackColor = true;
            this.btnApplyFolderSet.Click += new System.EventHandler(this.btnApplyFolderSet_Click);
            // 
            // btnDeleteFolderSet
            // 
            this.btnDeleteFolderSet.Location = new System.Drawing.Point(337, 64);
            this.btnDeleteFolderSet.Name = "btnDeleteFolderSet";
            this.btnDeleteFolderSet.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteFolderSet.TabIndex = 4;
            this.btnDeleteFolderSet.Text = "Delete";
            this.btnDeleteFolderSet.UseVisualStyleBackColor = true;
            this.btnDeleteFolderSet.Click += new System.EventHandler(this.btnDeleteFolderSet_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvwAvailableFolders);
            this.groupBox1.Controls.Add(this.btnMoveUpOneFolder);
            this.groupBox1.Controls.Add(this.cboCurrentPath);
            this.groupBox1.Location = new System.Drawing.Point(12, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 229);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available folders";
            // 
            // lvwAvailableFolders
            // 
            this.lvwAvailableFolders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvwAvailableFolders.FullRowSelect = true;
            this.lvwAvailableFolders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwAvailableFolders.HideSelection = false;
            this.lvwAvailableFolders.Location = new System.Drawing.Point(10, 46);
            this.lvwAvailableFolders.Name = "lvwAvailableFolders";
            this.lvwAvailableFolders.Size = new System.Drawing.Size(214, 175);
            this.lvwAvailableFolders.TabIndex = 16;
            this.lvwAvailableFolders.UseCompatibleStateImageBehavior = false;
            this.lvwAvailableFolders.View = System.Windows.Forms.View.Details;
            this.lvwAvailableFolders.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwAvailableFolders_MouseDoubleClick);
            this.lvwAvailableFolders.SelectedIndexChanged += new System.EventHandler(this.lvwAvailableFolders_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 210;
            // 
            // btnMoveUpOneFolder
            // 
            this.btnMoveUpOneFolder.ImageIndex = 2;
            this.btnMoveUpOneFolder.ImageList = this.imlButtons;
            this.btnMoveUpOneFolder.Location = new System.Drawing.Point(201, 19);
            this.btnMoveUpOneFolder.Name = "btnMoveUpOneFolder";
            this.btnMoveUpOneFolder.Size = new System.Drawing.Size(23, 23);
            this.btnMoveUpOneFolder.TabIndex = 15;
            this.btnMoveUpOneFolder.UseVisualStyleBackColor = true;
            this.btnMoveUpOneFolder.Click += new System.EventHandler(this.btnMoveUpOneFolder_Click);
            // 
            // imlButtons
            // 
            this.imlButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlButtons.ImageStream")));
            this.imlButtons.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imlButtons.Images.SetKeyName(0, "TransferToLeftArrow.bmp");
            this.imlButtons.Images.SetKeyName(1, "TransferToRightArrow.bmp");
            this.imlButtons.Images.SetKeyName(2, "UpFolder.bmp");
            this.imlButtons.Images.SetKeyName(3, "MoveUpArrow.bmp");
            this.imlButtons.Images.SetKeyName(4, "MoveDownArrow.bmp");
            // 
            // cboCurrentPath
            // 
            this.cboCurrentPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboCurrentPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.cboCurrentPath.FormattingEnabled = true;
            this.cboCurrentPath.Location = new System.Drawing.Point(10, 19);
            this.cboCurrentPath.Name = "cboCurrentPath";
            this.cboCurrentPath.Size = new System.Drawing.Size(185, 21);
            this.cboCurrentPath.TabIndex = 0;
            this.cboCurrentPath.Leave += new System.EventHandler(this.cboCurrentPath_Leave);
            this.cboCurrentPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboCurrentPath_KeyDown);
            // 
            // btnTransferRight
            // 
            this.btnTransferRight.ImageIndex = 1;
            this.btnTransferRight.ImageList = this.imlButtons;
            this.btnTransferRight.Location = new System.Drawing.Point(254, 178);
            this.btnTransferRight.Name = "btnTransferRight";
            this.btnTransferRight.Size = new System.Drawing.Size(23, 23);
            this.btnTransferRight.TabIndex = 6;
            this.btnTransferRight.UseVisualStyleBackColor = true;
            this.btnTransferRight.Click += new System.EventHandler(this.btnTransferRight_Click);
            // 
            // btnTransferLeft
            // 
            this.btnTransferLeft.ImageIndex = 0;
            this.btnTransferLeft.ImageList = this.imlButtons;
            this.btnTransferLeft.Location = new System.Drawing.Point(254, 207);
            this.btnTransferLeft.Name = "btnTransferLeft";
            this.btnTransferLeft.Size = new System.Drawing.Size(23, 23);
            this.btnTransferLeft.TabIndex = 7;
            this.btnTransferLeft.UseVisualStyleBackColor = true;
            this.btnTransferLeft.Click += new System.EventHandler(this.btnTransferLeft_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(384, 328);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(465, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lvwSelectedFolders
            // 
            this.lvwSelectedFolders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.lvwSelectedFolders.FullRowSelect = true;
            this.lvwSelectedFolders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwSelectedFolders.HideSelection = false;
            this.lvwSelectedFolders.Location = new System.Drawing.Point(285, 109);
            this.lvwSelectedFolders.Name = "lvwSelectedFolders";
            this.lvwSelectedFolders.Size = new System.Drawing.Size(226, 213);
            this.lvwSelectedFolders.TabIndex = 10;
            this.lvwSelectedFolders.UseCompatibleStateImageBehavior = false;
            this.lvwSelectedFolders.View = System.Windows.Forms.View.Details;
            this.lvwSelectedFolders.SelectedIndexChanged += new System.EventHandler(this.lvwSelectedFolders_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(282, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Selected folders:";
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.ImageIndex = 4;
            this.btnMoveDown.ImageList = this.imlButtons;
            this.btnMoveDown.Location = new System.Drawing.Point(517, 138);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(23, 23);
            this.btnMoveDown.TabIndex = 13;
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.ImageIndex = 3;
            this.btnMoveUp.ImageList = this.imlButtons;
            this.btnMoveUp.Location = new System.Drawing.Point(517, 109);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 23);
            this.btnMoveUp.TabIndex = 12;
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 222;
            // 
            // LuaEditFolderSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(548, 358);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvwSelectedFolders);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnTransferLeft);
            this.Controls.Add(this.btnTransferRight);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDeleteFolderSet);
            this.Controls.Add(this.btnApplyFolderSet);
            this.Controls.Add(this.cboFolderSets);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LuaEditFolderSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LuaEditFolderSelector";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboFolderSets;
        private System.Windows.Forms.Button btnApplyFolderSet;
        private System.Windows.Forms.Button btnDeleteFolderSet;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTransferRight;
        private System.Windows.Forms.ImageList imlButtons;
        private System.Windows.Forms.Button btnTransferLeft;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvwSelectedFolders;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.ListView lvwAvailableFolders;
        private System.Windows.Forms.Button btnMoveUpOneFolder;
        private System.Windows.Forms.ComboBox cboCurrentPath;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}