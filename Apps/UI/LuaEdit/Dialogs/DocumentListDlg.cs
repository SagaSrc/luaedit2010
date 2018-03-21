using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DotNetLib.Controls;
using LuaEdit.Documents;
using LuaEdit.Interfaces;
using LuaEdit.Win32;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Forms
{
    public partial class DocumentListDlg : Form
    {
        #region Constructors

        public DocumentListDlg()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of documents to open
        /// </summary>
        public List<ILuaEditDocument> DocumentsToOpen
        {
            get
            {
                List<ILuaEditDocument> docs = new List<ILuaEditDocument>();

                if (tlvwDocList.SelectedItems.Count > 0)
                {
                    foreach (TreeListViewItem tlvi in tlvwDocList.SelectedItems)
                    {
                        docs.Add(tlvi.Tag as ILuaEditDocument);
                    }
                }
                else if (tlvwDocList.Items.Count == 1)
                {
                    docs.Add(tlvwDocList.Items[0].Tag as ILuaEditDocument);
                }

                return docs;
            }
        }

        #endregion

        #region Event Handlers

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                tlvwDocList.Focus();
                User32.SendMessage(tlvwDocList.Handle, (int)WindowMessages.WM_KEYDOWN, (int)e.KeyCode, (int)e.Modifiers);
            }
        }

        private void tlvwDocList_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void DocumentListDlg_Shown(object sender, EventArgs e)
        {
            Filter();
        }

        private void DocumentListDlg_Load(object sender, EventArgs e)
        {
            txtFilter.Focus();
        }

        private void DocumentListDlg_Activated(object sender, EventArgs e)
        {
            txtFilter.Focus();
        }

        #endregion

        #region Methods

        private bool FilterDocument(ILuaEditDocument doc)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
            {
                return false;
            }

            return doc.FileName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) < 0;
        }

        private void Filter()
        {
            List<ILuaEditDocumentEditable> docs = new List<ILuaEditDocumentEditable>();
            foreach (ILuaEditDocument doc in DocumentsManager.Instance.OpenedDocuments.Values)
            {
                if (doc is ILuaEditDocumentEditable)
                {
                    docs.Add(doc as ILuaEditDocumentEditable);
                }
            }

            docs.RemoveAll(FilterDocument);

            try
            {
                tlvwDocList.BeginUpdate();
                tlvwDocList.SmallImageList = DocumentFactory.Instance.DocumentSmallImages;
                tlvwDocList.Items.Clear();

                foreach (ILuaEditDocumentEditable doc in docs)
                {
                    TreeListViewItem tlvi = new TreeListViewItem(doc, DocumentFactory.Instance.GetDocumentSmallImageIndex(doc.DocumentType));
                    tlvi.SubItems.Add(new TreeListViewSubItem(0, doc.FileName));
                    tlvi.Tag = doc;
                    tlvwDocList.Items.Add(tlvi);
                }

                if (tlvwDocList.Items.Count > 0)
                {
                    tlvwDocList.SelectedItems.Add(tlvwDocList.Items[0]);
                }
            }
            finally
            {
                tlvwDocList.EndUpdate();
            }
        }

        #endregion
    }
}