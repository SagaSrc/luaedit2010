using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Interfaces;

namespace LuaEdit
{
    public class DocumentFactory
    {
        #region Members

        private static readonly DocumentFactory _documentFactory;
        private Dictionary<string, DocumentTypeDescriptor> _registeredDocumentTypesByExt; // Key: extension, Value: DocumentTypeDescriptor instance
        private Dictionary<Type, DocumentTypeDescriptor> _registeredDocumentTypesByType; // Key: class type, Value: DocumentTypeDescriptor instance
        private ImageList _documentSmallImages;
        private ImageList _documentLargeImages;
        private ImageList _documentStateImages;

        #endregion

        #region Constructors

        static DocumentFactory()
        {
            _documentFactory = new DocumentFactory();
        }

        private DocumentFactory()
        {
            ////////////////////////////////////////////////////////////////////////////////////////////
            // Initialize document register dictionnaries
            ////////////////////////////////////////////////////////////////////////////////////////////

            _registeredDocumentTypesByExt = new Dictionary<string, DocumentTypeDescriptor>();
            _registeredDocumentTypesByType = new Dictionary<Type, DocumentTypeDescriptor>();

            ////////////////////////////////////////////////////////////////////////////////////////////
            // Initialize document image lists
            ////////////////////////////////////////////////////////////////////////////////////////////

            _documentSmallImages = new ImageList();
            _documentSmallImages.ImageSize = new Size(16, 16);

            _documentLargeImages = new ImageList();
            _documentLargeImages.ImageSize = new Size(32, 32);

            _documentStateImages = new ImageList();
            _documentStateImages.ImageSize = new Size(16, 16);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The only instance of DocumentFactory
        /// </summary>
        public static DocumentFactory Instance
        {
            get { return _documentFactory; }
        }

        /// <summary>
        /// Get the list of document type descriptor indexed by their associated extension
        /// </summary>
        public Dictionary<string, DocumentTypeDescriptor> RegisteredDocumentTypesByExtensions
        {
            get { return _registeredDocumentTypesByExt; }
        }

        /// <summary>
        /// Get the list of document type descriptor indexed by their associated type
        /// </summary>
        public Dictionary<Type, DocumentTypeDescriptor> RegisteredDocumentTypesByTypes
        {
            get { return _registeredDocumentTypesByType; }
        }

        /// <summary>
        /// Get the global image list of document small images
        /// </summary>
        public ImageList DocumentSmallImages
        {
            get { return _documentSmallImages; }
        }

        /// <summary>
        /// Get the global image list of document large images
        /// </summary>
        public ImageList DocumentLargeImages
        {
            get { return _documentLargeImages; }
        }

        /// <summary>
        /// Get the global state image list of document state images
        /// </summary>
        public ImageList DocumentStateImages
        {
            get { return _documentStateImages; }
        }

        #endregion

        #region Methods

        private ILuaEditDocument CreateDocumentByDocumentRef(IDocumentRef docRef)
        {
            DocumentTypeDescriptor type = _registeredDocumentTypesByExt[Path.GetExtension(docRef.FileName)];

            if (type != null)
            {
                return Activator.CreateInstance(type.DocumentType, new object[] { docRef }) as ILuaEditDocument;
            }

            return null;
        }

        /// <summary>
        /// Return true if the specified extension is supported. Otherwise false.
        /// </summary>
        /// <param name="docRef">The document ref on which to perform the test</param>
        /// <returns>True if the specified document ref is supported. Otherwise false.</returns>
        public bool IsDocumentSupported(IDocumentRef docRef)
        {
            return IsDocumentSupported(Path.GetExtension(docRef.FileName));
        }

        /// <summary>
        /// Return true if the specified extension is supported. Otherwise false.
        /// </summary>
        /// <param name="extension">The extension on which to perform the test</param>
        /// <returns>True if the specified extension is supported. Otherwise false.</returns>
        public bool IsDocumentSupported(string extension)
        {
            return _registeredDocumentTypesByExt.ContainsKey(extension);
        }

        /// <summary>
        /// Return true if the specified class type is supported. Otherwise false.
        /// </summary>
        /// <param name="classType">The class type on which to perform the test</param>
        /// <returns>True if the specified class type is supported. Otherwise false.</returns>
        public bool IsDocumentSupported(Type classType)
        {
            return _registeredDocumentTypesByType.ContainsKey(classType);
        }

        /// <summary>
        /// Create a new Document using the specified file name.
        /// </summary>
        /// <param name="docRef">The file name to the file to use during creation</param>
        /// <returns>A new instance of SyntaxDocument</returns>
        public ILuaEditDocument CreateDocument(IDocumentRef docRef)
        {
            return CreateDocumentByDocumentRef(docRef);
        }

        /// <summary>
        /// Register a new document type to the document factory
        /// </summary>
        /// <param name="documentType">The type of document to register</param>
        /// <param name="extension">The associated extension to associate with the provided type</param>
        /// <param name="descriptiveName">The associated descriptive name to associate with the provided type</param>
        /// <returns>The create document type descriptor</returns>
        public DocumentTypeDescriptor RegisterDocumentType(Type documentType, string extension, string descriptiveName)
        {
            DocumentTypeDescriptor newDocumentType = new DocumentTypeDescriptor(documentType, extension, descriptiveName);
            if (extension != string.Empty)
                _registeredDocumentTypesByExt.Add(extension, newDocumentType);
            _registeredDocumentTypesByType.Add(documentType, newDocumentType);
            return newDocumentType;
        }

        /// <summary>
        /// Register a new document type to the document factory
        /// </summary>
        /// <param name="documentType">The type of document to register</param>
        /// <param name="extension">The associated extension to associate with the provided type</param>
        /// <param name="descriptiveName">The associated descriptive name to associate with the provided type</param>
        /// <param name="smallImage">The small image associated with that type of document</param>
        /// <param name="largeImage">The large image associated with that type of document</param>
        /// <param name="stateImage">The state image associated with that type of document</param>
        /// <returns>The create document type descriptor</returns>
        public DocumentTypeDescriptor RegisterDocumentType(Type documentType, string extension, string descriptiveName,
                                                           Bitmap smallImage, Bitmap largeImage, Bitmap stateImage)
        {
            int smallImageIndex = _documentSmallImages.Images.Add(smallImage, smallImage.GetPixel(0, 0));
            int largeImageIndex = _documentLargeImages.Images.Add(largeImage, largeImage.GetPixel(0,0));
            int stateImageIndex = _documentSmallImages.Images.Add(stateImage, stateImage.GetPixel(0, 0));

            DocumentTypeDescriptor newDocumentType = new DocumentTypeDescriptor(documentType, extension,
                                                                                descriptiveName, smallImageIndex,
                                                                                largeImageIndex, stateImageIndex);
            if (!string.IsNullOrEmpty(extension))
            {
                _registeredDocumentTypesByExt.Add(extension, newDocumentType);
            }

            _registeredDocumentTypesByType.Add(documentType, newDocumentType);

            return newDocumentType;
        }

        /// <summary>
        /// Get the document type using a file name
        /// </summary>
        /// <param name="fileName">The file name from which to retreive the document type</param>
        /// <returns>The document type associated to the provided extension</returns>
        public Type GetDocumentTypeByFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            if (_registeredDocumentTypesByExt.ContainsKey(fileExtension))
                return _registeredDocumentTypesByExt[fileExtension].DocumentType;

            return null;
        }

