using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using LuaEdit;

namespace LuaEdit.Controls
{
    public partial class LuaEditFolderSelector : Form
    {
        #region Nested Classes

        public class FolderSetItem
        {
            #region Members

            private string _folderSetName = string.Empty;
            private List<string> _folders = null;

            #endregion

            #region Constructors

            public FolderSetItem(string folderSetName, List<string> folders)
            {
                _folderSetName = folderSetName;
                _folders = folders;
            }

            public FolderSetItem(string folderSetName)
                : this(folderSetName, new List<string>())
            {
            }

            public FolderSetItem(string folderSetName, string[] folders)
                : this(folderSetName, new List<string>(folders))
            {
            }

            public FolderSetItem(string folderSetName, string folders)
                : this(folderSetName, folders.Split(new char[] { ';' }))
            {
            }

            #endregion

            #region Properties

            public string FolderSetName
            {
                get { return _folderSetName; }
                set { _folderSetName = value; }
            }

            public List<string> Folders
            {
                get { return _folders; }
            }

            public string FoldersString
            {
                get { return string.Join(";", _folders.ToArray()); }
            }

            #endregion

            #region Methods

            public override string ToString()
            {
                return this.FolderSetName;
            }

            #endregion
        }

        #endregion

        #region Members

        public const string EmptyFolderSetName = "<Unnamed folder set>";

        private const string FolderSetsRegKeyName = @"FolderSelector\FolderSets";
        private const string MyComputerFolder = "My Computer";

        private string _initialDir = string.Empty;

        #endregion

        #region Constructors

        public LuaEditFolderSelector()
        {
            InitializeComponent();
            this.InitialDir = MyComputerFolder;
            BuildFolderSetsList();
        }

        #endregion

        #region Properties

        private List<string> SelectedFolders
        {
            get
            {
                List<string> selectedFolders = new List<string>();

                foreach (ListViewItem lvi in lvwSelectedFolders.Items)
                {
                    selectedFolders.Add(lvi.Name);
                }

                return selectedFolders;
            }
        }

