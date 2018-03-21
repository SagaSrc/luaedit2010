using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Fireball.Syntax;
using Fireball.Windows.Forms;
using Fireball.Windows.Forms.CodeEditor;

using LuaEdit.Controls;
using LuaEdit.Documents;
using LuaEdit.Interfaces;

namespace LuaEdit.Forms
{
    public partial class QuickFindControl : UserControl, IFindAndReplaceControl
    {
        #region Members

        private static readonly int MaxFindWhatHistoryEntries = 20;
        private static readonly int FindOptionsPanelCollapsedHeight = 3;
        private static readonly int FindOptionsPanelExpandedHeight = 125;
        private static readonly int FindOptionsPanelButtonsExtraSpacing = 10;
        private static readonly string CurrentDocumentString = "Current Document";
        private static readonly string CurrentProjectString = "Current Project";
        private static readonly string AllOpenDocumentsString = "All Open Documents";

        private bool _canInitialize = true;
        private bool _isFindOptionsShown = true;
        private ILuaEditDocument _currentDoc = null;
        private ExpressionBuilderContextMenu _expressionBuilderContextMenu = null;
        private bool _needSearchListRebuild = false;
        private bool _isFirstSearch = true;
        private string _previouslySearchedDoc = string.Empty;
        private List<ILuaEditDocumentEditable> _currentSearchList;
        private int _currentEndOfSearch = 0;
        private int _currentSearchIndex = 0;
        private int _currentSearchStartIndex = 0;

        #endregion

        #region Constructors

        public QuickFindControl()
        {
            InitializeComponent();
            _currentSearchList = new List<ILuaEditDocumentEditable>();
            _expressionBuilderContextMenu = new ExpressionBuilderContextMenu(cboFindWhat);
        }

        #endregion

        #region IFindAndReplaceControl Implementation

        public event LayoutChangedEventHandler LayoutChanged;

        public FindAndReplaceType FindAndReplaceType
        {
            get { return FindAndReplaceType.QuickFind; }
        }

        public Button DefaultButton
        {
            get { return btnFindNext; }
        }

        public bool ControlVisible
        {
            get { return this.Visible; }
            set { this.Visible = value; }
        }

        public int ControlStartWidth
        {
            get { return 300; }
        }

        public int ControlStartHeight
        {
            get { return 265; }
        }

        public int ControlLeft
        {
            get { return this.Left; }
            set { this.Left = value; }
        }

        public int ControlTop
        {
            get { return this.Top; }
            set { this.Top = value; }
        }

        public int ControlHeight
        {
            get { return this.Height; }
        }

        public int ControlWidth
        {
            get { return this.Width; }
        }

        public AnchorStyles ControlAnchor
        {
            get { return this.Anchor; }
            set { this.Anchor = value; }
        }

        public void Initialize(string initialText, ILuaEditDocument currentDoc)
        {
            if (_canInitialize)
            {
                _currentDoc = currentDoc;
                string currentLookIn = cboLookIn.Text;
                cboLookIn.Items.Clear();
                btnExpressionBuilder.Enabled = chkUseRegEx.Checked;

                if (_currentDoc is ILuaEditDocumentEditable)
                {
                    cboLookIn.Items.Add(CurrentDocumentString);
                }

                if (DocumentsManager.Instance.CurrentSolution != null && DocumentsManager.Instance.CurrentSolution.ActiveProject != null)
                {
                    cboLookIn.Items.Add(CurrentProjectString);
                }

                if (DocumentsManager.Instance.OpenedDocuments.Count > 0)
                {
                    cboLookIn.Items.Add(AllOpenDocumentsString);
                }

                if (cboLookIn.Items.Count > 0)
                {
                    if (currentLookIn == string.Empty && cboLookIn.Items.IndexOf(currentLookIn) >= 0)
                    {
                        cboLookIn.Text = currentLookIn;
                    }
                    else
                    {
                        cboLookIn.SelectedIndex = 0;
                    }
                }

                if (initialText == string.Empty)
                {
                    if (cboFindWhat.Text == string.Empty)
                    {
                        if (cboFindWhat.Items.Count > 0)
                        {
                            cboFindWhat.SelectedIndex = 0;
                        }
                        else
                        {
                            cboFindWhat.Text = string.Empty;
                        }
                    }
                }
                else
                {
                    cboFindWhat.Text = initialText;
                }

                ValidateButtons();
                cboFindWhat.Focus();
            }
        }

        public void Search()
        {
            PerformFindNext(false);
        }

        public void StopSearch()
        {
            // Not valid for QuickFind
        }

        #endregion

        #region Event Handlers

