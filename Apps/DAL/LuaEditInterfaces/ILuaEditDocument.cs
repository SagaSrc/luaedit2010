using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Utils;
using DotNetLib.Controls;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Interfaces
{
    public interface ILuaEditDocument : IDisposable, IDocumentNode
    {
        #region Members

        event EventHandler FileNameChanged;
        event EventHandler DocumentTerminated;
        //event EventHandler ContentChanged;
        event EventHandler ModifiedChanged;
        event EventHandler DiskFileChanged;
        event EventHandler ReadOnlyChanged;

        #endregion

        #region Properties

        bool IsTerminated
        {
            get;
        }

        ReferenceCounter ReferenceCount
        {
            get;
            set;
        }

        string FileName
        {
            get;
            set;
        }

        bool IsModified
        {
            get;
            set;
        }

        bool ReadOnly
        {
            get;
            set;
        }

        DockContent DockContent
        {
            get;
        }

        Type DocumentType
        {
            get;
        }

        ILuaEditDocumentGroup ParentDocument
        {
            get;
            set;
        }

        IDocumentRef DocumentRef
        {
            get;
        }

        #endregion

        #region Methods

        void SaveDocument(string fileName);
        void LoadDocument(string fileName);

        #endregion
    }
}
