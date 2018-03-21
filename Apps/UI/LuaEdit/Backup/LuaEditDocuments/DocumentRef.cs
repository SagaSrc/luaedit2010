using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using DotNetLib.Controls;
using LuaEdit.Interfaces;
using LuaEdit.Documents.DocumentUtils;

namespace LuaEdit.Documents
{
    public class DocumentRef : IDocumentRef
    {
        #region Members

        private string _fileName = string.Empty;
        private ILuaEditDocumentGroup _parentDoc = null;

        #endregion

        #region Constructors

        public DocumentRef()
        {
        }

        public DocumentRef(string fileName)
            : this()
        {
            _fileName = fileName;
        }

        public DocumentRef(string fileName, ILuaEditDocumentGroup parentDocument)
            : this(fileName)
        {
            _parentDoc = parentDocument;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The file name this document reference is representing
        /// </summary>
        [XmlAttribute("FileName")]
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// The owner (parent) document to that document
        /// </summary>
        [XmlIgnore]
        public ILuaEditDocumentGroup ParentDocument
        {
            get { return _parentDoc; }
            set { _parentDoc = value; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Path.GetFileName(_fileName);
        }

        #endregion
    }
}
