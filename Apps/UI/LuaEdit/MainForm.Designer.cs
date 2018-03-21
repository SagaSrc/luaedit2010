namespace LuaEdit
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainStatusBar = new System.Windows.Forms.StatusStrip();
            this.panelMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelLine = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelColumn = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelCharacter = new System.Windows.Forms.ToolStripStatusLabel();
            this.penelSpacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelCaretMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.findAndReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quickFindToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quickReplaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findInFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceInFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.bookmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableAllBookmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearBookmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outliningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleOutliningExpansionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleAllOutliningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopOutliningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solutionExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.documentListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuDebugWindows = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.breakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.localsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.callStackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coroutinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbDebugWindows = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.startDebuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.breakAllDebuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopDebuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attachToMachineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.checkSyntaxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.stepIntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepOverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stepOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllBreakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableAllBreakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.licenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutLuaEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuDocument = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllButThisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.copyFullPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openContainingFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.mainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.tsDebug = new System.Windows.Forms.ToolStrip();
            this.tsbStartDebugging = new System.Windows.Forms.ToolStripButton();
            this.tsbBreakAll = new System.Windows.Forms.ToolStripButton();
            this.tsbStopDebugging = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbStepInto = new System.Windows.Forms.ToolStripButton();
            this.tsbStepOver = new System.Windows.Forms.ToolStripButton();
            this.tsbStepOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsStandard = new System.Windows.Forms.ToolStrip();
            this.tsbNewProject = new System.Windows.Forms.ToolStripButton();
            this.tsbNewFile = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenFile = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbSaveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCut = new System.Windows.Forms.ToolStripButton();
            this.tsbCopy = new System.Windows.Forms.ToolStripButton();
            this.tsbPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUndo = new System.Windows.Forms.ToolStripButton();
            this.tsbRedo = new System.Windows.Forms.ToolStripButton();
            this.mainStatusBar.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.contextMenuDebugWindows.SuspendLayout();
            this.contextMenuDocument.SuspendLayout();
            this.mainToolStripContainer.ContentPanel.SuspendLayout();
            this.mainToolStripContainer.TopToolStripPanel.SuspendLayout();
            this.mainToolStripContainer.SuspendLayout();
            this.tsDebug.SuspendLayout();
            this.tsStandard.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainStatusBar
            // 
            this.mainStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.panelMessages,
            this.panelLine,
            this.panelColumn,
            this.panelCharacter,
            this.penelSpacer,
            this.panelCaretMode});
            this.mainStatusBar.Location = new System.Drawing.Point(0, 408);
            this.mainStatusBar.Name = "mainStatusBar";
            this.mainStatusBar.Size = new System.Drawing.Size(661, 22);
            this.mainStatusBar.TabIndex = 2;
            this.mainStatusBar.Text = "mainStatusBar";
            // 
            // panelMessages
            // 
            this.panelMessages.Name = "panelMessages";
            this.panelMessages.Size = new System.Drawing.Size(471, 17);
            this.panelMessages.Spring = true;
            this.panelMessages.Text = "Ready";
            this.panelMessages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelLine
            // 
            this.panelLine.AutoSize = false;
            this.panelLine.Name = "panelLine";
            this.panelLine.Size = new System.Drawing.Size(35, 17);
            // 
            // panelColumn
            // 
            this.panelColumn.AutoSize = false;
            this.panelColumn.Name = "panelColumn";
            this.panelColumn.Size = new System.Drawing.Size(35, 17);
            // 
            // panelCharacter
            // 
            this.panelCharacter.AutoSize = false;
            this.panelCharacter.Name = "panelCharacter";
            this.panelCharacter.Size = new System.Drawing.Size(35, 17);
            // 
            // penelSpacer
            // 
            this.penelSpacer.AutoSize = false;
            this.penelSpacer.Name = "penelSpacer";
            this.penelSpacer.Size = new System.Drawing.Size(35, 17);
            // 
            // panelCaretMode
            // 
            this.panelCaretMode.AutoSize = false;
            this.panelCaretMode.Name = "panelCaretMode";
            this.panelCaretMode.Size = new System.Drawing.Size(35, 17);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(661, 24);
            this.mainMenu.TabIndex = 3;
            this.mainMenu.Text = "mainMenu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeSolutionToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripMenuItem2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.toolStripMenuItem3,
            this.recentFilesToolStripMenuItem,
            this.recentProjectsToolStripMenuItem,
            this.toolStripMenuItem4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem,
            this.fileToolStripMenuItem1});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.NewProject;
            this.projectToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.N)));
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.projectToolStripMenuItem.Text = "Project...";
            this.projectToolStripMenuItem.Click += new System.EventHandler(this.projectToolStripMenuItem_Click);
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.Image = global::LuaEdit.Properties.Resources.NewFile;
            this.fileToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(200, 22);
            this.fileToolStripMenuItem1.Text = "File...";
            this.fileToolStripMenuItem1.Click += new System.EventHandler(this.fileToolStripMenuItem1_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProjectToolStripMenuItem,
            this.openFileToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.OpenProject;
            this.openProjectToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.O)));
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.openProjectToolStripMenuItem.Text = "Project/Solution...";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.OpenFile;
            this.openFileToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.openFileToolStripMenuItem.Text = "File...";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(188, 6);
            // 
            // closeSolutionToolStripMenuItem
            // 
            this.closeSolutionToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.CloseSolution;
            this.closeSolutionToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.closeSolutionToolStripMenuItem.Name = "closeSolutionToolStripMenuItem";
            this.closeSolutionToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.closeSolutionToolStripMenuItem.Text = "Close Solution";
            this.closeSolutionToolStripMenuItem.Click += new System.EventHandler(this.closeSolutionToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(188, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Save;
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.SaveAll;
            this.saveAllToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveAllToolStripMenuItem.Text = "Save All";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(188, 6);
            // 
            // recentFilesToolStripMenuItem
            // 
            this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.recentFilesToolStripMenuItem.Text = "Recent Files";
            // 
            // recentProjectsToolStripMenuItem
            // 
            this.recentProjectsToolStripMenuItem.Name = "recentProjectsToolStripMenuItem";
            this.recentProjectsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.recentProjectsToolStripMenuItem.Text = "Recent Projects";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(188, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem5,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem6,
            this.selectAllToolStripMenuItem,
            this.toolStripMenuItem7,
            this.findAndReplaceToolStripMenuItem,
            this.goToToolStripMenuItem,
            this.toolStripMenuItem8,
            this.bookmarksToolStripMenuItem,
            this.outliningToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            this.editToolStripMenuItem.DropDownOpening += new System.EventHandler(this.editToolStripMenuItem_DropDownOpening);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Undo;
            this.undoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Redo;
            this.redoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(164, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Cut;
            this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Copy;
            this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Paste;
            this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(164, 6);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(164, 6);
            // 
            // findAndReplaceToolStripMenuItem
            // 
            this.findAndReplaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quickFindToolStripMenuItem,
            this.quickReplaceToolStripMenuItem,
            this.findInFilesToolStripMenuItem,
            this.replaceInFilesToolStripMenuItem});
            this.findAndReplaceToolStripMenuItem.Name = "findAndReplaceToolStripMenuItem";
            this.findAndReplaceToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.findAndReplaceToolStripMenuItem.Text = "Find and Replace";
            // 
            // quickFindToolStripMenuItem
            // 
            this.quickFindToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Find;
            this.quickFindToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.quickFindToolStripMenuItem.Name = "quickFindToolStripMenuItem";
            this.quickFindToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.quickFindToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.quickFindToolStripMenuItem.Text = "Quick Find";
            this.quickFindToolStripMenuItem.Click += new System.EventHandler(this.quickFindToolStripMenuItem_Click);
            // 
            // quickReplaceToolStripMenuItem
            // 
            this.quickReplaceToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Repalce;
            this.quickReplaceToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.quickReplaceToolStripMenuItem.Name = "quickReplaceToolStripMenuItem";
            this.quickReplaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.quickReplaceToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.quickReplaceToolStripMenuItem.Text = "Quick Replace";
            // 
            // findInFilesToolStripMenuItem
            // 
            this.findInFilesToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.FindInFiles;
            this.findInFilesToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.findInFilesToolStripMenuItem.Name = "findInFilesToolStripMenuItem";
            this.findInFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.F)));
            this.findInFilesToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.findInFilesToolStripMenuItem.Text = "Find in Files";
            this.findInFilesToolStripMenuItem.Click += new System.EventHandler(this.findInFilesToolStripMenuItem_Click);
            // 
            // replaceInFilesToolStripMenuItem
            // 
            this.replaceInFilesToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.ReplaceInFiles;
            this.replaceInFilesToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.replaceInFilesToolStripMenuItem.Name = "replaceInFilesToolStripMenuItem";
            this.replaceInFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.H)));
            this.replaceInFilesToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.replaceInFilesToolStripMenuItem.Text = "Replace in File";
            // 
            // goToToolStripMenuItem
            // 
            this.goToToolStripMenuItem.Name = "goToToolStripMenuItem";
            this.goToToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.goToToolStripMenuItem.Text = "Go To...";
            this.goToToolStripMenuItem.Click += new System.EventHandler(this.goToToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(164, 6);
            // 
            // bookmarksToolStripMenuItem
            // 
            this.bookmarksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleBookmarkToolStripMenuItem,
            this.disableAllBookmarksToolStripMenuItem,
            this.enableBookmarkToolStripMenuItem,
            this.previousBookmarkToolStripMenuItem,
            this.nextBookmarkToolStripMenuItem,
            this.clearBookmarksToolStripMenuItem});
            this.bookmarksToolStripMenuItem.Name = "bookmarksToolStripMenuItem";
            this.bookmarksToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.bookmarksToolStripMenuItem.Text = "Bookmarks";
            // 
            // toggleBookmarkToolStripMenuItem
            // 
            this.toggleBookmarkToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.BookmarkEnabled;
            this.toggleBookmarkToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.toggleBookmarkToolStripMenuItem.Name = "toggleBookmarkToolStripMenuItem";
            this.toggleBookmarkToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.toggleBookmarkToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.toggleBookmarkToolStripMenuItem.Text = "Toggle Bookmark";
            this.toggleBookmarkToolStripMenuItem.Click += new System.EventHandler(this.toggleBookmarkToolStripMenuItem_Click);
            // 
            // disableAllBookmarksToolStripMenuItem
            // 
            this.disableAllBookmarksToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.DisableAllBookmarks;
            this.disableAllBookmarksToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.disableAllBookmarksToolStripMenuItem.Name = "disableAllBookmarksToolStripMenuItem";
            this.disableAllBookmarksToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.disableAllBookmarksToolStripMenuItem.Text = "Disable All Bookmarks";
            this.disableAllBookmarksToolStripMenuItem.Click += new System.EventHandler(this.disableAllBookmarksToolStripMenuItem_Click);
            // 
            // enableBookmarkToolStripMenuItem
            // 
            this.enableBookmarkToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.EnableBookmark;
            this.enableBookmarkToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.enableBookmarkToolStripMenuItem.Name = "enableBookmarkToolStripMenuItem";
            this.enableBookmarkToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.enableBookmarkToolStripMenuItem.Text = "Enable Bookmark";
            this.enableBookmarkToolStripMenuItem.Click += new System.EventHandler(this.enableBookmarkToolStripMenuItem_Click);
            // 
            // previousBookmarkToolStripMenuItem
            // 
            this.previousBookmarkToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.PreviousBookmark;
            this.previousBookmarkToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.previousBookmarkToolStripMenuItem.Name = "previousBookmarkToolStripMenuItem";
            this.previousBookmarkToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.previousBookmarkToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.previousBookmarkToolStripMenuItem.Text = "Previous Bookmark";
            this.previousBookmarkToolStripMenuItem.Click += new System.EventHandler(this.previousBookmarkToolStripMenuItem_Click);
            // 
            // nextBookmarkToolStripMenuItem
            // 
            this.nextBookmarkToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.NextBookmark;
            this.nextBookmarkToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.nextBookmarkToolStripMenuItem.Name = "nextBookmarkToolStripMenuItem";
            this.nextBookmarkToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.nextBookmarkToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.nextBookmarkToolStripMenuItem.Text = "Next Bookmark";
            this.nextBookmarkToolStripMenuItem.Click += new System.EventHandler(this.nextBookmarkToolStripMenuItem_Click);
            // 
            // clearBookmarksToolStripMenuItem
            // 
            this.clearBookmarksToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.ClearBookmarks;
            this.clearBookmarksToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.clearBookmarksToolStripMenuItem.Name = "clearBookmarksToolStripMenuItem";
            this.clearBookmarksToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.clearBookmarksToolStripMenuItem.Text = "Clear Bookmarks";
            this.clearBookmarksToolStripMenuItem.Click += new System.EventHandler(this.clearBookmarksToolStripMenuItem_Click);
            // 
            // outliningToolStripMenuItem
            // 
            this.outliningToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleOutliningExpansionToolStripMenuItem,
            this.toggleAllOutliningToolStripMenuItem,
            this.stopOutliningToolStripMenuItem});
            this.outliningToolStripMenuItem.Name = "outliningToolStripMenuItem";
            this.outliningToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.outliningToolStripMenuItem.Text = "Outlining";
            // 
            // toggleOutliningExpansionToolStripMenuItem
            // 
            this.toggleOutliningExpansionToolStripMenuItem.Name = "toggleOutliningExpansionToolStripMenuItem";
            this.toggleOutliningExpansionToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.toggleOutliningExpansionToolStripMenuItem.Text = "Toggle Outlining Expansion";
            this.toggleOutliningExpansionToolStripMenuItem.Click += new System.EventHandler(this.toggleOutliningExpressionToolStripMenuItem_Click);
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
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solutionExplorerToolStripMenuItem,
            this.toolStripSeparator2,
            this.documentListToolStripMenuItem,
            this.outputToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // solutionExplorerToolStripMenuItem
            // 
            this.solutionExplorerToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.SolutionExplorer;
            this.solutionExplorerToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.solutionExplorerToolStripMenuItem.Name = "solutionExplorerToolStripMenuItem";
            this.solutionExplorerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.E)));
            this.solutionExplorerToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.solutionExplorerToolStripMenuItem.Text = "Solution Explorer";
            this.solutionExplorerToolStripMenuItem.Click += new System.EventHandler(this.solutionExplorerToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(227, 6);
            // 
            // documentListToolStripMenuItem
            // 
            this.documentListToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.DocumentList;
            this.documentListToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.documentListToolStripMenuItem.Name = "documentListToolStripMenuItem";
            this.documentListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.O)));
            this.documentListToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.documentListToolStripMenuItem.Text = "Document List";
            this.documentListToolStripMenuItem.Click += new System.EventHandler(this.documentListToolStripMenuItem_Click);
            // 
            // outputToolStripMenuItem
            // 
            this.outputToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.Output;
            this.outputToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            this.outputToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.U)));
            this.outputToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.outputToolStripMenuItem.Text = "Output";
            this.outputToolStripMenuItem.Click += new System.EventHandler(this.outputToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowsToolStripMenuItem,
            this.toolStripSeparator1,
            this.startDebuggingToolStripMenuItem,
            this.breakAllDebuggingToolStripMenuItem,
            this.stopDebuggingToolStripMenuItem,
            this.attachToMachineToolStripMenuItem,
            this.toolStripMenuItem15,
            this.checkSyntaxToolStripMenuItem,
            this.toolStripMenuItem9,
            this.stepIntoToolStripMenuItem,
            this.stepOverToolStripMenuItem,
            this.stepOutToolStripMenuItem,
            this.toolStripMenuItem10,
            this.toggleBreakpointToolStripMenuItem,
            this.deleteAllBreakpointsToolStripMenuItem,
            this.disableAllBreakpointsToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.debugToolStripMenuItem.Text = "&Debug";
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDown = this.contextMenuDebugWindows;
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.windowsToolStripMenuItem.Text = "Windows";
            // 
            // contextMenuDebugWindows
            // 
            this.contextMenuDebugWindows.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.breakpointsToolStripMenuItem,
            this.toolStripMenuItem14,
            this.toolStripMenuItem11,
            this.localsToolStripMenuItem,
            this.toolStripMenuItem12,
            this.callStackToolStripMenuItem,
            this.coroutinesToolStripMenuItem});
            this.contextMenuDebugWindows.Name = "contextMenuDebugWindows";
            this.contextMenuDebugWindows.OwnerItem = this.windowsToolStripMenuItem;
            this.contextMenuDebugWindows.Size = new System.Drawing.Size(185, 126);
            this.contextMenuDebugWindows.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuDebugWindows_Opening);
            // 
            // breakpointsToolStripMenuItem
            // 
            this.breakpointsToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.BreakpointsWindow;
            this.breakpointsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.breakpointsToolStripMenuItem.Name = "breakpointsToolStripMenuItem";
            this.breakpointsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.breakpointsToolStripMenuItem.Text = "Breakpoints";
            this.breakpointsToolStripMenuItem.Click += new System.EventHandler(this.breakpointsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Image = global::LuaEdit.Properties.Resources.Output;
            this.toolStripMenuItem14.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.U)));
            this.toolStripMenuItem14.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem14.Text = "Output";
            this.toolStripMenuItem14.Click += new System.EventHandler(this.outputToolStripMenuItem_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(181, 6);
            // 
            // localsToolStripMenuItem
            // 
            this.localsToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.LocalsWindow;
            this.localsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.localsToolStripMenuItem.Name = "localsToolStripMenuItem";
            this.localsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.localsToolStripMenuItem.Text = "Locals";
            this.localsToolStripMenuItem.Click += new System.EventHandler(this.localsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(181, 6);
            // 
            // callStackToolStripMenuItem
            // 
            this.callStackToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.CallStack;
            this.callStackToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.callStackToolStripMenuItem.Name = "callStackToolStripMenuItem";
            this.callStackToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.callStackToolStripMenuItem.Text = "Call Stack";
            this.callStackToolStripMenuItem.Click += new System.EventHandler(this.callStackToolStripMenuItem_Click);
            // 
            // coroutinesToolStripMenuItem
            // 
            this.coroutinesToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.ThreadsWindow;
            this.coroutinesToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.coroutinesToolStripMenuItem.Name = "coroutinesToolStripMenuItem";
            this.coroutinesToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.coroutinesToolStripMenuItem.Text = "Coroutines";
            this.coroutinesToolStripMenuItem.Click += new System.EventHandler(this.coroutinesToolStripMenuItem_Click);
            // 
            // tsbDebugWindows
            // 
            this.tsbDebugWindows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDebugWindows.DropDown = this.contextMenuDebugWindows;
            this.tsbDebugWindows.Image = global::LuaEdit.Properties.Resources.BreakpointsWindow;
            this.tsbDebugWindows.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDebugWindows.Name = "tsbDebugWindows";
            this.tsbDebugWindows.Size = new System.Drawing.Size(32, 22);
            this.tsbDebugWindows.Text = "Breakpoints";
            this.tsbDebugWindows.ButtonClick += new System.EventHandler(this.tsbDebugWindows_ButtonClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(260, 6);
            // 
            // startDebuggingToolStripMenuItem
            // 
            this.startDebuggingToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.StartDebugging;
            this.startDebuggingToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.startDebuggingToolStripMenuItem.Name = "startDebuggingToolStripMenuItem";
            this.startDebuggingToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.startDebuggingToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.startDebuggingToolStripMenuItem.Text = "Start Debugging";
            this.startDebuggingToolStripMenuItem.Click += new System.EventHandler(this.startDebuggingToolStripMenuItem_Click);
            // 
            // breakAllDebuggingToolStripMenuItem
            // 
            this.breakAllDebuggingToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.BreakAll;
            this.breakAllDebuggingToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.breakAllDebuggingToolStripMenuItem.Name = "breakAllDebuggingToolStripMenuItem";
            this.breakAllDebuggingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.Pause)));
            this.breakAllDebuggingToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.breakAllDebuggingToolStripMenuItem.Text = "Break All";
            this.breakAllDebuggingToolStripMenuItem.Click += new System.EventHandler(this.breakAllDebuggingToolStripMenuItem_Click);
            // 
            // stopDebuggingToolStripMenuItem
            // 
            this.stopDebuggingToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.StopDebugging;
            this.stopDebuggingToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.stopDebuggingToolStripMenuItem.Name = "stopDebuggingToolStripMenuItem";
            this.stopDebuggingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.stopDebuggingToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.stopDebuggingToolStripMenuItem.Text = "Stop";
            this.stopDebuggingToolStripMenuItem.Click += new System.EventHandler(this.stopDebuggingToolStripMenuItem_Click);
            // 
            // attachToMachineToolStripMenuItem
            // 
            this.attachToMachineToolStripMenuItem.Name = "attachToMachineToolStripMenuItem";
            this.attachToMachineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
                        | System.Windows.Forms.Keys.P)));
            this.attachToMachineToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.attachToMachineToolStripMenuItem.Text = "Attach to Machine...";
            this.attachToMachineToolStripMenuItem.Click += new System.EventHandler(this.attachToMachineToolStripMenuItem_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(260, 6);
            // 
            // checkSyntaxToolStripMenuItem
            // 
            this.checkSyntaxToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("checkSyntaxToolStripMenuItem.Image")));
            this.checkSyntaxToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.checkSyntaxToolStripMenuItem.Name = "checkSyntaxToolStripMenuItem";
            this.checkSyntaxToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.C)));
            this.checkSyntaxToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.checkSyntaxToolStripMenuItem.Text = "Check Syntax";
            this.checkSyntaxToolStripMenuItem.Click += new System.EventHandler(this.checkSyntaxToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(260, 6);
            // 
            // stepIntoToolStripMenuItem
            // 
            this.stepIntoToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.StepInto;
            this.stepIntoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.stepIntoToolStripMenuItem.Name = "stepIntoToolStripMenuItem";
            this.stepIntoToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.stepIntoToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.stepIntoToolStripMenuItem.Text = "Step Into";
            this.stepIntoToolStripMenuItem.Click += new System.EventHandler(this.stepIntoToolStripMenuItem_Click);
            // 
            // stepOverToolStripMenuItem
            // 
            this.stepOverToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.StepOver;
            this.stepOverToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.stepOverToolStripMenuItem.Name = "stepOverToolStripMenuItem";
            this.stepOverToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.stepOverToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.stepOverToolStripMenuItem.Text = "Step Over";
            this.stepOverToolStripMenuItem.Click += new System.EventHandler(this.stepOverToolStripMenuItem_Click);
            // 
            // stepOutToolStripMenuItem
            // 
            this.stepOutToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.StepOut;
            this.stepOutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.stepOutToolStripMenuItem.Name = "stepOutToolStripMenuItem";
            this.stepOutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F11)));
            this.stepOutToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.stepOutToolStripMenuItem.Text = "Step Out";
            this.stepOutToolStripMenuItem.Click += new System.EventHandler(this.stepOutToolStripMenuItem_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(260, 6);
            // 
            // toggleBreakpointToolStripMenuItem
            // 
            this.toggleBreakpointToolStripMenuItem.Name = "toggleBreakpointToolStripMenuItem";
            this.toggleBreakpointToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.toggleBreakpointToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.toggleBreakpointToolStripMenuItem.Text = "Toggle Breakpoint";
            this.toggleBreakpointToolStripMenuItem.Click += new System.EventHandler(this.toggleBreakpointToolStripMenuItem_Click);
            // 
            // deleteAllBreakpointsToolStripMenuItem
            // 
            this.deleteAllBreakpointsToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.DeleteAllBreakpoints;
            this.deleteAllBreakpointsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.deleteAllBreakpointsToolStripMenuItem.Name = "deleteAllBreakpointsToolStripMenuItem";
            this.deleteAllBreakpointsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.F9)));
            this.deleteAllBreakpointsToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.deleteAllBreakpointsToolStripMenuItem.Text = "Delete All Breakpoints";
            this.deleteAllBreakpointsToolStripMenuItem.Click += new System.EventHandler(this.deleteAllBreakpointsToolStripMenuItem_Click);
            // 
            // disableAllBreakpointsToolStripMenuItem
            // 
            this.disableAllBreakpointsToolStripMenuItem.Image = global::LuaEdit.Properties.Resources.DisableAllBreakpoints;
            this.disableAllBreakpointsToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.disableAllBreakpointsToolStripMenuItem.Name = "disableAllBreakpointsToolStripMenuItem";
            this.disableAllBreakpointsToolStripMenuItem.Size = new System.Drawing.Size(263, 22);
            this.disableAllBreakpointsToolStripMenuItem.Text = "Disable All Breakpoints";
            this.disableAllBreakpointsToolStripMenuItem.Click += new System.EventHandler(this.disableAllBreakpointsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.licenseToolStripMenuItem,
            this.aboutLuaEditToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // licenseToolStripMenuItem
            // 
            this.licenseToolStripMenuItem.Name = "licenseToolStripMenuItem";
            this.licenseToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.licenseToolStripMenuItem.Text = "License...";
            this.licenseToolStripMenuItem.Click += new System.EventHandler(this.licenseToolStripMenuItem_Click);
            // 
            // aboutLuaEditToolStripMenuItem
            // 
            this.aboutLuaEditToolStripMenuItem.Name = "aboutLuaEditToolStripMenuItem";
            this.aboutLuaEditToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.aboutLuaEditToolStripMenuItem.Text = "About LuaEdit...";
            this.aboutLuaEditToolStripMenuItem.Click += new System.EventHandler(this.aboutLuaEditToolStripMenuItem_Click);
            // 
            // contextMenuDocument
            // 
            this.contextMenuDocument.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem1,
            this.closeToolStripMenuItem1,
            this.closeAllButThisToolStripMenuItem,
            this.toolStripMenuItem13,
            this.copyFullPathToolStripMenuItem,
            this.openContainingFolderToolStripMenuItem});
            this.contextMenuDocument.Name = "contextMenuDocument";
            this.contextMenuDocument.Size = new System.Drawing.Size(199, 120);
            this.contextMenuDocument.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuDocument_Opening);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Image = global::LuaEdit.Properties.Resources.Save;
            this.saveToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(198, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem1
            // 
            this.closeToolStripMenuItem1.Name = "closeToolStripMenuItem1";
            this.closeToolStripMenuItem1.Size = new System.Drawing.Size(198, 22);
            this.closeToolStripMenuItem1.Text = "Close";
            this.closeToolStripMenuItem1.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // closeAllButThisToolStripMenuItem
            // 
            this.closeAllButThisToolStripMenuItem.Name = "closeAllButThisToolStripMenuItem";
            this.closeAllButThisToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.closeAllButThisToolStripMenuItem.Text = "Close All But This";
            this.closeAllButThisToolStripMenuItem.Click += new System.EventHandler(this.closeAllButThisToolStripMenuItem_Click);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(195, 6);
            // 
            // copyFullPathToolStripMenuItem
            // 
            this.copyFullPathToolStripMenuItem.Name = "copyFullPathToolStripMenuItem";
            this.copyFullPathToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.copyFullPathToolStripMenuItem.Text = "Copy Full Path";
            this.copyFullPathToolStripMenuItem.Click += new System.EventHandler(this.copyFullPathToolStripMenuItem_Click);
            // 
            // openContainingFolderToolStripMenuItem
            // 
            this.openContainingFolderToolStripMenuItem.Name = "openContainingFolderToolStripMenuItem";
            this.openContainingFolderToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.openContainingFolderToolStripMenuItem.Text = "Open Containing Folder";
            this.openContainingFolderToolStripMenuItem.Click += new System.EventHandler(this.openContainingFolderToolStripMenuItem_Click);
            // 
            // mainToolStripContainer
            // 
            this.mainToolStripContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // mainToolStripContainer.ContentPanel
            // 
            this.mainToolStripContainer.ContentPanel.Controls.Add(this.mainDockPanel);
            this.mainToolStripContainer.ContentPanel.Size = new System.Drawing.Size(661, 333);
            this.mainToolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.mainToolStripContainer.Name = "mainToolStripContainer";
            this.mainToolStripContainer.Size = new System.Drawing.Size(661, 383);
            this.mainToolStripContainer.TabIndex = 6;
            this.mainToolStripContainer.Text = "toolStripContainer1";
            // 
            // mainToolStripContainer.TopToolStripPanel
            // 
            this.mainToolStripContainer.TopToolStripPanel.Controls.Add(this.tsDebug);
            this.mainToolStripContainer.TopToolStripPanel.Controls.Add(this.tsStandard);
            // 
            // mainDockPanel
            // 
            this.mainDockPanel.ActiveAutoHideContent = null;
            this.mainDockPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainDockPanel.DockBottomPortion = 150;
            this.mainDockPanel.DockLeftPortion = 200;
            this.mainDockPanel.DockRightPortion = 200;
            this.mainDockPanel.DockTopPortion = 150;
            this.mainDockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
            this.mainDockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.mainDockPanel.Location = new System.Drawing.Point(3, 3);
            this.mainDockPanel.Name = "mainDockPanel";
            this.mainDockPanel.RightToLeftLayout = true;
            this.mainDockPanel.Size = new System.Drawing.Size(655, 327);
            this.mainDockPanel.TabIndex = 11;
            this.mainDockPanel.ActiveContentChanged += new System.EventHandler(this.mainDockPanel_ActiveContentChanged);
            this.mainDockPanel.ActiveDocumentChanged += new System.EventHandler(this.mainDockPanel_ActiveDocumentChanged);
            this.mainDockPanel.ActivePaneChanged += new System.EventHandler(this.mainDockPanel_ActivePaneChanged);
            // 
            // tsDebug
            // 
            this.tsDebug.Dock = System.Windows.Forms.DockStyle.None;
            this.tsDebug.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbStartDebugging,
            this.tsbBreakAll,
            this.tsbStopDebugging,
            this.toolStripSeparator5,
            this.tsbStepInto,
            this.tsbStepOver,
            this.tsbStepOut,
            this.toolStripSeparator6,
            this.tsbDebugWindows});
            this.tsDebug.Location = new System.Drawing.Point(3, 0);
            this.tsDebug.Name = "tsDebug";
            this.tsDebug.Size = new System.Drawing.Size(192, 25);
            this.tsDebug.TabIndex = 1;
            // 
            // tsbStartDebugging
            // 
            this.tsbStartDebugging.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStartDebugging.Image = global::LuaEdit.Properties.Resources.StartDebugging;
            this.tsbStartDebugging.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStartDebugging.Name = "tsbStartDebugging";
            this.tsbStartDebugging.Size = new System.Drawing.Size(23, 22);
            this.tsbStartDebugging.Text = "Start Debugging (F5)";
            this.tsbStartDebugging.Click += new System.EventHandler(this.startDebuggingToolStripMenuItem_Click);
            // 
            // tsbBreakAll
            // 
            this.tsbBreakAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbBreakAll.Image = global::LuaEdit.Properties.Resources.BreakAll;
            this.tsbBreakAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBreakAll.Name = "tsbBreakAll";
            this.tsbBreakAll.Size = new System.Drawing.Size(23, 22);
            this.tsbBreakAll.Text = "Break All (Ctrl+Alt+Break)";
            this.tsbBreakAll.Click += new System.EventHandler(this.breakAllDebuggingToolStripMenuItem_Click);
            // 
            // tsbStopDebugging
            // 
            this.tsbStopDebugging.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStopDebugging.Image = global::LuaEdit.Properties.Resources.StopDebugging;
            this.tsbStopDebugging.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStopDebugging.Name = "tsbStopDebugging";
            this.tsbStopDebugging.Size = new System.Drawing.Size(23, 22);
            this.tsbStopDebugging.Text = "Stop Debugging (Shift+F5)";
            this.tsbStopDebugging.Click += new System.EventHandler(this.stopDebuggingToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbStepInto
            // 
            this.tsbStepInto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStepInto.Image = global::LuaEdit.Properties.Resources.StepInto;
            this.tsbStepInto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStepInto.Name = "tsbStepInto";
            this.tsbStepInto.Size = new System.Drawing.Size(23, 22);
            this.tsbStepInto.Text = "Step Into (F11)";
            this.tsbStepInto.Click += new System.EventHandler(this.stepIntoToolStripMenuItem_Click);
            // 
            // tsbStepOver
            // 
            this.tsbStepOver.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStepOver.Image = global::LuaEdit.Properties.Resources.StepOver;
            this.tsbStepOver.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStepOver.Name = "tsbStepOver";
            this.tsbStepOver.Size = new System.Drawing.Size(23, 22);
            this.tsbStepOver.Text = "Step Over (F10)";
            this.tsbStepOver.Click += new System.EventHandler(this.stepOverToolStripMenuItem_Click);
            // 
            // tsbStepOut
            // 
            this.tsbStepOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStepOut.Image = global::LuaEdit.Properties.Resources.StepOut;
            this.tsbStepOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStepOut.Name = "tsbStepOut";
            this.tsbStepOut.Size = new System.Drawing.Size(23, 22);
            this.tsbStepOut.Text = "Step Out (Shift+F11)";
            this.tsbStepOut.Click += new System.EventHandler(this.stepOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsStandard
            // 
            this.tsStandard.Dock = System.Windows.Forms.DockStyle.None;
            this.tsStandard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewProject,
            this.tsbNewFile,
            this.tsbOpenFile,
            this.tsbSave,
            this.tsbSaveAll,
            this.toolStripSeparator3,
            this.tsbCut,
            this.tsbCopy,
            this.tsbPaste,
            this.toolStripSeparator4,
            this.tsbUndo,
            this.tsbRedo});
            this.tsStandard.Location = new System.Drawing.Point(3, 25);
            this.tsStandard.Name = "tsStandard";
            this.tsStandard.Size = new System.Drawing.Size(252, 25);
            this.tsStandard.TabIndex = 0;
            // 
            // tsbNewProject
            // 
            this.tsbNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewProject.Image = global::LuaEdit.Properties.Resources.NewProject;
            this.tsbNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewProject.Name = "tsbNewProject";
            this.tsbNewProject.Size = new System.Drawing.Size(23, 22);
            this.tsbNewProject.Text = "New Project... (Ctrl+Shift+N)";
            this.tsbNewProject.Click += new System.EventHandler(this.projectToolStripMenuItem_Click);
            // 
            // tsbNewFile
            // 
            this.tsbNewFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewFile.Image = global::LuaEdit.Properties.Resources.NewFile;
            this.tsbNewFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewFile.Name = "tsbNewFile";
            this.tsbNewFile.Size = new System.Drawing.Size(23, 22);
            this.tsbNewFile.Text = "New File... (Ctrl+N)";
            this.tsbNewFile.Click += new System.EventHandler(this.fileToolStripMenuItem1_Click);
            // 
            // tsbOpenFile
            // 
            this.tsbOpenFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpenFile.Image = global::LuaEdit.Properties.Resources.OpenFile;
            this.tsbOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenFile.Name = "tsbOpenFile";
            this.tsbOpenFile.Size = new System.Drawing.Size(23, 22);
            this.tsbOpenFile.Text = "Open File... (Ctrl+O)";
            this.tsbOpenFile.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Image = global::LuaEdit.Properties.Resources.Save;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "Save (Ctrl+S)";
            this.tsbSave.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // tsbSaveAll
            // 
            this.tsbSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSaveAll.Image = global::LuaEdit.Properties.Resources.SaveAll;
            this.tsbSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSaveAll.Name = "tsbSaveAll";
            this.tsbSaveAll.Size = new System.Drawing.Size(23, 22);
            this.tsbSaveAll.Text = "Save All (Ctrl+Shift+S)";
            this.tsbSaveAll.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCut
            // 
            this.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCut.Image = global::LuaEdit.Properties.Resources.Cut;
            this.tsbCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCut.Name = "tsbCut";
            this.tsbCut.Size = new System.Drawing.Size(23, 22);
            this.tsbCut.Text = "Cut (Ctrl+X)";
            this.tsbCut.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // tsbCopy
            // 
            this.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCopy.Image = global::LuaEdit.Properties.Resources.Copy;
            this.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopy.Name = "tsbCopy";
            this.tsbCopy.Size = new System.Drawing.Size(23, 22);
            this.tsbCopy.Text = "Copy (Ctrl+C)";
            this.tsbCopy.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // tsbPaste
            // 
            this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaste.Image = global::LuaEdit.Properties.Resources.Paste;
            this.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPaste.Name = "tsbPaste";
            this.tsbPaste.Size = new System.Drawing.Size(23, 22);
            this.tsbPaste.Text = "Paste (Ctrl+V)";
            this.tsbPaste.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbUndo
            // 
            this.tsbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUndo.Image = global::LuaEdit.Properties.Resources.Undo;
            this.tsbUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUndo.Name = "tsbUndo";
            this.tsbUndo.Size = new System.Drawing.Size(23, 22);
            this.tsbUndo.Text = "Undo (Ctrl+Z)";
            this.tsbUndo.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // tsbRedo
            // 
            this.tsbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRedo.Image = global::LuaEdit.Properties.Resources.Redo;
            this.tsbRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRedo.Name = "tsbRedo";
            this.tsbRedo.Size = new System.Drawing.Size(23, 22);
            this.tsbRedo.Text = "Redo (Ctrl+Y)";
            this.tsbRedo.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 430);
            this.Controls.Add(this.mainToolStripContainer);
            this.Controls.Add(this.mainStatusBar);
            this.Controls.Add(this.mainMenu);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LuaEdit";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.mainStatusBar.ResumeLayout(false);
            this.mainStatusBar.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.contextMenuDebugWindows.ResumeLayout(false);
            this.contextMenuDocument.ResumeLayout(false);
            this.mainToolStripContainer.ContentPanel.ResumeLayout(false);
            this.mainToolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.mainToolStripContainer.TopToolStripPanel.PerformLayout();
            this.mainToolStripContainer.ResumeLayout(false);
            this.mainToolStripContainer.PerformLayout();
            this.tsDebug.ResumeLayout(false);
            this.tsDebug.PerformLayout();
            this.tsStandard.ResumeLayout(false);
            this.tsStandard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip mainStatusBar;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel panelMessages;
        private System.Windows.Forms.ToolStripStatusLabel panelLine;
        private System.Windows.Forms.ToolStripStatusLabel panelColumn;
        private System.Windows.Forms.ToolStripStatusLabel panelCharacter;
        private System.Windows.Forms.ToolStripStatusLabel penelSpacer;
        private System.Windows.Forms.ToolStripStatusLabel panelCaretMode;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem recentFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem findAndReplaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem bookmarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleBookmarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableAllBookmarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableBookmarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem previousBookmarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextBookmarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearBookmarksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outliningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleOutliningExpansionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleAllOutliningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopOutliningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solutionExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentProjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem startDebuggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attachToMachineToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem stepIntoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepOverToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toggleBreakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllBreakpointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableAllBreakpointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stepOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopDebuggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem breakAllDebuggingToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuDocument;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeAllButThisToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem copyFullPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openContainingFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quickFindToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quickReplaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findInFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceInFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem documentListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutLuaEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripContainer mainToolStripContainer;
        private System.Windows.Forms.ToolStrip tsStandard;
        private System.Windows.Forms.ToolStripButton tsbNewProject;
        private System.Windows.Forms.ToolStripButton tsbNewFile;
        private System.Windows.Forms.ToolStripButton tsbOpenFile;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbCut;
        private System.Windows.Forms.ToolStripButton tsbCopy;
        private System.Windows.Forms.ToolStripButton tsbPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbUndo;
        private System.Windows.Forms.ToolStripButton tsbRedo;
        private System.Windows.Forms.ToolStripMenuItem closeSolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStrip tsDebug;
        private System.Windows.Forms.ToolStripButton tsbStartDebugging;
        private System.Windows.Forms.ToolStripButton tsbBreakAll;
        private System.Windows.Forms.ToolStripButton tsbStopDebugging;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbStepInto;
        private System.Windows.Forms.ToolStripButton tsbStepOver;
        private System.Windows.Forms.ToolStripButton tsbStepOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSplitButton tsbDebugWindows;
        private System.Windows.Forms.ContextMenuStrip contextMenuDebugWindows;
        private System.Windows.Forms.ToolStripMenuItem breakpointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem localsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem callStackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coroutinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private WeifenLuo.WinFormsUI.Docking.DockPanel mainDockPanel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem checkSyntaxToolStripMenuItem;
    }
}

