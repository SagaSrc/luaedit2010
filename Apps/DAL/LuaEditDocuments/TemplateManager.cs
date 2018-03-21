using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

using LuaEdit;
using LuaEdit.HelperDialogs;
using ICSharpCode.SharpZipLib.Zip;

namespace LuaEdit.Managers
{
    public class TemplateManager
    {
        #region Members

        public static string TemplateDefinitionsPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Templates");

        private static readonly TemplateManager _templateManager;
        private List<DocumentTemplateDef> _documentTemplates;

        #endregion

        #region Constructors

        static TemplateManager()
        {
            _templateManager = new TemplateManager();
        }

        private TemplateManager()
        {
            _documentTemplates = new List<DocumentTemplateDef>();
            LoadTemplates();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The only instance of TemplateManager
        /// </summary>
        public static TemplateManager Instance
        {
            get { return _templateManager; }
        }

        /// <summary>
        /// Get the list of document templates
        /// </summary>
        public List<DocumentTemplateDef> DocumentTemplates
        {
            get { return _documentTemplates; }
        }

        #endregion

        #region Methods

        private void LoadTemplates()
        {
            foreach (string fileName in Directory.GetFiles(TemplateDefinitionsPath, "*.letemplate"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(DocumentTemplateDef));

                using (XmlTextReader xmlReader = new XmlTextReader(fileName))
                {
                    DocumentTemplateDef templateDef = xmlSerializer.Deserialize(xmlReader) as DocumentTemplateDef;
                    templateDef.DefinitionFileName = fileName;
                    _documentTemplates.Add(templateDef);
                }
            }
        }

        /// <summary>
        /// Deploy the specified template using the specified file name
        /// </summary>
        /// <param name="templateDef">The template definition to be deployed</param>
        /// <param name="deployFileName">The file name used during deploy</param>
        /// <returns>True if the template was actually deployed. Otherwise false</returns>
        public bool DeployTemplate(DocumentTemplateDef templateDef, string deployFileName)
        {
            bool isDeployed = true;
            string deployPath = string.IsNullOrEmpty(deployFileName) ? null : Path.GetDirectoryName(deployFileName);

            if (NewItemTypes.Item == (NewItemTypes)Enum.Parse(typeof(NewItemTypes), templateDef.TemplateType))
            {
                if (!string.IsNullOrEmpty(deployPath))
                {
                    DialogResult result = DialogResult.Yes;
                    string entryOutFileName = deployFileName;

                    if (!Directory.Exists(deployPath))
                    {
                        Directory.CreateDirectory(deployPath);
                    }

                    if (File.Exists(entryOutFileName))
                    {
                        string msg = string.Format("The file '{0}' already exists. Do you want to replace it?", entryOutFileName);
                        result = MessageBox.Show(msg, "LuaEdit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    }

                    if (result == DialogResult.Yes)
                    {
                        FileStream fs = File.Create(entryOutFileName);
                        fs.Close();
                    }
                    else
                    {
                        isDeployed = false;
                    }
                }
            }
            else if (NewItemTypes.ItemGroup == (NewItemTypes)Enum.Parse(typeof(NewItemTypes), templateDef.TemplateType) &&
                     templateDef.TemplateContent != null && templateDef.ContentItemToRename != null)
            {
                string contentToRename = templateDef.ContentItemToRename.TemplateContentItemName;
                ZipFile zipFile = new ZipFile(templateDef.TemplateFileName);

                foreach (ZipEntry zipEntry in zipFile)
                {
                    Stream zipEntryStream = zipFile.GetInputStream(zipEntry);
                    DialogResult result = DialogResult.Yes;
                    string outFileName = (contentToRename == zipEntry.Name) ? Path.GetFileName(deployFileName) : zipEntry.Name;
                    string entryOutFileName = Path.Combine(deployPath, outFileName);

                    if (File.Exists(entryOutFileName))
                    {
                        string msg = string.Format("The file '{0}' already exists. Do you want to replace it?", entryOutFileName);
                        result = MessageBox.Show(msg, "LuaEdit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    }

                    if (result == DialogResult.Yes)
                    {
                        using (FileStream fileStream = new FileStream(entryOutFileName, FileMode.OpenOrCreate))
                        {
                            byte[] data = new byte[] { };
                            Array.Resize<byte>(ref data, (int)zipEntry.Size);
                            zipEntryStream.Read(data, 0, (int)zipEntry.Size);

                            fileStream.Write(data, 0, data.Length);
                            fileStream.Flush();
                        }
                    }
                    else
                    {
                        isDeployed = false;
                        break;
                    }
                }
            }

            return isDeployed;
        }

        #endregion
    }
}
