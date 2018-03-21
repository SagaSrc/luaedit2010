using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using LuaEdit.Win32;

namespace LuaEdit
{
    public class DocumentTemplateDef
    {
        #region Members

        private string _name = string.Empty;
        private string _type = string.Empty;
        private string _description = string.Empty;
        private string _associatedExtension = string.Empty;
        private string _suggestiveName = string.Empty;
        private string _definitionPath = string.Empty;
        private string _templatePath = string.Empty;
        private TemplateContentItem[] _templateContent = null;

        #endregion

        #region Constructors

        public DocumentTemplateDef()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name associated to the template
        /// </summary>
        public string TemplateName
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The type under which to classify the template
        /// </summary>
        public string TemplateType
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// The description to the template
        /// </summary>
        public string TemplateDescription
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// The extension associated to the template
        /// </summary>
        [XmlAttribute("AssociatedExtension")]
        public string AssociatedExtension
        {
            get { return _associatedExtension; }
            set { _associatedExtension = value; }
        }

        /// <summary>
        /// The extension associated to the template
        /// </summary>
        [XmlAttribute("SuggestiveName")]
        public string SuggestiveName
        {
            get { return _suggestiveName; }
            set { _suggestiveName = value; }
        }

        /// <summary>
        /// The full path to the actual template file (must be a zip file)
        /// </summary>
        public string TemplateFileName
        {
            get { return Win32Utils.GetAbsolutePath(_templatePath, _definitionPath); }
            set { _templatePath = value; }
        }

        /// <summary>
        /// The template definition file name
        /// </summary>
        [XmlIgnore]
        public string DefinitionFileName
        {
            get { return _definitionPath; }
            set { _definitionPath = value; }
        }

        /// <summary>
        /// The content of the template
        /// </summary>
        public TemplateContentItem[] TemplateContent
        {
            get { return _templateContent; }
            set { _templateContent = value; }
        }

        /// <summary>
        /// Get the content item that needs to be renamed
        /// </summary>
        [XmlIgnore]
        public TemplateContentItem ContentItemToRename
        {
            get
            {
                if (_templateContent != null)
                {
                    foreach (TemplateContentItem item in _templateContent)
                    {
                        if (item.NeedRename)
                            return item;
                    }
                }

                return null;
            }
        }

        #endregion
    }

    public class TemplateContentItem
    {
        #region Members

        [XmlAttribute("NeedRename")]
        public bool NeedRename = false;
        [XmlText]
        public string TemplateContentItemName;

        #endregion
    }
}
