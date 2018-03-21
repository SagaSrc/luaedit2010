using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Fireball.Syntax;
using Fireball.Windows.Forms;
using Fireball.Windows.Forms.CodeEditor;

using DotNetLib.Controls;
using LuaEdit.Interfaces;
using LuaEdit.Managers;
using LuaEdit.Documents.DocumentUtils;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Documents
{
    public class EditableDocumentBase : DocumentBase, ILuaEditDocumentEditable
    {
        #region Members

        protected SyntaxDocument _doc = null;
        protected EditableDocumentBaseUI _documentUI = null;

        #endregion

        #region Constructors

        public EditableDocumentBase() :
            base()
        {
            Initialize();
        }

        public EditableDocumentBase(IDocumentRef docRef)
            : base(docRef)
        {
            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the syntax descriptor file name for this document
        /// </summary>
        public virtual string SyntaxFileName
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the icon associated with this document
        /// </summary>
        public virtual Icon DocumentIcon
        {
            get { return null; }
        }

        /// <summary>
        /// Get the type of document
        /// </summary>
        public override Type DocumentType
        {
            get { return typeof(EditableDocumentBase); }
        }

        /// <summary>
        /// Get the current document instance
        /// </summary>
        public SyntaxDocument Document
        {
            get { return _doc; }
        }

        public virtual ILuaEditDocumentEditableUI DocumentUI
        {
            get
            {
                // Lazy instantiation
                if (_documentUI == null || _documentUI.IsDisposed)
                {
                    _documentUI = new EditableDocumentBaseUI(this, _doc);
                    _documentUI.Icon = this.DocumentIcon;
                }

                return _documentUI;
            }
        }

        public override DockContent DockContent
        {
            get { return this.DocumentUI as DockContent; }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Occurs when the document modified status has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDocumentModifiedChanged(object sender, EventArgs e)
        {
            base.IsModified = _doc.Modified;
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            // Create new syntax document
            SetDocument(new SyntaxDocument());
        }

        protected override bool TerminateDocument()
        {
            try
            {
                _terminating = true;

                if (base.TerminateDocument())
                {
                    _documentUI.TerminateUI();
                    _documentUI.Close();
                    return true;
                }
            }
            finally
            {
                _terminating = false;
            }

            return false;
        }

        public void SetDocument(SyntaxDocument doc)
        {
            _doc = doc;

            if (!string.IsNullOrEmpty(SyntaxFileName))
            {
                _doc.SyntaxFile = SyntaxFileName;
            }

            _doc.ModifiedChanged += OnDocumentModifiedChanged;
        }

        public override void SaveDocument(string fileName)
        {
            _fileWatcher.EnableRaisingEvents = false;

            using (StreamWriter fileStream = new StreamWriter(fileName))
            {
                fileStream.Write(_doc.Text);
            }

            base.SaveDocument(fileName);
            _doc.Modified = false;
            _fileWatcher.EnableRaisingEvents = true;
        }

        public override void LoadDocument(string fileName)
        {
            try
            {
                _opening = true;

                if (File.Exists(fileName))
                {
                    using (StreamReader fileStream = new StreamReader(fileName))
                    {
                        _doc.Text = fileStream.ReadToEnd();
                    }
                }

                base.LoadDocument(fileName);
            }
            finally
            {
                _opening = false;
            }
        }

        #endregion
    }
}
