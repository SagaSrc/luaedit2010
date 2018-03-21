namespace LuaEdit.Forms
{
    partial class CallStackDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CallStackDlg));
            this.tlvwCallStack = new LuaEdit.Controls.LuaEditTreeListView();
            this.colIcon = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colName = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colLanguage = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colSource = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.imlCallStack = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tlvwCallStack
            // 
            this.tlvwCallStack.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.tlvwCallStack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvwCallStack.Columns.AddRange(new DotNetLib.Controls.TreeListViewColumnHeader[] {
            this.colIcon,
            this.colName,
            this.colLanguage,
            this.colSource});
            this.tlvwCallStack.DefaultItemHeight = 16;
            this.tlvwCallStack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlvwCallStack.FullRowSelect = true;
            this.tlvwCallStack.GridLines = ((DotNetLib.Controls.GridLines)((DotNetLib.Controls.GridLines.Horizontal | DotNetLib.Controls.GridLines.Vertical)));
            this.tlvwCallStack.HeaderHeight = 16;
            this.tlvwCallStack.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvwCallStack.Location = new System.Drawing.Point(0, 0);
            this.tlvwCallStack.Name = "tlvwCallStack";
            this.tlvwCallStack.Size = new System.Drawing.Size(427, 262);
            this.tlvwCallStack.SmallImageList = this.imlCallStack;
            this.tlvwCallStack.TabIndex = 0;
            this.tlvwCallStack.VisualStyles = false;
            // 
            // colIcon
            // 
            this.colIcon.CustomSortTag = null;
            this.colIcon.Tag = null;
            this.colIcon.Width = 18;
            // 
            // colName
            // 
            this.colName.CustomSortTag = null;
            this.colName.DisplayIndex = 1;
            this.colName.Tag = null;
            this.colName.Text = "Name";
            this.colName.Width = 150;
            // 
            // colLanguage
            // 
            this.colLanguage.CustomSortTag = null;
            this.colLanguage.DisplayIndex = 2;
            this.colLanguage.Tag = null;
            this.colLanguage.Text = "Language";
            this.colLanguage.Width = 70;
            // 
            // colSource
            // 
            this.colSource.CustomSortTag = null;
            this.colSource.DisplayIndex = 3;
            this.colSource.Tag = null;
            this.colSource.Text = "Source";
            this.colSource.Width = 200;
            // 
            // imlCallStack
            // 
            this.imlCallStack.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlCallStack.ImageStream")));
            this.imlCallStack.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imlCallStack.Images.SetKeyName(0, "LineBreakMarker.bmp");
            this.imlCallStack.Images.SetKeyName(1, "LineBreakMarkerOverBreakpoint.bmp");
            this.imlCallStack.Images.SetKeyName(2, "LineBreakMarkerOverDisableBreakpoint.bmp");
            this.imlCallStack.Images.SetKeyName(3, "CallStackCurrentLevel.bmp");
            // 
            // CallStackDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 262);
            this.Controls.Add(this.tlvwCallStack);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CallStackDlg";
            this.TabText = "Call Stack";
            this.Text = "Call Stack";
            this.ResumeLayout(false);

        }

        #endregion

        private LuaEdit.Controls.LuaEditTreeListView tlvwCallStack;
        private DotNetLib.Controls.TreeListViewColumnHeader colName;
        private DotNetLib.Controls.TreeListViewColumnHeader colIcon;
        private DotNetLib.Controls.TreeListViewColumnHeader colSource;
        private DotNetLib.Controls.TreeListViewColumnHeader colLanguage;
        private System.Windows.Forms.ImageList imlCallStack;
    }
}