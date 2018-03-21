namespace LuaEdit.Forms
{
    partial class QuickFindControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickFindControl));
            this.imlQuickFind = new System.Windows.Forms.ImageList(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnExpressionBuilder = new System.Windows.Forms.Button();
            this.pnlQuickFind = new LuaEdit.Controls.LuaEditBorderPanel();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.btnBookmarkAll = new System.Windows.Forms.Button();
            this.lblFindOptions = new System.Windows.Forms.Label();
            this.btnShowHideFindOptions = new System.Windows.Forms.Button();
            this.cboLookIn = new System.Windows.Forms.ComboBox();
            this.cboFindWhat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlFindOptions = new LuaEdit.Controls.LuaEditBorderPanel();
            this.chkUseRegEx = new System.Windows.Forms.CheckBox();
            this.chkSearchUp = new System.Windows.Forms.CheckBox();
            this.chkMatchWholeWord = new System.Windows.Forms.CheckBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.pnlQuickFind.SuspendLayout();
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
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // btnExpressionBuilder
            // 
            this.btnExpressionBuilder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpressionBuilder.ImageIndex = 0;
            this.btnExpressionBuilder.ImageList = this.imlQuickFind;
            this.btnExpressionBuilder.Location = new System.Drawing.Point(273, 26);
            this.btnExpressionBuilder.Name = "btnExpressionBuilder";
            this.btnExpressionBuilder.Size = new System.Drawing.Size(20, 21);
            this.btnExpressionBuilder.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnExpressionBuilder, "Expression Builder");
            this.btnExpressionBuilder.UseVisualStyleBackColor = true;
            this.btnExpressionBuilder.Click += new System.EventHandler(this.btnExpressionBuilder_Click);
            // 
            // pnlQuickFind
            // 
            this.pnlQuickFind.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.pnlQuickFind.BorderStyle = System.Windows.Forms.Border3DStyle.Flat;
            this.pnlQuickFind.Controls.Add(this.btnFindNext);
            this.pnlQuickFind.Controls.Add(this.btnBookmarkAll);
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
            this.pnlQuickFind.Size = new System.Drawing.Size(300, 265);
            this.pnlQuickFind.TabIndex = 4;
            // 
            // btnFindNext
            // 
            this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNext.Location = new System.Drawing.Point(87, 234);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(100, 23);
            this.btnFindNext.TabIndex = 4;
            this.btnFindNext.Text = "Find Next";
            this.btnFindNext.UseVisualStyleBackColor = true;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // btnBookmarkAll
            // 
            this.btnBookmarkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBookmarkAll.Location = new System.Drawing.Point(193, 234);
            this.btnBookmarkAll.Name = "btnBookmarkAll";
            this.btnBookmarkAll.Size = new System.Drawing.Size(100, 23);
            this.btnBookmarkAll.TabIndex = 5;
            this.btnBookmarkAll.Text = "Bookmark All";
            this.btnBookmarkAll.UseVisualStyleBackColor = true;
            this.btnBookmarkAll.Click += new System.EventHandler(this.btnBookmarkAll_Click);
            // 
            // lblFindOptions
            // 
            this.lblFindOptions.AutoSize = true;
            this.lblFindOptions.Location = new System.Drawing.Point(24, 97);
            this.lblFindOptions.Name = "lblFindOptions";
            this.lblFindOptions.Size = new System.Drawing.Size(64, 13);
            this.lblFindOptions.TabIndex = 9;
            this.lblFindOptions.Text = "Find options";
            // 
            // btnShowHideFindOptions
            // 
            this.btnShowHideFindOptions.Font = new System.Drawing.Font("Elephant", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowHideFindOptions.ImageIndex = 2;
            this.btnShowHideFindOptions.ImageList = this.imlQuickFind;
            this.btnShowHideFindOptions.Location = new System.Drawing.Point(8, 98);
            this.btnShowHideFindOptions.Margin = new System.Windows.Forms.Padding(0);
            this.btnShowHideFindOptions.Name = "btnShowHideFindOptions";
            this.btnShowHideFindOptions.Size = new System.Drawing.Size(16, 16);
            this.btnShowHideFindOptions.TabIndex = 3;
            this.btnShowHideFindOptions.UseVisualStyleBackColor = true;
            this.btnShowHideFindOptions.Click += new System.EventHandler(this.btnShowHideFindOptions_Click);
            // 
            // cboLookIn
            // 
            this.cboLookIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLookIn.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLookIn.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLookIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLookIn.FormattingEnabled = true;
            this.cboLookIn.Location = new System.Drawing.Point(9, 68);
            this.cboLookIn.MaxDropDownItems = 12;
            this.cboLookIn.Name = "cboLookIn";
            this.cboLookIn.Size = new System.Drawing.Size(284, 21);
            this.cboLookIn.TabIndex = 2;
            this.cboLookIn.SelectedIndexChanged += new System.EventHandler(this.cboLookIn_SelectedIndexChanged);
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
            this.cboFindWhat.TabIndex = 0;
            this.cboFindWhat.TextChanged += new System.EventHandler(this.cboFindWhat_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Look in:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find what:";
            // 
            // pnlFindOptions
            // 
            this.pnlFindOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFindOptions.BorderColor = System.Drawing.Color.Black;
            this.pnlFindOptions.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.pnlFindOptions.Controls.Add(this.chkUseRegEx);
            this.pnlFindOptions.Controls.Add(this.chkSearchUp);
            this.pnlFindOptions.Controls.Add(this.chkMatchWholeWord);
            this.pnlFindOptions.Controls.Add(this.chkMatchCase);
            this.pnlFindOptions.Location = new System.Drawing.Point(8, 103);
            this.pnlFindOptions.Name = "pnlFindOptions";
            this.pnlFindOptions.ShowBottomBorder = true;
            this.pnlFindOptions.ShowLeftBorder = true;
            this.pnlFindOptions.ShowRightBorder = true;
            this.pnlFindOptions.ShowTopBorder = true;
            this.pnlFindOptions.Size = new System.Drawing.Size(285, 125);
            this.pnlFindOptions.TabIndex = 8;
            // 
            // chkUseRegEx
            // 
            this.chkUseRegEx.AutoSize = true;
            this.chkUseRegEx.Location = new System.Drawing.Point(10, 93);
            this.chkUseRegEx.Name = "chkUseRegEx";
            this.chkUseRegEx.Size = new System.Drawing.Size(138, 17);
            this.chkUseRegEx.TabIndex = 3;
            this.chkUseRegEx.Text = "Use regular expressions";
            this.chkUseRegEx.UseVisualStyleBackColor = true;
            this.chkUseRegEx.CheckedChanged += new System.EventHandler(this.chkUseRegEx_CheckedChanged);
            // 
            // chkSearchUp
            // 
            this.chkSearchUp.AutoSize = true;
            this.chkSearchUp.Location = new System.Drawing.Point(10, 70);
            this.chkSearchUp.Name = "chkSearchUp";
            this.chkSearchUp.Size = new System.Drawing.Size(75, 17);
            this.chkSearchUp.TabIndex = 2;
            this.chkSearchUp.Text = "Search up";
            this.chkSearchUp.UseVisualStyleBackColor = true;
            this.chkSearchUp.CheckedChanged += new System.EventHandler(this.chkSearchUp_CheckedChanged);
            // 
            // chkMatchWholeWord
            // 
            this.chkMatchWholeWord.AutoSize = true;
            this.chkMatchWholeWord.Location = new System.Drawing.Point(10, 47);
            this.chkMatchWholeWord.Name = "chkMatchWholeWord";
            this.chkMatchWholeWord.Size = new System.Drawing.Size(113, 17);
            this.chkMatchWholeWord.TabIndex = 1;
            this.chkMatchWholeWord.Text = "Match whole word";
            this.chkMatchWholeWord.UseVisualStyleBackColor = true;
            this.chkMatchWholeWord.CheckedChanged += new System.EventHandler(this.chkMatchWholeWord_CheckedChanged);
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(10, 24);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
            this.chkMatchCase.TabIndex = 0;
            this.chkMatchCase.Text = "Match case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            this.chkMatchCase.CheckedChanged += new System.EventHandler(this.chkMatchCase_CheckedChanged);
            // 
            // QuickFindControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlQuickFind);
            this.Name = "QuickFindControl";
            this.Size = new System.Drawing.Size(300, 265);
            this.pnlQuickFind.ResumeLayout(false);
            this.pnlQuickFind.PerformLayout();
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
        private System.Windows.Forms.Label lblFindOptions;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.Button btnBookmarkAll;
        private System.Windows.Forms.CheckBox chkMatchCase;
        private System.Windows.Forms.CheckBox chkMatchWholeWord;
        private System.Windows.Forms.CheckBox chkUseRegEx;
        private System.Windows.Forms.CheckBox chkSearchUp;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
