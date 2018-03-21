using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Fireball.Syntax;
using Fireball.Windows.Forms;
using Fireball.Windows.Forms.CodeEditor;

using DotNetLib.Controls;
using LuaEdit.Interfaces;
using LuaEdit.Managers;
using LuaEdit.Documents.DocumentUtils;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Documents
{
    public partial class EditableDocumentBaseUI : DockContent, ILuaEditDocumentEditableUI
    {
        #region Members

        protected delegate void HighlightLineEventHandler(int line, Color color);
        protected delegate void MarkLineEventHandler(int line, bool marked);
        protected delegate void GoToDelegate(int line);
        protected delegate void InitializeEventHandler(SyntaxDocument doc);

        protected EditableDocumentBase _parentDoc = null;
        protected List<int> _markedLines = null;
        protected int _lastLineCount = 0;

        public event EventHandler CaretChanged;
        public event EventHandler ContentChanged;
        public event EventHandler SelectionChanged;
        public event EventHandler RunToRequested;

        #endregion

        #region Constructors

        public EditableDocumentBaseUI(EditableDocumentBase parentDoc, SyntaxDocument doc) :
            base()
        {
            _parentDoc = parentDoc;
            Initialize(doc);
        }

        #endregion

        #region Properties

        public ILuaEditDocument ParentDocument
        {
            get { return _parentDoc; }
        }

        /// <summary>
        /// Get whether or not the current script's is in overwrite mode
        /// </summary>
        public bool IsOverwrite
        {
            get { return _editor.OverWrite; }
        }

        /// <summary>
        /// Get/Set the caret's current line
        /// </summary>
        public int CurrentLine
        {
            get { return _editor.Caret.LogicalPosition.Y + 1; }
            set { _editor.GotoLine(value); }
        }

        /// <summary>
        /// Get the caret's current column
        /// </summary>
        public int CurrentColumn
        {
            get { return _editor.Caret.LogicalPosition.X; }
        }

        /// <summary>
        /// Get the caret's current character
        /// </summary>
        public int CurrentCharacter
        {
            get { return _editor.Caret.Position.X; }
        }

        /// <summary>
        /// Get the caret's current row object
        /// </summary>
        public Row CurrentRow
        {
            get { return _editor.Caret.CurrentRow; }
        }

        /// <summary>
        /// Get the undo avaialability status
        /// </summary>
        public bool CanUndo
        {
            get { return _editor.CanUndo; }
        }

        /// <summary>
        /// Get the redo avaialability status
        /// </summary>
        public bool CanRedo
        {
            get { return _editor.CanRedo; }
        }

        /// <summary>
        /// Get/Set the outlining activity status
        /// </summary>
        public bool IsOutlining
        {
            get { return _parentDoc.Document.Folding; }
            set { _parentDoc.Document.Folding = value; }
        }

        /// <summary>
        /// Get whether or not all bookmarks are currently enabled
        /// </summary>
        public bool IsAllBookmarksEnabled
        {
            get
            {
                bool result = true;
                foreach (IBookmark bm in _parentDoc.Document.Bookmarks)
                {
                    if (bm != null && !bm.Enabled)
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Get whether or not all code folding segments are expanded
        /// </summary>
        public bool IsAllSegmentsExpanded
        {
            get
            {
                foreach (Row r in _parentDoc.Document)
                {
                    if (r.CanFold && !r.Expanded)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// List of all marked lines in the document
        /// </summary>
        public List<int> MarkedLines
        {
            get { return _markedLines; }
        }

        /// <summary>
        /// The document's text/code editor control
        /// </summary>
        public CodeEditorControl Editor
        {
            get { return _editor; }
        }

        #endregion

        #region Event Handlers

        #region Dialog Events

        /// <summary>
        /// Occurs when the caret position has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _editor_CaretChange(object sender, EventArgs e)
        {
            if (CaretChanged != null)
            {
                CaretChanged(this, e);
            }
        }

        /// <summary>
        /// Occurs when the editor's selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _editor_SelectionChange(object sender, EventArgs e)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(sender, e);
            }
        }

        /// <summary>
        /// Occurs when the script's text has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _editor_TextChanged(object sender, EventArgs e)
        {
            if (_lastLineCount != _editor.Document.Lines.Length)
            {
                if (_parentDoc.FileName != null)
                {
                    BreakpointManager.Instance.DirtyBreakpoints(_parentDoc.FileName);
                }

                _lastLineCount = _editor.Document.Lines.Length;
            }

            if (ContentChanged != null)
            {
                ContentChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Occurs when the user click anywhere on the _editor control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _editor_RowMouseDown(object sender, RowMouseEventArgs e)
        {
            if (e.Area == RowArea.GutterArea)
                ToggleBreakpoint(e.Row);
        }

        #endregion

        #region Other Events

        private void OnBreakpointAdded(object sender, Breakpoint bp)
        {
            if (bp.FileName == _parentDoc.FileName)
            {
                _parentDoc.Document[_parentDoc.Document.LineToRow(bp.Line).Index].Breakpoint = bp;
            }
        }

        private void OnBreakpointRemoved(object sender, Breakpoint bp)
        {
            if (bp.FileName == _parentDoc.FileName)
            {
                _parentDoc.Document[_parentDoc.Document.LineToRow(bp.Line).Index].Breakpoint = null;
            }
        }

        private void OnBreakpointChanged(object sender, EventArgs e)
        {
            if (sender is Breakpoint)
            {
                _editor.Refresh();
            }
        }

        #endregion

        #region Context Menu Events

        private void _editorContextMenu_Opening(object sender, CancelEventArgs e)
        {
            IBreakpoint bp = this.CurrentRow.Breakpoint;

            undoToolStripMenuItem.Enabled = this.CanUndo;
            redoToolStripMenuItem.Enabled = this.CanRedo;
            insertBreakpointToolStripMenuItem.Text = bp == null ? "Insert Breakpoint" : "Delete Breakpoint";

            if (bp != null)
            {
                disableBreakpointToolStripMenuItem.Text = bp.Enabled ? "Disable Breakpoint" : "Enable Breakpoint";
            }

            disableBreakpointToolStripMenuItem.Visible = bp != null;
            toolStripMenuItem2.Visible = bp != null;
            conditionToolStripMenuItem.Visible = bp != null;
            hitCountToolStripMenuItem.Visible = bp != null;
            cutToolStripMenuItem.Enabled = this.Editor.Selection.SelLength != 0;
            copyToolStripMenuItem.Enabled = this.Editor.Selection.SelLength != 0;
            pasteToolStripMenuItem.Enabled = Clipboard.ContainsText();
            stopOutliningToolStripMenuItem.Text = this.IsOutlining ? "Stop Outlining" : "Start Outlining";
            toggleAllOutliningToolStripMenuItem.Visible = this.IsOutlining;
            toggleOutliningExpansionToolStripMenuItem.Visible = this.IsOutlining;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Redo();
        }

        private void insertBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ToggleBreakpoint(this.CurrentRow);
        }

        private void disableBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBreakpoint bp = this.CurrentRow.Breakpoint;

            if (bp != null)
            {
                bp.Enabled = !bp.Enabled;
            }
        }

        private void runToCursorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RunToRequested != null)
            {
                RunToRequested(this, EventArgs.Empty);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Paste();
        }

        private void toggleOutliningExpansionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ToggleOutliningExpansion();
        }

        private void toggleAllOutliningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ToggleAllOutlining();
        }

        private void stopOutliningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.IsOutlining = !this.IsOutlining;
        }

        private void conditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Breakpoint bp = this.CurrentRow.Breakpoint as Breakpoint;

            if (bp != null)
            {
                bp.EditCondition();
            }
        }

        private void hitCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Breakpoint bp = this.CurrentRow.Breakpoint as Breakpoint;

            if (bp != null)
            {
                bp.EditHitCountCondition();
            }
        }

        #endregion

        #endregion

        #region Methods

        private void Initialize(SyntaxDocument doc)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InitializeEventHandler(Initialize), new object[] { doc });
                return;
            }

            InitializeComponent();

            // Set the syntax document to the editor
            _editor.Document = doc;

            // Create marked lines list
            _markedLines = new List<int>();

            if (BreakpointManager.Instance.Breakpoints.ContainsKey(_parentDoc.FileName))
            {
                Dictionary<Row, Breakpoint> bpsByRow = BreakpointManager.Instance.Breakpoints[_parentDoc.FileName];
                Dictionary<Row, Breakpoint> newBpsByRow = new Dictionary<Row, Breakpoint>();

                foreach (Row oldRow in bpsByRow.Keys)
                {
                    Row newRow = _editor.Document.LineToRow(bpsByRow[oldRow].Line);

                    if (newRow != null)
                    {
                        Breakpoint bp = bpsByRow[oldRow];
                        bp.Row = newRow;
                        newBpsByRow.Add(newRow, bp);
                        newRow.Breakpoint = bp;
                    }
                }

                BreakpointManager.Instance.Breakpoints[_parentDoc.FileName] = newBpsByRow;
            }

            // Bind events
            BreakpointManager.Instance.BreakpointAdded += OnBreakpointAdded;
            BreakpointManager.Instance.BreakpointRemoved += OnBreakpointRemoved;
        }

        public bool TerminateUI()
        {
            CaretChanged = null;
            return true;
        }

        public void ToggleBreakpoint(Row row)
        {
            if (_parentDoc.Document[row.Index].Breakpoint == null)
            {
                Breakpoint bp = new Breakpoint(_parentDoc.FileName, row, 0,
                                               LuaEdit.Documents.Properties.Resources.BreakpointEnabled,
                                               LuaEdit.Documents.Properties.Resources.BreakpointDisabled,
                                               LuaEdit.Documents.Properties.Resources.BreakpointConditionedEnabled,
                                               LuaEdit.Documents.Properties.Resources.BreakpointConditionedDisabled);
                bp.BreakpointChanged += OnBreakpointChanged;
                BreakpointManager.Instance.AddBreakpoint(_parentDoc.FileName, bp);
            }
            else
            {
                BreakpointManager.Instance.RemoveBreakpoint(_parentDoc.FileName, row);
            }
        }

        public void ToggleBookmark(Row row)
        {
            if (_parentDoc.Document[row.Index] != null && row.Bookmark == null)
            {
                row.Bookmark = new Bookmark(row.Index, LuaEdit.Documents.Properties.Resources.BookmarkEnabled,
                                            LuaEdit.Documents.Properties.Resources.BookmarkDisabled);
            }
            else
            {
                row.Bookmark = null;
            }
        }

        public void EnableDisableBookmark(IBookmark bm)
        {
            bm.Enabled = !bm.Enabled;
        }

        public void EnableDisableAllBookmarks()
        {
            bool isDisabling = !IsAllBookmarksEnabled;

            foreach (IBookmark bm in _parentDoc.Document.Bookmarks)
                bm.Enabled = isDisabling;
        }

        public void ClearBookmarks()
        {
            _parentDoc.Document.ClearBookmarks();
        }

        public void GotoNextBookmark()
        {
            _editor.GotoNextBookmark();
        }

        public void GotoPreviousBookmark()
        {
            _editor.GotoPreviousBookmark();
        }

        public void Undo()
        {
            _editor.Undo();
        }

        public void Redo()
        {
            _editor.Redo();
        }

        public void Cut()
        {
            _editor.Cut();
        }

        public void Copy()
        {
            _editor.Copy();
        }

        public void Paste()
        {
            _editor.Paste();
        }

        public void SelectAll()
        {
            _editor.SelectAll();
        }

        
        public void Goto(int line)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new GoToDelegate(Goto), new object[] { line });
                return;
            }

            _editor.GotoLine(_editor.Document.LineToRow(line).Index);
        }

        public void ShowGoto()
        {
            _editor.ShowGotoLine();
        }

        public void ToggleOutliningExpansion()
        {
            _parentDoc.Document.ToggleRow(CurrentRow);
        }

        public void ToggleAllOutlining()
        {
            if (IsAllSegmentsExpanded)
            {
                _parentDoc.Document.FoldAll();
            }
            else
            {
                _parentDoc.Document.UnFoldAll();
            }
        }

        public void HighlightLine(int line, Color color)
        {
            if (_editor.InvokeRequired)
            {
                _editor.Invoke(new HighlightLineEventHandler(HighlightLine), new object[] { line, color });
                return;
            }

            _editor.Document.LineToRow(line).BackColor = color;
            _editor.Refresh();
        }

        public void MarkLine(int line, bool marked)
        {
            if (_editor.InvokeRequired)
            {
                _editor.Invoke(new MarkLineEventHandler(MarkLine), new object[] { line, marked });
                return;
            }

            _editor.Document.LineToRow(line).LineMarked = marked;

            if (marked)
            {
                _markedLines.Add(line);
            }
            else
            {
                _markedLines.Remove(line);
            }

            _editor.Refresh();
        }

        #endregion
    }
}