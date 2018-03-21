namespace LuaEdit.HelperDialogs
{
    partial class FileIsReadOnlyDialog
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnOverwrite = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(460, 65);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "The file {0} cannot be saved because it is write-protected.";
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSaveAs.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnSaveAs.Location = new System.Drawing.Point(119, 77);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAs.TabIndex = 1;
            this.btnSaveAs.Text = "Save &As...";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            // 
            // btnOverwrite
            // 
            this.btnOverwrite.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOverwrite.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnOverwrite.Location = new System.Drawing.Point(200, 77);
            this.btnOverwrite.Name = "btnOverwrite";
            this.btnOverwrite.Size = new System.Drawing.Size(75, 23);
            this.btnOverwrite.TabIndex = 2;
            this.btnOverwrite.Text = "&Overwrite";
            this.btnOverwrite.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(281, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FileIsReadOnlyDialog
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(484, 112);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOverwrite);
            this.Controls.Add(this.btnSaveAs);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileIsReadOnlyDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save of Read-Only File";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Button btnOverwrite;
        private System.Windows.Forms.Button btnCancel;
    }
}