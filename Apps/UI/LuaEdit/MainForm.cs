using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using Fireball.Syntax;

using LuaEdit.Win32;
using LuaEdit.Documents;
using LuaEdit.Forms;
using LuaEdit.HelperDialogs;
using LuaEdit.Interfaces;
using LuaEdit.LuaEditDebugger;
using LuaEdit.LuaEditDebugger.DebugCommands;
using LuaEdit.Managers;
using LuaEdit.Rpc;
using WeifenLuo.WinFormsUI.Docking;

using LuaInterface;

namespace LuaEdit
{
    public partial class MainForm : Form
    {
        #region Members

        private delegate void RefreshDocumentTabDelegate(ILuaEditDocument doc);
        private delegate void ShowDocumentDelegate(ILuaEditDocument doc);
        private delegate void RefreshStatusBarDelegate();
        private delegate void RefreshMenusDelegate();
        private delegate ILuaEditDocument OpenDocumentDelegate(string fileName, bool addToRecents);

        [DllImport("rdbg.dll", CharSet = CharSet.Ansi)]
        private static extern bool CheckLuaScriptSyntax(StringBuilder script, StringBuilder scriptName, StringBuilder errMsg, int errMsgLen);

        private readonly string _editLayoutFileName = Path.Combine(FrameworkManager.ApplicationDataPath, "editlayout.xml");
        private readonly string _debugLayoutFileName = Path.Combine(FrameworkManager.ApplicationDataPath, "debuglayout.xml");
        private ToolStripItem _debugWindowsOwnerToolStripItem = null;
        private FindAndReplaceDlg _findAndReplaceDlg = null;
        private DocumentListDlg _docListDlg = null;
        AttachToMachineDialog _attachToMachineDlg = null;
        private Dictionary<Type, DockContent> _windows = new Dictionary<Type,DockContent>();

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();

            // Create members
            _findAndReplaceDlg = new FindAndReplaceDlg();
            _docListDlg = new DocumentListDlg();
            _attachToMachineDlg = new AttachToMachineDialog();

            // Create output dialog (must be created at all time so it contains the data
            // even if it's not showing)
            CreateOrFindWindow(typeof(OutputDlg));

            tsbDebugWindows.Tag = breakpointsToolStripMenuItem;

            // Register this dialog to the framework manager as the software's main dialog
            FrameworkManager.Instance.RegisterMainDialog(this);

            // Bind to debugging events
            ClientDebugManager.Instance.DebuggingStarted += OnDebuggingStarted;
            ClientDebugManager.Instance.DebuggingStopped += OnDebuggingStopped;
            ClientDebugManager.Instance.BreakChanged += OnBreakChanged;

            // Bind to framework events
            DocumentsManager.Instance.CurrentSolutionChanged += OnCurrentSolutionChanged;
            DocumentsManager.Instance.RecentFilesChanged += OnRecentFilesChanged;
            DocumentsManager.Instance.RecentProjectsChanged += OnRecentProjectsChanged;
            DocumentsManager.Instance.DocumentsOpened += OnDocumentsOpened;
            DocumentsManager.Instance.DocumentsClosed += OnDocumentsClosed;
            FrameworkManager.Instance.StatusMessageRequested += OnStatusMessageRequested;

            // Load UI Layout
            LoadLayout(_editLayoutFileName);
            ToolStripManager.LoadSettings(this, this.Name);

            // Initialize UI
            RefreshMenusAndToolbars();
        }

        #endregion

        #region Events

        #region File Menu Events

