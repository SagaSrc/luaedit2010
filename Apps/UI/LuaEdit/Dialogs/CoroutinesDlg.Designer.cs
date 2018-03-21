namespace LuaEdit.Forms
{
    partial class CoroutinesDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoroutinesDlg));
            this.tlvwThreads = new LuaEdit.Controls.LuaEditTreeListView();
            this.colIcon = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colID = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.colLocation = new DotNetLib.Controls.TreeListViewColumnHeader();
            this.imlCoroutines = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // tlvwThreads
            // 
            this.tlvwThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tlvwThreads.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.tlvwThreads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvwThreads.Columns.AddRange(new DotNetLib.Controls.TreeListViewColumnHeader[] {
            this.colIcon,
            this.colID,
            this.colLocation});
            this.tlvwThreads.DefaultItemHeight = 16;
            this.tlvwThreads.FullRowSelect = true;
            this.tlvwThreads.HeaderHeight = 16;
            this.tlvwThreads.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvwThreads.Location = new System.Drawing.Point(0, 2);
            this.tlvwThreads.Name = "tlvwThreads";
            this.tlvwThreads.Size = new System.Drawing.Size(376, 253);
            this.tlvwThreads.SmallImageList = this.imlCoroutines;
            this.tlvwThreads.TabIndex = 4;
            this.tlvwThreads.VisualStyles = false;
            // 
            // colIcon
            // 
            this.colIcon.CustomSortTag = null;
            this.colIcon.Tag = null;
            this.colIcon.Width = 18;
            // 
            // colID
            // 
            this.colID.CustomSortTag = null;
            this.colID.DisplayIndex = 1;
            this.colID.Tag = null;
            this.colID.Text = "ID";
            this.colID.Width = 75;
            // 
            // colLocation
            // 
            this.colLocation.CustomSortTag = null;
            this.colLocation.DisplayIndex = 2;
            this.colLocation.Tag = null;
            this.colLocation.Text = "Location";
            this.colLocation.Width = 300;
            // 
            // imlCoroutines
            // 
            this.imlCoroutines.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlCoroutines.ImageStream")));
            this.imlCoroutines.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imlCoroutines.Images.SetKeyName(0, "LineBreakMarker.bmp");
            this.imlCoroutines.Images.SetKeyName(1, "LineBreakMarkerOverBreakpoint.bmp");
            this.imlCoroutines.Images.SetKeyName(2, "LineBreakMarkerOverDisableBreakpoint.bmp");
            this.imlCoroutines.Images.SetKeyName(3, "CallStackCurrentLevel.bmp");
            // 
            // CoroutinesDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 255);
            this.Controls.Add(this.tlvwThreads);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CoroutinesDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TabText = "Coroutines";
            this.Text = "Coroutines";
            this.Shown += new System.EventHandler(this.CoroutinesDlg_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private LuaEdit.Controls.LuaEditTreeListView tlvwThreads;
        private DotNetLib.Controls.TreeListViewColumnHeader colIcon;
        private DotNetLib.Controls.TreeListViewColumnHeader colID;
        private DotNetLib.Controls.TreeListViewColumnHeader colLocation;
        private System.Windows.Forms.ImageList imlCoroutines;
    }
}