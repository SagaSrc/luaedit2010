namespace LuaEdit.Documents.ProjectProperties
{
    partial class ProjectPropertiesMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectPropertiesMain));
            this.projectPropertiesPageControl = new LuaEdit.Controls.LuaEditPageControl();
            this.SuspendLayout();
            // 
            // projectPropertiesPageControl
            // 
            this.projectPropertiesPageControl.BackGroundEndColor = System.Drawing.SystemColors.Control;
            this.projectPropertiesPageControl.BackGroundStartColor = System.Drawing.Color.White;
            this.projectPropertiesPageControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectPropertiesPageControl.Location = new System.Drawing.Point(0, 0);
            this.projectPropertiesPageControl.Name = "projectPropertiesPageControl";
            this.projectPropertiesPageControl.Size = new System.Drawing.Size(418, 340);
            this.projectPropertiesPageControl.TabIndex = 0;
            // 
            // ProjectPropertiesMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 340);
            this.Controls.Add(this.projectPropertiesPageControl);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.HideOnClose = true;
            this.Name = "ProjectPropertiesMain";
            this.ShowStatusIcon = true;
            this.StatusIcon = ((System.Drawing.Icon)(resources.GetObject("$this.StatusIcon")));
            this.TabText = "ProjectPropertiesMain";
            this.Text = "ProjectPropertiesMain";
            this.ResumeLayout(false);

        }

        #endregion

        private LuaEdit.Controls.LuaEditPageControl projectPropertiesPageControl;
    }
}