        /// <summary>
        /// Occurs when the new project menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DocumentsManager.Instance.NewDocument("New Project", null, NewItemTypes.ItemGroup);
        }

        /// <summary>
        /// Occurs when the new file menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DocumentsManager.Instance.NewDocument("New File", null, NewItemTypes.Item);
        }

        /// <summary>
        /// Occurs when the open project menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string initialDir = string.Empty;
                ILuaEditDocument activeDoc = GetActiveDocument();

                if (activeDoc != null)
                {
                    initialDir = Path.GetDirectoryName(activeDoc.FileName);
                }

                DocumentsManager.Instance.OpenProjectDocument(true, initialDir);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Occurs when the open file menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string initialDir = string.Empty;
                ILuaEditDocument activeDoc = GetActiveDocument();

                if (activeDoc != null)
                {
                    initialDir = Path.GetDirectoryName(activeDoc.FileName);
                }

                DocumentsManager.Instance.OpenDocuments(true, initialDir);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        
        /// <summary>
        /// Occurs when the close menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ILuaEditDocument> docsToClose = new List<ILuaEditDocument>();
            ILuaEditDocument activeDoc = GetActiveDocument();
            docsToClose.Add(activeDoc);

            if (activeDoc != null)
            {
                DocumentsManager.Instance.CloseDocuments(docsToClose);
            }
        }

        /// <summary>
        /// Occurs when the close solution menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;

            if (ClientDebugManager.Instance.IsDebugging)
            {
                result = FrameworkManager.ShowMessageBox("Do you want to stop debugging?",
                                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (result == DialogResult.Yes && CloseAllTabbedDocuments())
            {
                DocumentsManager.Instance.CloseDocument(DocumentsManager.Instance.CurrentSolution);
            }
        }

        /// <summary>
        /// Occurs when the save menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocument doc = GetActiveDocument();
            DocumentsManager.Instance.SaveDocument(doc.DocumentType, doc, false);
            RefreshActiveDocumentTab();
        }

        /// <summary>
        /// Occurs when the save as menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocument doc = GetActiveDocument();
            DocumentsManager.Instance.SaveDocument(doc.DocumentType, doc, true);
            RefreshActiveDocumentTab();
        }

        /// <summary>
        /// Occurs when the save all menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DocumentsManager.Instance.SaveAllDocuments();
            RefreshAllDocumentTabs();
        }

        /// <summary>
        /// Occurs when a recent file submenu has been clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRecentFileSubMenuClicked(object sender, EventArgs e)
        {
            ToolStripItem subMenu = sender as ToolStripItem;

            if (subMenu != null)
            {
                string docFileName = subMenu.Text.Substring(2);

                if (File.Exists(docFileName))
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        DocumentsManager.Instance.OpenDocument(docFileName, true);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    string msg = string.Format("The file '{0}' does not exists. Do you want to remove it from the recent projects list?", docFileName);

                    if (FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DocumentsManager.Instance.RemoveFromRecentFiles(docFileName);
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when a recent project submenu has been clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRecentProjectSubMenuClicked(object sender, EventArgs e)
        {
            ToolStripItem subMenu = sender as ToolStripItem;

            if (subMenu != null)
            {
                string docFileName = subMenu.Text.Substring(2);

                if (File.Exists(docFileName))
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        DocumentsManager.Instance.OpenProjectDocument(docFileName, true);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
                else
                {
                    string msg = string.Format("The project '{0}' does not exists. Do you want to remove it from the recent projects list?", docFileName);

                    if (FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        DocumentsManager.Instance.RemoveFromRecentProjects(docFileName);
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when the close menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Edit Menu Events

        /// <summary>
        /// Occurs just before the edit menu opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            RefreshEditMenuAndToolbar();
        }

        /// <summary>
        /// Occurs when the undo menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentUIUndoRedo doc = GetActiveDocumentUI() as ILuaEditDocumentUIUndoRedo;

            if (doc != null)
            {
                doc.Undo();
            }
        }

        /// <summary>
        /// Occurs when the redo menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentUIUndoRedo doc = GetActiveDocumentUI() as ILuaEditDocumentUIUndoRedo;

            if (doc != null)
            {
                doc.Redo();
            }
        }

        /// <summary>
        /// Occurs when the cut menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.Cut();
        }

        /// <summary>
        /// Occurs when the copy menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.Copy();
        }

        /// <summary>
        /// Occurs when the paste menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.Paste();
        }

        /// <summary>
        /// Occurs when the select all menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.SelectAll();
        }

        /// <summary>
        /// Occurs when the quick find menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quickFindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _findAndReplaceDlg.CurrentDocument = GetActiveDocument();
            _findAndReplaceDlg.Show(mainDockPanel, FindAndReplaceType.QuickFind);
        }

        /// <summary>
        /// Occurs when the find in files menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void findInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _findAndReplaceDlg.CurrentDocument = GetActiveDocument();
            _findAndReplaceDlg.Show(mainDockPanel, FindAndReplaceType.FindInFiles);
        }

        /// <summary>
        /// Occurs when the goto menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.ShowGoto();
        }

        /// <summary>
        /// Occurs when the toggle bookmark menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggleBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
            {
                activeDoc.ToggleBookmark(activeDoc.CurrentRow);
                RefreshBookmarksMenu();
            }
        }

        /// <summary>
        /// Occurs when the disable all bookmarks menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disableAllBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
            {
                activeDoc.EnableDisableAllBookmarks();
                RefreshBookmarksMenu();
            }
        }

        /// <summary>
        /// Occurs when the enable bookmark menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enableBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
            {
                if (activeDoc.CurrentRow.Bookmark != null)
                {
                    activeDoc.EnableDisableBookmark(activeDoc.CurrentRow.Bookmark);
                    RefreshBookmarksMenu();
                }
            }
        }

        /// <summary>
        /// Occurs when the previous bookmark menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previousBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.GotoPreviousBookmark();
        }

        /// <summary>
        /// Occurs when the next bookmark menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.GotoNextBookmark();
        }

        /// <summary>
        /// Occurs when the clear bookmarks menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null &&
                FrameworkManager.ShowMessageBox("Are you sure you want to delete all of the bookmark(s)?",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                activeDoc.ClearBookmarks();
            }
        }

        /// <summary>
        /// Occurs when the toggle outlining expansion menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggleOutliningExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.ToggleOutliningExpansion();
        }

        /// <summary>
        /// Occurs when the toggle all outlining menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggleAllOutliningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.ToggleAllOutlining();
        }

        /// <summary>
        /// Occurs when the stop outlining menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopOutliningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
            {
                activeDoc.IsOutlining = !activeDoc.IsOutlining;
                RefreshOutliningMenu();
            }
        }

        #endregion

        #region View Menu Events

        /// <summary>
        /// Occurs when the view solution explorer menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void solutionExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindow(typeof(SolutionExplorerDlg));
        }

        /// <summary>
        /// Occurs when the view document list menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void documentListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_docListDlg.ShowDialog(this) == DialogResult.OK)
            {
                foreach (ILuaEditDocument doc in _docListDlg.DocumentsToOpen)
                {
                    DocumentsManager.Instance.OpenDocument(doc);
                }
            }
        }

        /// <summary>
        /// Occurs when the view output menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void outputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindow(typeof(OutputDlg));

            ToolStripItem tsiSender = sender as ToolStripItem;
            if (tsiSender != null && _debugWindowsOwnerToolStripItem == tsbDebugWindows)
            {
                tsbDebugWindows.Image = tsiSender.Image;
                tsbDebugWindows.Text = tsiSender.Text;
                tsbDebugWindows.ToolTipText = tsbDebugWindows.Text;
                tsbDebugWindows.Tag = tsiSender;
            }
        }

        #endregion

        #region Debug Menu Events

        /// <summary>
        /// Occurs when the breakpoints window menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void breakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindow(typeof(BreakpointsDlg));

            ToolStripItem tsiSender = sender as ToolStripItem;
            if (tsiSender != null && _debugWindowsOwnerToolStripItem == tsbDebugWindows)
            {
                tsbDebugWindows.Image = tsiSender.Image;
                tsbDebugWindows.Text = tsiSender.Text;
                tsbDebugWindows.ToolTipText = tsbDebugWindows.Text;
                tsbDebugWindows.Tag = tsiSender;
            }
        }

        /// <summary>
        /// Occurs when the locals window menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void localsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindow(typeof(LocalsDlg));

            ToolStripItem tsiSender = sender as ToolStripItem;
            if (tsiSender != null && _debugWindowsOwnerToolStripItem == tsbDebugWindows)
            {
                tsbDebugWindows.Image = tsiSender.Image;
                tsbDebugWindows.Text = tsiSender.Text;
                tsbDebugWindows.ToolTipText = tsbDebugWindows.Text;
                tsbDebugWindows.Tag = tsiSender;
            }
        }

        /// <summary>
        /// Occurs when the call stack window menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void callStackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindow(typeof(CallStackDlg));

            ToolStripItem tsiSender = sender as ToolStripItem;
            if (tsiSender != null && _debugWindowsOwnerToolStripItem == tsbDebugWindows)
            {
                tsbDebugWindows.Image = tsiSender.Image;
                tsbDebugWindows.Text = tsiSender.Text;
                tsbDebugWindows.ToolTipText = tsbDebugWindows.Text;
                tsbDebugWindows.Tag = tsiSender;
            }
        }

        /// <summary>
        /// Occurs when the threads window menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void coroutinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWindow(typeof(CoroutinesDlg));

            ToolStripItem tsiSender = sender as ToolStripItem;
            if (tsiSender != null && _debugWindowsOwnerToolStripItem == tsbDebugWindows)
            {
                tsbDebugWindows.Image = tsiSender.Image;
                tsbDebugWindows.Text = tsiSender.Text;
                tsbDebugWindows.ToolTipText = tsbDebugWindows.Text;
                tsbDebugWindows.Tag = tsiSender;
            }
        }

        /// <summary>
        /// Occurs when the start debugging menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ClientDebugManager.Instance.IsDebugging)
            {
                StartDebugging(null, Int32.MaxValue);
            }
            else
            {
                // Pursue code's execution
                ClientDebugManager.Instance.AddCommand(new DebugActionCommand(DebugAction.Run));
            }

            RefreshMainWindowText();
            RefreshMenusAndToolbars();
        }

        /// <summary>
        /// Occurs when the break all menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void breakAllDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClientDebugManager.Instance.IsDebugging)
            {
                ClientDebugManager.Instance.AddCommand(new DebugActionCommand(DebugAction.Break));
            }

            RefreshMainWindowText();
            RefreshMenusAndToolbars();
        }

        /// <summary>
        /// Occurs when the stop debugging menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopDebuggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientDebugManager.Instance.StopDebugging();

            RefreshMainWindowText();
            RefreshMenusAndToolbars();
        }

        /// <summary>
        /// Occurs when the step over menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stepOverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ClientDebugManager.Instance.IsDebugging)
            {
                StartDebugging(null, 1);
            }
            else
            {
                ClientDebugManager.Instance.AddCommand(new DebugActionCommand(DebugAction.StepOver));
            }

            RefreshMainWindowText();
            RefreshMenusAndToolbars();
        }

        /// <summary>
        /// Occurs when the step into menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stepIntoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ClientDebugManager.Instance.IsDebugging)
            {
                StartDebugging(null, 1);
            }
            else
            {
                ClientDebugManager.Instance.AddCommand(new DebugActionCommand(DebugAction.StepInto));
            }

            RefreshMainWindowText();
            RefreshMenusAndToolbars();
        }

        /// <summary>
        /// Occurs when the step out menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stepOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ClientDebugManager.Instance.IsDebugging)
            {
                ClientDebugManager.Instance.AddCommand(new DebugActionCommand(DebugAction.StepOut));
            }

            RefreshMainWindowText();
            RefreshMenusAndToolbars();
        }

        /// <summary>
        /// Occurs when the attach to process menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void attachToMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_attachToMachineDlg.ShowDialog() == DialogResult.OK)
            {
                int port = _attachToMachineDlg.Port;
                string machineName = _attachToMachineDlg.MachineName;

                try
                {
                    IPAddress machineIP = _attachToMachineDlg.MachineIP;

                    if (machineIP == IPAddress.None)
                    {
                        throw new Exception("The specified target could not be resolved!");
                    }

                    DebugInfo di = new DebugInfo(machineIP, port, DebugStartAction.StartAttach);
                    ClientDebugManager.Instance.StartDebugging(di, Int32.MaxValue);
                }
                catch (Exception ex)
                {
                    string msg = string.Format("Cannot attach to machine '{0}': {1}", machineName, ex.Message);
                    FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Occurs when the syntax check menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkSyntaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentUI docUI = GetActiveDocumentUI();

            if (docUI != null && docUI.ParentDocument is ILuaEditDocumentEditable)
            {
                StringBuilder script = new StringBuilder((docUI.ParentDocument as ILuaEditDocumentEditable).Document.Text);
                StringBuilder scriptName = new StringBuilder(docUI.ParentDocument.FileName);
                StringBuilder err = new StringBuilder(2048);

                if (CheckLuaScriptSyntax(script, scriptName, err, err.Capacity))
                {
                    string errMsg = err.ToString();
                    ILuaEditDocumentEditableUI editableDocUI = docUI as ILuaEditDocumentEditableUI;

                    // Expected error message layout to be one of these cases:
                    //
                    //   -  [string <StringName>]:<LineNumber>:<ErrorMsg>
                    //   -  <FileName>:<LineNumber>:<ErrorMsg>
                    //
                    Regex regEx = new Regex("\\[string \\\"(?<StringName>.*)\\\"\\]\\:(?<LineNumber>[0-9]+)\\:(?<ErrorMsg>.*$)");
                    Match regExMatch = regEx.Match(errMsg);
                    string isolatedErrMsg = string.Empty;
                    int lineNumber = -1;
                    string strName = string.Empty;
                    
                    if (!regExMatch.Success)
                    {
                        regEx = new Regex("(?<StringName>.*)\\:(?<LineNumber>[0-9]+)\\:(?<ErrorMsg>.*$)");
                        regExMatch = regEx.Match(errMsg);
                    }

                    if (regExMatch.Success)
                    {
                        strName = regExMatch.Groups["StringName"].Value;
                        isolatedErrMsg = regExMatch.Groups["ErrorMsg"].Value;
                        lineNumber = Convert.ToInt32(regExMatch.Groups["LineNumber"].Value);
                    }

                    if (!string.IsNullOrEmpty(isolatedErrMsg))
                    {
                        isolatedErrMsg = isolatedErrMsg.TrimStart(new char[] { ' ' });
                        isolatedErrMsg = isolatedErrMsg.TrimEnd(new char[] { ' ' });
                        isolatedErrMsg = isolatedErrMsg[0].ToString().ToUpper() + isolatedErrMsg.Substring(1);
                        string msg = string.Format("'{0}': {1} ({2})", strName, isolatedErrMsg, lineNumber);
                        FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (lineNumber >= 0)
                        editableDocUI.Goto(lineNumber);
                }
                else
                {
                    FrameworkManager.ShowMessageBox("Syntax checked successfully!", MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Occurs when the toggle breakpoint menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggleBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
                activeDoc.ToggleBreakpoint(activeDoc.CurrentRow);
        }

        /// <summary>
        /// Occurs when the delete all breakpoints menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteAllBreakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BreakpointManager.Instance.DeleteAllBreakpoints();
        }

        /// <summary>
        /// Occurs when the disable all breakpoints menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disableAllBreakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BreakpointManager.Instance.DisableAllBreakpoints();
        }

        /// <summary>
        /// Occurs when the tool strip button debug windows is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDebugWindows_ButtonClick(object sender, EventArgs e)
        {
            ToolStripItem tsbSender = sender as ToolStripItem;

            if (tsbSender != null && tsbSender.Tag is ToolStripItem)
            {
                (tsbSender.Tag as ToolStripItem).PerformClick();
            }
        }

        #endregion

        #region Help Menu Events

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"License\License.html"));
        }

        private void aboutLuaEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutScreen aboutDlg = new AboutScreen();
            aboutDlg.ShowDialog();
        }

        #endregion

        #region Document Events

        /// <summary>
        /// Occurs when the document context menu is showing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuDocument_Opening(object sender, CancelEventArgs e)
        {
            ILuaEditDocument currentDoc = GetActiveDocument();

            if (currentDoc != null)
            {
                saveToolStripMenuItem1.Text = string.Format("Save {0}", Path.GetFileName(currentDoc.FileName));
            }
        }

        /// <summary>
        /// Occurs when the open containing folder menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openContainingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocument currentDoc = GetActiveDocument();

            if (currentDoc != null)
            {
                System.Diagnostics.Process.Start("explorer.exe", string.Format("/select, \"{0}\"", currentDoc.FileName));
            }
        }

        /// <summary>
        /// Occurs when the copy full path menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyFullPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocument currentDoc = GetActiveDocument();

            if (currentDoc != null)
            {
                Clipboard.SetText(currentDoc.FileName);
            }
        }

        /// <summary>
        /// Occurs when the close all but this menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeAllButThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ILuaEditDocument currentDoc = GetActiveDocument();
            List<ILuaEditDocument> docsToClose = new List<ILuaEditDocument>();

            foreach (IDockContent dockContent in mainDockPanel.Documents)
            {
                if (currentDoc != dockContent)
                {
                    docsToClose.Add(dockContent as ILuaEditDocument);
                }
            }

            if (docsToClose.Count > 0)
            {
                DocumentsManager.Instance.CloseDocuments(docsToClose);
            }
        }

        /// <summary>
        /// Occurs when a status message is requested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStatusMessageRequested(object sender, StatusMessageResquestArgs e)
        {
            // Multi-threading safety
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new StatusMessageResquestHandler(OnStatusMessageRequested), new object[] {sender, e});
                return;
            }

            // Display requested message using back and fore colors
            panelMessages.Text = e.Message;
            panelMessages.ForeColor = e.ForeColor;
            panelMessages.BackColor = e.BackColor;
        }

        /// <summary>
        /// Occurs when the FrameworkManager's current solution changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCurrentSolutionChanged(object sender, EventArgs e)
        {
            RefreshMenusAndToolbars();
            RefreshMainWindowText();
        }

        /// <summary>
        /// Occurs when the list of recent files has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRecentFilesChanged(object sender, EventArgs e)
        {
            RefreshRecentFilesMenu();
        }

        /// <summary>
        /// Occurs when the list of recent projects has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRecentProjectsChanged(object sender, EventArgs e)
        {
            RefreshRecentProjectsMenu();
        }

        /// <summary>
        /// Occurs when documents are being opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentsOpened(object sender, DocumentsActionArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DocumentsActionHandler(OnDocumentsOpened), new object[] { sender, e } );
                return;
            }

            foreach (ILuaEditDocument doc in e.Documents.Values)
            {
                if ((doc is ILuaEditDocumentEditable || doc is ILuaEditDocumentProject) && doc.DockContent != null)
                    ShowDocument(doc);
            }
        }

        /// <summary>
        /// Occurs when documents are being closed
        /// </summary>
        /// <param name="sende"></param>
        /// <param name="e"></param>
        private void OnDocumentsClosed(object sender, DocumentsActionArgs e)
        {
            foreach (ILuaEditDocument doc in e.Documents.Values)
            {
                if ((doc is ILuaEditDocumentEditable || doc is ILuaEditDocumentProject) &&
                    doc.DockContent != null)
                {
                    if (!doc.IsTerminated)
                    {
                        doc.DockContent.Close();
                    }

                    // Unbind document events
                    if (doc.DockContent is ILuaEditDocumentUI)
                    {
                        ILuaEditDocumentUI documentUI = doc.DockContent as ILuaEditDocumentUI;
                        documentUI.ContentChanged -= OnDocumentContentChanged;
                    }

                    if (doc.DockContent is ILuaEditDocumentEditableUI)
                    {
                        ILuaEditDocumentEditableUI documentUI = doc.DockContent as ILuaEditDocumentEditableUI;
                        documentUI.CaretChanged -= OnDocumentCaretChanged;
                        documentUI.SelectionChanged -= OnDocumentSelectionChanged;
                        documentUI.RunToRequested -= OnDocumentRunToRequested;
                    }

                    if (doc is ILuaEditDocument)
                    {
                        ILuaEditDocument document = doc as ILuaEditDocument;
                        document.ModifiedChanged -= OnDocumentModifiedChanged;
                        document.ReadOnlyChanged -= OnDocumentDiskReadOnlyChanged;
                        document.FileNameChanged -= OnDocumentFileNameChanged;
                    }

                    RefreshStatusBar();
                    RefreshFileMenuAndToolbar();
                }
            }
        }

        /// <summary>
        /// Occurs when a document has been modified from outside the framework
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentModifiedChanged(object sender, EventArgs e)
        {
            RefreshDocumentTab(sender as ILuaEditDocument);
        }

        /// <summary>
        /// Occurs when the content of a document changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentContentChanged(object sender, EventArgs e)
        {
            RefreshEditMenuAndToolbar();
        }

        /// <summary>
        /// Occurs when the selection of a document changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentSelectionChanged(object sender, EventArgs e)
        {
            RefreshEditMenuAndToolbar();
        }

        /// <summary>
        /// Occurs when a document has requested a run to operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentRunToRequested(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI senderUI = sender as ILuaEditDocumentEditableUI;

            if (senderUI != null)
            {
                if (ClientDebugManager.Instance.IsDebugging)
                {
                    RunToCommand runToCmd = new RunToCommand(senderUI.ParentDocument.FileName, senderUI.CurrentLine);
                    ClientDebugManager.Instance.AddCommand(runToCmd);
                }
                else
                {
                    StartDebugging(senderUI.ParentDocument as ILuaEditDocumentEditable, senderUI.CurrentLine);
                }
            }
        }

        /// <summary>
        /// Occurs when a document's file name has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentFileNameChanged(object sender, EventArgs e)
        {
            RefreshDocumentTab(sender as ILuaEditDocument);
        }

        /// <summary>
        /// Occurs when a document's readonly status has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentDiskReadOnlyChanged(object sender, EventArgs e)
        {
            RefreshDocumentTab(sender as ILuaEditDocument);
        }

        /// <summary>
        /// Occurs when a document's caret position has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentCaretChanged(object sender, EventArgs e)
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;
            RefreshStatusBar();
        }

        #endregion

        #region Dock Events

        /// <summary>
        /// Occurs when the active content has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainDockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            ShowReadyStatusMessage();
        }

        /// <summary>
        /// Occurs when the active pane has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainDockPanel_ActivePaneChanged(object sender, EventArgs e)
        {
            ShowReadyStatusMessage();
        }

        /// <summary>
        /// Occurs when the active document has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainDockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            _findAndReplaceDlg.CurrentDocument = GetActiveDocument();

            ShowReadyStatusMessage();
            RefreshMenusAndToolbars();
        }

        /// <summary>
        /// Occurs when the layout is being deserialized from the disk
        /// </summary>
        /// <param name="persistString"></param>
        /// <returns></returns>
        public IDockContent OnDeserializeDockContent(string persistString)
        {
            Type dockContentType = Type.GetType(persistString);

            if (dockContentType != null)
            {
                if (_windows.ContainsKey(dockContentType))
                {
                    return _windows[dockContentType];
                }
                else
                {
                    _windows.Add(dockContentType, Activator.CreateInstance(dockContentType) as DockContent);
                    return _windows[dockContentType];
                }
            }

            return null;

            //return Assembly.GetExecutingAssembly().CreateInstance(persistString) as IDockContent;
        }

        #endregion

        #region Dialog Events

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);

                if (a != null)
                {
                    for (int x = 0; x < a.Length; ++x)
                    {
                        // Extract string from array
                        string fileName = a.GetValue(x).ToString();

                        // Opens the files asynchronously.
                        // Explorer instance from which file is dropped is not responding
                        // all the time when DragDrop handler is active, so we need to return
                        // immidiately (especially if opening the file shows MessageBox).
                        this.BeginInvoke(new OpenDocumentDelegate(DocumentsManager.Instance.OpenDocument),
                                         new object[] { fileName, true });
                    }

                    // In the case Explorer overlaps this form
                    this.Activate();
                }
            }
            catch (Exception ex)
            {
                // todo: report error in output window
                //Trace.WriteLine("Error in DragDrop function: " + ex.Message);
            }

        }

        /// <summary>
        /// Occurs when this dialog is just about to close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ClientDebugManager.Instance.IsDebugging)
            {
                if (FrameworkManager.ShowMessageBox("Do you want to stop debugging?",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ClientDebugManager.Instance.StopDebugging();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            List<ILuaEditDocument> docs = new List<ILuaEditDocument>();
            foreach (ILuaEditDocument doc in DocumentsManager.Instance.OpenedDocuments.Values)
            {
                docs.Add(doc);
            }

            e.Cancel = !DocumentsManager.Instance.CloseDocuments(docs);

            if (!e.Cancel)
            {
                SaveCurrentLayout(_editLayoutFileName);
                ToolStripManager.SaveSettings(this, this.Name);
            }
        }

        /// <summary>
        /// Occurs once this dialog has been closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FrameworkManager.Instance.Dispose();
            DocumentsManager.Instance.Dispose();
            (FindWindow(typeof(OutputDlg)) as OutputDlg).Dispose();
        }

        /// <summary>
        /// Occurs when the debug windows context menu is about to open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuDebugWindows_Opening(object sender, CancelEventArgs e)
        {
            _debugWindowsOwnerToolStripItem = (sender as ContextMenuStrip).OwnerItem;
        }

        #endregion

        #region Debug Events

        void OnDebuggingStarted(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DebuggingStoppedEventHandler(OnDebuggingStarted), new object[] { sender, e });
                return;
            }

            SwitchLayout(_editLayoutFileName, _debugLayoutFileName);
        }

        void OnDebuggingStopped(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DebuggingStoppedEventHandler(OnDebuggingStopped), new object[] { sender, e });
                return;
            }

            // Reset everything here (EG: remove break markers, clear locals and call stack, etc...)
            foreach (ILuaEditDocument doc in DocumentsManager.Instance.OpenedDocuments.Values)
            {
                if (doc is ILuaEditDocumentEditable)
                {
                    ILuaEditDocumentEditable luaDoc = doc as ILuaEditDocumentEditable;

                    if (luaDoc.DocumentUI.MarkedLines != null && luaDoc.DocumentUI.MarkedLines.Count > 0)
                    {
                        int[] markedLines = new int[] { };
                        Array.Resize<int>(ref markedLines, luaDoc.DocumentUI.MarkedLines.Count);
                        luaDoc.DocumentUI.MarkedLines.CopyTo(markedLines);

                        foreach (int markedLine in markedLines)
                        {
                            luaDoc.DocumentUI.MarkLine(markedLine, false);
                            luaDoc.DocumentUI.HighlightLine(markedLine, Color.White);
                        }

                        luaDoc.DocumentUI.MarkedLines.Clear();
                    }
                }
            }

            SwitchLayout(_debugLayoutFileName, _editLayoutFileName);

            RefreshMenusAndToolbars();
            RefreshMainWindowText();
        }

        private void OnBreakChanged(object sender, EventArgs e)
        {
            RefreshMenusAndToolbars();
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Switches between one layout and another
        /// </summary>
        /// <param name="currentLayoutFileName">The layout file name to use to save the current layout</param>
        /// <param name="layoutToLoadFileName">The layout file name to load</param>
        private void SwitchLayout(string currentLayoutFileName, string layoutToLoadFileName)
        {
            // Bakc-up all opened documents and the currently active document
            List<ILuaEditDocument> openedDocs = new List<ILuaEditDocument>();
            List<IDockContent> dockContents = new List<IDockContent>();
            ILuaEditDocument activeDoc = GetActiveDocument();
            ILuaEditDocumentEditable activeDocEditable = activeDoc as ILuaEditDocumentEditable;
            TextPoint caretPos = null;
            
            if (activeDocEditable != null)
            {
                caretPos = activeDocEditable.DocumentUI.Editor.Caret.Position;
            }

            foreach (IDockContent dockContent in mainDockPanel.Documents)
            {
                dockContents.Add(dockContent);

                if (dockContent is ILuaEditDocumentUI)
                {
                    openedDocs.Add((dockContent as ILuaEditDocumentUI).ParentDocument);
                }
            }

            while (dockContents.Count > 0)
            {
                dockContents[0].DockHandler.Close();
                dockContents.RemoveAt(0);
            }

            // Does the layout switch
            SaveCurrentLayout(currentLayoutFileName);
            LoadLayout(layoutToLoadFileName);

            // Restore opened documents and reselect the previously active document
            foreach (ILuaEditDocument doc in openedDocs)
            {
                DocumentsManager.Instance.OpenDocument(doc);
            }

            if (activeDoc != null)
            {
                if (activeDocEditable != null)
                {
                    DocumentsManager.Instance.OpenDocument(activeDoc, new PostOpenDocumentOptions(caretPos));
                }
                else
                {
                    DocumentsManager.Instance.OpenDocument(activeDoc);
                }
            }
        }

        /// <summary>
        /// Save the current layout under the specified file name
        /// </summary>
        /// <param name="layoutFileName">The file name to use for saving the current layout</param>
        private void SaveCurrentLayout(string layoutFileName)
        {
            try
            {
                mainDockPanel.SaveAsXml(layoutFileName);
            }
            catch (Exception e)
            {
                string msg = string.Format("Could not save the current layout under '{0}': {1}", layoutFileName, e.Message);
                FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads the specified layout
        /// </summary>
        /// <param name="layoutFileName">The layout's file name to load</param>
        private void LoadLayout(string layoutFileName)
        {
            try
            {
                if (File.Exists(layoutFileName))
                {
                    while (mainDockPanel.Contents.Count > 0)
                    {
                        if (mainDockPanel.Contents.Count > 0)
                        {
                            mainDockPanel.Contents[0].DockHandler.DockPanel = null;
                        }

                        if (mainDockPanel.Contents.Count > 0)
                        {
                            mainDockPanel.Contents[0].DockHandler.FloatPane = null;
                        }

                        if (mainDockPanel.Contents.Count > 0)
                        {
                            mainDockPanel.Contents[0].DockHandler.PanelPane = null;
                        }
                    }

                    mainDockPanel.LoadFromXml(layoutFileName, OnDeserializeDockContent);
                }
            }
            catch (Exception e)
            {
                string msg = string.Format("Could not load layout '{0}': {1}", layoutFileName, e.Message);
                FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ILuaEditDocument GetActiveDocument()
        {
            ILuaEditDocumentUI docUI = GetActiveDocumentUI();

            if (docUI != null)
            {
                return docUI.ParentDocument;
            }

            return null;
        }

        /// <summary>
        /// Get the current active document
        /// </summary>
        /// <returns>The current active document</returns>
        private ILuaEditDocumentUI GetActiveDocumentUI()
        {
            return mainDockPanel.ActiveDocument as ILuaEditDocumentUI;
        }

        /// <summary>
        /// Determine whether or not the specified document is currently opened
        /// </summary>
        /// <param name="doc">The document on which to perform the test</param>
        /// <returns>True if the document is currently opened. Otherwise false.</returns>
        private bool IsDocumentOpened(ILuaEditDocument doc)
        {
            foreach (IDockContent dockedDoc in mainDockPanel.Documents)
            {
                ILuaEditDocumentEditableUI docUI = dockedDoc as ILuaEditDocumentEditableUI;

                if (docUI != null && docUI.ParentDocument == doc)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a window of the specified type or finds it if it already exists
        /// </summary>
        /// <param name="windowType">The type of the window to create</param>
        /// <returns></returns>
        private DockContent CreateOrFindWindow(Type windowType)
        {
            if (windowType.IsSubclassOf(typeof(DockContent)) || windowType == typeof(DockContent))
            {
                if (!_windows.ContainsKey(windowType))
                {
                    DockContent dockContent = windowType.Assembly.CreateInstance(windowType.FullName) as DockContent;
                    _windows.Add(windowType, dockContent);
                }

                return _windows[windowType];
            }

            return null;
        }

        /// <summary>
        /// Show the window of the specified type
        /// </summary>
        /// <param name="windowType">The type of the window to show</param>
        private void ShowWindow(Type windowType)
        {
            if (windowType.IsSubclassOf(typeof(DockContent)) || windowType == typeof(DockContent))
            {
                DockContent dockContent = CreateOrFindWindow(windowType);

                if (dockContent != null)
                {
                    dockContent.Show(mainDockPanel);
                }
            }
        }

        /// <summary>
        /// Finds the window of the specified type
        /// </summary>
        /// <param name="windowType">The type of the window to find</param>
        /// <returns></returns>
        private DockContent FindWindow(Type windowType)
        {
            return _windows.ContainsKey(windowType) ? _windows[windowType] : null;
        }

        /// <summary>
        /// Bind all document specific events to the specified document
        /// </summary>
        /// <param name="doc">The document on which to bind the events</param>
        private void ShowDocument(ILuaEditDocument doc)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowDocumentDelegate(ShowDocument), new object[] { doc });
                return;
            }

            if (doc != null)
            {
                if (IsDocumentOpened(doc))
                {
                    doc.DockContent.Show(mainDockPanel);
                }
                else
                {
                    doc.DockContent.DockHandler.TabPageContextMenuStrip = contextMenuDocument;

                    if (doc.DockContent is ILuaEditDocumentUI)
                    {
                        // Bind document events
                        ILuaEditDocumentUI documentUI = doc.DockContent as ILuaEditDocumentUI;
                        documentUI.ContentChanged += OnDocumentContentChanged;
                    }

                    if (doc.DockContent is ILuaEditDocumentEditableUI)
                    {
                        // Bind document events
                        ILuaEditDocumentEditableUI documentUI = doc.DockContent as ILuaEditDocumentEditableUI;
                        documentUI.CaretChanged += OnDocumentCaretChanged;
                        documentUI.SelectionChanged += OnDocumentSelectionChanged;
                        documentUI.RunToRequested += OnDocumentRunToRequested;
                    }
                    
                    if (doc is ILuaEditDocument)
                    {
                        // Show the document
                        ILuaEditDocument document = doc as ILuaEditDocument;
                        document.DockContent.Show(mainDockPanel);

                        // Bind document events
                        document.ModifiedChanged += OnDocumentModifiedChanged;
                        document.ReadOnlyChanged += OnDocumentDiskReadOnlyChanged;
                        document.FileNameChanged += OnDocumentFileNameChanged;
                    }
                }

                RefreshActiveDocumentTab();
            }
        }

        /// <summary>
        /// Closes all currently opened documents
        /// </summary>
        /// <returns>False if cancelation has been requester by user. Otherwise true.</returns>
        private bool CloseAllTabbedDocuments()
        {
            List<ILuaEditDocument> docs = new List<ILuaEditDocument>();

            foreach (IDockContent doc in mainDockPanel.Documents)
            {
                docs.Add((doc as ILuaEditDocumentUI).ParentDocument);
            }

            return DocumentsManager.Instance.CloseDocuments(docs);
        }

        /// <summary>
        /// Refreshes this dialog's text
        /// </summary>
        private void RefreshMainWindowText()
        {
            if (DocumentsManager.Instance.CurrentSolution != null)
            {
                string solutionShotName = Path.GetFileNameWithoutExtension(DocumentsManager.Instance.CurrentSolution.FileName);

                if (ClientDebugManager.Instance.IsDebugging)
                {
                    this.Text = solutionShotName + " (Running) - LuaEdit";
                }
                else
                {
                    this.Text = solutionShotName + " - LuaEdit";
                }
            }
            else
            {
                this.Text = "LuaEdit";
            }
        }

        /// <summary>
        /// Show a simple "Ready" status message
        /// </summary>
        private void ShowReadyStatusMessage()
        {
            FrameworkManager.Instance.RequestStatusMessage("Ready", SystemColors.Control, Color.Black);
        }

        /// <summary>
        /// Refresh all menus
        /// </summary>
        private void RefreshMenusAndToolbars()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new RefreshMenusDelegate(RefreshMenusAndToolbars));
                return;
            }

            RefreshFileMenuAndToolbar();
            RefreshEditMenuAndToolbar();
            RefreshDebugMenuAndToolbar();
        }

        /// <summary>
        /// Refresh the file menu
        /// </summary>
        private void RefreshFileMenuAndToolbar()
        {
            ILuaEditDocument activeDoc = GetActiveDocument();

            if (activeDoc != null)
            {
                // Update both status bar and current tab
                RefreshStatusBar();
                RefreshActiveDocumentTab();

                // Update menu captions
                saveToolStripMenuItem.Text = string.Format("Save {0}", activeDoc.ToString());
                tsbSave.Text = string.Format("Save {0} (Ctrl+S)", activeDoc.ToString());
                tsbSave.ToolTipText = tsbSave.Text;
                saveAsToolStripMenuItem.Text = string.Format("Save {0} As...", activeDoc.ToString());
            }
            else
            {
                saveToolStripMenuItem.Text = "Save";
                tsbSave.Text = "Save (Ctrl+S)";
                tsbSave.ToolTipText = tsbSave.Text;
                saveAsToolStripMenuItem.Text = "Save As...";
            }

            RefreshRecentFilesMenu();
            RefreshRecentProjectsMenu();

            closeToolStripMenuItem.Enabled = activeDoc != null;
            closeSolutionToolStripMenuItem.Enabled = DocumentsManager.Instance.CurrentSolution != null;
            saveToolStripMenuItem.Enabled = activeDoc != null;
            tsbSave.Enabled = saveToolStripMenuItem.Enabled;
            saveAsToolStripMenuItem.Enabled = activeDoc != null;
            saveAllToolStripMenuItem.Enabled = DocumentsManager.Instance.OpenedDocuments.Count > 1;
            tsbSaveAll.Enabled = saveAllToolStripMenuItem.Enabled;
        }

        /// <summary>
        /// Refresh the debug menu
        /// </summary>
        public void RefreshDebugMenuAndToolbar()
        {
            startDebuggingToolStripMenuItem.Enabled = DocumentsManager.Instance.CurrentSolution != null;
            tsbStartDebugging.Enabled = startDebuggingToolStripMenuItem.Enabled;

            if (ClientDebugManager.Instance.IsDebugging)
            {
                startDebuggingToolStripMenuItem.Text = "Continue";
                tsbStartDebugging.Text = "Continue (F5)";

                if (ClientDebugManager.Instance.DebugInfo != null &&
                    ClientDebugManager.Instance.DebugInfo.StartAction == DebugStartAction.StartAttach)
                {
                    stopDebuggingToolStripMenuItem.Text = "Detach";
                    tsbStopDebugging.Text = "Detach (Shift+F5)";
                }
                else
                {
                    stopDebuggingToolStripMenuItem.Text = "Stop";
                    tsbStopDebugging.Text = "Stop Debugging (Shift+F5)";
                }
            }
            else
            {
                startDebuggingToolStripMenuItem.Text = "Start Debugging";
                tsbStartDebugging.Text = "Start Debugging (F5)";
            }

            stepIntoToolStripMenuItem.Enabled = ClientDebugManager.Instance.IsBreaked;
            tsbStepInto.Enabled = stepIntoToolStripMenuItem.Enabled;
            stepOverToolStripMenuItem.Enabled = ClientDebugManager.Instance.IsBreaked;
            tsbStepOver.Enabled = stepOverToolStripMenuItem.Enabled;
            stepOutToolStripMenuItem.Visible = ClientDebugManager.Instance.IsDebugging;
            stepOutToolStripMenuItem.Enabled = ClientDebugManager.Instance.IsBreaked;
            tsbStepOut.Enabled = stepOutToolStripMenuItem.Enabled;
            breakAllDebuggingToolStripMenuItem.Visible = ClientDebugManager.Instance.IsDebugging;
            breakAllDebuggingToolStripMenuItem.Enabled = !ClientDebugManager.Instance.IsBreaked;
            tsbBreakAll.Enabled = breakAllDebuggingToolStripMenuItem.Enabled && ClientDebugManager.Instance.IsDebugging;

            stopDebuggingToolStripMenuItem.Visible = ClientDebugManager.Instance.IsDebugging;
            stopDebuggingToolStripMenuItem.Enabled = ClientDebugManager.Instance.IsDebugging;
            tsbStopDebugging.Enabled = stopDebuggingToolStripMenuItem.Enabled;

            toolStripMenuItem11.Visible = ClientDebugManager.Instance.IsDebugging;
            localsToolStripMenuItem.Visible = ClientDebugManager.Instance.IsDebugging;
            localsToolStripMenuItem.Enabled = ClientDebugManager.Instance.IsDebugging;
            toolStripMenuItem12.Visible = ClientDebugManager.Instance.IsDebugging;
            callStackToolStripMenuItem.Visible = ClientDebugManager.Instance.IsDebugging;
            callStackToolStripMenuItem.Enabled = ClientDebugManager.Instance.IsDebugging;
            coroutinesToolStripMenuItem.Visible = ClientDebugManager.Instance.IsDebugging;
            coroutinesToolStripMenuItem.Enabled = ClientDebugManager.Instance.IsDebugging;

            deleteAllBreakpointsToolStripMenuItem.Enabled = BreakpointManager.Instance.Breakpoints.Count > 0;
            disableAllBreakpointsToolStripMenuItem.Enabled = BreakpointManager.Instance.Breakpoints.Count > 0;
        }

        /// <summary>
        /// Refresh the edit menu
        /// </summary>
        private void RefreshEditMenuAndToolbar()
        {
            ILuaEditDocumentUIUndoRedo undoRedoDoc = GetActiveDocumentUI() as ILuaEditDocumentUIUndoRedo;
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            undoToolStripMenuItem.Enabled = undoRedoDoc != null && undoRedoDoc.CanUndo;
            tsbUndo.Enabled = undoToolStripMenuItem.Enabled;
            redoToolStripMenuItem.Enabled = undoRedoDoc != null && undoRedoDoc.CanRedo;
            tsbRedo.Enabled = redoToolStripMenuItem.Enabled;
            cutToolStripMenuItem.Enabled = activeDoc != null && activeDoc.Editor.Selection.SelLength != 0;
            tsbCut.Enabled = cutToolStripMenuItem.Enabled;
            copyToolStripMenuItem.Enabled = activeDoc != null && activeDoc.Editor.Selection.SelLength != 0;
            tsbCopy.Enabled = copyToolStripMenuItem.Enabled;
            pasteToolStripMenuItem.Enabled = activeDoc != null && Clipboard.ContainsText();
            tsbPaste.Enabled = pasteToolStripMenuItem.Enabled;
            selectAllToolStripMenuItem.Enabled = activeDoc != null;
            goToToolStripMenuItem.Enabled = activeDoc != null;
            toggleBookmarkToolStripMenuItem.Enabled = activeDoc != null;
            disableAllBookmarksToolStripMenuItem.Enabled = activeDoc != null;
            enableBookmarkToolStripMenuItem.Enabled = activeDoc != null;
            previousBookmarkToolStripMenuItem.Enabled = activeDoc != null;
            nextBookmarkToolStripMenuItem.Enabled = activeDoc != null;
            clearBookmarksToolStripMenuItem.Enabled = activeDoc != null;
            toggleOutliningExpansionToolStripMenuItem.Enabled = activeDoc != null;
            toggleAllOutliningToolStripMenuItem.Enabled = activeDoc != null;
            stopOutliningToolStripMenuItem.Enabled = activeDoc != null;
            toggleAllOutliningToolStripMenuItem.Visible = activeDoc != null && activeDoc.IsOutlining;
            toggleAllOutliningToolStripMenuItem.Enabled = activeDoc != null && activeDoc.IsOutlining;
            toggleOutliningExpansionToolStripMenuItem.Visible = activeDoc != null && activeDoc.IsOutlining;
            toggleOutliningExpansionToolStripMenuItem.Enabled = activeDoc != null && activeDoc.IsOutlining;
            checkSyntaxToolStripMenuItem.Enabled = activeDoc != null;
            toggleBreakpointToolStripMenuItem.Enabled = activeDoc != null;

            RefreshBookmarksMenu();
            RefreshOutliningMenu();
        }

        /// <summary>
        /// Refresh the bookmarks menu
        /// </summary>
        private void RefreshBookmarksMenu()
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
            {
                if (activeDoc.IsAllBookmarksEnabled)
                    disableAllBookmarksToolStripMenuItem.Text = "Disable All Bookmarks";
                else
                    disableAllBookmarksToolStripMenuItem.Text = "Enable All Bookmarks";

                if (activeDoc.CurrentRow != null && activeDoc.CurrentRow.Bookmark != null)
                {
                    if (activeDoc.CurrentRow.Bookmark != null && activeDoc.CurrentRow.Bookmark.Enabled)
                        enableBookmarkToolStripMenuItem.Text = "Disable Bookmark";
                    else
                        enableBookmarkToolStripMenuItem.Text = "Enable Bookmark";
                }
            }
        }

        /// <summary>
        /// Refresh the outlining menu
        /// </summary>
        private void RefreshOutliningMenu()
        {
            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
            {
                if (activeDoc.IsOutlining)
                    stopOutliningToolStripMenuItem.Text = "Stop Outlining";
                else
                    stopOutliningToolStripMenuItem.Text = "Start Outlining";
            }
        }

        /// <summary>
        /// Refresh the recent projects menu
        /// </summary>
        private void RefreshRecentProjectsMenu()
        {
            // Refresh text
            int count = 0;
            recentProjectsToolStripMenuItem.DropDownItems.Clear();

            if (DocumentsManager.Instance.RecentProjects.Count > 0)
            {
                foreach (string fileName in DocumentsManager.Instance.RecentProjects)
                {
                    ToolStripMenuItem subMenu = new ToolStripMenuItem();
                    subMenu.Text = string.Format("{0} {1}", ++count, fileName);
                    subMenu.Click += OnRecentProjectSubMenuClicked;
                    recentProjectsToolStripMenuItem.DropDownItems.Add(subMenu);
                }
            }
            else
            {
                ToolStripMenuItem subMenu = new ToolStripMenuItem();
                subMenu.Text = "(Empty)";
                subMenu.Enabled = false;
                recentProjectsToolStripMenuItem.DropDownItems.Add(subMenu);
            }
        }

        /// <summary>
        /// Refresh the recent files menu
        /// </summary>
        private void RefreshRecentFilesMenu()
        {
            // Refresh text
            int count = 0;
            recentFilesToolStripMenuItem.DropDownItems.Clear();

            if (DocumentsManager.Instance.RecentFiles.Count > 0)
            {
                foreach (string fileName in DocumentsManager.Instance.RecentFiles)
                {
                    ToolStripMenuItem subMenu = new ToolStripMenuItem();
                    subMenu.Text = string.Format("{0} {1}", ++count, fileName);
                    subMenu.Click += OnRecentFileSubMenuClicked;
                    recentFilesToolStripMenuItem.DropDownItems.Add(subMenu);
                }
            }
            else
            {
                ToolStripMenuItem subMenu = new ToolStripMenuItem();
                subMenu.Text = "(Empty)";
                subMenu.Enabled = false;
                recentFilesToolStripMenuItem.DropDownItems.Add(subMenu);
            }
        }
        
        /// <summary>
        /// Refresh the currently active document's tab
        /// </summary>
        private void RefreshActiveDocumentTab()
        {
            ILuaEditDocument activeDoc = GetActiveDocument();
            if (activeDoc != null)
                RefreshDocumentTab(activeDoc);
        }

        /// <summary>
        /// Refresh all document's tab
        /// </summary>
        private void RefreshAllDocumentTabs()
        {
            foreach (IDockContent doc in mainDockPanel.Documents)
            {
                if (doc is ILuaEditDocument)
                {
                    RefreshDocumentTab(doc as ILuaEditDocument);
                }
                else if (doc is ILuaEditDocumentProject)
                {
                    RefreshDocumentTab(doc as ILuaEditDocumentProject);
                }
            }
        }

        /// <summary>
        /// Refresh the specified document's tab
        /// </summary>
        /// <param name="doc">The document for which the tab must be refresh</param>
        private void RefreshDocumentTab(ILuaEditDocument doc)
        {
            if (!doc.DockContent.IsDisposed)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new RefreshDocumentTabDelegate(RefreshDocumentTab), new object[] { doc });
                    return;
                }

                string tabText = "";
                string tabToolTip = "";

                // Set the modfied visual status
                if (doc.IsModified)
                {
                    tabText = string.Format("{0}*", doc.ToString());
                    tabToolTip = string.Format("{0}*", doc.FileName);
                }
                else
                {
                    tabText = doc.ToString();
                    tabToolTip = doc.FileName;
                }

                doc.DockContent.ShowStatusIcon = doc.ReadOnly;
                if (doc.ReadOnly)
                    tabToolTip += " [Read Only]";

                doc.DockContent.TabText = tabText;
                doc.DockContent.ToolTipText = tabToolTip;
            }
        }

        /// <summary>
        /// Refresh the status bar to display proper information about the current active document
        /// </summary>
        /// <param name="activeDoc"></param>
        private void RefreshStatusBar()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new RefreshStatusBarDelegate(RefreshStatusBar));
                return;
            }

            ILuaEditDocumentEditableUI activeDoc = GetActiveDocumentUI() as ILuaEditDocumentEditableUI;

            if (activeDoc != null)
            {
                mainStatusBar.Items["panelLine"].Text = string.Format("Ln {0}", activeDoc.CurrentLine);
                mainStatusBar.Items["panelColumn"].Text = string.Format("Col {0}", activeDoc.CurrentColumn);
                mainStatusBar.Items["panelCharacter"].Text = string.Format("Ch {0}", activeDoc.CurrentCharacter);
                mainStatusBar.Items["panelCaretMode"].Text = activeDoc.IsOverwrite ? "OVR" : "INS";
            }
            else
            {
                mainStatusBar.Items["panelLine"].Text = "";
                mainStatusBar.Items["panelColumn"].Text = "";
                mainStatusBar.Items["panelCharacter"].Text = "";
                mainStatusBar.Items["panelCaretMode"].Text = "";
            }
        }

        private void StartDebugging(ILuaEditDocumentEditable documentToDebug, int runToLine)
        {
            if (!ClientDebugManager.Instance.IsDebugging)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    // Start the debugging session
                    int processID = -1;
                    List<ILuaEditDocument> docs = new List<ILuaEditDocument>();
                    ILuaEditDocument doc = null;
                    ILuaEditDocumentProject activePrj = DocumentsManager.Instance.CurrentSolution.ActiveProject;
                    int debugPort = activePrj != null && activePrj.ProjectProperties.RemotePort > 0 ? activePrj.ProjectProperties.RemotePort : ClientDebugManager.DefaultDebugPort;
                    string workingDir = activePrj != null ? activePrj.ProjectProperties.WorkingDirectory : string.Empty;
                    DebugStartAction startAction = activePrj != null ? activePrj.ProjectProperties.StartAction : DebugStartAction.StartProject;

                    if (documentToDebug == null)
                    {
                        // Take currently active document if there is no currently active project
                        if (activePrj == null)
                        {
                            doc = GetActiveDocument();

                            if (doc == null || !(doc is ILuaEditDocumentEditable))
                            {
                                doc = null;
                            }
                        }
                        else
                        {
                            // If start action of currently active project is StartProject, we either
                            // start the specified startup file name or the currently active document.
                            // Otherwise, if start action is StartExternalProgram, we start the specified
                            // external program and we then attach to it.
                            if (activePrj.ProjectProperties.StartAction == DebugStartAction.StartProject)
                            {
                                // If startup file name is null or empty, we take the currently
                                // active document as start up file name
                                if (string.IsNullOrEmpty(activePrj.ProjectProperties.StartupFileName))
                                {
                                    doc = GetActiveDocument();

                                    if (doc == null || !(doc is ILuaEditDocumentEditable))
                                    {
                                        doc = null;
                                    }
                                }
                                else
                                {
                                    doc = activePrj.FindDocument(Win32Utils.GetAbsolutePath(activePrj.ProjectProperties.StartupFileName, activePrj.FileName));

                                    if (doc == null)
                                    {
                                        throw new Exception(string.Format("The startup file '{0}' could not be found in project '{1}'!", activePrj, activePrj.ProjectProperties.StartupFileName));
                                    }
                                }
                            }
                            else if (activePrj.ProjectProperties.StartAction == DebugStartAction.StartExternalProgram)
                            {
                                string cmdLineArgs = activePrj.ProjectProperties.CommandLineArguments;
                                string programFileName = activePrj.ProjectProperties.ExternalProgram;

                                if (!string.IsNullOrEmpty(programFileName) &&
                                    !Path.IsPathRooted(programFileName))
                                {
                                    programFileName = Win32Utils.GetAbsolutePath(activePrj.ProjectProperties.ExternalProgram,
                                                                                 activePrj.FileName);
                                }

                                if (string.IsNullOrEmpty(programFileName) || !File.Exists(programFileName))
                                {
                                    throw new Exception(string.Format("Cannot start external program '{0}' because it doesn't exists!", programFileName));
                                }

                                ProcessStartInfo psi = new ProcessStartInfo(programFileName);
                                psi.Arguments = string.IsNullOrEmpty(cmdLineArgs) ? string.Empty : cmdLineArgs;

                                if (!string.IsNullOrEmpty(workingDir))
                                {
                                    psi.WorkingDirectory = workingDir;
                                }

                                Process p = Process.Start(psi);
                                processID = p.Id;

                                // Wait until process starts and opens the specified port
                                // (or 10 seconds timeout)
                                const int ProcessTimeOut = 10000;
                                int timeout = 0;

                                while (timeout < ProcessTimeOut)
                                {
                                    IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                                    IPEndPoint[] tcpListeners = ipGlobalProperties.GetActiveTcpListeners();
                                    int pidFromPort = -1;

                                    foreach (IPEndPoint ipep in tcpListeners)
                                    {
                                        if (ipep.Port == debugPort)
                                        {
                                            pidFromPort = IpHlpApi.PortToPID(debugPort);
                                            break;
                                        }
                                    }

                                    if (pidFromPort > 0)
                                    {
                                        if (pidFromPort != processID)
                                        {
                                            throw new Exception(string.Format("The port {0} is used by another process (PID: {1}) which is not the specified external program!", debugPort, pidFromPort));
                                        }

                                        break;
                                    }

                                    Thread.Sleep(100);
                                    Application.DoEvents();
                                    timeout += 120;
                                }

                                if (timeout >= ProcessTimeOut)
                                {
                                    throw new Exception(string.Format("The external program '{0}' failed to open port {1} within the 10 seconds timeout.", programFileName, debugPort));
                                }
                            }
                        }
                    }
                    else
                    {
                        doc = documentToDebug;
                    }

                    // Ensure the starting document is valid
                    if (activePrj.ProjectProperties.StartAction == DebugStartAction.StartProject)
                    {
                        if (doc == null)
                        {
                            throw new Exception("There is no currently active document to start debugging!");
                        }
                        else if (!(doc is ILuaEditDocumentEditable))
                        {
                            throw new Exception(string.Format("The currently active document {0} is not debuggable!", doc));
                        }
                    }

                    // Always save all documents before deubgging
                    DocumentsManager.Instance.SaveAllDocuments();

                    // Determine IP to use for debugging
                    IPAddress debugIP = activePrj != null ? activePrj.ProjectProperties.RemoteMachineIP : IPAddress.Parse("127.0.0.1");

                    if (debugIP == IPAddress.None)
                    {
                        throw new Exception(string.Format("The remote machine '{0}' does not resolve to any IP address!", activePrj.ProjectProperties.RemoteMachineName));
                    }

                    DebugInfo di = new DebugInfo(processID, debugIP, debugPort, workingDir, docs, doc, startAction);
                    ClientDebugManager.Instance.StartDebugging(di, runToLine);
                }
                catch (Exception e)
                {
                    string msg = string.Format("Could not start debugging: {0}", e.Message);
                    FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        #endregion
    }
}