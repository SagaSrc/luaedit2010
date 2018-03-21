using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Fireball.Syntax;

using LuaEdit;
using LuaEdit.Managers;
using LuaEdit.HelperDialogs;
using LuaEdit.Interfaces;
using LuaEdit.Win32;

namespace LuaEdit.Documents
{
    public class DocumentsManager : IDisposable
    {
        #region Members

        private static DocumentsManager _documentsManager = null;
        private const int MaxMRU = 10;
        private Dictionary<string, ILuaEditDocument> _openedDocuments; // Key: filename, Value: document instance
        private Dictionary<string, ILuaEditDocument> _diskModifiedDocuments; // Key: filename, Value: document instance
        private LuaSolutionDocument _currentSolution;

        private OpenFileDialog _openFileDlg;
        private SaveFileDialog _saveFileDlg;
        private Thread _diskModifiedDocumentChecker;

        private Bitmap _lineMarker = null;
        private List<string> _recentFiles;
        private List<string> _recentProjects;

        public static string LuaEditDocumentsFilterString;
        public static string DocumentsFilterString = string.Empty;
        public static string AllFilesFilterString = "All Files (*.*)|*.*";

        public event EventHandler RecentFilesChanged;
        public event EventHandler RecentProjectsChanged;
        public event EventHandler CurrentSolutionChanged;
        public event DocumentsActionHandler DocumentsOpened;
        public event DocumentsActionHandler DocumentsClosed;

        #endregion

        #region Constructors

        static DocumentsManager()
        {
            _documentsManager = new DocumentsManager();

            // Register all known document types
            RegisterDocumentTypes();
        }

        private DocumentsManager()
        {
            // Create empty list of opened documents
            _openedDocuments = new Dictionary<string, ILuaEditDocument>();

            // Create empty list of documents that differ from the disk
            _diskModifiedDocuments = new Dictionary<string, ILuaEditDocument>();

            // Create empty list of recent files
            _recentFiles = new List<string>(MaxMRU);
            _recentProjects = new List<string>(MaxMRU);

            // Create thread that will be responsible to watch the list of documents that differ from the disk
            _diskModifiedDocumentChecker = new Thread(CheckDiskModifiedDocuments);
            _diskModifiedDocumentChecker.Name = "CheckDiskModDocsWorkerThread";
            _diskModifiedDocumentChecker.SetApartmentState(ApartmentState.STA);
            _diskModifiedDocumentChecker.Start();

            // Create and intialize the only open file dialog used in this framework
            _openFileDlg = new OpenFileDialog();

            // Create and intialize the only save file dialog used in this framework
            _saveFileDlg = new SaveFileDialog();
            _saveFileDlg.Filter = AllFilesFilterString;
            _saveFileDlg.Title = "Save File";
            _saveFileDlg.OverwritePrompt = true;

            // Gets the line marker image from global properties
            _lineMarker = LuaEdit.Documents.Properties.Resources.LineBreakMarker.Clone() as Bitmap;
            _lineMarker.MakeTransparent(_lineMarker.GetPixel(0, 0));

            // Load recent files from registry
            LoadRecentFiles();
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            SaveRecentFiles();

            if (_diskModifiedDocumentChecker.IsAlive)
                _diskModifiedDocumentChecker.Abort();
        }

        #endregion

        #region Event Handlers

        private void OnDocumentTerminated(object sender, EventArgs e)
        {
            ILuaEditDocument doc = sender as ILuaEditDocument;

            if (doc != null)
            {
                _openedDocuments.Remove(doc.FileName.ToLower());
            }
        }

