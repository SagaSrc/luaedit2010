namespace LuaEdit.Forms
{
    partial class FindInFilesControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindInFilesControl));
            this.imlQuickFind = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.folderBrowserDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.pnlQuickFind = new LuaEdit.Controls.LuaEditBorderPanel();
            this.chkIncludeSubFolders = new System.Windows.Forms.CheckBox();
            this.lblResultOptions = new System.Windows.Forms.Label();
            this.btnShowHideResultOptions = new System.Windows.Forms.Button();
            this.pnlResultOptions = new LuaEdit.Controls.LuaEditBorderPanel();
            this.radResultWin2 = new System.Windows.Forms.RadioButton();
            this.radResultWin1 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.chkDisplayFileNamesOnly = new System.Windows.Forms.CheckBox();
            this.btnBrowseLookIn = new System.Windows.Forms.Button();
            this.btnFindAll = new System.Windows.Forms.Button();
            this.lblFindOptions = new System.Windows.Forms.Label();
            this.btnShowHideFindOptions = new System.Windows.Forms.Button();
            this.btnExpressionBuilder = new System.Windows.Forms.Button();
            this.cboLookIn = new System.Windows.Forms.ComboBox();
            this.cboFindWhat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlFindOptions = new LuaEdit.Controls.LuaEditBorderPanel();
            this.cboFileTypes = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkUseRegEx = new System.Windows.Forms.CheckBox();
            this.chkMatchWholeWord = new System.Windows.Forms.CheckBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.pnlQuickFind.SuspendLayout();
            this.pnlResultOptions.SuspendLayout();
            this.pnlFindOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // imlQuickFind
            // 
            this.imlQuickFind.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlQuickFind.ImageStream")));
            this.imlQuickFind.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imlQuickFind.Images.SetKeyName(0, "RightArrow.bmp");
            this.imlQuickFind.Images.SetKeyName(1, "Expand.bmp");
            this.imlQuickFind.Images.SetKeyName(2, "Collapse.bmp");
            this.imlQuickFind.Images.SetKeyName(3, "Browse.bmp");
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // pnlQuickFind
            // 
            this.pnlQuickFind.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.pnlQuickFind.BorderStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.pnlQuickFind.Controls.Add(this.chkIncludeSubFolders);
            this.pnlQuickFind.Controls.Add(this.lblResultOptions);
            this.pnlQuickFind.Controls.Add(this.btnShowHideResultOptions);
            this.pnlQuickFind.Controls.Add(this.pnlResultOptions);
            this.pnlQuickFind.Controls.Add(this.btnBrowseLookIn);
            this.pnlQuickFind.Controls.Add(this.btnFindAll);
            this.pnlQuickFind.Controls.Add(this.lblFindOptions);
            this.pnlQuickFind.Controls.Add(this.btnShowHideFindOptions);
            this.pnlQuickFind.Controls.Add(this.btnExpressionBuilder);
            this.pnlQuickFind.Controls.Add(this.cboLookIn);
            this.pnlQuickFind.Controls.Add(this.cboFindWhat);
            this.pnlQuickFind.Controls.Add(this.label2);
            this.pnlQuickFind.Controls.Add(this.label1);
            this.pnlQuickFind.Controls.Add(this.pnlFindOptions);
            this.pnlQuickFind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlQuickFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlQuickFind.Location = new System.Drawing.Point(0, 0);
            this.pnlQuickFind.Name = "pnlQuickFind";
            this.pnlQuickFind.ShowBottomBorder = true;
            this.pnlQuickFind.ShowLeftBorder = true;
            this.pnlQuickFind.ShowRightBorder = true;
            this.pnlQuickFind.ShowTopBorder = true;
            this.pnlQuickFind.Size = new System.Drawing.Size(300, 425);
            this.pnlQuickFind.TabIndex = 4;
            // 
            // chkIncludeSubFolders
            // 
            this.chkIncludeSubFolders.AutoSize = true;
            this.chkIncludeSubFolders.Location = new System.Drawing.Point(9, 92);
            this.chkIncludeSubFolders.Name = "chkIncludeSubFolders";
            this.chkIncludeSubFolders.Size = new System.Drawing.Size(115, 17);
            this.chkIncludeSubFolders.TabIndex = 6;
            this.chkIncludeSubFolders.Text = "Include su&b-folders";
            this.chkIncludeSubFolders.UseVisualStyleBackColor = true;
            // 
            // lblResultOptions
            // 
            this.lblResultOptions.AutoSize = true;
            this.lblResultOptions.Location = new System.Drawing.Point(24, 276);
            this.lblResultOptions.Name = "lblResultOptions";
            this.lblResultOptions.Size = new System.Drawing.Size(74, 13);
            this.lblResultOptions.TabIndex = 13;
            this.lblResultOptions.Text = "Re&sult options";
            // 
            // btnShowHideResultOptions
            // 
            this.btnShowHideResultOptions.Font = new System.Drawing.Font("Elephant", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowHideResultOptions.ImageIndex = 2;
            this.btnShowHideResultOptions.ImageList = this.imlQuickFind;
            this.btnShowHideResultOptions.Location = new System.Drawing.Point(8, 276);
            this.btnShowHideResultOptions.Margin = new System.Windows.Forms.Padding(0);
            this.btnShowHideResultOptions.Name = "btnShowHideResultOptions";
            this.btnShowHideResultOptions.Size = new System.Drawing.Size(16, 16);
            this.btnShowHideResultOptions.TabIndex = 11;
            this.btnShowHideResultOptions.UseVisualStyleBackColor = true;
            this.btnShowHideResultOptions.Click += new System.EventHandler(this.btnShowHideResultOptions_Click);
            // 
            // pnlResultOptions
            // 
            this.pnlResultOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlResultOptions.BorderColor = System.Drawing.Color.Black;
            this.pnlResultOptions.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.pnlResultOptions.Controls.Add(this.radResultWin2);
            this.pnlResultOptions.Controls.Add(this.radResultWin1);
            this.pnlResultOptions.Controls.Add(this.label5);
            this.pnlResultOptions.Controls.Add(this.chkDisplayFileNamesOnly);
            this.pnlResultOptions.Location = new System.Drawing.Point(8, 281);
            this.pnlResultOptions.Name = "pnlResultOptions";
            this.pnlResultOptions.ShowBottomBorder = true;
            this.pnlResultOptions.ShowLeftBorder = true;
            this.pnlResultOptions.ShowRightBorder = true;
            this.pnlResultOptions.ShowTopBorder = true;
            this.pnlResultOptions.Size = new System.Drawing.Size(285, 110);
            this.pnlResultOptions.TabIndex = 12;
            // 
            // radResultWin2
            // 
            this.radResultWin2.AutoSize = true;
            this.radResultWin2.Location = new System.Drawing.Point(18, 58);
            this.radResultWin2.Name = "radResultWin2";
            this.radResultWin2.Size = new System.Drawing.Size(126, 17);
            this.radResultWin2.TabIndex = 16;
            this.radResultWin2.Text = "Find results &2 window";
            this.radResultWin2.UseVisualStyleBackColor = true;
            // 
            // radResultWin1
            // 
            this.radResultWin1.AutoSize = true;
            this.radResultWin1.Location = new System.Drawing.Point(18, 39);
            this.radResultWin1.Name = "radResultWin1";
            this.radResultWin1.Size = new System.Drawing.Size(126, 17);
            this.radResultWin1.TabIndex = 15;
            this.radResultWin1.Text = "Find results &1 window";
            this.radResultWin1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "List results in:";
            // 
            // chkDisplayFileNamesOnly
            // 
            this.chkDisplayFileNamesOnly.AutoSize = true;
            this.chkDisplayFileNamesOnly.Location = new System.Drawing.Point(9, 81);
            this.chkDisplayFileNamesOnly.Name = "chkDisplayFileNamesOnly";
            this.chkDisplayFileNamesOnly.Size = new System.Drawing.Size(132, 17);
            this.chkDisplayFileNamesOnly.TabIndex = 17;
            this.chkDisplayFileNamesOnly.Text = "&Display file names only";
            this.chkDisplayFileNamesOnly.UseVisualStyleBackColor = true;
            // 
            // btnBrowseLookIn
            // 
            this.btnBrowseLookIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseLookIn.ImageIndex = 3;
            this.btnBrowseLookIn.ImageList = this.imlQuickFind;
            this.btnBrowseLookIn.Location = new System.Drawing.Point(273, 68);
            this.btnBrowseLookIn.Name = "btnBrowseLookIn";
            this.btnBrowseLookIn.Size = new System.Drawing.Size(20, 21);
            this.btnBrowseLookIn.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnBrowseLookIn, "Choose Search Folder");
            this.btnBrowseLookIn.UseVisualStyleBackColor = true;
            this.btnBrowseLookIn.Click += new System.EventHandler(this.btnBrowseLookIn_Click);
            // 
            // btnFindAll
            // 
            this.btnFindAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindAll.Location = new System.Drawing.Point(194, 396);
            this.btnFindAll.Name = "btnFindAll";
            this.btnFindAll.Size = new System.Drawing.Size(100, 23);
            this.btnFindAll.TabIndex = 18;
            this.btnFindAll.Text = "&Find All";
            this.btnFindAll.UseVisualStyleBackColor = true;
            this.btnFindAll.Click += new System.EventHandler(this.btnFindAll_Click);
            // 
            // lblFindOptions
            // 
            this.lblFindOptions.AutoSize = true;
            this.lblFindOptions.Location = new System.Drawing.Point(24, 112);
            this.lblFindOptions.Name = "lblFindOptions";
            this.lblFindOptions.Size = new System.Drawing.Size(64, 13);
            this.lblFindOptions.TabIndex = 7;
            this.lblFindOptions.Text = "Find &options";
            // 
            // btnShowHideFindOptions
            // 
            this.btnShowHideFindOptions.Font = new System.Drawing.Font("Elephant", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowHideFindOptions.ImageIndex = 2;
            this.btnShowHideFindOptions.ImageList = this.imlQuickFind;
            this.btnShowHideFindOptions.Location = new System.Drawing.Point(8, 113);
            this.btnShowHideFindOptions.Margin = new System.Windows.Forms.Padding(0);
            this.btnShowHideFindOptions.Name = "btnShowHideFindOptions";
            this.btnShowHideFindOptions.Size = new System.Drawing.Size(16, 16);
            this.btnShowHideFindOptions.TabIndex = 3;
            this.btnShowHideFindOptions.UseVisualStyleBackColor = true;
            this.btnShowHideFindOptions.Click += new System.EventHandler(this.btnShowHideFindOptions_Click);
            // 
            // btnExpressionBuilder
            // 
            this.btnExpressionBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpressionBuilder.ImageIndex = 0;
            this.btnExpressionBuilder.ImageList = this.imlQuickFind;
            this.btnExpressionBuilder.Location = new System.Drawing.Point(273, 26);
            this.btnExpressionBuilder.Name = "btnExpressionBuilder";
            this.btnExpressionBuilder.Size = new System.Drawing.Size(20, 21);
            this.btnExpressionBuilder.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnExpressionBuilder, "Expression Builder");
            this.btnExpressionBuilder.UseVisualStyleBackColor = true;
            this.btnExpressionBuilder.Click += new System.EventHandler(this.btnExpressionBuilder_Click);
            // 
            // cboLookIn
            // 
            this.cboLookIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLookIn.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLookIn.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLookIn.FormattingEnabled = true;
            this.cboLookIn.Location = new System.Drawing.Point(9, 68);
            this.cboLookIn.MaxDropDownItems = 12;
            this.cboLookIn.Name = "cboLookIn";
            this.cboLookIn.Size = new System.Drawing.Size(258, 21);
            this.cboLookIn.TabIndex = 4;
            this.cboLookIn.SelectedIndexChanged += new System.EventHandler(this.cboLookIn_SelectedIndexChanged);
            this.cboLookIn.TextChanged += new System.EventHandler(this.cboLookIn_TextChanged);
            // 
            // cboFindWhat
            // 
            this.cboFindWhat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFindWhat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFindWhat.FormattingEnabled = true;
            this.cboFindWhat.Location = new System.Drawing.Point(8, 26);
            this.cboFindWhat.MaxDropDownItems = 12;
            this.cboFindWhat.Name = "cboFindWhat";
            this.cboFindWhat.Size = new System.Drawing.Size(259, 21);
            this.cboFindWhat.TabIndex = 1;
            this.cboFindWhat.TextChanged += new System.EventHandler(this.cboFindWhat_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "&Look in:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fi&nd what:";
            // 
            // pnlFindOptions
            // 
            this.pnlFindOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFindOptions.BorderColor = System.Drawing.Color.Black;
            this.pnlFindOptions.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.pnlFindOptions.Controls.Add(this.cboFileTypes);
            this.pnlFindOptions.Controls.Add(this.label4);
            this.pnlFindOptions.Controls.Add(this.chkUseRegEx);
            this.pnlFindOptions.Controls.Add(this.chkMatchWholeWord);
            this.pnlFindOptions.Controls.Add(this.chkMatchCase);
            this.pnlFindOptions.Location = new System.Drawing.Point(8, 118);
            this.pnlFindOptions.Name = "pnlFindOptions";
            this.pnlFindOptions.ShowBottomBorder = true;
            this.pnlFindOptions.ShowLeftBorder = true;
            this.pnlFindOptions.ShowRightBorder = true;
            this.pnlFindOptions.ShowTopBorder = true;
            this.pnlFindOptions.Size = new System.Drawing.Size(285, 150);
            this.pnlFindOptions.TabIndex = 8;
            // 
            // cboFileTypes
            // 
            this.cboFileTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFileTypes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFileTypes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFileTypes.FormattingEnabled = true;
            this.cboFileTypes.Location = new System.Drawing.Point(10, 116);
            this.cboFileTypes.MaxDropDownItems = 12;
            this.cboFileTypes.Name = "cboFileTypes";
            this.cboFileTypes.Size = new System.Drawing.Size(265, 21);
            this.cboFileTypes.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Look at these file &types:";
            // 
            // chkUseRegEx
            // 
            this.chkUseRegEx.AutoSize = true;
            this.chkUseRegEx.Location = new System.Drawing.Point(10, 70);
            this.chkUseRegEx.Name = "chkUseRegEx";
            this.chkUseRegEx.Size = new System.Drawing.Size(138, 17);
            this.chkUseRegEx.TabIndex = 10;
            this.chkUseRegEx.Text = "Us&e regular expressions";
            this.chkUseRegEx.UseVisualStyleBackColor = true;
            this.chkUseRegEx.CheckedChanged += new System.EventHandler(this.chkUseRegEx_CheckedChanged);
            // 
            // chkMatchWholeWord
            // 
            this.chkMatchWholeWord.AutoSize = true;
            this.chkMatchWholeWord.Location = new System.Drawing.Point(10, 47);
            this.chkMatchWholeWord.Name = "chkMatchWholeWord";
            this.chkMatchWholeWord.Size = new System.Drawing.Size(113, 17);
            this.chkMatchWholeWord.TabIndex = 9;
            this.chkMatchWholeWord.Text = "Match &whole word";
            this.chkMatchWholeWord.UseVisualStyleBackColor = true;
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(10, 24);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
            this.chkMatchCase.TabIndex = 8;
            this.chkMatchCase.Text = "Match &case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // FindInFilesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlQuickFind);
            this.Name = "FindInFilesControl";
            this.Size = new System.Drawing.Size(300, 425);
            this.pnlQuickFind.ResumeLayout(false);
            this.pnlQuickFind.PerformLayout();
            this.pnlResultOptions.ResumeLayout(false);
            this.pnlResultOptions.PerformLayout();
            this.pnlFindOptions.ResumeLayout(false);
            this.pnlFindOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LuaEdit.Controls.LuaEditBorderPanel pnlQuickFind;
        private System.Windows.Forms.ComboBox cboFindWhat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboLookIn;
        private System.Windows.Forms.Button btnExpressionBuilder;
        private System.Windows.Forms.ImageList imlQuickFind;
        private System.Windows.Forms.Button btnShowHideFindOptions;
        private LuaEdit.Controls.LuaEditBorderPanel pnlFindOptions;
        private System.Windows.Forms.Button btnFindAll;
        private System.Windows.Forms.CheckBox chkMatchCase;
        private System.Windows.Forms.CheckBox chkMatchWholeWord;
        private System.Windows.Forms.CheckBox chkUseRegEx;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnBrowseLookIn;
        private System.Windows.Forms.ComboBox cboFileTypes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnShowHideResultOptions;
        private LuaEdit.Controls.LuaEditBorderPanel pnlResultOptions;
        private System.Windows.Forms.CheckBox chkDisplayFileNamesOnly;
        private System.Windows.Forms.RadioButton radResultWin2;
        private System.Windows.Forms.RadioButton radResultWin1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblFindOptions;
        private System.Windows.Forms.Label lblResultOptions;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDlg;
        private System.Windows.Forms.CheckBox chkIncludeSubFolders;
    }
}
