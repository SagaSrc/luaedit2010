namespace LuaEdit.HelperDialogs
{
    partial class NewItem
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("LuaEdit Installed Templates", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("My Templates", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewItem));
            this.lvwNewItems = new System.Windows.Forms.ListView();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSmallIcons = new System.Windows.Forms.RadioButton();
            this.btnLargeIcons = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblSolution = new System.Windows.Forms.Label();
            this.cboSolution = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwNewItems
            // 
            listViewGroup1.Header = "LuaEdit Installed Templates";
            listViewGroup1.Name = "lvgrpLuaEditTemplates";
            listViewGroup2.Header = "My Templates";
            listViewGroup2.Name = "lvgrpUserTemplates";
            this.lvwNewItems.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.lvwNewItems.HideSelection = false;
            this.lvwNewItems.Location = new System.Drawing.Point(12, 35);
            this.lvwNewItems.MultiSelect = false;
            this.lvwNewItems.Name = "lvwNewItems";
            this.lvwNewItems.Size = new System.Drawing.Size(650, 252);
            this.lvwNewItems.TabIndex = 8;
            this.lvwNewItems.UseCompatibleStateImageBehavior = false;
            this.lvwNewItems.SelectedIndexChanged += new System.EventHandler(this.lvwNewItems_SelectedIndexChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDescription.Location = new System.Drawing.Point(12, 290);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(650, 20);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Description";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(119, 319);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(462, 20);
            this.txtFileName.TabIndex = 0;
            this.txtFileName.Text = "Temp.lua";
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Templates:";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "SmallIconsListing.bmp");
            this.imageList1.Images.SetKeyName(1, "LargeIconsListing.bmp");
            // 
            // btnSmallIcons
            // 
            this.btnSmallIcons.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSmallIcons.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSmallIcons.FlatAppearance.BorderSize = 0;
            this.btnSmallIcons.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(189)))), ((int)(((byte)(210)))));
            this.btnSmallIcons.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(189)))), ((int)(((byte)(210)))));
            this.btnSmallIcons.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(189)))), ((int)(((byte)(210)))));
            this.btnSmallIcons.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSmallIcons.ImageKey = "SmallIconsListing.bmp";
            this.btnSmallIcons.ImageList = this.imageList1;
            this.btnSmallIcons.Location = new System.Drawing.Point(27, 4);
            this.btnSmallIcons.Name = "btnSmallIcons";
            this.btnSmallIcons.Size = new System.Drawing.Size(18, 18);
            this.btnSmallIcons.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btnSmallIcons, "Small Icons");
            this.btnSmallIcons.UseVisualStyleBackColor = false;
            this.btnSmallIcons.CheckedChanged += new System.EventHandler(this.btnSmallIcons_CheckedChanged);
            // 
            // btnLargeIcons
            // 
            this.btnLargeIcons.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnLargeIcons.Checked = true;
            this.btnLargeIcons.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnLargeIcons.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(189)))), ((int)(((byte)(210)))));
            this.btnLargeIcons.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(189)))), ((int)(((byte)(210)))));
            this.btnLargeIcons.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(189)))), ((int)(((byte)(210)))));
            this.btnLargeIcons.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLargeIcons.ImageKey = "LargeIconsListing.bmp";
            this.btnLargeIcons.ImageList = this.imageList1;
            this.btnLargeIcons.Location = new System.Drawing.Point(3, 3);
            this.btnLargeIcons.Name = "btnLargeIcons";
            this.btnLargeIcons.Size = new System.Drawing.Size(18, 18);
            this.btnLargeIcons.TabIndex = 6;
            this.btnLargeIcons.TabStop = true;
            this.toolTip1.SetToolTip(this.btnLargeIcons, "Large Icons");
            this.btnLargeIcons.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(12, 399);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(650, 2);
            this.label2.TabIndex = 6;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 322);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "Name:";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAdd.Location = new System.Drawing.Point(506, 409);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(587, 409);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSmallIcons);
            this.panel1.Controls.Add(this.btnLargeIcons);
            this.panel1.Location = new System.Drawing.Point(616, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(51, 30);
            this.panel1.TabIndex = 14;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(12, 348);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 15;
            this.lblLocation.Text = "Location:";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(119, 345);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(462, 20);
            this.txtLocation.TabIndex = 1;
            this.txtLocation.Text = "C:\\";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(587, 344);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.Location = new System.Drawing.Point(12, 374);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(48, 13);
            this.lblSolution.TabIndex = 18;
            this.lblSolution.Text = "Solution:";
            // 
            // cboSolution
            // 
            this.cboSolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSolution.FormattingEnabled = true;
            this.cboSolution.Items.AddRange(new object[] {
            "Create New Solution",
            "Add to Solution"});
            this.cboSolution.Location = new System.Drawing.Point(119, 371);
            this.cboSolution.Name = "cboSolution";
            this.cboSolution.Size = new System.Drawing.Size(260, 21);
            this.cboSolution.TabIndex = 3;
            // 
            // NewItem
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(674, 440);
            this.Controls.Add(this.cboSolution);
            this.Controls.Add(this.lblSolution);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lvwNewItems);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewItem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Item Dialog";
            this.Load += new System.EventHandler(this.NewItem_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewItem_FormClosing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwNewItems;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton btnSmallIcons;
        private System.Windows.Forms.RadioButton btnLargeIcons;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblSolution;
        private System.Windows.Forms.ComboBox cboSolution;
    }
}