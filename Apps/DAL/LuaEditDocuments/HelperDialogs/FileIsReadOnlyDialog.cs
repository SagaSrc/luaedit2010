using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.HelperDialogs
{
    public enum FileIsReadOnlyDialogResult
    {
        SaveAs,
        Overwrite,
        Cancel
    }

    public partial class FileIsReadOnlyDialog : Form
    {
        #region Members

        private delegate FileIsReadOnlyDialogResult ShowDialogDelegate(string fileName);
        private const string messageTemplate = "The file {0} cannot be saved because it is write-protected.\n\nYou can either save the file in a different location or LuaEdit can attempt to remove the write-protection and overwrite the file in its current location.";

        private FileIsReadOnlyDialogResult _dialogResult = FileIsReadOnlyDialogResult.Cancel;

        #endregion

        #region Constructors

        public FileIsReadOnlyDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public FileIsReadOnlyDialogResult ShowDialog(string fileName)
        {
            if (FrameworkManager.Instance.MainDialog.InvokeRequired)
            {
                FrameworkManager.Instance.MainDialog.Invoke(new ShowDialogDelegate(ShowDialog), new object[] { fileName });
                return _dialogResult;
            }

            lblMessage.Text = string.Format(messageTemplate, fileName);

            switch (this.ShowDialog(FrameworkManager.Instance.MainDialog))
            {
                case DialogResult.Yes: _dialogResult = FileIsReadOnlyDialogResult.Overwrite; break;
                case DialogResult.No: _dialogResult = FileIsReadOnlyDialogResult.SaveAs; break;
                case DialogResult.Cancel: _dialogResult = FileIsReadOnlyDialogResult.Cancel; break;
            }

            return _dialogResult;
        }

        #endregion
    }
}