        private void OnSolutionTerminated(object sender, EventArgs e)
        {
            this.CurrentSolution = null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the only instance of DocumentsManager
        /// </summary>
        static public DocumentsManager Instance
        {
            get { return _documentsManager; }
        }

        /// <summary>
        /// Get/Set the currently opened solution
        /// </summary>
        public LuaSolutionDocument CurrentSolution
        {
            get { return _currentSolution; }
            set
            {
                if (_currentSolution != null)
                {
                    if (!_currentSolution.IsTerminated && !CloseDocument(_currentSolution))
                    {
                        return;
                    }

                    _currentSolution.DocumentTerminated -= OnSolutionTerminated;
                }

                _currentSolution = value;

                if (_currentSolution != null)
                {
                    _currentSolution.DocumentTerminated += OnSolutionTerminated;
                }

                if (CurrentSolutionChanged != null)
                {
                    CurrentSolutionChanged(this, new EventArgs());
                }

            }
        }

        /// <summary>
        /// List of currently opened documents indexed by their file name
        /// </summary>
        public Dictionary<string, ILuaEditDocument> OpenedDocuments
        {
            get { return _openedDocuments; }
        }

        /// <summary>
        /// Get the list of recently opened files
        /// </summary>
        public List<string> RecentFiles
        {
            get { return _recentFiles; }
        }

        /// <summary>
        /// Get the list of recently opened projects
        /// </summary>
        public List<string> RecentProjects
        {
            get { return _recentProjects; }
        }

        public Bitmap LineMarker
        {
            get { return _lineMarker; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers all known document types
        /// </summary>
        static private void RegisterDocumentTypes()
        {
            DocumentFactory.Instance.RegisterDocumentType(typeof(LuaSolutionDocument), LuaSolutionDocument.Extension,
                                                          LuaSolutionDocument.DescriptiveName,
                                                          LuaEdit.Documents.Properties.Resources.LuaSolution_16x16,
                                                          LuaEdit.Documents.Properties.Resources.LuaSolution_32x32,
                                                          LuaEdit.Documents.Properties.Resources.LuaSolution_16x16);
            DocumentFactory.Instance.RegisterDocumentType(typeof(LuaProjectDocument), LuaProjectDocument.Extension,
                                                          LuaProjectDocument.DescriptiveName,
                                                          Properties.Resources.LuaProject_16x16,
                                                          Properties.Resources.LuaProject_32x32,
                                                          Properties.Resources.LuaProject_16x16);
            DocumentFactory.Instance.RegisterDocumentType(typeof(LuaScriptDocument), LuaScriptDocument.Extension,
                                                          LuaScriptDocument.DescriptiveName,
                                                          Properties.Resources.LuaScript_16x16,
                                                          Properties.Resources.LuaScript_32x32,
                                                          Properties.Resources.LuaScript_16x16);
            DocumentFactory.Instance.RegisterDocumentType(typeof(TextDocument), TextDocument.Extension,
                                                          TextDocument.DescriptiveName,
                                                          Properties.Resources.TextDocument_16x16,
                                                          Properties.Resources.TextDocument_32x32,
                                                          Properties.Resources.TextDocument_16x16);

            List<Type> documentTypes = new List<Type>();
            documentTypes.Add(typeof(LuaSolutionDocument));
            documentTypes.Add(typeof(LuaProjectDocument));
            documentTypes.Add(typeof(LuaScriptDocument));
            documentTypes.Add(typeof(TextDocument));
            DocumentsFilterString = DocumentFactory.Instance.GetDocumentsFilter();
            LuaEditDocumentsFilterString = DocumentFactory.Instance.GetDocumentFilter(documentTypes, "LuaEdit Supported Files");
        }

        private void LoadRecentFiles()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(FrameworkManager.ApplicationRegistryKeyName);

            try
            {
                if (regKey != null && regKey.ValueCount > 0)
                {
                    // Manage recent files list
                    regKey = Registry.CurrentUser.OpenSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentFiles");
                    if (regKey != null && regKey.ValueCount > 0)
                    {
                        Dictionary<int, string> indexedList = new Dictionary<int, string>();

                        foreach (string valueName in regKey.GetValueNames())
                        {
                            if (!string.IsNullOrEmpty(valueName))
                            {
                                int index = Convert.ToInt32(regKey.GetValue(valueName));
                                indexedList.Add(index, valueName);
                            }
                        }

                        for (int x = 0; x < indexedList.Count; ++x)
                            _recentFiles.Insert(x, indexedList[x]);
                    }

                    // Manage recent projects list
                    regKey = Registry.CurrentUser.OpenSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentProjects");
                    if (regKey != null && regKey.ValueCount > 0)
                    {
                        Dictionary<int, string> indexedList = new Dictionary<int, string>();

                        foreach (string valueName in regKey.GetValueNames())
                        {
                            if (!string.IsNullOrEmpty(valueName))
                            {
                                int index = Convert.ToInt32(regKey.GetValue(valueName));
                                indexedList.Add(index, valueName);
                            }
                        }

                        for (int x = 0; x < indexedList.Count; ++x)
                            _recentProjects.Insert(x, indexedList[x]);
                    }
                }
            }
            catch (Exception e)
            {
                string msg = string.Format("An error occured while loading recent files list: {0}\n\n{1}", e.Message, e.StackTrace);
                FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (regKey != null)
                    regKey.Close();
            }
        }

        private void SaveRecentFiles()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(FrameworkManager.ApplicationRegistryKeyName, true);

            try
            {
                // Save recent files
                Registry.CurrentUser.DeleteSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentFiles", false);
                regKey = Registry.CurrentUser.CreateSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentFiles");
                if (regKey == null)
                    regKey = Registry.CurrentUser.CreateSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentFiles");

                for (int x = 0; x < _recentFiles.Count; ++x)
                    regKey.SetValue(_recentFiles[x], x);

                regKey.Flush();

                // Save recent projects
                Registry.CurrentUser.DeleteSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentProjects", false);
                regKey = Registry.CurrentUser.CreateSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentProjects");
                if (regKey == null)
                    regKey = Registry.CurrentUser.CreateSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentProjects");

                for (int x = 0; x < _recentProjects.Count; ++x)
                    regKey.SetValue(_recentProjects[x], x);

                regKey.Flush();
            }
            catch (Exception e)
            {
                string msg = string.Format("An error occured while saving recent files list: {0}\n\n{1}", e.Message, e.StackTrace);
                FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (regKey != null)
                    regKey.Close();
            }
        }

        /// <summary>
        /// Will constantly check if there are any item in the list of documents that differ from the disk
        /// </summary>
        private void CheckDiskModifiedDocuments()
        {
            while (true)
            {
                // Check every seconds if any file has changed on the disk
                Thread.Sleep(1000);

                if (_diskModifiedDocuments.Count > 0)
                {
                    lock (_diskModifiedDocuments)
                    {
                        FileHasChangedOnDiskDialogResult previousResult = FileHasChangedOnDiskDialogResult.No;
                        FileHasChangedOnDiskDialog fileHasChangedOnDiskDialog = new FileHasChangedOnDiskDialog();

                        foreach (KeyValuePair<string, ILuaEditDocument> kvp in _diskModifiedDocuments)
                        {
                            if (previousResult != FileHasChangedOnDiskDialogResult.NoToAll)
                            {
                                if (previousResult != FileHasChangedOnDiskDialogResult.YesToAll)
                                {
                                    FileHasChangedOnDiskDialogResult result = fileHasChangedOnDiskDialog.ShowDialog(kvp.Key);
                                    previousResult = result;

                                    switch (result)
                                    {
                                        case FileHasChangedOnDiskDialogResult.Yes:
                                        case FileHasChangedOnDiskDialogResult.YesToAll:
                                            {
                                                ReloadDocument(kvp.Key);
                                                break;
                                            }
                                        case FileHasChangedOnDiskDialogResult.No:
                                        case FileHasChangedOnDiskDialogResult.NoToAll:
                                            {
                                                break;
                                            }
                                    }
                                }
                                else
                                    ReloadDocument(kvp.Key);
                            }
                        }

                        _diskModifiedDocuments.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Apply set of options to a document
        /// </summary>
        /// <param name="doc">The opened document</param>
        /// <param name="options">Optional set of options to apply once the document is opened</param>
        private void ApplyPostDocumentOptions(ILuaEditDocument doc, PostOpenDocumentOptions options)
        {
            if (options != null && doc is ILuaEditDocumentEditable)
            {
                ILuaEditDocumentEditable editableDoc = doc as ILuaEditDocumentEditable;

                if (options.GotoLine != PostOpenDocumentOptions.GotoLineDefaultValue)
                {
                    editableDoc.DocumentUI.Goto(options.GotoLine);

                    // Highlight line if required
                    if (options.HighlightLineColor != PostOpenDocumentOptions.HighLightLineColor)
                    {
                        editableDoc.DocumentUI.HighlightLine(options.GotoLine, options.HighlightLineColor);
                    }

                    // Mark line
                    editableDoc.DocumentUI.MarkLine(options.GotoLine, options.LineMarked);
                }
                else
                {
                    editableDoc.DocumentUI.Editor.Caret.Position = options.CaretPosition;
                }
            }
        }

        /// <summary>
        /// Add the specified file name to the list of recent files
        /// </summary>
        /// <param name="fileName">The file name to be added</param>
        public void AddToRecentFiles(string fileName)
        {
            if (_recentFiles.IndexOf(fileName) != -1)
                _recentFiles.RemoveAt(_recentFiles.IndexOf(fileName));

            _recentFiles.Insert(0, fileName);

            if (_recentFiles.Count > MaxMRU)
                _recentFiles.RemoveAt(MaxMRU);

            // Saving values
            SaveRecentFiles();

            if (RecentFilesChanged != null)
                RecentFilesChanged(this, new EventArgs());
        }

        /// <summary>
        /// Add the specified project file name to the list of recent projects
        /// </summary>
        /// <param name="fileName">The file name to be added</param>
        public void AddToRecentProjects(string fileName)
        {
            if (_recentProjects.IndexOf(fileName) != -1)
                _recentProjects.RemoveAt(_recentProjects.IndexOf(fileName));

            _recentProjects.Insert(0, fileName);

            if (_recentProjects.Count > MaxMRU)
                _recentProjects.RemoveAt(MaxMRU);

            // Saving values
            SaveRecentFiles();

            if (RecentProjectsChanged != null)
                RecentProjectsChanged(this, new EventArgs());
        }

        /// <summary>
        /// Remove the specified file name from the list of recent files
        /// </summary>
        /// <param name="fileName"></param>
        public void RemoveFromRecentFiles(string fileName)
        {
            if (_recentFiles.IndexOf(fileName) != -1)
            {
                _recentFiles.RemoveAt(_recentFiles.IndexOf(fileName));

                if (_recentFiles.Count > MaxMRU)
                {
                    _recentFiles.RemoveAt(MaxMRU);
                }

                // Saving values
                SaveRecentFiles();

                if (RecentFilesChanged != null)
                {
                    RecentFilesChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Remove the specified project file name from the list of recent projects
        /// </summary>
        /// <param name="fileName"></param>
        public void RemoveFromRecentProjects(string fileName)
        {
            if (_recentProjects.IndexOf(fileName) != -1)
            {
                _recentProjects.RemoveAt(_recentProjects.IndexOf(fileName));

                if (_recentProjects.Count > MaxMRU)
                {
                    _recentProjects.RemoveAt(MaxMRU);
                }

                // Saving values
                SaveRecentFiles();

                if (RecentProjectsChanged != null)
                {
                    RecentProjectsChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Notify to the framework manager that a file has changed on the disk
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="doc"></param>
        public void NotifyDocumentHasChangedOnDisk(string fileName, ILuaEditDocument doc)
        {
            lock (_diskModifiedDocuments)
            {
                if (!_diskModifiedDocuments.ContainsKey(fileName))
                {
                    _diskModifiedDocuments.Add(fileName, doc);
                }
            }
        }

        /// <summary>
        /// Copies the specified files to their matching new destinations
        /// </summary>
        /// <param name="fileNames">Dictionary of key-value pairs (source-destination)</param>
        /// <returns>The final list of copied files</returns>
        public List<string> CopyFiles(Dictionary<string, string> fileNames)
        {
            bool applyToAllItems = false;
            DialogResult applyToAllItemsResult = DialogResult.Yes;
            Dictionary<string, string> finalFileNames = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> fileName in fileNames)
            {
                if (!File.Exists(fileName.Key) || !Path.IsPathRooted(fileName.Key))
                {
                    throw new Exception("The source was probably renamed, moved or deleted directly on the disk.");
                }

                DialogResult result = DialogResult.Yes;

                if (File.Exists(fileName.Value) && Path.IsPathRooted(fileName.Value))
                {
                    if (applyToAllItems)
                    {
                        result = applyToAllItemsResult;
                    }
                    else
                    {
                        ItemAlreadyExistDialog dlg = new ItemAlreadyExistDialog();
                        dlg.Title = "Destination File Exists";
                        dlg.Message = string.Format("A file with the name '{0}' already exists. Do you want to replace it?", Path.GetFileName(fileName.Value));
                        result = dlg.ShowDialog();
                        applyToAllItems = dlg.ApplyAll;

                        if (applyToAllItems)
                        {
                            applyToAllItemsResult = result;
                        }
                    }
                }

                if (result == DialogResult.Yes)
                {
                    finalFileNames.Add(fileName.Key, fileName.Value);
                }
                else if (result == DialogResult.Cancel)
                {
                    return null;
                }
            }

            List<string> finalList = new List<string>();

            foreach (KeyValuePair<string, string> fileName in finalFileNames)
            {
                File.Copy(fileName.Key, fileName.Value, true);
                finalList.Add(fileName.Value);
            }

            return finalList;
        }

        /// <summary>
        /// Deletes the specified document internaly from the disk
        /// </summary>
        /// <param name="doc"></param>
        public void DeleteDocument(ILuaEditDocument doc)
        {
            if (doc != null)
            {
                try
                {
                    bool isDir = (File.GetAttributes(doc.FileName) & FileAttributes.Directory) == FileAttributes.Directory;

                    if (CloseDocument(doc) && doc.ParentDocument != null)
                    {
                        doc.ParentDocument.RemoveDocument(doc);
                    }

                    if (isDir)
                    {
                        if (!Directory.Exists(doc.FileName))
                        {
                            throw new Exception("The source was probably renamed, moved or deleted directly on the disk.");
                        }

                        FileSystem.DeleteDirectory(doc.FileName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                    else
                    {
                        if (!File.Exists(doc.FileName) || !Path.IsPathRooted(doc.FileName))
                        {
                            throw new Exception("The source was probably renamed, moved or deleted directly on the disk.");
                        }
                        
                        FileSystem.DeleteFile(doc.FileName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    }
                }
                catch (Exception e)
                {
                    string msg = string.Format("An error occured while trying to delete '{0}': {1}", doc.FileName, e.Message);
                    FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Rename the specified document internaly and on the disk
        /// </summary>
        /// <param name="doc">The document to be renamed</param>
        /// <param name="newFileName">The new file name for the specified document</param>
        public void RenameDocument(ILuaEditDocument doc, string newFileName)
        {
            if (doc != null)
            {
                string oldFileName = doc.FileName;

                try
                {
                    bool isDir = (File.GetAttributes(doc.FileName) & FileAttributes.Directory) == FileAttributes.Directory;

                    if (string.IsNullOrEmpty(newFileName))
                    {
                        throw new Exception("Invalid destination file name!");
                    }

                    if (isDir)
                    {
                        if (!Directory.Exists(doc.FileName))
                        {
                            throw new Exception("The source was probably renamed, moved or deleted directly on the disk.");
                        }

                        if (Directory.Exists(newFileName))
                        {
                            throw new Exception("The destination already exists!");
                        }

                        Directory.Move(oldFileName, newFileName);
                        doc.FileName = newFileName;
                    }
                    else
                    {
                        if (!File.Exists(doc.FileName) || !Path.IsPathRooted(doc.FileName))
                        {
                            throw new Exception("The source was probably renamed, moved or deleted directly on the disk.");
                        }

                        if (File.Exists(newFileName) && Path.IsPathRooted(newFileName))
                        {
                            throw new Exception("The destination already exists!");
                        }

                        File.Move(oldFileName, newFileName);
                        doc.FileName = newFileName;
                    }

                    if (doc.ParentDocument != null)
                    {
                        doc.ParentDocument.IsModified = true;
                    }
                }
                catch (Exception e)
                {
                    string msg = string.Format("An error occured while trying to rename '{0}' to '{1}': {2}", oldFileName, newFileName, e.Message);
                    doc.FileName = oldFileName;
                    throw new Exception(msg);
                }
            }
        }

        /// <summary>
        /// Creates a new directory at the specified path using a default unique name.
        /// </summary>
        /// <param name="dirPath">The path at which to create the new directory.</param>
        /// <returns>A DirectoryInfo instance containing data about the newly created directory.</returns>
        public DirectoryInfo CreateDirectory(string dirPath)
        {
            return CreateDirectory(dirPath, null);
        }

        /// <summary>
        /// Creates a new directory at the specified path using the specified name.
        /// </summary>
        /// <param name="dirPath">The path at which to create the new directory.</param>
        /// <param name="newDirName">The new directory's name (if null or empty, using a default unique name)</param>
        /// <returns>A DirectoryInfo instance containing data about the newly created directory.</returns>
        public DirectoryInfo CreateDirectory(string dirPath, string newDirName)
        {
            try
            {
                string dirFullPath = null;

                if (string.IsNullOrEmpty(newDirName))
                {
                    int dirInc = 1;

                    do
                    {
                        newDirName = "NewFolder" + dirInc;
                        dirFullPath = Path.Combine(dirPath, newDirName);
                        ++dirInc;
                    } while (Directory.Exists(dirFullPath));
                }
                else
                {
                    dirFullPath = Path.Combine(dirPath, newDirName);
                }

                return Directory.CreateDirectory(dirFullPath);
            }
            catch (Exception e)
            {
                string msg = string.Format("An error occured while trying to create a new directory in '{0}': {1}", dirPath, e.Message);
                FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        /// <summary>
        /// Create a new document of type selected by the user using templates
        /// </summary>
        /// <param name="title">The title for the dialog to show</param>
        /// <param name="parentContext">The context in which the document must be created</param>
        /// <param name="newItemType">The type of new item to add</param>
        /// <returns>The created document</returns>
        public ILuaEditDocument NewDocument(string title, ILuaEditDocumentGroup parentContext, NewItemTypes newItemType)
        {
            try
            {
                NewItemDialogStyle dialogStyle = parentContext == null ? NewItemDialogStyle.ItemNoName : NewItemDialogStyle.Item;
                NewItem newItemDialog = new NewItem();

                if (newItemType == NewItemTypes.ItemGroup)
                {
                    if (parentContext == null)
                    {
                        dialogStyle = NewItemDialogStyle.ItemGroupWithSolutionOption;
                    }
                    else
                    {
                        dialogStyle = NewItemDialogStyle.ItemGroup;
                    }
                }

                if (newItemDialog.ShowDialog(title, parentContext, newItemType, dialogStyle) == DialogResult.OK)
                {
                    if (parentContext == null && newItemType == NewItemTypes.ItemGroup)
                    {
                        if (newItemDialog.SolutionOption == NewItemGroupSolutionOption.AddToCurrentSolution)
                        {
                            parentContext = this.CurrentSolution;
                        }
                        else if (newItemDialog.SolutionOption == NewItemGroupSolutionOption.CreateNewSolution)
                        {
                            string newSolutionPath = Path.ChangeExtension(newItemDialog.FullFileName, LuaSolutionDocument.Extension);
                            this.CurrentSolution = DocumentFactory.Instance.CreateDocument(new DocumentRef(newSolutionPath)) as LuaSolutionDocument;
                            this.CurrentSolution.ReferenceCount += this;

                            // Make sure user didn't cancel the operation (EG: said cancel to close current one)
                            if (this.CurrentSolution == null || this.CurrentSolution.FileName != newSolutionPath)
                            {
                                return null;
                            }

                            parentContext = this.CurrentSolution;
                        }
                    }

                    if (parentContext != null)
                    {
                        if (newItemType == NewItemTypes.Item)
                        {
                            string deployPath = Path.Combine(Path.GetDirectoryName(parentContext.FileName), newItemDialog.FileName);
                            if (TemplateManager.Instance.DeployTemplate(newItemDialog.SelectedTemplateDefinition, deployPath))
                            {
                                parentContext.AddDocument(OpenDocument(deployPath, false));
                            }
                        }
                        else if (newItemType == NewItemTypes.ItemGroup)
                        {
                            string deployPath = Path.Combine(newItemDialog.LocationPath, newItemDialog.FileName);
                            deployPath = Path.Combine(deployPath, newItemDialog.FileName) + newItemDialog.SelectedTemplateDefinition.AssociatedExtension;

                            if (!Directory.Exists(Path.GetDirectoryName(deployPath)))
                            {
                                DirectoryInfo di = Directory.CreateDirectory(Path.GetDirectoryName(deployPath));
                            }

                            if (parentContext != null && TemplateManager.Instance.DeployTemplate(newItemDialog.SelectedTemplateDefinition, deployPath))
                            {
                                parentContext.AddDocument(OpenProjectDocument(deployPath, false));
                            }

                            if (newItemDialog.SolutionOption == NewItemGroupSolutionOption.CreateNewSolution &&
                                parentContext is ILuaEditDocumentSolution)
                            {
                                parentContext.SaveDocument(parentContext.FileName);
                            }
                        }
                    }
                    else
                    {
                        if (newItemType == NewItemTypes.Item)
                        {
                            DocumentTemplateDef templateDef = newItemDialog.SelectedTemplateDefinition;
                            DocumentRef docRef = new DocumentRef(templateDef.SuggestiveName + templateDef.AssociatedExtension);
                            OpenDocument(DocumentFactory.Instance.CreateDocument(docRef));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                FrameworkManager.ShowMessageBox("An error occured while creating new item: " + e.Message,
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        /// <summary>
        /// Create a dummy solution and add the specified files to it
        /// </summary>
        /// <param name="childDocs">The list of child documents to add</param>
        /// <param name="childGroupDocs">The list of child document group to add</param>
        /// <returns>The dummy solution</returns>
        /// <remarks>used when no solution is opened but a file or a project is opened</remarks>
        private LuaSolutionDocument CreateDummySolution(Dictionary<string, ILuaEditDocument> childDocs,
                                                        Dictionary<string, ILuaEditDocumentGroup> childGroupDocs)
        {
            LuaSolutionDocument dummySolution = CreateDummySolution();

            if (childDocs != null)
            {
                foreach (ILuaEditDocument doc in childDocs.Values)
                    dummySolution.AddDocument(doc);
            }

            if (childGroupDocs != null)
            {
                foreach (ILuaEditDocumentGroup docGrp in childGroupDocs.Values)
                    dummySolution.AddDocument(docGrp);
            }

            dummySolution.ReferenceCount += this;

            return dummySolution;
        }

        /// <summary>
        /// Create an empty dummy solution
        /// </summary>
        /// <returns>The dummy solution</returns>
        /// <remarks>used when no solution is opened but a file or a project is opened</remarks>
        private LuaSolutionDocument CreateDummySolution()
        {
            LuaSolutionDocument dummySolution = new LuaSolutionDocument();
            dummySolution.FileName = "Solution1";

            return dummySolution;
        }

        /// <summary>
        /// Reload the specified document from the disk
        /// </summary>
        /// <param name="fileName">The document's file name to reload</param>
        public void ReloadDocument(string fileName)
        {
            if (_openedDocuments.ContainsKey(fileName.ToLower()))
                _openedDocuments[fileName.ToLower()].LoadDocument(fileName);
        }

        /// <summary>
        /// Open any supported project document (EG: Projects, Solutions, etc)
        /// </summary>
        /// <param name="addToRecentProjects">True if should be added to the recent projects list</param>
        /// <param name="contextDir">Directoy full path to initial suggestion to user</param>
        /// <returns>The list of opened file</returns>
        public Dictionary<string, ILuaEditDocumentGroup> OpenProjectDocument(bool addToRecentProjects, string contextDir)
        {
            Dictionary<string, ILuaEditDocumentGroup> newlyOpenedDocs = new Dictionary<string, ILuaEditDocumentGroup>();
            _openFileDlg.Multiselect = false;
            _openFileDlg.Title = "Open Project";
            _openFileDlg.Filter = DocumentFactory.Instance.GetDocumentFilterOfInheritedType(typeof(ILuaEditDocumentGroup), "All Project Files");

            if (contextDir != string.Empty)
            {
                _openFileDlg.InitialDirectory = contextDir;
            }

            if (_openFileDlg.ShowDialog() == DialogResult.OK)
            {
                ILuaEditDocumentGroup newDocGrp = BaseOpenDocument(new DocumentRef(_openFileDlg.FileName)) as ILuaEditDocumentGroup;

                if (newDocGrp != null)
                {
                    if (addToRecentProjects)
                        AddToRecentProjects(_openFileDlg.FileName);

                    newlyOpenedDocs.Add(_openFileDlg.FileName, newDocGrp);
                }

                if (_currentSolution == null)
                    CurrentSolution = CreateDummySolution(null, newlyOpenedDocs);

                newDocGrp.ParentDocument = CurrentSolution;
                newDocGrp.DocumentRef.ParentDocument = CurrentSolution;
            }

            return newlyOpenedDocs;
        }

        /// <summary>
        /// Open the specified project document (EG: Projects, Solutions, etc)
        /// </summary>
        /// <param name="fileName">The project's file name to be opened</param>
        /// <param name="addToRecentProjects">True if should be added to the recent projects list</param>
        /// <returns>The opened project document</returns>
        public ILuaEditDocumentGroup OpenProjectDocument(string fileName, bool addToRecentProjects)
        {
            Dictionary<string, ILuaEditDocumentGroup> newlyOpenedDocs = new Dictionary<string, ILuaEditDocumentGroup>();
            ILuaEditDocumentGroup newDocGrp = BaseOpenDocument(new DocumentRef(fileName)) as ILuaEditDocumentGroup;

            if (newDocGrp != null)
            {
                if (addToRecentProjects)
                {
                    this.AddToRecentProjects(fileName);
                }

                newlyOpenedDocs.Add(fileName, newDocGrp);

                if (_currentSolution == null)
                {
                    this.CurrentSolution = this.CreateDummySolution(null, newlyOpenedDocs);
                }

                if (newDocGrp != this.CurrentSolution)
                {
                    newDocGrp.ParentDocument = this.CurrentSolution;
                    newDocGrp.DocumentRef.ParentDocument = CurrentSolution;
                }
            }

            return newDocGrp;
        }

        /// <summary>
        /// Open projects using a parent context
        /// </summary>
        /// <param name="descriptiveName">Descriptive name to use for filter group</param>
        /// <param name="parentContext">The parent context in which to open the new document</param>
        /// <param name="title">The open dialog's title to display</param>
        /// <param name="contextDir">Directoy full path to initial suggestion to user</param>
        public void OpenProjectDocument(string descriptiveName, ILuaEditDocumentGroup parentContext, string title, string contextDir)
        {
            _openFileDlg.Multiselect = false;
            _openFileDlg.Title = title;
            _openFileDlg.Filter = DocumentFactory.Instance.GetDocumentFilterOfInheritedType(typeof(ILuaEditDocumentGroup), descriptiveName);

            if (contextDir != string.Empty)
            {
                _openFileDlg.InitialDirectory = contextDir;
            }

            if (_openFileDlg.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in _openFileDlg.FileNames)
                {
                    ILuaEditDocumentGroup newDocGrp = BaseOpenDocument(new DocumentRef(fileName, parentContext)) as ILuaEditDocumentGroup;

                    if (newDocGrp != null)
                        parentContext.AddDocument(newDocGrp);
                }
            }
        }

        /// <summary>
        /// Open documents using a parent context
        /// </summary>
        /// <param name="baseDocumentType">Class type used for the open dialog filter property</param>
        /// <param name="parentContext">The parent context in which to open the new document</param>
        /// <param name="title">The open dialog's title to display</param>
        /// <param name="contextDir">Directoy full path to initial suggestion to user</param>
        public void OpenDocuments(Type baseDocumentType, ILuaEditDocumentGroup parentContext, string title, string contextDir)
        {
            try
            {
                if (parentContext == null)
                {
                    throw new Exception("The parent context cannot be null!");
                }

                _openFileDlg.Title = title;
                _openFileDlg.Filter = DocumentFactory.Instance.GetDocumentFilterOfInheritedType(baseDocumentType);
                _openFileDlg.Multiselect = true;

                if (contextDir != string.Empty)
                {
                    _openFileDlg.InitialDirectory = contextDir;
                }

                if (_openFileDlg.ShowDialog() == DialogResult.OK)
                {
                    List<string> fileNames = new List<string>();
                    Dictionary<string, string> fileNamesToCopy = new Dictionary<string,string>();

                    foreach (string fileName in _openFileDlg.FileNames)
                    {
                        string newDocNewPath = Path.Combine(Path.GetDirectoryName(parentContext.FileName),
                                                                Path.GetFileName(fileName));

                        if (newDocNewPath != fileName)
                        {
                            fileNamesToCopy.Add(fileName, newDocNewPath);
                        }
                        else
                        {
                            fileNames.Add(fileName);
                        }
                    }

                    List<string> fileNamesCopied = null;

                    if (fileNamesToCopy.Count > 0)
                    {
                        fileNamesCopied = CopyFiles(fileNamesToCopy);
                    }

                    if (fileNamesCopied == null && fileNamesToCopy.Count > 0)
                    {
                        return;
                    }
                    else if (fileNamesCopied != null)
                    {
                        fileNames.AddRange(fileNamesCopied);
                    }

                    foreach (string fileName in fileNames)
                    {
                        ILuaEditDocument newDoc = BaseOpenDocument(new DocumentRef(fileName, parentContext));

                        if (newDoc != null)
                        {
                            parentContext.AddDocument(newDoc);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string msg = string.Format("Cannot open documents: {0}", e.Message);
                FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Open any supported files using an OpenFileDialog
        /// </summary>
        /// <param name="addToRecentFiles">True if should be added to the recent files list</param>
        /// <param name="contextDir">Directoy full path to initial suggestion to user</param>
        /// <returns>The list of opened file</returns>
        public Dictionary<string, ILuaEditDocument> OpenDocuments(bool addToRecentFiles, string contextDir)
        {
            Dictionary<string, ILuaEditDocument> newlyOpenedDocs = new Dictionary<string, ILuaEditDocument>();
            _openFileDlg.Multiselect = true;
            _openFileDlg.Title = "Open File";
            _openFileDlg.Filter = DocumentsFilterString + "|" + LuaEditDocumentsFilterString;

            if (contextDir != string.Empty)
            {
                _openFileDlg.InitialDirectory = contextDir;
            }

            if (_openFileDlg.ShowDialog() == DialogResult.OK)
            {
                if (_currentSolution == null)
                    CurrentSolution = CreateDummySolution();

                foreach (string fileName in _openFileDlg.FileNames)
                {
                    ILuaEditDocument newDoc = BaseOpenDocument(new DocumentRef(fileName));

                    if (newDoc != null)
                    {
                        if (!_openedDocuments.ContainsKey(fileName.ToLower()))
                        {
                            if (addToRecentFiles)
                                AddToRecentFiles(fileName);

                            _openedDocuments.Add(fileName.ToLower(), newDoc);
                        }

                        newlyOpenedDocs.Add(fileName, newDoc);
                    }

                    newDoc.ParentDocument = CurrentSolution;
                    newDoc.DocumentRef.ParentDocument = CurrentSolution;
                }

                if (DocumentsOpened != null)
                    DocumentsOpened(this, new DocumentsActionArgs(newlyOpenedDocs));
            }

            return newlyOpenedDocs;
        }

        /// <summary>
        /// Open the specified document
        /// </summary>
        /// <param name="fileName">The document to open</param>
        /// <returns>The opened document</returns>
        public ILuaEditDocument OpenDocument(ILuaEditDocument doc)
        {
            return OpenDocument(doc, null);
        }

        /// <summary>
        /// Open the specified document
        /// </summary>
        /// <param name="fileName">The document to open</param>
        /// <param name="options">Optional set of options to apply once the document is opened</param>
        /// <returns>The opened document</returns>
        public ILuaEditDocument OpenDocument(ILuaEditDocument doc, PostOpenDocumentOptions options)
        {
            doc = BaseOpenDocument(doc.DocumentRef);

            if (DocumentsOpened != null)
            {
                Dictionary<string, ILuaEditDocument> openedDocs = new Dictionary<string, ILuaEditDocument>();
                openedDocs.Add(doc.FileName, doc);
                DocumentsOpened(this, new DocumentsActionArgs(openedDocs));
            }

            ApplyPostDocumentOptions(doc, options);

            return doc;
        }

        /// <summary>
        /// Open the specified document ref
        /// </summary>
        /// <param name="docRef">The document ref to open</param>
        /// <param name="addToRecentFiles">True if the document ref to open must be added to recent files</param>
        /// <returns>The opened document</returns>
        public ILuaEditDocument OpenDocument(string fileName, bool addToRecentFiles)
        {
            return OpenDocument(fileName, addToRecentFiles, null);
        }

        /// <summary>
        /// Open the specified document ref
        /// </summary>
        /// <param name="docRef">The document ref to open</param>
        /// <param name="addToRecentFiles">True if the document ref to open must be added to recent files</param>
        /// <param name="options">Optional set of options to apply once the document is opened</param>
        /// <returns>The opened document</returns>
        public ILuaEditDocument OpenDocument(string fileName, bool addToRecentFiles, PostOpenDocumentOptions options)
        {
            return OpenDocument(new DocumentRef(fileName), addToRecentFiles, options);
        }

        /// <summary>
        /// Open the specified document
        /// </summary>
        /// <param name="docRef">The document ref to open</param>
        /// <param name="addToRecentFiles">True if should be added to the recent files list</param>
        /// <returns>The opened document</returns>
        public ILuaEditDocument OpenDocument(IDocumentRef docRef, bool addToRecentFiles)
        {
            return OpenDocument(docRef, addToRecentFiles, null);
        }

        /// <summary>
        /// Open the specified document
        /// </summary>
        /// <param name="docRef">The document ref to open</param>
        /// <param name="addToRecentFiles">True if should be added to the recent files list</param>
        /// <param name="options">Optional set of options to apply once the document is opened</param>
        /// <returns>The opened document</returns>
        public ILuaEditDocument OpenDocument(IDocumentRef docRef, bool addToRecentFiles, PostOpenDocumentOptions options)
        {
            ILuaEditDocument doc = BaseOpenDocument(docRef);

            if (doc != null)
            {
                if (_currentSolution == null)
                    CurrentSolution = CreateDummySolution();

                if (addToRecentFiles)
                    AddToRecentFiles(docRef.FileName);

                if (DocumentsOpened != null)
                {
                    Dictionary<string, ILuaEditDocument> openedDocs = new Dictionary<string, ILuaEditDocument>();
                    openedDocs.Add(docRef.FileName, doc);
                    DocumentsOpened(this, new DocumentsActionArgs(openedDocs));
                }

                ApplyPostDocumentOptions(doc, options);
            }

            return doc;
        }

        /// <summary>
        /// Open the specified document in the framework but will not trigger the DocumentsOpened event
        /// </summary>
        /// <param name="fileName">The document to open</param>
        /// <returns>The opened document</returns>
        public ILuaEditDocument BaseOpenDocument(IDocumentRef docRef)
        {
            if (!_openedDocuments.ContainsKey(docRef.FileName.ToLower()))
            {
                if (DocumentFactory.Instance.IsDocumentSupported(Path.GetExtension(docRef.FileName)))
                {
                    ILuaEditDocument newDoc = DocumentFactory.Instance.CreateDocument(docRef);
                    newDoc.LoadDocument(docRef.FileName);

                    if (newDoc is LuaSolutionDocument)
                    {
                        if (_currentSolution != null)
                        {
                            string msg = string.Format("The selected file '{0}' is a solution file. Do you want to close the current solution file and open this file as the solution?", docRef.FileName);

                            if (FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                if (CloseDocument(_currentSolution))
                                {
                                    newDoc = null;
                                }
                            }
                            else
                            {
                                newDoc = null;
                            }
                        }
                        else
                        {
                            CurrentSolution = newDoc as LuaSolutionDocument;
                        }
                    }

                    if (newDoc != null && !_openedDocuments.ContainsKey(newDoc.FileName.ToLower()))
                    {
                        newDoc.ReferenceCount += this;
                        newDoc.DocumentTerminated += OnDocumentTerminated;
                        _openedDocuments.Add(newDoc.FileName.ToLower(), newDoc);
                    }

                    return newDoc;
                }
            }
            else
            {
                ILuaEditDocument doc = _openedDocuments[docRef.FileName.ToLower()];
                doc.ReferenceCount += this;
                return doc;
            }

            return null;
        }

        /// <summary>
        /// Save all currently modified documents
        /// </summary>
        public void SaveAllDocuments()
        {
            List<ILuaEditDocument> docs = new List<ILuaEditDocument>();
            CurrentSolution.GetAllDocuments(docs);

            foreach (ILuaEditDocument doc in _openedDocuments.Values)
            {
                if (doc.IsModified)
                    SaveDocument(doc.DocumentType, doc, false, false);
            }

            FrameworkManager.Instance.RequestStatusMessage("Item(s) saved", SystemColors.Control, Color.Black);
        }

        /// <summary>
        /// Save the specified document
        /// </summary>
        /// <param name="fileName">Current file name to be saved</param>
        /// <param name="forceSaveAs">True if a save as dialog should be displayed</param>
        /// <returns>Return false if document wasn't saved. Otherwise true.</returns>
        public bool SaveDocument(Type documentType, ILuaEditDocument doc, bool forceSaveAs)
        {
            return SaveDocument(documentType, doc, forceSaveAs, true);
        }

        /// <summary>
        /// Save the specified document
        /// </summary>
        /// <param name="doc">The document to be saved</param>
        /// <param name="forceSaveAs">True if a save as dialog should be displayed</param>
        /// <param name="showStatusMessage">True if a status message should be displayed</param>
        /// <returns>Return false if document wasn't saved. Otherwise true.</returns>
        public bool SaveDocument(Type documentType, ILuaEditDocument doc, bool forceSaveAs, bool showStatusMessage)
        {
            bool needSave = true;
            string fileName = doc.FileName;
            string newFileName = fileName;

            // Display save as dialog if required
            if (forceSaveAs || !Path.IsPathRooted(doc.FileName) || !File.Exists(doc.FileName))
            {
                _saveFileDlg.Filter = DocumentFactory.Instance.GetDocumentFilter(documentType);

                if (_saveFileDlg.ShowDialog() == DialogResult.OK)
                    newFileName = _saveFileDlg.FileName;
                else
                    needSave = false;
            }

            // Check read-only status if required
            if (Path.IsPathRooted(newFileName) && File.Exists(newFileName) && needSave)
            {
                FileInfo fileInfo = new FileInfo(newFileName);

                if (fileInfo.IsReadOnly)
                {
                    FileIsReadOnlyDialog fileIsReadOnlyDialog = new FileIsReadOnlyDialog();
                    fileIsReadOnlyDialog.Owner = FrameworkManager.Instance.MainDialog;
                    FileIsReadOnlyDialogResult result = fileIsReadOnlyDialog.ShowDialog(Path.GetFileName(newFileName));

                    if (result == FileIsReadOnlyDialogResult.SaveAs)
                    {
                        SaveDocument(documentType, doc, true);
                        needSave = false;
                    }
                    else if (result == FileIsReadOnlyDialogResult.Overwrite)
                        doc.ReadOnly = false;
                    else
                        return false;
                }
            }

            // Save content to file if required
            if (needSave)
            {
                try
                {
                    doc.SaveDocument(newFileName);
                }
                catch (Exception e)
                {
                    doc.FileName = fileName;
                    string msg = string.Format("Could not save document '{0}' to '{1}': {2}", doc.FileName, newFileName, e.Message);
                    FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Update list of currently opened files
                if (fileName != newFileName && _openedDocuments.ContainsKey(fileName.ToLower()))
                {
                    _openedDocuments.Remove(fileName.ToLower());
                    _openedDocuments.Add(newFileName.ToLower(), doc);
                }

                if (showStatusMessage)
                {
                    FrameworkManager.Instance.RequestStatusMessage("Item(s) saved", SystemColors.Control, Color.Black);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Close all specified documents recursively
        /// </summary>
        /// <param name="doc">The document to close</param>
        /// <returns>False if cancelation has been requester by user</returns>
        public bool CloseDocument(ILuaEditDocument doc)
        {
            return CloseDocument(doc, true);
        }

        /// <summary>
        /// Close all specified documents recursively
        /// </summary>
        /// <param name="doc">The document to close</param>
        /// <param name="promptForSave">True if should prompt the user to save before closing.</param>
        /// <returns>False if cancelation has been requester by user</returns>
        public bool CloseDocument(ILuaEditDocument doc, bool promptForSave)
        {
            List<ILuaEditDocument> docs = new List<ILuaEditDocument>(1);
            docs.Add(doc);
            return CloseDocuments(docs, promptForSave);
        }

        /// <summary>
        /// Close all specified documents recursively
        /// </summary>
        /// <param name="docs">List of documents to close</param>
        /// <returns>False if cancelation has been requester by user</returns>
        public bool CloseDocuments(List<ILuaEditDocument> docs)
        {
            return CloseDocuments(docs, true);
        }

        /// <summary>
        /// Close all specified documents recursively
        /// </summary>
        /// <param name="docs">List of documents to close</param>
        /// <param name="promptForSave">True if should prompt the user to save before closing.</param>
        /// <returns>False if cancelation has been requester by user</returns>
        public bool CloseDocuments(List<ILuaEditDocument> docs, bool promptForSave)
        {
            CloseDialogResult result = null;
            DialogResult dlgResult = DialogResult.No;

            if (promptForSave)
            {
                CloseDialog closeDialog = new CloseDialog();
                result = closeDialog.ShowDialog(docs);
                dlgResult = result.Result;
            }

            if (dlgResult == DialogResult.Yes && result != null)
            {
                foreach (ILuaEditDocument doc in result.Selection)
                {
                    SaveDocument(doc.DocumentType, doc, (!File.Exists(doc.FileName) || !Path.IsPathRooted(doc.FileName)));
                }
            }
            else if (dlgResult == DialogResult.Cancel)
            {
                return false;
            }

            Dictionary<string, ILuaEditDocument> closedDocuments = new Dictionary<string, ILuaEditDocument>();
            foreach (ILuaEditDocument doc in docs)
            {
                doc.ReferenceCount -= this;
                closedDocuments.Add(doc.FileName, doc);
            }

            if (DocumentsClosed != null)
            {
                DocumentsClosed(this, new DocumentsActionArgs(closedDocuments));
            }

            return true;
        }

        #endregion
    }

    public class PostOpenDocumentOptions
    {
        #region Members

        public static int GotoLineDefaultValue = -1;
        public static Color HighLightLineColor = Color.Empty;

        private TextPoint _caretPos = null;
        private int _gotoLine = GotoLineDefaultValue;
        private Color _highlightLineColor = HighLightLineColor;
        private bool _lineMarked = false;

        #endregion

        #region Constructors

        public PostOpenDocumentOptions(int gotoLine)
        {
            _gotoLine = gotoLine;
        }

        public PostOpenDocumentOptions(TextPoint caretPos)
        {
            _caretPos = caretPos;
        }

        public PostOpenDocumentOptions(int gotoLine, Color highlightLineColor)
            : this(gotoLine)
        {
            _highlightLineColor = highlightLineColor;
        }

        public PostOpenDocumentOptions(int gotoLine, bool lineMarked)
            : this(gotoLine)
        {
            _lineMarked = lineMarked;
        }

        public PostOpenDocumentOptions(int gotoLine, Color highlightLineColor, bool lineMarked)
            : this(gotoLine, highlightLineColor)
        {
            _lineMarked = lineMarked;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The TextPoit at which to position the caret once
        /// the document is opened
        /// </summary>
        public TextPoint CaretPosition
        {
            get { return _caretPos; }
        }

        /// <summary>
        /// The line at which to go to once the document is opened
        /// </summary>
        public int GotoLine
        {
            get { return _gotoLine; }
        }

        /// <summary>
        /// The color to highlight the at which to go to
        /// </summary>
        public Color HighlightLineColor
        {
            get { return _highlightLineColor; }
        }

        /// <summary>
        /// The line marker to apply in the gutter at the line to go to
        /// </summary>
        public bool LineMarked
        {
            get { return _lineMarked; }
        }

        #endregion
    };

    /// <summary>
    /// Enum all kinds of open types
    /// </summary>
    public enum OpenTypes
    {
        File,
        ProjectSolution
    }

    public delegate void DocumentsActionHandler(object sender, DocumentsActionArgs e);
    public class DocumentsActionArgs : EventArgs
    {
        #region Members

        private Dictionary<string, ILuaEditDocument> _documents;

        #endregion

        #region Constructors

        public DocumentsActionArgs(Dictionary<string, ILuaEditDocument> docs)
            :
            base()
        {
            _documents = docs;
        }

        #endregion

        #region Properties

        public Dictionary<string, ILuaEditDocument> Documents
        {
            get { return _documents; }
        }

        #endregion
    }
}