        public FolderSetItem SelectedFolderSet
        {
            get { return cboFolderSets.SelectedItem as FolderSetItem; }
        }

        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public string Message
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; }
        }

        public string InitialDir
        {
            get { return _initialDir; }
            set
            {
                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    if (_initialDir.ToLower() != value.ToLower() &&
                        ((Directory.Exists(value) && value.IndexOf('\\') >= 0) ||
                         value.ToLower() == MyComputerFolder.ToLower()))
                    {
                        _initialDir = value;
                        SetCurrentPath(_initialDir);
                    }
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        #endregion

        #region Event Handlers

        private void cboCurrentPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.InitialDir = cboCurrentPath.Text;
            }
        }

        private void cboCurrentPath_Leave(object sender, EventArgs e)
        {
            this.InitialDir = cboCurrentPath.Text;
        }

        private void cboFolderSets_TextChanged(object sender, EventArgs e)
        {
            ValidateFolderSetsButtons();
        }

        private void cboFolderSets_SelectedValueChanged(object sender, EventArgs e)
        {
            FolderSetItem currentFolderSet = cboFolderSets.SelectedItem as FolderSetItem;

            if (currentFolderSet is FolderSetItem)
            {
                lvwSelectedFolders.Items.Clear();

                foreach (string folder in currentFolderSet.Folders)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(folder);
                    SystemIconManager.Instance.GetDirectorySystemImage(dirInfo.FullName);
                    lvwSelectedFolders.Items.Add(CreateNewSelectedDir(dirInfo));
                }
            }
        }

        private void btnMoveUpOneFolder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.InitialDir))
            {
                DirectoryInfo dirInfo = Directory.GetParent(this.InitialDir);

                if (dirInfo == null)
                {
                    this.InitialDir = MyComputerFolder;
                }
                else
                {
                    this.InitialDir = dirInfo.FullName;
                }
            }
        }

        private void lvwAvailableFolders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvwAvailableFolders.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvwAvailableFolders.SelectedItems[0];
                this.InitialDir = (lvi.Tag as DirectoryInfo).FullName;
            }
        }

        private void lvwAvailableFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateTransferButtons();
        }

        private void lvwSelectedFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateTransferButtons();
        }

        private void btnTransferRight_Click(object sender, EventArgs e)
        {
            if (lvwAvailableFolders.SelectedItems.Count > 0)
            {
                int lastIndex = 0;

                // Transfer items to selected folders list
                foreach (ListViewItem lvi in lvwAvailableFolders.SelectedItems)
                {
                    DirectoryInfo dirInfo = lvi.Tag as DirectoryInfo;

                    if (lvi.Index >= lastIndex)
                    {
                        lastIndex = lvi.Index;
                    }

                    if (lvwSelectedFolders.Items.Find(dirInfo.FullName, false).Length == 0)
                    {
                        lvwSelectedFolders.Items.Add(CreateNewSelectedDir(dirInfo));
                    }
                }

                // Select next folder in the list to ease user with transfer
                if (lastIndex < lvwAvailableFolders.Items.Count - 1)
                {
                    lvwAvailableFolders.SelectedIndices.Clear();
                    lvwAvailableFolders.SelectedIndices.Add(lastIndex + 1);
                }

                ValidateTransferButtons();
            }
        }

        private void btnTransferLeft_Click(object sender, EventArgs e)
        {
            int lastIndex = 0;
            int[] selectedFolderIndices = new int[] { };
            Array.Resize<int>(ref selectedFolderIndices, lvwSelectedFolders.SelectedIndices.Count);
            lvwSelectedFolders.SelectedIndices.CopyTo(selectedFolderIndices, 0);

            // Remove selected folders from the list (giving impression to user to transfer them back
            // to the list of available folders)
            foreach (int lviIndex in selectedFolderIndices)
            {
                if (lviIndex >= lastIndex)
                {
                    lastIndex = lviIndex;
                }

                lvwSelectedFolders.Items.RemoveAt(lviIndex);
            }

            // Select next folder in the list to ease user with transfer
            if (lastIndex < lvwAvailableFolders.Items.Count - 1)
            {
                lvwAvailableFolders.SelectedIndices.Clear();
                lvwAvailableFolders.SelectedIndices.Add(lastIndex + 1);
            }

            ValidateTransferButtons();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int[] selectedFolderIndices = new int[] { };
            Array.Resize<int>(ref selectedFolderIndices, lvwSelectedFolders.SelectedIndices.Count);
            lvwSelectedFolders.SelectedIndices.CopyTo(selectedFolderIndices, 0);

            for (int x = 0; x < selectedFolderIndices.Length; ++x)
            {
                int y = selectedFolderIndices[x];

                if (y > 0 && !lvwSelectedFolders.Items[y - 1].Selected)
                {
                    ListViewItem lviSwap = lvwSelectedFolders.Items[y];
                    lvwSelectedFolders.Items.RemoveAt(y);
                    lvwSelectedFolders.Items.Insert(y - 1, lviSwap);
                }
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            int[] selectedFolderIndices = new int[] { };
            Array.Resize<int>(ref selectedFolderIndices, lvwSelectedFolders.SelectedIndices.Count);
            lvwSelectedFolders.SelectedIndices.CopyTo(selectedFolderIndices, 0);

            for (int x = selectedFolderIndices.Length - 1; x >= 0; --x)
            {
                int y = selectedFolderIndices[x];

                if (y < lvwSelectedFolders.Items.Count - 1 && !lvwSelectedFolders.Items[y + 1].Selected)
                {
                    ListViewItem lviSwap = lvwSelectedFolders.Items[y];
                    lvwSelectedFolders.Items.RemoveAt(y);
                    lvwSelectedFolders.Items.Insert(y + 1, lviSwap);
                }
            }
        }

        private void btnApplyFolderSet_Click(object sender, EventArgs e)
        {
            ApplySelectedFoldersToFolderSet();
        }

        private void btnDeleteFolderSet_Click(object sender, EventArgs e)
        {
            if (cboFolderSets.Text != EmptyFolderSetName)
            {
                FolderSetItem selectedFolderSet = cboFolderSets.SelectedItem as FolderSetItem;
                cboFolderSets.Items.Remove(selectedFolderSet);
                cboFolderSets.SelectedIndex = 0;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cboFolderSets.Items.Count >= 0 && cboFolderSets.Text == EmptyFolderSetName)
            {
                FolderSetItem emptyFolderSet = cboFolderSets.Items[0] as FolderSetItem;
                emptyFolderSet.Folders.Clear();
                emptyFolderSet.Folders.AddRange(this.SelectedFolders);
                cboFolderSets.SelectedItem = emptyFolderSet;
            }
        }

        #endregion

        #region Methods

        public new DialogResult ShowDialog(string title, string message)
        {
            this.Text = title;
            lblMessage.Text = message;

            return base.ShowDialog();
        }

        private ListViewItem CreateNewSelectedDir(DirectoryInfo dirInfo)
        {
            ListViewItem newLvi = new ListViewItem(dirInfo.FullName);
            newLvi.Tag = dirInfo;
            newLvi.Name = dirInfo.FullName;
            newLvi.ImageKey = dirInfo.FullName;
            return newLvi;
        }

        private void ApplySelectedFoldersToFolderSet()
        {
            string currentFolderSetName = cboFolderSets.Text;
            FolderSetItem currentFolderSet = null;

            // Attempt to find the folder set in the list
            foreach (FolderSetItem fsi in cboFolderSets.Items)
            {
                if (fsi.FolderSetName == currentFolderSetName)
                {
                    currentFolderSet = fsi;
                    break;
                }
            }

            if (currentFolderSet == null)
            {
                currentFolderSet = new FolderSetItem(currentFolderSetName, this.SelectedFolders);
                cboFolderSets.Items.Add(currentFolderSet);
                cboFolderSets.SelectedItem = currentFolderSet;
            }
            else
            {
                currentFolderSet.Folders.Clear();
                currentFolderSet.Folders.AddRange(this.SelectedFolders);
            }

            SaveFolderSetsList();
        }

        private void SaveFolderSetsList()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(FolderSetsRegKeyName, true);

            try
            {
                Registry.CurrentUser.DeleteSubKey(FrameworkManager.ApplicationRegistryKeyName + "\\" + FolderSetsRegKeyName, false);
                regKey = Registry.CurrentUser.CreateSubKey(FrameworkManager.ApplicationRegistryKeyName + "\\" + FolderSetsRegKeyName);
                if (regKey == null)
                    regKey = Registry.CurrentUser.CreateSubKey(FrameworkManager.ApplicationRegistryKeyName + "\\" + FolderSetsRegKeyName);

                foreach (FolderSetItem fsi in cboFolderSets.Items)
                {
                    if (fsi.FolderSetName != EmptyFolderSetName)
                    {
                        regKey.SetValue(fsi.FolderSetName, string.Join(";", fsi.Folders.ToArray()));
                    }
                }

                regKey.Flush();
            }
            catch (Exception e)
            {
                string msg = string.Format("An error occured while loading folder sets: {0}\n\n{1}", e.Message, e.StackTrace);
                MessageBox.Show(msg, "LuaEdit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (regKey != null)
                    regKey.Close();
            }
        }

        static public List<FolderSetItem> LoadFolderSetsList()
        {
            List<FolderSetItem> folderSets = new List<FolderSetItem>();
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(FrameworkManager.ApplicationRegistryKeyName);

            try
            {
                if (regKey != null && regKey.ValueCount > 0)
                {
                    regKey = Registry.CurrentUser.OpenSubKey(FrameworkManager.ApplicationRegistryKeyName + "\\" + FolderSetsRegKeyName);

                    if (regKey != null && regKey.ValueCount > 0)
                    {
                        foreach (string valueName in regKey.GetValueNames())
                        {
                            folderSets.Add(new FolderSetItem(valueName, Convert.ToString(regKey.GetValue(valueName))));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string msg = string.Format("An error occured while loading folder sets: {0}\n\n{1}", e.Message, e.StackTrace);
                MessageBox.Show(msg, "LuaEdit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (regKey != null)
                    regKey.Close();
            }

            return folderSets;
        }

        private void BuildFolderSetsList()
        {
            List<FolderSetItem> folderSets = LoadFolderSetsList();
            cboFolderSets.Items.Clear();
            cboFolderSets.Items.Insert(0, new FolderSetItem(EmptyFolderSetName));
            cboFolderSets.Items.AddRange(folderSets.ToArray());
            cboFolderSets.SelectedIndex = 0;
        }

        private void SetCurrentPath(string path)
        {
            List<string> subDirectories = new List<string>();
            cboCurrentPath.Text = path;

            btnMoveUpOneFolder.Enabled = path != MyComputerFolder;

            if (path == MyComputerFolder)
            {
                foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                {
                    subDirectories.Add(driveInfo.Name);
                }
            }
            else
            {
                subDirectories.AddRange(Directory.GetDirectories(path));
            }

            // Fill the list of available paths
            lvwAvailableFolders.Items.Clear();
            lvwAvailableFolders.SmallImageList = SystemIconManager.Instance.SystemImages;
            lvwSelectedFolders.SmallImageList = SystemIconManager.Instance.SystemImages;
            int largestItemWidth = 0;

            foreach (string subDirectory in subDirectories)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(subDirectory);
                ListViewItem lvi = new ListViewItem(dirInfo.Name);
                SystemIconManager.Instance.GetDirectorySystemImage(subDirectory);

                Size textSize = TextRenderer.MeasureText(dirInfo.Name, lvwAvailableFolders.Font);
                if (textSize.Width > largestItemWidth)
                {
                    lvwAvailableFolders.Columns[0].Width = textSize.Width + 20;
                    largestItemWidth = textSize.Width;
                }
                
                lvi.Tag = dirInfo;
                lvi.Name = subDirectory;
                lvi.ImageKey = subDirectory;
                lvwAvailableFolders.Items.Add(lvi);
            }

            if (lvwAvailableFolders.Columns[0].Width < 210)
                lvwAvailableFolders.Columns[0].Width = 210;

            // Revalidate buttons state
            ValidateTransferButtons();
        }

        private void ValidateFolderSetsButtons()
        {
            btnApplyFolderSet.Enabled = cboFolderSets.Text != EmptyFolderSetName;
            btnDeleteFolderSet.Enabled = cboFolderSets.Text != EmptyFolderSetName;
        }

        private void ValidateTransferButtons()
        {
            btnTransferRight.Enabled = lvwAvailableFolders.SelectedItems.Count > 0;
            btnTransferLeft.Enabled = lvwSelectedFolders.SelectedItems.Count > 0;
        }

        #endregion
    }
}