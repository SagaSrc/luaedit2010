using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Fireball.Syntax;
using Fireball.Windows.Forms;
using Fireball.Windows.Forms.CodeEditor;

using LuaEdit.Controls;
using LuaEdit.Documents;
using LuaEdit.Interfaces;

namespace LuaEdit.Forms
{
    public partial class FindInFilesControl : UserControl, IFindAndReplaceControl
    {
        #region Members

        private delegate void SimpleDelegate();

        private static readonly int MaxFindWhatHistoryEntries = 20;
        private static readonly int FindOptionsPanelCollapsedHeight = 3;
        private static readonly int FindOptionsPanelExpandedHeight = 150;
        private static readonly int FindOptionsPanelButtonsExtraSpacing = 10;
        private static readonly int ResultOptionsPanelCollapsedHeight = 3;
        private static readonly int ResultOptionsPanelExpandedHeight = 110;
        private static readonly int ResultOptionsPanelButtonsExtraSpacing = 10;
        private static readonly string CurrentDocumentString = "Current Document";
        private static readonly string CurrentProjectString = "Current Project";
        private static readonly string AllOpenDocumentsString = "All Open Documents";
        private static readonly string EntireSolutionString = "Entire Solution";

        private bool _canInitialize = true;
        private bool _isFindOptionsShown = true;
        private bool _isResultOptionsShown = true;
        private bool _isSearching = false;
        private ILuaEditDocument _currentDoc = null;
        private ExpressionBuilderContextMenu _expressionBuilderContextMenu = null;
        private LuaEditFolderSelector _folderSelector = null;
        private LuaEditFolderSelector.FolderSetItem _selectedFolderSet = null;
        private Thread _searchThread = null;

        #endregion

        #region Constructors

        public FindInFilesControl(/*FindResultWindow findResultWnd1, FindResultWindow findResultWnd2*/)
        {
            InitializeComponent();
            _expressionBuilderContextMenu = new ExpressionBuilderContextMenu(cboFindWhat);
            _folderSelector = new LuaEditFolderSelector();
            radResultWin1.Checked = true;
            chkIncludeSubFolders.Checked = true;
        }

        #endregion

        #region IFindAndReplaceControl Implementation

        public event LayoutChangedEventHandler LayoutChanged;

        public FindAndReplaceType FindAndReplaceType
        {
            get { return FindAndReplaceType.FindInFiles; }
        }

        public Button DefaultButton
        {
            get { return btnFindAll; }
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
            get { return 425; }
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
                    cboLookIn.SelectedIndex = 0;
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
                BuildLookInList();
                cboFindWhat.Focus();
            }
        }

        public void Search()
        {
            PerformFindAll();
        }

        public void StopSearch()
        {
            // todo
        }

        #endregion

        #region Event Handlers

        private void cboFindWhat_TextChanged(object sender, EventArgs e)
        {
            ValidateButtons();
        }

