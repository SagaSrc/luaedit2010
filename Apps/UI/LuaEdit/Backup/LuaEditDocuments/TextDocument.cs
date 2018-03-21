using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Interfaces;

namespace LuaEdit.Documents
{
    public partial class TextDocument : EditableDocumentBase
    {
        #region Members

        public const string Extension = ".txt";
        public const string DescriptiveName = "Text Document";

        #endregion

        #region Constructors

        public TextDocument()
            : base()
        {
        }

        public TextDocument(IDocumentRef docRef)
            : base(docRef)
        {
        }

        #endregion

        #region Properties

        public override Icon DocumentIcon
        {
            get { return global::LuaEdit.Documents.Properties.Resources.TextDocument_16x16_ico; }
        }

        public override Type DocumentType
        {
            get { return typeof(TextDocument); }
        }

        #endregion
    }
}