        private void cboFindWhat_TextChanged(object sender, EventArgs e)
        {
            ValidateButtons();
            _needSearchListRebuild = true;
        }

        private void cboLookIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateButtons();
            _needSearchListRebuild = true;
        }

        private void btnShowHideFindOptions_Click(object sender, EventArgs e)
        {
            _isFindOptionsShown = !_isFindOptionsShown;
            ShowHideFindOptions(_isFindOptionsShown);
        }

        private void chkMatchCase_CheckedChanged(object sender, EventArgs e)
        {
            _needSearchListRebuild = true;
        }

        private void chkMatchWholeWord_CheckedChanged(object sender, EventArgs e)
        {
            _needSearchListRebuild = true;
        }

        private void chkSearchUp_CheckedChanged(object sender, EventArgs e)
        {
            _needSearchListRebuild = true;
        }

        private void chkUseRegEx_CheckedChanged(object sender, EventArgs e)
        {
            btnExpressionBuilder.Enabled = chkUseRegEx.Checked;
            _needSearchListRebuild = true;
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            // Add search text to list
            AddFindWhatTextToList(cboFindWhat.Text);
            // Perform search
            PerformFindNext(false);
        }

        private void btnBookmarkAll_Click(object sender, EventArgs e)
        {
            // Add search text to list
            AddFindWhatTextToList(cboFindWhat.Text);
            // Perform search
            PerformFindNext(true);
        }

        private void btnExpressionBuilder_Click(object sender, EventArgs e)
        {
            Control senderCtrl = sender as Control;

            if (senderCtrl != null)
            {
                _expressionBuilderContextMenu.Show(senderCtrl, new Point(senderCtrl.Width, 0));
            }
        }

        #endregion

        #region Methods

        private void ValidateButtons()
        {
            btnFindNext.Enabled = cboFindWhat.Text != string.Empty && cboLookIn.Text != string.Empty;
            btnBookmarkAll.Enabled = btnFindNext.Enabled;
        }

        private void PerformFindNext(bool shouldBookmark)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string findWhatText = cboFindWhat.Text;
                string lookIn = cboLookIn.Text;

                // Determine list of documents in which to perform the search
                // Only do this when search parameters have changed
                if (_needSearchListRebuild)
                {
                    _currentSearchList.Clear();

                    if (lookIn == CurrentDocumentString && _currentDoc is ILuaEditDocumentEditable)
                    {
                        _currentSearchList.Add(_currentDoc as ILuaEditDocumentEditable);
                    }
                    else if (lookIn == CurrentProjectString)
                    {
                        DocumentsManager.Instance.CurrentSolution.ActiveProject.GetAllDocumentsOfType<ILuaEditDocumentEditable>(_currentSearchList);
                    }
                    else if (lookIn == AllOpenDocumentsString)
                    {
                        foreach (ILuaEditDocument doc in DocumentsManager.Instance.OpenedDocuments.Values)
                        {
                            if (doc is ILuaEditDocumentEditable && doc.DockContent != null)
                            {
                                _currentSearchList.Add(doc as ILuaEditDocumentEditable);
                            }
                        }
                    }

                    if (_currentDoc != null && _currentDoc is ILuaEditDocumentEditable)
                    {
                        _currentSearchIndex = _currentSearchList.IndexOf(_currentDoc as ILuaEditDocumentEditable);
                    }
                    else
                    {
                        _currentSearchIndex = 0;
                    }

                    _currentSearchStartIndex = _currentSearchIndex;
                    _currentEndOfSearch = chkSearchUp.Checked ? 0 : _currentSearchList.Count;
                    _isFirstSearch = true;
                    _needSearchListRebuild = false;
                }