        private void cboLookIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateButtons();
        }

        private void cboLookIn_TextChanged(object sender, EventArgs e)
        {
            chkIncludeSubFolders.Enabled = cboLookIn.Text != CurrentDocumentString &&
                                           cboLookIn.Text != CurrentProjectString &&
                                           cboLookIn.Text != AllOpenDocumentsString &&
                                           cboLookIn.Text != EntireSolutionString;
        }

        private void btnShowHideFindOptions_Click(object sender, EventArgs e)
        {
            _isFindOptionsShown = !_isFindOptionsShown;
            ShowHideFindOptions(_isFindOptionsShown);
        }

        private void btnShowHideResultOptions_Click(object sender, EventArgs e)
        {
            _isResultOptionsShown = !_isResultOptionsShown;
            ShowHideResultOptions(_isResultOptionsShown);
        }

        private void chkUseRegEx_CheckedChanged(object sender, EventArgs e)
        {
            btnExpressionBuilder.Enabled = chkUseRegEx.Checked;
        }

        private void btnFindAll_Click(object sender, EventArgs e)
        {
            // Add search text to list
            AddFindWhatTextToList(cboFindWhat.Text);
            // Perform search
            Search();
        }

        private void btnExpressionBuilder_Click(object sender, EventArgs e)
        {
            Control senderCtrl = sender as Control;

            if (senderCtrl != null)
            {
                _expressionBuilderContextMenu.Show(senderCtrl, new Point(senderCtrl.Width, 0));
            }
        }

        private void btnBrowseLookIn_Click(object sender, EventArgs e)
        {
            DialogResult res = _folderSelector.ShowDialog("Choose Search Folders", "Choose the search folders from the available folders. You may also create a new set of search folders or modify an existing set of search folders.");
            if (res == DialogResult.OK)
            {
                BuildLookInList();

                if (_folderSelector.SelectedFolderSet.FolderSetName == LuaEditFolderSelector.EmptyFolderSetName)
                {
                    cboLookIn.Items.Add(new LuaEditFolderSelector.FolderSetItem(_folderSelector.SelectedFolderSet.FoldersString, _folderSelector.SelectedFolderSet.Folders));
                    cboLookIn.SelectedText = _folderSelector.SelectedFolderSet.FoldersString;
                }
                else
                {
                    cboLookIn.SelectedText = _folderSelector.SelectedFolderSet.FolderSetName;
                }
            }
        }

        #endregion

        #region Methods

        private void BuildLookInList()
        {
            cboLookIn.Items.Clear();
            cboLookIn.Items.Add(new LuaEditFolderSelector.FolderSetItem(CurrentDocumentString));
            cboLookIn.Items.Add(new LuaEditFolderSelector.FolderSetItem(AllOpenDocumentsString));
            cboLookIn.Items.Add(new LuaEditFolderSelector.FolderSetItem(EntireSolutionString));
            cboLookIn.Items.Add(new LuaEditFolderSelector.FolderSetItem(CurrentProjectString));

            List<LuaEditFolderSelector.FolderSetItem> folderSets = LuaEditFolderSelector.LoadFolderSetsList();
            cboLookIn.Items.AddRange(folderSets.ToArray());
        }

        private void ValidateButtons()
        {
            btnFindAll.Enabled = cboFindWhat.Text != string.Empty && cboLookIn.Text != string.Empty;
        }

        private void PerformFindAll()
        {
            _selectedFolderSet = cboLookIn.SelectedItem as LuaEditFolderSelector.FolderSetItem;

            if (_selectedFolderSet != null)
            {
                // todo: start search thread
            }
        }

        private void PerformMultiThreadedSearch()
        {
            List<string> fileTypes = new List<string>(cboFileTypes.Text.Split(new char[] { ';' }));
            _isSearching = true;
            SetSearchButtonText();

            try
            {
                foreach (string searchLocation in _selectedFolderSet.Folders)
                {
                    try
                    {
                        if (Directory.Exists(searchLocation))
                        {
                            List<FileInfo> files = new List<FileInfo>();
                            DirectoryInfo dirInfo = new DirectoryInfo(searchLocation);

                            foreach (string ext in fileTypes)
                            {
                                files.AddRange(dirInfo.GetFiles(ext, chkIncludeSubFolders.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
                            }

                            foreach (FileInfo fileInfo in files)
                            {
                                PerformSearchInFile(fileInfo);
                            }
                        }
                        else if (File.Exists(searchLocation))
                        {
                            PerformSearchInFile(new FileInfo(searchLocation));
                        }
                    }
                    catch (System.Security.SecurityException)
                    {
                        // Error occured... just ignore it since it's
                        // only due to a lack of access permissions
                    }
                }
            }
            finally
            {
                _isSearching = false;
                SetSearchButtonText();
            }
        }

        private void PerformSearchInFile(FileInfo fileInfo)
        {
            List<string> lines = new List<string>(File.ReadAllLines(fileInfo.FullName));

            foreach (string line in lines)
            {
                int resultIndex = line.IndexOf(cboFindWhat.Text);
                while (resultIndex != -1)
                {
                    //todo: add result
                    resultIndex = line.IndexOf(cboFindWhat.Text);
                }
            }
        }

        private void SetSearchButtonText()
        {
            if (btnFindAll.InvokeRequired)
            {
                btnFindAll.BeginInvoke(new SimpleDelegate(SetSearchButtonText));
                return;
            }

            btnFindAll.Text = _isSearching ? "&Find All" : "Stop &Find";
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
                finalHeight = this.Height + heightChangeVal;
                pnlResultOptions.Top += heightChangeVal;
                btnShowHideResultOptions.Top += heightChangeVal;
                lblResultOptions.Top += heightChangeVal;
                btnFindAll.Top += heightChangeVal;
            }
            else
            {
                btnShowHideFindOptions.ImageIndex = 1;
                pnlFindOptions.Height = FindOptionsPanelCollapsedHeight;
                finalHeight = this.Height - heightChangeVal;
                pnlResultOptions.Top -= heightChangeVal;
                btnShowHideResultOptions.Top -= heightChangeVal;
                lblResultOptions.Top -= heightChangeVal;
                btnFindAll.Top -= heightChangeVal;
            }

            if (LayoutChanged != null)
            {
                LayoutChanged(this, 0, finalHeight);
            }
        }

        private void ShowHideResultOptions(bool show)
        {
            int heightChangeVal = ResultOptionsPanelExpandedHeight - ResultOptionsPanelCollapsedHeight - ResultOptionsPanelButtonsExtraSpacing;
            int finalHeight = 0;

            if (show)
            {
                btnShowHideResultOptions.ImageIndex = 2;
                pnlResultOptions.Height = ResultOptionsPanelExpandedHeight;
                finalHeight = this.Height + heightChangeVal; ;
                btnFindAll.Top += heightChangeVal;
            }
            else
            {
                btnShowHideResultOptions.ImageIndex = 1;
                pnlResultOptions.Height = ResultOptionsPanelCollapsedHeight;
                finalHeight = this.Height - heightChangeVal; ;
                btnFindAll.Top -= heightChangeVal;
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
