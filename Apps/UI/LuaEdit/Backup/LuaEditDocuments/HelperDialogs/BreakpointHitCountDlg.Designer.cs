namespace LuaEdit.HelperDialogs
{
    partial class BreakpointHitCountDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboHitCountCondition = new System.Windows.Forms.ComboBox();
            this.nudHitCount = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudHitCount)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(382, 38);
            this.label1.TabIndex = 3;
            this.label1.Text = "A breakpoint is hit when the breakpoint location is reached and the condition is " +
                "statisfied. The hit count is the number of times the breakpoint has been hit.";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(316, 119);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(235, 119);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "When the breakpoint is &hit:";
            // 
            // cboHitCountCondition
            // 
            this.cboHitCountCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHitCountCondition.FormattingEnabled = true;
            this.cboHitCountCondition.Items.AddRange(new object[] {
            "break always",
            "break when the hit count is equal to",
            "break when the hit count is a multiple of",
            "break when the hit count is greater than or equal to"});
            this.cboHitCountCondition.Location = new System.Drawing.Point(15, 74);
            this.cboHitCountCondition.Name = "cboHitCountCondition";
            this.cboHitCountCondition.Size = new System.Drawing.Size(310, 21);
            this.cboHitCountCondition.TabIndex = 1;
            this.cboHitCountCondition.SelectedIndexChanged += new System.EventHandler(this.cboHitCountCondition_SelectedIndexChanged);
            // 
            // nudHitCount
            // 
            this.nudHitCount.Location = new System.Drawing.Point(331, 75);
            this.nudHitCount.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudHitCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHitCount.Name = "nudHitCount";
            this.nudHitCount.Size = new System.Drawing.Size(60, 20);
            this.nudHitCount.TabIndex = 5;
            this.nudHitCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // BreakpointHitCountDlg
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(399, 150);
            this.Controls.Add(this.nudHitCount);
            this.Controls.Add(this.cboHitCountCondition);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BreakpointHitCountDlg";
            this.Text = "Breakpoint Hit Count";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BreakpointHitCountDlg_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudHitCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboHitCountCondition;
        private System.Windows.Forms.NumericUpDown nudHitCount;
    }
}