                // Perform search through the document list (if required)
                if (_currentSearchList.Count > 0)
                {
                    while (chkSearchUp.Checked ? _currentSearchIndex >= _currentEndOfSearch : _currentSearchIndex < _currentEndOfSearch)
                    {
                        ILuaEditDocumentEditable doc = _currentSearchList[_currentSearchIndex];
                        FindResult res = new FindResult(0, 0, 0, false, true);

                        if (!_isFirstSearch && _previouslySearchedDoc != doc.FileName)
                        {
                            doc.DocumentUI.Editor.Caret.Position = new TextPoint(0, chkSearchUp.Checked ? doc.Document.Count - 1 : 0);
                        }

                        _previouslySearchedDoc = doc.FileName;

                        while (res.Matched)
                        {
                            _isFirstSearch = false;
                            res = doc.DocumentUI.Editor.ActiveViewControl.FindNext(findWhatText,
                                                                        chkMatchCase.Checked,
                                                                        chkMatchWholeWord.Checked,
                                                                        chkUseRegEx.Checked,
                                                                        chkSearchUp.Checked);

                            if (res.PassedEndOfDocument)
                            {
                                if (lookIn != CurrentDocumentString || shouldBookmark)
                                {
                                    break;
                                }
                            }

                            if (res.Matched)
                            {
                                _canInitialize = false;
                                DocumentsManager.Instance.OpenDocument(doc);
                                this.Focus();
                                _canInitialize = true;

                                if (res.PassedEndOfDocument)
                                {
                                    ShowPassedEndOfDocStatusMessage();
                                }

                                // Select found text
                                doc.DocumentUI.Editor.ActiveViewControl.SelectText(res.RowIndex, res.Column, res.Length);

                                if (shouldBookmark)
                                {
                                    // Bookmark the line if required
                                    doc.DocumentUI.ToggleBookmark(doc.Document[res.RowIndex]);
                                }
                                else
                                {
                                    // Display search status to user
                                    if (!res.PassedEndOfDocument)
                                    {
                                        string findOptionsString = string.Empty;
                                        string lookInStr = lookIn;

                                        if (chkSearchUp.Checked)
                                            findOptionsString += ", Search up";
                                        if (chkMatchCase.Checked)
                                            findOptionsString += ", Match case";
                                        if (chkMatchWholeWord.Checked)
                                            findOptionsString += ", Match whole word";
                                        if (chkUseRegEx.Checked)
                                            findOptionsString += ", Regular expressions";

                                        if (lookIn == CurrentProjectString)
                                        {
                                            lookInStr += ": " + Path.GetFileName(DocumentsManager.Instance.CurrentSolution.ActiveProject.FileName);
                                        }

                                        string statusMsg = string.Format("Find \"{0}\"{1}, {2}", findWhatText, findOptionsString, lookIn);
                                        FrameworkManager.Instance.RequestStatusMessage(statusMsg, SystemColors.Control, Color.Black);
                                    }

                                    return;
                                }
                            }
                            else if (lookIn == CurrentDocumentString)
                            {
                                if (res.PassedEndOfDocument)
                                {
                                    ShowPassedEndOfDocStatusMessage();
                                }

                                string msg = string.Format("The following specified text was not found:\n\n{0}", findWhatText);
                                MessageBox.Show(msg, "LuaEdit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        if (chkSearchUp.Checked)
                        {
                            if (_currentSearchIndex == 0)
                            {
                                _currentSearchIndex = _currentSearchList.Count - 1;
                            }
                            else
                            {
                                --_currentSearchIndex;
                            }
                        }
                        else
                        {
                            if (_currentSearchIndex == _currentSearchList.Count - 1 && _currentSearchStartIndex != 0)
                            {
                                _currentSearchIndex = 0;
                            }
                            else
                            {
                                ++_currentSearchIndex;
                            }
                        }
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void AddFindWhatTextToList(string findWhatText)
        {
            bool found = false;

            foreach (string item in cboFindWhat.Items)
            {
                if (item == findWhatText)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                cboFindWhat.Items.Insert(0, findWhatText);

                if (cboFindWhat.Items.Count > MaxFindWhatHistoryEntries)
                {
                    cboFindWhat.Items.RemoveAt(cboFindWhat.Items.Count - 1);
                }
            }
        }

        private void ShowHideFindOptions(bool show)
        {
            int heightChangeVal = FindOptionsPanelExpandedHeight - FindOptionsPanelCollapsedHeight - FindOptionsPanelButtonsExtraSpacing;
            int finalHeight = 0;

            if (show)
            {
                btnShowHideFindOptions.ImageIndex = 2;
                pnlFindOptions.Height = FindOptionsPanelExpandedHeight;
                finalHeight = this.ControlStartHeight;
                btnBookmarkAll.Top += heightChangeVal;
                btnFindNext.Top += heightChangeVal;
            }
            else
            {
                btnShowHideFindOptions.ImageIndex = 1;
                pnlFindOptions.Height = FindOptionsPanelCollapsedHeight;

                finalHeight = this.ControlStartHeight - heightChangeVal;
                btnBookmarkAll.Top -= heightChangeVal;
                btnFindNext.Top -= heightChangeVal;
            }

            if (LayoutChanged != null)
            {
                LayoutChanged(this, 0, finalHeight);
            }
        }

        private void ShowPassedEndOfDocStatusMessage()
        {
            FrameworkManager.Instance.RequestStatusMessage("Passed the end of the document", Color.Navy, Color.White);
        }

        #endregion
    }
}