        /// <summary>
        /// Get the large image index associated with the specified file name
        /// </summary>
        /// <param name="fileName">The file name from which to retreive the large image index</param>
        /// <returns>The large image index associated with the specified file name (to use with DocumentLargeImages property)</returns>
        public int GetDocumentLargeImageIndex(string fileName)
        {
            return GetDocumentLargeImageIndex(GetDocumentTypeByFileName(fileName));
        }

        /// <summary>
        /// Get the small image index associated with the specified file name
        /// </summary>
        /// <param name="classType">The class type from which to retreive the small image index</param>
        /// <returns>The small image index associated with the specified file name (to use with DocumentLargeImages property)</returns>
        public int GetDocumentLargeImageIndex(Type classType)
        {
            if (_registeredDocumentTypesByType.ContainsKey(classType))
                return _registeredDocumentTypesByType[classType].LargeImageIndex;

            return -1;
        }

        /// <summary>
        /// Get the small image index associated with the specified file name
        /// </summary>
        /// <param name="fileName">The file name from which to retreive the small image index</param>
        /// <returns>The small image index associated with the specified file name (to use with DocumentSmallImages property)</returns>
        public int GetDocumentSmallImageIndex(string fileName)
        {
            return GetDocumentSmallImageIndex(GetDocumentTypeByFileName(fileName));
        }

