using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using DotNetLib.Controls;
using Fireball.Syntax;
using LuaEdit.Interfaces;
using LuaEdit.Documents.DocumentUtils;

namespace LuaEdit.Documents
{
    public partial class LuaScriptDocument : EditableDocumentBase
    {
        #region Members

        public const string Extension = ".lua";
        public const string DescriptiveName = "Lua Script";

        #endregion

        #region Constructors

        public LuaScriptDocument()
            : base()
        {
        }

        public LuaScriptDocument(IDocumentRef docRef)
            : base(docRef)
        {
        }

        #endregion

        #region Properties

        public override Icon DocumentIcon
        {
            get { return global::LuaEdit.Documents.Properties.Resources.LuaScript_16x16_ico; }
        }

        /// <summary>
        /// Gets the syntax descriptor file name for this document
        /// </summary>
        public override string SyntaxFileName
        {
            get { return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "SyntaxFiles\\Lua.syn"); }
        }

        /// <summary>
        /// Get the type of document
        /// </summary>
        public override Type DocumentType
        {
            get { return typeof(LuaScriptDocument); }
        }

        #endregion
    }
}