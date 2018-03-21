using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Interfaces;

namespace LuaEdit.HelperDialogs
{
    public class CloseDialogResult
    {
        #region Members

        private DialogResult _result;
        private List<ILuaEditDocument> _selection;

        #endregion

        #region Constructors

        public CloseDialogResult(DialogResult result, List<ILuaEditDocument> selection)
        {
            _result = result;
            _selection = selection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the result of the dialog accordingly to user's choice
        /// </summary>
        public DialogResult Result
        {
            get { return _result; }
        }

        /// <summary>
        /// Get the user's selection of files to be saved
        /// </summary>
        public List<ILuaEditDocument> Selection
        {
            get { return _selection; }
        }

        #endregion
    }

    public partial class CloseDialog : Form
    {
        #region Nested Classes

        private class CloseDialogItem
        {
            #region Members

            private ILuaEditDocument _data;
            private int _level;

            #endregion

            #region Constructors

            public CloseDialogItem(ILuaEditDocument data, int level)
            {
                _data = data;
                _level = level;
            }

            #endregion

            #region Properties

            /// <summary>
            /// Get the data associated to this item
            /// </summary>
            public ILuaEditDocument Data
            {
                get { return _data; }
            }

            /// <summary>
            /// Get the level associated to this item
            /// </summary>
            public int Level
            {
                get { return _level; }
            }

            #endregion

            #region Methods

            public override string ToString()
            {
                string result = "";

                for (int x = 0; x < _level; ++x)
                    result += "  ";

                return result += _data.ToString();
            }

            #endregion
        }

        #endregion

        #region Constructors

        public CloseDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        private void AddItemRecursively(ILuaEditDocument doc, int level)
        {
            if (doc.IsModified)
                lstItems.Items.Add(new CloseDialogItem(doc, level));
        }

        public CloseDialogResult ShowDialog(List<ILuaEditDocument> docs)
        {
            List<ILuaEditDocument> selection = new List<ILuaEditDocument>();
            DialogResult result = DialogResult.No;

            foreach (ILuaEditDocument doc in docs)
                AddItemRecursively(doc, 0);

            if (lstItems.Items.Count > 0)
            {
                // Show the dialog
                result = this.ShowDialog(FrameworkManager.Instance.MainDialog);

                // Compute selection list
                if (result == DialogResult.Yes)
                {
                    // Add all items when nothing is selected but the Yes button was clicked
                    // When some items are selected, only saving the selected ones
                    if (lstItems.SelectedItems.Count > 0)
                    {
                        foreach (object item in lstItems.SelectedItems)
                            selection.Add((item as CloseDialogItem).Data);
                    }
                    else
                    {
                        foreach (object item in lstItems.Items)
                            selection.Add((item as CloseDialogItem).Data);
                    }
                }
            }

            return new CloseDialogResult(result, selection);
        }

        #endregion
    }
}