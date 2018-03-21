namespace LuaEdit.HelperDialogs
{
    partial class AttachToMachineDialog
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
            this.btnAttach = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboMachine = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseMachine = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAttach
            // 
            this.btnAttach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAttach.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAttach.Enabled = false;
            this.btnAttach.Location = new System.Drawing.Point(306, 77);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(75, 23);
            this.btnAttach.TabIndex = 3;
            this.btnAttach.Text = "&Attach";
            this.btnAttach.UseVisualStyleBackColor = true;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(387, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cboMachine
            // 
            this.cboMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMachine.FormattingEnabled = true;
            this.cboMachine.Location = new System.Drawing.Point(76, 8);
            this.cboMachine.Name = "cboMachine";
            this.cboMachine.Size = new System.Drawing.Size(356, 21);
            this.cboMachine.Sorted = true;
            this.cboMachine.TabIndex = 0;
            this.cboMachine.TextChanged += new System.EventHandler(this.cboMachine_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Machine:";
            // 
            // btnBrowseMachine
            // 
            this.btnBrowseMachine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseMachine.Location = new System.Drawing.Point(438, 9);
            this.btnBrowseMachine.Name = "btnBrowseMachine";
            this.btnBrowseMachine.Size = new System.Drawing.Size(24, 20);
            this.btnBrowseMachine.TabIndex = 1;
            this.btnBrowseMachine.Text = "...";
            this.btnBrowseMachine.UseVisualStyleBackColor = true;
            this.btnBrowseMachine.Click += new System.EventHandler(this.btnBrowseMachine_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Port:";
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(76, 35);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(120, 20);
            this.nudPort.TabIndex = 2;
            this.nudPort.ValueChanged += new System.EventHandler(this.nudPort_ValueChanged);
            // 
            // AttachToMachineDialog
            // 
            this.AcceptButton = this.btnAttach;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(470, 108);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBrowseMachine);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboMachine);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAttach);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AttachToMachineDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Attach to Machine";
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAttach;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cboMachine;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseMachine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudPort;
    }
}