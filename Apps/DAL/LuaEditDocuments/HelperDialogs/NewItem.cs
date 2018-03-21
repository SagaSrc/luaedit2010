using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Documents;
using LuaEdit.Interfaces;
using LuaEdit.Managers;

namespace LuaEdit.HelperDialogs
{
    public partial class NewItem : Form
    {
        #region Members

        private const int NewItemNoNameDlgHeight = 400;
        private const int NewItemDlgHeight = 420;
        private const int NewProjectItemDlgHeight = 440;
        private const int NewProjectItemWithSolutionOptionDlgHeight = 465;

        private NewItemDialogStyle _dialogStyle = NewItemDialogStyle.Item;
        private ILuaEditDocumentGroup _parentContext = null;
        private NewItemTypes _itemType;
        private DocumentTemplateDef _selectedTemplateDef;
        private bool _isNameModified = false;

        #endregion

        #region Constructors

        public NewItem()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the selected template definition
        /// </summary>
        public DocumentTemplateDef SelectedTemplateDefinition
        {
            get { return _selectedTemplateDef; }
        }

        /// <summary>
        /// Gets the combined file name and location the user entered
        /// </summary>
        public string FullFileName
        {
            get { return Path.Combine(this.LocationPath, this.FileName); }
        }

        /// <summary>
        /// Gets the file name the user entered
        /// </summary>
        public string FileName
        {
            get { return txtFileName.Text; }
        }

        /// <summary>
        /// Gets the location path the user entered
        /// </summary>
        public string LocationPath
        {
            get { return txtLocation.Text; }
        }

