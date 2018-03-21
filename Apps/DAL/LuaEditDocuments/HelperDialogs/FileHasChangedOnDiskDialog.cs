using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.HelperDialogs
{
    public enum FileHasChangedOnDiskDialogResult
    {
        Yes,
        YesToAll,
        No,
        NoToAll
    }

    public partial class FileHasChangedOnDiskDialog : Form
    {
        #region Members

        private delegate FileHasChangedOnDiskDialogResult ShowDialogDelegate(string fileName);
        private const string messageTemplate = "{0}\n\nThis file has been modified of the source editor.\nDo you want to reload it?";

        private FileHasChangedOnDiskDialogResult _dialogResult = FileHasChangedOnDiskDialogResult.No;

        #endregion

        #region Constructors

        public FileHasChangedOnDiskDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public FileHasChangedOnDiskDialogResult ShowDialog(string fileName)
        {
            if (FrameworkManager.Instance.MainDialog.InvokeRequired)
            {
                FrameworkManager.Instance.MainDialog.Invoke(new ShowDialogDelegate(ShowDialog), new object[] { fileName });
                return _dialogResult;
            }
            
            lblMessage.Text = string.Format(messageTemplate, fileName);

            switch (this.ShowDialog(FrameworkManager.Instance.MainDialog))
            {
                case DialogResult.Yes: _dialogResult = FileHasChangedOnDiskDialogResult.Yes; break;
                case DialogResult.Retry: _dialogResult = FileHasChangedOnDiskDialogResult.YesToAll; break;
                case DialogResult.No: _dialogResult = FileHasChangedOnDiskDialogResult.No; break;
                case DialogResult.Ignore: _dialogResult = FileHasChangedOnDiskDialogResult.NoToAll; break;
            }

            return _dialogResult;
        }

        #endregion
    }
}