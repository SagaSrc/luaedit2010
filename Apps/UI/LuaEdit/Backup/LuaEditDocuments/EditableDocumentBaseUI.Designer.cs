namespace LuaEdit.Documents
{
    partial class EditableDocumentBaseUI
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
            Fireball.Windows.Forms.LineMarginRender lineMarginRender1 = new Fireball.Windows.Forms.LineMarginRender();
            this._editor = new Fireball.Windows.Forms.CodeEditorControl();
            this._editorContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.breakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.conditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hitCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.runToCursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.outliningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleOutliningExpansionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleAllOutliningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopOutliningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._editorContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _editor
            // 
            this._editor.ActiveView = Fireball.Windows.Forms.CodeEditor.ActiveView.BottomRight;
            this._editor.AutoListPosition = null;
            this._editor.AutoListSelectedText = "a123";
            this._editor.AutoListVisible = false;
            this._editor.ChildBorderStyle = Fireball.Windows.Forms.ControlBorderStyle.None;
            this._editor.ContextMenuStrip = this._editorContextMenu;
            this._editor.CopyAsRTF = false;
            this._editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._editor.InfoTipCount = 1;
            this._editor.InfoTipPosition = null;
            this._editor.InfoTipSelectedIndex = 1;
            this._editor.InfoTipVisible = false;
            lineMarginRender1.Bounds = new System.Drawing.Rectangle(19, 0, 19, 16);
            this._editor.LineMarginRender = lineMarginRender1;
            this._editor.Location = new System.Drawing.Point(0, 0);
            this._editor.LockCursorUpdate = false;
            this._editor.Name = "_editor";
            this._editor.Saved = false;
            this._editor.ShowScopeIndicator = false;
            this._editor.Size = new System.Drawing.Size(616, 375);
            this._editor.SmoothScroll = false;
            this._editor.SplitviewH = -4;
            this._editor.SplitviewV = -4;
            this._editor.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(219)))), ((int)(((byte)(214)))));
            this._editor.TabIndex = 1;
            this._editor.Text = "codeEditorControl1";
            this._editor.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
            this._editor.RowMouseDown += new Fireball.Windows.Forms.CodeEditor.RowMouseHandler(this._editor_RowMouseDown);
            this._editor.SelectionChange += new System.EventHandler(this._editor_SelectionChange);
            this._editor.CaretChange += new System.EventHandler(this._editor_CaretChange);
            this._editor.TextChanged += new System.EventHandler(this._editor_TextChanged);
            // 
            // _editorContextMenu
            // 
            this._editorContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem1,
            this.breakpointToolStripMenuItem,
            this.toolStripMenuItem3,
            this.runToCursorToolStripMenuItem,
            this.toolStripMenuItem4,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem5,
            this.outliningToolStripMenuItem});
            this._editorContextMenu.Name = "_editorContextMenu";
            this._editorContextMenu.Size = new System.Drawing.Size(155, 204);
            this._editorContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this._editorContextMenu_Opening);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Image = global::LuaEdit.Documents.Properties.Resources.Undo;
            this.undoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Image = global::LuaEdit.Documents.Properties.Resources.Redo;
            this.redoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.redoToolStripMenuItem.Text = "&Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(151, 6);
            // 
            // breakpointToolStripMenuItem
            // 
            this.breakpointToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insertBreakpointToolStripMenuItem,
            this.disableBreakpointToolStripMenuItem,
            this.toolStripMenuItem2,
            this.conditionToolStripMenuItem,
            this.hitCountToolStripMenuItem});
            this.breakpointToolStripMenuItem.Name = "breakpointToolStripMenuItem";
            this.breakpointToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.breakpointToolStripMenuItem.Text = "Breakpoint";
            // 
            // insertBreakpointToolStripMenuItem
            // 
            this.insertBreakpointToolStripMenuItem.Image = global::LuaEdit.Documents.Properties.Resources.InsertBreakpoint;
            this.insertBreakpointToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.insertBreakpointToolStripMenuItem.Name = "insertBreakpointToolStripMenuItem";
            this.insertBreakpointToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.insertBreakpointToolStripMenuItem.Text = "Insert Breakpoint";
            this.insertBreakpointToolStripMenuItem.Click += new System.EventHandler(this.insertBreakpointToolStripMenuItem_Click);
            // 
            // disableBreakpointToolStripMenuItem
            // 
            this.disableBreakpointToolStripMenuItem.Image = global::LuaEdit.Documents.Properties.Resources.DisableAllBreakpoints;
            this.disableBreakpointToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.disableBreakpointToolStripMenuItem.Name = "disableBreakpointToolStripMenuItem";
            this.disableBreakpointToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.disableBreakpointToolStripMenuItem.Text = "Disable Breakpoint";
            this.disableBreakpointToolStripMenuItem.Click += new System.EventHandler(this.disableBreakpointToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(170, 6);
            // 
            // conditionToolStripMenuItem
            // 
            this.conditionToolStripMenuItem.Name = "conditionToolStripMenuItem";
            this.conditionToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.conditionToolStripMenuItem.Text = "Condition...";
            this.conditionToolStripMenuItem.Click += new System.EventHandler(this.conditionToolStripMenuItem_Click);
            // 
            // hitCountToolStripMenuItem
            // 
            this.hitCountToolStripMenuItem.Name = "hitCountToolStripMenuItem";
            this.hitCountToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.hitCountToolStripMenuItem.Text = "Hit Count...";
            this.hitCountToolStripMenuItem.Click += new System.EventHandler(this.hitCountToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(151, 6);
            // 
            // runToCursorToolStripMenuItem
            // 
            this.runToCursorToolStripMenuItem.Image = global::LuaEdit.Documents.Properties.Resources.RunToCursor;
            this.runToCursorToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.runToCursorToolStripMenuItem.Name = "runToCursorToolStripMenuItem";
            this.runToCursorToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.runToCursorToolStripMenuItem.Text = "Run To Cursor";
            this.runToCursorToolStripMenuItem.Click += new System.EventHandler(this.runToCursorToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(151, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = global::LuaEdit.Documents.Properties.Resources.Cut;
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.cutToolStripMenuItem.Text = "&Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::LuaEdit.Documents.Properties.Resources.Copy;
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::LuaEdit.Documents.Properties.Resources.Paste;
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(151, 6);
            // 
            // outliningToolStripMenuItem
            // 
            this.outliningToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleOutliningExpansionToolStripMenuItem,
            this.toggleAllOutliningToolStripMenuItem,
            this.stopOutliningToolStripMenuItem});
            this.outliningToolStripMenuItem.Name = "outliningToolStripMenuItem";
            this.outliningToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.outliningToolStripMenuItem.Text = "Outlining";
            // 
            // toggleOutliningExpansionToolStripMenuItem
            // 
            this.toggleOutliningExpansionToolStripMenuItem.Name = "toggleOutliningExpansionToolStripMenuItem";
            this.toggleOutliningExpansionToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.toggleOutliningExpansionToolStripMenuItem.Text = "Toggle Outlining Expansion";
            this.toggleOutliningExpansionToolStripMenuItem.Click += new System.EventHandler(this.toggleOutliningExpansionToolStripMenuItem_Click);
            // 
            // toggleAllOutliningToolStripMenuItem
            // 
            this.toggleAllOutliningToolStripMenuItem.Name = "toggleAllOutliningToolStripMenuItem";
            this.toggleAllOutliningToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.toggleAllOutliningToolStripMenuItem.Text = "Toggle All Outlining";
            this.toggleAllOutliningToolStripMenuItem.Click += new System.EventHandler(this.toggleAllOutliningToolStripMenuItem_Click);
            // 
            // stopOutliningToolStripMenuItem
            // 
            this.stopOutliningToolStripMenuItem.Name = "stopOutliningToolStripMenuItem";
            this.stopOutliningToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.stopOutliningToolStripMenuItem.Text = "Stop Outlining";
            this.stopOutliningToolStripMenuItem.Click += new System.EventHandler(this.stopOutliningToolStripMenuItem_Click);
            // 
            // EditableDocumentBaseUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 375);
            this.Controls.Add(this._editor);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.HideOnClose = true;
            this.Name = "EditableDocumentBaseUI";
            this._editorContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Fireball.Windows.Forms.CodeEditorControl _editor;
        private System.Windows.Forms.ContextMenuStrip _editorContextMenu;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem breakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertBreakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableBreakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem conditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hitCountToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem runToCursorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem outliningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleOutliningExpansionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleAllOutliningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopOutliningToolStripMenuItem;
    }
}