        /// <summary>
        /// Get the small image index associated with the specified file name
        /// </summary>
        /// <param name="classType">The class type from which to retreive the small image index</param>
        /// <returns>The small image index associated with the specified file name (to use with DocumentSmallImages property)</returns>
        public int GetDocumentSmallImageIndex(Type classType)
        {
            if (_registeredDocumentTypesByType.ContainsKey(classType))
                return _registeredDocumentTypesByType[classType].SmallImageIndex;

            return -1;
        }

        /// <summary>
        /// Get the state image index associated with the specified file name
        /// </summary>
        /// <param name="fileName">The file name from which to retreive the state image index</param>
        /// <returns>The state image index associated with the specified file name (to use with DocumentImages property)</returns>
        public int GetDocumentStateImageIndex(string fileName)
        {
            return GetDocumentStateImageIndex(GetDocumentTypeByFileName(fileName));
        }

        /// <summary>
        /// Get the state image index associated with the specified file name
        /// </summary>
        /// <param name="fileName">The file name from which to retreive the state image index</param>
        /// <returns>The state image index associated with the specified file name (to use with DocumentImages property)</returns>
        public int GetDocumentStateImageIndex(Type classType)
        {
            if (_registeredDocumentTypesByType.ContainsKey(classType))
                return _registeredDocumentTypesByType[classType].StateImageIndex;

            return -1;
        }

        /// <summary>
        /// Get the filter string (used especially with browse dialogs) according to the specified underlying types
        /// </summary>
        /// <param name="inheritedType">Type from which the valid types must inherits to be part of the filter string</param>
        /// <returns>The filter string</returns>
        public string GetDocumentFilterOfInheritedType(Type inheritedType)
        {
            return GetDocumentFilterOfInheritedType(inheritedType, string.Empty);
        }

        /// <summary>
        /// Get the filter string (used especially with browse dialogs) according to the specified underlying types
        /// </summary>
        /// <param name="inheritedType">Type from which the valid types must inherits to be part of the filter string</param>
        /// <param name="descriptiveName">The descriptive name to use for the filter</param>
        /// <returns>The filter string</returns>
        public string GetDocumentFilterOfInheritedType(Type inheritedType, string descriptiveName)
        {
            List<Type> matchingTypes = new List<Type>();

            foreach (Type docType in _registeredDocumentTypesByType.Keys)
            {
                if (inheritedType.IsAssignableFrom(docType) && _registeredDocumentTypesByType[docType].Extension != string.Empty)
                    matchingTypes.Add(docType);
            }

            return descriptiveName == string.Empty ? GetDocumentFilter(matchingTypes) : GetDocumentFilter(matchingTypes, descriptiveName);;
        }