        /// <summary>
        /// Gets/sets the UI layout style for this dialog
        /// </summary>
        public NewItemDialogStyle DialogStyle
        {
            get { return _dialogStyle; }
            set
            {
                _dialogStyle = value;

                switch (_dialogStyle)
                {
                    case NewItemDialogStyle.ItemNoName:
                        {
                            lblName.Visible = false;
                            txtFileName.Visible = false;
                            lblLocation.Visible = false;
                            txtLocation.Visible = false;
                            btnBrowse.Visible = false;
                            lblSolution.Visible = false;
                            cboSolution.Visible = false;
                            this.Height = NewItemNoNameDlgHeight;
                            break;
                        }
                    case NewItemDialogStyle.Item:
                        {
                            lblLocation.Visible = false;
                            txtLocation.Visible = false;
                            btnBrowse.Visible = false;
                            lblSolution.Visible = false;
                            cboSolution.Visible = false;
                            this.Height = NewItemDlgHeight;
                            break;
                        }
                    case NewItemDialogStyle.ItemGroup:
                        {
                            lblLocation.Visible = true;
                            txtLocation.Visible = true;
                            btnBrowse.Visible = true;
                            lblSolution.Visible = false;
                            cboSolution.Visible = false;
                            this.Height = NewProjectItemDlgHeight;
                            break;
                        }
                    case NewItemDialogStyle.ItemGroupWithSolutionOption:
                        {
                            lblLocation.Visible = true;
                            txtLocation.Visible = true;
                            btnBrowse.Visible = true;
                            lblSolution.Visible = true;
                            cboSolution.Visible = true;

                            cboSolution.Items.Clear();
                            cboSolution.Items.Add("Create New Solution");

                            if (DocumentsManager.Instance.CurrentSolution != null)
                            {
                                cboSolution.Items.Add("Add to Solution");
                            }

                            cboSolution.SelectedIndex = 0;
                            this.Height = NewProjectItemWithSolutionOptionDlgHeight;
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Gets the selected solution option
        /// </summary>
        public NewItemGroupSolutionOption SolutionOption
        {
            get { return (NewItemGroupSolutionOption)cboSolution.SelectedIndex; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when this dialog is about to close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && this.DialogStyle != NewItemDialogStyle.ItemNoName)
            {
                char[] invalidChars = null;

                string msg = "Item and file names cannot:\n\n- {0}\n- {1}\n- {2}\n- {3}\n\nPlease enter a valid name.";
                msg = string.Format(msg, "contain any of the following characters: / ? : & \\ * \" < > | # %",
                                         "contain Unicode control characters",
                                         "contain surrogate characters",
                                         "be '.' or '..'");

                if (this.DialogStyle == NewItemDialogStyle.Item)
                {
                    invalidChars = Path.GetInvalidFileNameChars();
                }
                else
                {
                    List<char> tmpInvalidChars = new List<char>();
                    tmpInvalidChars.AddRange(Path.GetInvalidPathChars());
                    tmpInvalidChars.AddRange(Path.GetInvalidFileNameChars());
                    invalidChars = tmpInvalidChars.ToArray();
                }

                foreach (char c in invalidChars)
                {
                    if (txtFileName.Text.Contains(c.ToString()))
                    {
                        MessageBox.Show(msg, "LuaEdit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Cancel = true;
                        txtFileName.Focus();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when this dialog is first shown on screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewItem_Load(object sender, EventArgs e)
        {
            lvwNewItems.SmallImageList = DocumentFactory.Instance.DocumentSmallImages;
            lvwNewItems.LargeImageList = DocumentFactory.Instance.DocumentLargeImages;
        }

        /// <summary>
        /// Occurs when the selection changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwNewItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = lvwNewItems.SelectedItems.Count > 0;

            if (lvwNewItems.SelectedItems.Count > 0)
            {
                _selectedTemplateDef = lvwNewItems.SelectedItems[0].Tag as DocumentTemplateDef;
                lblDescription.Text = _selectedTemplateDef.TemplateDescription;

                if (_itemType == NewItemTypes.Item)
                {
                    if (_isNameModified)
                        Path.ChangeExtension(txtFileName.Text, _selectedTemplateDef.AssociatedExtension);
                    else
                        txtFileName.Text = FrameworkManager.Instance.GetItemNewFileName(_parentContext,
                                                                                        _selectedTemplateDef.SuggestiveName,
                                                                                        _selectedTemplateDef.AssociatedExtension);
                }
                else if (_itemType == NewItemTypes.ItemGroup)
                    txtFileName.Text = _selectedTemplateDef.SuggestiveName;
            }
            else
                txtFileName.Text = string.Empty;
        }

        /// <summary>
        /// Occurs when the show small icon button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSmallIcons_CheckedChanged(object sender, EventArgs e)
        {
            btnSmallIcons.FlatAppearance.BorderSize = btnSmallIcons.Checked ? 1 : 0;
            btnLargeIcons.FlatAppearance.BorderSize = btnLargeIcons.Checked ? 1 : 0;
            lvwNewItems.View = btnSmallIcons.Checked ? View.SmallIcon : View.LargeIcon;
            RefreshList();
        }

        /// <summary>
        /// Occurs when the file name content changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = txtFileName.Text != "";
        }

        /// <summary>
        /// Occurs when the browse button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog();
            folderBrowserDlg.Description = "Select a folder where the item should be created:";

            if (Directory.Exists(txtLocation.Text))
                folderBrowserDlg.SelectedPath = txtLocation.Text;
            else if (_parentContext != null)
                folderBrowserDlg.SelectedPath = Path.GetDirectoryName(_parentContext.FileName);

            if (folderBrowserDlg.ShowDialog(this) == DialogResult.OK)
                txtLocation.Text = folderBrowserDlg.SelectedPath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Show the dialog
        /// </summary>
        /// <param name="title">The title to display for this dialog</param>
        /// <param name="itemType">The type of new item to suggest in the dialog</param>
        /// <param name="dialogStyle">The layout display style</param>
        /// <returns>The dialog user's choice (will either be OK or Cancel)</returns>
        public DialogResult ShowDialog(string title, NewItemTypes itemType, NewItemDialogStyle dialogStyle)
        {
            return this.ShowDialog(title, null, itemType, dialogStyle);
        }

        /// <summary>
        /// Show the dialog
        /// </summary>
        /// <param name="title">The title to display for this dialog</param>
        /// <param name="parentContext">The parent contextual to the new object to add</param>
        /// <param name="itemType">The type of new item to suggest in the dialog</param>
        /// <param name="dialogStyle">The layout display style</param>
        /// <returns>The dialog user's choice (will either be OK or Cancel)</returns>
        public DialogResult ShowDialog(string title, ILuaEditDocumentGroup parentContext,
                                       NewItemTypes itemType, NewItemDialogStyle dialogStyle)
        {
            string location = parentContext != null ? Path.GetDirectoryName(parentContext.FileName) : string.Empty;
            return this.ShowDialog(title, parentContext, location, itemType, dialogStyle);
        }

        /// <summary>
        /// Show the dialog
        /// </summary>
        /// <param name="title">The title to display for this dialog</param>
        /// <param name="parentContext">The parent contextual to the new object to add</param>
        /// <param name="location">The location directory to suggest</param>
        /// <param name="itemType">The type of new item to suggest in the dialog</param>
        /// <param name="dialogStyle">The layout display style</param>
        /// <returns>The dialog user's choice (will either be OK or Cancel)</returns>
        public DialogResult ShowDialog(string title, ILuaEditDocumentGroup parentContext, string location,
                                       NewItemTypes itemType, NewItemDialogStyle dialogStyle)
        {
            this.DialogStyle = dialogStyle;
            this.Text = title;
            btnAdd.Text = parentContext == null ? "OK" : "Add";
            _parentContext = parentContext;
            _itemType = itemType;
            lvwNewItems.SmallImageList = DocumentFactory.Instance.DocumentSmallImages;
            lvwNewItems.LargeImageList = DocumentFactory.Instance.DocumentLargeImages;
            txtLocation.Text = location;
            FillList();

            if (lvwNewItems.Items.Count > 0)
                lvwNewItems.Items[0].Selected = true;

            return this.ShowDialog(FrameworkManager.Instance.MainDialog);
        }

        /// <summary>
        /// Fill the lis of templates
        /// </summary>
        public void FillList()
        {
            foreach (DocumentTemplateDef templateDef in TemplateManager.Instance.DocumentTemplates)
            {
                if (templateDef.TemplateType == Enum.GetName(typeof(NewItemTypes), _itemType))
                {
                    ListViewItem item = lvwNewItems.Items.Add(templateDef.TemplateName);
                    item.Group = lvwNewItems.Groups["lvgrpLuaEditTemplates"];
                    item.Tag = templateDef;

                    if (templateDef.AssociatedExtension != string.Empty)
                    {
                        if (lvwNewItems.View == View.LargeIcon)
                            item.ImageIndex = DocumentFactory.Instance.GetDocumentLargeImageIndex(templateDef.AssociatedExtension);
                        else
                            item.ImageIndex = DocumentFactory.Instance.GetDocumentSmallImageIndex(templateDef.AssociatedExtension);
                    }
                }
            }
        }

        public void RefreshList()
        {
            foreach (ListViewItem item in lvwNewItems.Items)
            {
                DocumentTemplateDef templateDef = item.Tag as DocumentTemplateDef;

                if (lvwNewItems.View == View.LargeIcon)
                    item.ImageIndex = DocumentFactory.Instance.GetDocumentLargeImageIndex(templateDef.AssociatedExtension);
                else
                    item.ImageIndex = DocumentFactory.Instance.GetDocumentSmallImageIndex(templateDef.AssociatedExtension);
            }
        }

        #endregion
    }

    /// <summary>
    /// Enum for the different UI layouts of this dialog
    /// </summary>
    public enum NewItemDialogStyle
    {
        Item,
        ItemNoName,
        ItemGroup,
        ItemGroupWithSolutionOption
    }

    /// <summary>
    /// Enum for the type of new items
    /// </summary>
    public enum NewItemTypes
    {
        Item,
        ItemGroup
    }

    /// <summary>
    /// Enum for the different solution options
    /// </summary>
    public enum NewItemGroupSolutionOption
    {
        None = -1,
        CreateNewSolution = 0,
        AddToCurrentSolution
    }
}