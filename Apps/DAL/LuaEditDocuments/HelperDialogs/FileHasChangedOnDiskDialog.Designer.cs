namespace LuaEdit.HelperDialogs
{
    partial class FileHasChangedOnDiskDialog
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
            this.btnYes = new System.Windows.Forms.Button();
            this.btnYesToAll = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnNoToAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(385, 66);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "This file has been modified";
            // 
            // btnYes
            // 
            this.btnYes.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(44, 78);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // btnYesToAll
            // 
            this.btnYesToAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnYesToAll.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnYesToAll.Location = new System.Drawing.Point(125, 78);
            this.btnYesToAll.Name = "btnYesToAll";
            this.btnYesToAll.Size = new System.Drawing.Size(75, 23);
            this.btnYesToAll.TabIndex = 2;
            this.btnYesToAll.Text = "Yes to All";
            this.btnYesToAll.UseVisualStyleBackColor = true;
            // 
            // btnNo
            // 
            this.btnNo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(206, 78);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 3;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // btnNoToAll
            // 
            this.btnNoToAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNoToAll.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnNoToAll.Location = new System.Drawing.Point(287, 78);
            this.btnNoToAll.Name = "btnNoToAll";
            this.btnNoToAll.Size = new System.Drawing.Size(75, 23);
            this.btnNoToAll.TabIndex = 4;
            this.btnNoToAll.Text = "No to All";
            this.btnNoToAll.UseVisualStyleBackColor = true;
            // 
            // FileHasChangedOnDiskDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 113);
            this.Controls.Add(this.btnNoToAll);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYesToAll);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FileHasChangedOnDiskDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LuaEdit";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnYesToAll;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnNoToAll;
    }
}