        /// <summary>
        /// Gets the filter for each type of registered document
        /// </summary>
        /// <returns>The filer string</returns>
        public string GetDocumentsFilter()
        {
            int count = 0;
            StringBuilder sb = new StringBuilder();

            foreach (DocumentTypeDescriptor docTypeDesc in _registeredDocumentTypesByType.Values)
            {
                if (count != 0)
                {
                    sb.Append("|");
                }

                sb.Append(docTypeDesc.DescriptiveName);
                sb.Append(" (*");
                sb.Append(docTypeDesc.Extension);
                sb.Append(")|*");
                sb.Append(docTypeDesc.Extension);
                ++count;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get the filter string (used especially with browse dialogs) according to the specified types
        /// </summary>
        /// <param name="documentTypes">The types to use to build the filter string</param>
        /// <param name="descriptiveName">The descriptive name to use for the filter</param>
        /// <returns>The filter string</returns>
        public string GetDocumentFilter(List<Type> documentTypes, string descriptiveName)
        {
            string descriptiveFilter = descriptiveName + " (";
            string filterString = string.Empty;

            foreach (Type type in documentTypes)
            {
                if (_registeredDocumentTypesByType.ContainsKey(type))
                {
                    DocumentTypeDescriptor docTypeDesc = _registeredDocumentTypesByType[type];

                    if (docTypeDesc.Extension != string.Empty)
                    {
                        if (descriptiveFilter != string.Empty)
                            descriptiveFilter += " ";

                        descriptiveFilter += "*" + docTypeDesc.Extension + ";";
                        filterString += "*" + docTypeDesc.Extension + ";";
                    }
                }
            }

            // Remove extra useless ';' character
            descriptiveFilter = descriptiveFilter.Substring(0, descriptiveFilter.Length - 1);
            filterString = filterString.Substring(0, filterString.Length - 1);

            // Concatenate both actual filters and descriptive name
            filterString = descriptiveFilter += ")|" + filterString;

            return filterString;
        }

        /// <summary>
        /// Get the filter string (used especially with browse dialogs) according to the specified types
        /// </summary>
        /// <param name="documentTypes">The types to use to build the filter string</param>
        /// <returns>The filter string</returns>
        public string GetDocumentFilter(List<Type> documentTypes)
        {
            string filterString = string.Empty;

            foreach (Type type in documentTypes)
            {
                if (filterString != string.Empty)
                    filterString += "|";

                if (_registeredDocumentTypesByType.ContainsKey(type))
                    filterString += _registeredDocumentTypesByType[type].Filter;
            }

            return filterString;
        }

        /// <summary>
        /// Get the filter string (used especially with browse dialogs) according to the specified type
        /// </summary>
        /// <param name="documentTypes">The type to use to build the filter string</param>
        /// <returns>The filter string</returns>
        public string GetDocumentFilter(Type documentType)
        {
            if (_registeredDocumentTypesByType.ContainsKey(documentType))
                return _registeredDocumentTypesByType[documentType].Filter;

            return string.Empty;
        }

        #endregion
    }

    public class DocumentTypeDescriptor
    {
        #region Members

        private Type _documentType = null;
        private string _extension = string.Empty;
        private string _descriptiveName = string.Empty;
        private int _smallImageIndex = -1;
        private int _largeImageIndex = -1;
        private int _stateImageIndex = -1;

        #endregion

        #region Constructors

        public DocumentTypeDescriptor(Type documentType, string extension, string descriptiveName)
        {
            _documentType = documentType;
            _extension = extension;
            _descriptiveName = descriptiveName;
        }

        public DocumentTypeDescriptor(Type documentType, string extension, string descriptiveName, int smallImageIndex,
                                      int largeImageIndex, int stateImageIndex)
            : this(documentType, extension, descriptiveName)
        {
            _smallImageIndex = smallImageIndex;
            _largeImageIndex = largeImageIndex;
            _stateImageIndex = stateImageIndex;
        }

        public DocumentTypeDescriptor(Type documentType, string extension, string descriptiveName, int smallImageIndex, int largeImageIndex)
            : this(documentType, extension, descriptiveName, smallImageIndex, largeImageIndex, -1)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The type of document associated to the extension
        /// </summary>
        public Type DocumentType
        {
            get { return _documentType; }
        }

        /// <summary>
        /// The extension associated to the document type
        /// </summary>
        public string Extension
        {
            get { return _extension; }
        }

        /// <summary>
        /// The descriptive name used to build the filter string for this type of document
        /// </summary>
        public string DescriptiveName
        {
            get { return _descriptiveName; }
        }

        /// <summary>
        /// The filter associated to this type of document
        /// </summary>
        public string Filter
        {
            get
            {
                return string.Format("{0} Files (*{1})|*{1}", _descriptiveName, _extension);
            }
        }

        /// <summary>
        /// The small image index in the DocumentFactory's documents small image list
        /// </summary>
        public int SmallImageIndex
        {
            get { return _smallImageIndex; }
        }

        /// <summary>
        /// The large image index in the DocumentFactory's documents large image list
        /// </summary>
        public int LargeImageIndex
        {
            get { return _largeImageIndex; }
        }

        /// <summary>
        /// The state image index in the DocumentFactory's documents state image list
        /// </summary>
        public int StateImageIndex
        {
            get { return _stateImageIndex; }
        }

        #endregion
    }
}
