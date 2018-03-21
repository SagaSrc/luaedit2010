using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using DotNetLib.Controls;
using LuaEdit.Interfaces;
using LuaEdit.Win32;

namespace LuaEdit.Documents
{
    public class DocumentFolder : DocumentGroupBase, ILuaEditDocumentFolder
    {
        #region Members

        protected DirectoryInfo _dirInfo = null;

        public static int OpenedSmallImageIndex;
        public static int ClosedSmallImageIndex;

        #endregion

        #region Constructors

        static DocumentFolder()
        {
            Bitmap openedImage = global::LuaEdit.Documents.Properties.Resources.OpenedFolder;
            Bitmap closedImage = global::LuaEdit.Documents.Properties.Resources.ClosedFolder;
            OpenedSmallImageIndex = DocumentFactory.Instance.DocumentSmallImages.Images.Add(openedImage, openedImage.GetPixel(0,0));
            ClosedSmallImageIndex = DocumentFactory.Instance.DocumentSmallImages.Images.Add(closedImage, closedImage.GetPixel(0, 0));
        }

        public DocumentFolder() :
            base(false)
        {
            Initialize();
        }

        public DocumentFolder(DirectoryInfo dirInfo) :
            base()
        {
            _dirInfo = dirInfo;
            base.FileName = dirInfo.FullName;
        }

        #endregion

        #region Properties

        public override bool IsModified
        {
            get { return ParentDocument.IsModified; }
            set { ParentDocument.IsModified = value; }
        }

        public override string FileName
        {
            get { return base.FileName; }
            set
            {
                if (value != base.FileName)
                {
                    if (value[value.Length - 1] != '\\')
                    {
                        value = value + '\\';
                    }

                    base.FileName = value;
                    _dirInfo = new DirectoryInfo(base.FileName);
                }
            }
        }

        public DirectoryInfo DirectoryInfo
        {
            get { return _dirInfo; }
        }

        #endregion

        #region Methods

        private void Initialize()
        {
        }

        public override string ToString()
        {
            return _dirInfo.Name;
        }

        public override void ToTreeListViewItem(TreeListViewItem parentItem)
        {
            TreeListViewItem node = new TreeListViewItem(this, ClosedSmallImageIndex,
                                                         ClosedSmallImageIndex, OpenedSmallImageIndex);
            node.ToolTip = this.FileName;
            node.Tag = this;

            parentItem.Items.Add(node);
            base.ToTreeListViewItem(node);
        }

        internal XmlDocumentFolder ToXml(XmlDocumentFolder objectToSerialize)
        {
            if (objectToSerialize == null)
            {
                objectToSerialize = new XmlDocumentFolder();
            }

            objectToSerialize.RelativePath = Win32Utils.GetRelativePath(this.FileName, Path.GetDirectoryName(ParentDocument.FileName));

            base.ToXml(objectToSerialize);

            return objectToSerialize;
        }

        internal void FromXml(XmlDocumentFolder objectToDeserialize)
        {
            try
            {
                _opening = true;
                this.FileName = Win32Utils.GetAbsolutePath(objectToDeserialize.RelativePath,
                                                           ParentDocument.FileName);

                base.FromXml(objectToDeserialize);
            }
            finally
            {
                _opening = false;
            }
        }

        #endregion
    }

    public class XmlDocumentFolder : XmlLuaGroupDocumentBase
    {
        #region Members

        public string RelativePath;

        #endregion

        #region Constructors

        public XmlDocumentFolder() :
            base()
        {
        }

        #endregion
    }
}
