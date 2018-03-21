using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DotNetLib.Controls;
using LuaEdit.Controls;
using LuaEdit.Documents;
using LuaEdit.Interfaces;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Forms
{
    public partial class SolutionExplorerDlg : DockContent
    {
        #region Constructors

        public SolutionExplorerDlg()
        {
            InitializeComponent();

            // Bind events
            DocumentsManager.Instance.CurrentSolutionChanged += OnCurrentSolutionChanged;

            // Initialize UI
            solutionExplorerToolStrip.Renderer = new ToolStripFlatBorderRenderer();
        }

        #endregion

        #region Event Handlers

        #region Document Events

        /// <summary>
        /// Occurs when a document gets removed from a group document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentRemoved(object sender, DocGroupDocActionEventArgs e)
        {
            TreeListViewItem result = null;
            FindTreeListViewItemByDocument(solutionExplorerTreeView.Items[0], e.Document, ref result);

            if (result != null)
            {
                if (result.ParentItem == null)
                    solutionExplorerTreeView.Items.Remove(result);
                else
                    result.ParentItem.Items.Remove(result);
            }

            solutionExplorerTreeView.ShowRootTreeLines = (solutionExplorerTreeView.Items.Count > 0 && solutionExplorerTreeView.Items[0].Items.Count > 0);
            solutionExplorerTreeView.AutoSizeColumnWidths(true);
        }

        /// <summary>
        /// Occurs when a document gets added to a group document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentAdded(object sender, DocGroupDocActionEventArgs e)
        {
            TreeListViewItem result = null;
            ILuaEditDocument doc = sender as ILuaEditDocument;
            FindTreeListViewItemByDocument(solutionExplorerTreeView.Items[0], doc, ref result);

            if (result != null)
            {
                e.Document.ToTreeListViewItem(result);
                result.Expand();

                if (e.Document is ILuaEditDocumentGroup)
                {
                    ILuaEditDocumentGroup grpDoc = e.Document as ILuaEditDocumentGroup;
                    grpDoc.DocumentRemoved += OnDocumentRemoved;
                    grpDoc.DocumentAdded += OnDocumentAdded;
                }
            }

            solutionExplorerTreeView.ShowRootTreeLines = (solutionExplorerTreeView.Items.Count > 0 && solutionExplorerTreeView.Items[0].Items.Count > 0);
            solutionExplorerTreeView.AutoSizeColumnWidths(true);
        }

        /// <summary>
        /// Occurs when the current solution changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCurrentSolutionChanged(object sender, EventArgs e)
        {
            solutionExplorerTreeView.Items.Clear();
            solutionExplorerTreeView.SmallImageList = DocumentFactory.Instance.DocumentSmallImages;

            if (DocumentsManager.Instance.CurrentSolution == null)
            {
                solutionExplorerTreeView.Items.Clear();
            }
            else
            {
                DocumentsManager.Instance.CurrentSolution.ActiveProjectChanged += OnActiveProjectChanged;
                DocumentsManager.Instance.CurrentSolution.ToTreeListViewItem(solutionExplorerTreeView.RootItem);

                if (solutionExplorerTreeView.RootItem.Items.Count > 0)
                {
                    BindDocumentEventsRecursive(solutionExplorerTreeView.RootItem.Items[0]);
                }
            }

            solutionExplorerTreeView.ShowRootTreeLines = (solutionExplorerTreeView.Items.Count > 0 &&
                                                              solutionExplorerTreeView.Items[0].Items.Count > 0);
            solutionExplorerTreeView.AutoSizeColumnWidths(true);
        }

        private void OnActiveProjectChanged(object sender, ActiveProjectEventArgs e)
        {
            TreeListViewItem tlvi = null;

            if (e.PreviouslyActiveProject != null)
            {
                FindTreeListViewItemByDocument(solutionExplorerTreeView.Items[0], e.PreviouslyActiveProject, ref tlvi);

                if (tlvi != null)
                {
                    tlvi.Font = new Font(tlvi.Font, FontStyle.Regular);
                }
            }

            tlvi = null;
            FindTreeListViewItemByDocument(solutionExplorerTreeView.Items[0], e.CurrentlyActiveProject, ref tlvi);

            if (tlvi != null)
            {
                tlvi.Font = new Font(tlvi.Font, FontStyle.Bold);
            }
        }

        #endregion

        #region Dialog Events

        private void excludeFromProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count > 0)
            {
                TreeListViewItem[] selectedItems = new TreeListViewItem[0];
                Array.Resize<TreeListViewItem>(ref selectedItems, solutionExplorerTreeView.SelectedItems.Count);
                solutionExplorerTreeView.SelectedItems.CopyTo(selectedItems, 0);

                foreach (TreeListViewItem tlvi in selectedItems)
                {
                    ILuaEditDocument doc = tlvi.Tag as ILuaEditDocument;

                    if (doc != null && doc.ParentDocument != null)
                    {
                        ILuaEditDocument currentDoc = doc;

                        while (!(currentDoc.ParentDocument is ILuaEditDocumentProject))
                        {
                            currentDoc = currentDoc.ParentDocument;
                        }

                        doc.ParentDocument.RemoveDocument(doc);
                    }
                }
            }
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count == 1)
            {
                ILuaEditDocumentGroup grpDoc = solutionExplorerTreeView.SelectedItems[0].Tag as ILuaEditDocumentGroup;
                ILuaEditDocumentFolder folderDoc = grpDoc.CreateNewFolder();
                TreeListViewItem tlvi = null;

                FindTreeListViewItemByDocument(solutionExplorerTreeView.RootItem, folderDoc, ref tlvi);

                if (tlvi != null)
                {
                    solutionExplorerTreeView.SelectedItems.Clear();
                    solutionExplorerTreeView.SelectedItems.Add(tlvi);
                    solutionExplorerTreeView.Refresh();
                    solutionExplorerTreeView.EditTreeListViewItem(tlvi, null);
                }
            }
        }

        private bool IsParentProjectSelected(TreeListViewItem tlvi)
        {
            TreeListViewItem currentItem = tlvi;

            while (!(currentItem.Tag is ILuaEditDocumentProject) && currentItem.ParentItem != null)
            {
                currentItem = currentItem.ParentItem;
            }

            return currentItem != null && currentItem.Tag is ILuaEditDocumentProject && currentItem.Selected;
        }

        private void _solutionExplorerContextMenu_Opening(object sender, CancelEventArgs e)
        {
            bool hasSlnSelected = false;
            bool hasPrjSelected = false;
            bool hasFileSelected = false;
            bool hasPrjFolderSelected = false;
            bool hasGrpSelected = false;
            bool hasFilesOrPrjFoldersParentPrjSelected = true;
            bool isDefaultPrjSelected = false;
            int selCount = solutionExplorerTreeView.SelectedItems.Count;

            if (selCount > 0)
            {
                TreeListViewItem[] selectedItems = new TreeListViewItem[0];
                Array.Resize<TreeListViewItem>(ref selectedItems, solutionExplorerTreeView.SelectedItems.Count);
                solutionExplorerTreeView.SelectedItems.CopyTo(selectedItems, 0);

                foreach (TreeListViewItem tlvi in selectedItems)
                {
                    if (tlvi.Tag is ILuaEditDocumentEditable)
                    {
                        if (hasFilesOrPrjFoldersParentPrjSelected)
                        {
                            hasFilesOrPrjFoldersParentPrjSelected = IsParentProjectSelected(tlvi);
                        }

                        hasFileSelected = true;
                    }
                    else if (tlvi.Tag is ILuaEditDocumentFolder)
                    {
                        if (hasFilesOrPrjFoldersParentPrjSelected)
                        {
                            hasFilesOrPrjFoldersParentPrjSelected = IsParentProjectSelected(tlvi);
                        }

                        hasPrjFolderSelected = true;
                        hasGrpSelected = true;
                    }
                    else if (tlvi.Tag is ILuaEditDocumentProject)
                    {
                        if (!isDefaultPrjSelected)
                        {
                            isDefaultPrjSelected = DocumentsManager.Instance.CurrentSolution.ActiveProject == tlvi.Tag;
                        }

                        hasPrjSelected = true;
                        hasGrpSelected = true;
                    }
                    else if (tlvi.Tag is ILuaEditDocumentSolution)
                    {
                        hasFilesOrPrjFoldersParentPrjSelected = false;
                        hasSlnSelected = true;
                        hasGrpSelected = true;
                    }
                }

                bool hasOnlyFilesSelected = hasFileSelected && !(hasSlnSelected || hasPrjSelected || hasPrjFolderSelected);
                bool hasOnlyFilesOrPrjFoldersSelected = hasOnlyFilesSelected || hasPrjFolderSelected;
                bool hasOnlyPrjsSelected = hasPrjSelected && !(hasSlnSelected || hasFileSelected || hasPrjFolderSelected);

                openToolStripMenuItem.Visible = hasOnlyFilesSelected;
                toolStripMenuItem5.Visible = hasOnlyFilesSelected;

                addToolStripMenuItem.Visible = hasGrpSelected && selCount == 1;
                toolStripMenuItem1.Visible = hasGrpSelected && selCount == 1;
                newProjectToolStripMenuItem.Visible = hasSlnSelected;
                existingProjectToolStripMenuItem.Visible = hasSlnSelected;
                toolStripMenuItem8.Visible = hasSlnSelected;
                newFolderToolStripMenuItem.Visible = hasPrjSelected || hasPrjFolderSelected;

                setAsStartupProjectToolStripMenuItem.Visible = hasPrjSelected && selCount == 1;
                setAsStartupProjectToolStripMenuItem.Enabled = !isDefaultPrjSelected;
                excludeFromProjectToolStripMenuItem.Visible = hasOnlyFilesOrPrjFoldersSelected;
                toolStripMenuItem2.Visible = (hasPrjSelected && selCount == 1) || hasOnlyFilesOrPrjFoldersSelected;

                cutToolStripMenuItem.Visible = hasOnlyFilesSelected;
                copyToolStripMenuItem.Visible = hasOnlyFilesSelected;
                pasteToolStripMenuItem.Enabled = (hasPrjSelected || hasPrjFolderSelected) && selCount == 1;
                renameToolStripMenuItem.Enabled = selCount == 1;
                removeToolStripMenuItem.Visible = hasOnlyFilesOrPrjFoldersSelected || (hasFilesOrPrjFoldersParentPrjSelected || hasOnlyPrjsSelected);

                if (hasOnlyFilesOrPrjFoldersSelected)
                {
                    removeToolStripMenuItem.Text = "Delete";
                }
                else if (hasFilesOrPrjFoldersParentPrjSelected || hasOnlyPrjsSelected)
                {
                    removeToolStripMenuItem.Text = "Remove";
                }

                toolStripMenuItem3.Visible = hasOnlyPrjsSelected;
                propertiesToolStripMenuItem.Visible = hasOnlyPrjsSelected;
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count > 0)
            {
                TreeListViewItem[] selectedItems = new TreeListViewItem[0];
                Array.Resize<TreeListViewItem>(ref selectedItems, solutionExplorerTreeView.SelectedItems.Count);
                solutionExplorerTreeView.SelectedItems.CopyTo(selectedItems, 0);

                foreach (TreeListViewItem tlvi in selectedItems)
                {
                    ILuaEditDocumentProject prjDoc = tlvi.Tag as ILuaEditDocumentProject;

                    if (prjDoc != null)
                    {
                        DocumentsManager.Instance.OpenDocument(prjDoc);
                    }
                }
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count == 1)
            {
                solutionExplorerTreeView.EditTreeListViewItem(solutionExplorerTreeView.SelectedItems[0], null);
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count == 1)
            {
                ILuaEditDocumentGroup docGrp = solutionExplorerTreeView.SelectedItems[0].Tag as ILuaEditDocumentGroup;

                if (docGrp != null && docGrp.CanAdd())
                {
                    string title = string.Format("Add New Project - {0}", docGrp);
                    DocumentsManager.Instance.NewDocument(title, docGrp, LuaEdit.HelperDialogs.NewItemTypes.ItemGroup);
                }
            }
        }

        private void existingProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count == 1)
            {
                ILuaEditDocumentGroup docGrp = solutionExplorerTreeView.SelectedItems[0].Tag as ILuaEditDocumentGroup;

                if (docGrp != null && docGrp.CanAdd())
                {
                    string title = string.Format("Add Existing Project - {0}", docGrp);
                    DocumentsManager.Instance.OpenProjectDocument("All Project Files", docGrp, title, Path.GetDirectoryName(docGrp.FileName));
                }
            }
        }

        private void newItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count == 1)
            {
                ILuaEditDocumentGroup docGrp = solutionExplorerTreeView.SelectedItems[0].Tag as ILuaEditDocumentGroup;

                if (docGrp != null && docGrp.CanAdd())
                {
                    string title = string.Format("Add New Item - {0}", docGrp);
                    DocumentsManager.Instance.NewDocument(title, docGrp, LuaEdit.HelperDialogs.NewItemTypes.Item);
                }
            }
        }

        private void existingItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count == 1)
            {
                ILuaEditDocumentGroup docGrp = solutionExplorerTreeView.SelectedItems[0].Tag as ILuaEditDocumentGroup;

                if (docGrp != null && docGrp.CanAdd())
                {
                    string title = string.Format("Add Existing Item - {0}", docGrp);
                    DocumentsManager.Instance.OpenDocuments(typeof(ILuaEditDocumentEditable), docGrp, title, Path.GetDirectoryName(docGrp.FileName));
                }
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count > 0)
            {
                string confirmMsg = string.Empty;

                if (removeToolStripMenuItem.Text == "Remove")
                {
                    if (solutionExplorerTreeView.SelectedItems.Count == 1)
                    {
                        ILuaEditDocument doc = solutionExplorerTreeView.SelectedItems[0].Tag as ILuaEditDocument;
                        confirmMsg = string.Format("'{0}' will be removed.", doc);
                    }
                    else
                    {
                        confirmMsg = "The selected items will be removed.";
                    }

                    if (!string.IsNullOrEmpty(confirmMsg) &&
                        FrameworkManager.ShowMessageBox(confirmMsg, MessageBoxButtons.OKCancel,
                                                        MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        TreeListViewItem[] selectedItems = new TreeListViewItem[0];
                        Array.Resize<TreeListViewItem>(ref selectedItems, solutionExplorerTreeView.SelectedItems.Count);
                        solutionExplorerTreeView.SelectedItems.CopyTo(selectedItems, 0);

                        foreach (TreeListViewItem tlvi in selectedItems)
                        {
                            ILuaEditDocumentProject prjDoc = tlvi.Tag as ILuaEditDocumentProject;

                            if (prjDoc != null && prjDoc.ParentDocument != null)
                            {
                                if (DocumentsManager.Instance.CloseDocument(prjDoc))
                                {
                                    prjDoc.ParentDocument.RemoveDocument(prjDoc);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (solutionExplorerTreeView.SelectedItems.Count == 1)
                    {
                        ILuaEditDocument doc = solutionExplorerTreeView.SelectedItems[0].Tag as ILuaEditDocument;

                        if (doc != null && doc is ILuaEditDocumentFolder)
                        {
                            confirmMsg = string.Format("'{0}' and all its content will be deleted permanently.", doc);
                        }
                        else
                        {
                            confirmMsg = string.Format("'{0}' will be deleted permanently.", doc);
                        }
                    }
                    else
                    {
                        confirmMsg = "The selected items will be deleted permanently.";
                    }

                    if (!string.IsNullOrEmpty(confirmMsg) &&
                        FrameworkManager.ShowMessageBox(confirmMsg, MessageBoxButtons.OKCancel,
                                                        MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        TreeListViewItem[] selectedItems = new TreeListViewItem[0];
                        Array.Resize<TreeListViewItem>(ref selectedItems, solutionExplorerTreeView.SelectedItems.Count);
                        solutionExplorerTreeView.SelectedItems.CopyTo(selectedItems, 0);

                        foreach (TreeListViewItem tlvi in selectedItems)
                        {
                            ILuaEditDocument doc = tlvi.Tag as ILuaEditDocument;
                            DocumentsManager.Instance.DeleteDocument(doc);
                        }
                    }
                }
            }
        }

        private void setAsStartupProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count == 1)
            {
                ILuaEditDocumentProject prjDoc = solutionExplorerTreeView.SelectedItems[0].Tag as ILuaEditDocumentProject;

                if (prjDoc != null)
                {
                    DocumentsManager.Instance.CurrentSolution.ActiveProject = prjDoc;
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeListViewItem tlvi in solutionExplorerTreeView.SelectedItems)
            {
                ILuaEditDocumentEditable doc = tlvi.Tag as ILuaEditDocumentEditable;

                if (doc != null)
                {
                    DocumentsManager.Instance.OpenDocument(doc);
                }
            }
        }

        private void solutionExplorerTreeView_ItemBeforeEdit(object sender, TreeListViewBeforeEditEventArgs e)
        {
            TextBox editor = new TextBox();
            editor.Multiline = true;
            editor.WordWrap = false;
            editor.BorderStyle = BorderStyle.FixedSingle;
            e.Editor = editor;

            if (e.Item.Tag is ILuaEditDocumentSolution)
            {
                e.DefaultValue = Path.GetFileNameWithoutExtension((e.Item.Tag as ILuaEditDocumentSolution).FileName);
            }
            else
            {
                e.DefaultValue = e.Item.Text;
            }
        }

        private void solutionExplorerTreeView_ItemAfterEdit(object sender, TreeListViewAfterEditEventArgs e)
        {
            ILuaEditDocument doc = e.Item.Tag as ILuaEditDocument;

            if (doc == null)
            {
                e.Cancel = true;
            }
            else
            {
                bool isDir = false;

                try
                {
                    isDir = (File.GetAttributes(doc.FileName) & FileAttributes.Directory) == FileAttributes.Directory;
                }
                catch (Exception ex)
                {
                    string msg = string.Format("Error while trying to rename '{0}': {1}", doc.FileName, ex.Message);
                    FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                try
                {
                    string newFileName = null;

                    if (isDir)
                    {
                        newFileName = Path.Combine(Directory.GetParent(doc.FileName).FullName, e.NewValue);
                    }
                    else
                    {
                        if (e.Item.Tag is ILuaEditDocumentProject || e.Item.Tag is ILuaEditDocumentSolution)
                        {
                            newFileName = Path.Combine(Path.GetDirectoryName(doc.FileName), e.NewValue + Path.GetExtension(doc.FileName));
                        }
                        else
                        {
                            newFileName = Path.Combine(Path.GetDirectoryName(doc.FileName), e.NewValue);
                        }
                    }

                    if (newFileName != doc.FileName)
                    {
                        DocumentsManager.Instance.RenameDocument(doc, newFileName);

                        if (e.Item != null)
                        {
                            e.Item.ToolTip = doc.FileName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    FrameworkManager.ShowMessageBox(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Occurs when a double click is performed in the tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void solutionExplorerTreeView_DoubleClick(object sender, EventArgs e)
        {
            if (solutionExplorerTreeView.SelectedItems.Count > 0)
            {
                ILuaEditDocument luaDoc = solutionExplorerTreeView.SelectedItems[0].Tag as ILuaEditDocument;

                if (luaDoc != null && !solutionExplorerTreeView.SelectedItems[0].HasChildren)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        DocumentsManager.Instance.OpenDocument((luaDoc).DocumentRef, false);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Find the node associated with the specified document
        /// </summary>
        /// <param name="root">The node from which to start searching</param>
        /// <param name="doc">The document to search for</param>
        /// <param name="result">A null initialized node that will get filled by the search</param>
        private void FindTreeListViewItemByDocument(TreeListViewItem root, ILuaEditDocument doc, ref TreeListViewItem result)
        {
            if (root.Tag == doc)
                result = root;

            if (result == null)
            {
                foreach (TreeListViewItem childItem in root.Items)
                {
                    FindTreeListViewItemByDocument(childItem, doc, ref result);

                    if (result != null)
                        break;
                }
            }
        }

        /// <summary>
        /// Bind all necessary document events
        /// </summary>
        /// <param name="root">The node from which to start</param>
        private void BindDocumentEventsRecursive(TreeListViewItem root)
        {
            if (root.Tag is ILuaEditDocumentGroup)
            {
                (root.Tag as ILuaEditDocumentGroup).DocumentRemoved += OnDocumentRemoved;
                (root.Tag as ILuaEditDocumentGroup).DocumentAdded += OnDocumentAdded;
            }

            foreach (TreeListViewItem childItem in root.Items)
            {
                BindDocumentEventsRecursive(childItem);
            }
        }

        #endregion
    }
}