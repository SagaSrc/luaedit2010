using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;

using LuaEdit.Win32;

namespace LuaEdit.Controls
{
    public class SystemIconManager
    {
        #region Members

        /// <summary>
        /// The only instance of SystemIconManager
        /// </summary>
        private static readonly SystemIconManager _systemIconManager = null;

        private const int SmallSystemImageWidth = 16;
        private const int SmallSystemImageHeight = 16;

        private ImageList _systemImages = null;

        #endregion

        #region Constructors

        static SystemIconManager()
        {
            _systemIconManager = new SystemIconManager();
        }

        private SystemIconManager()
        {
            _systemImages = new ImageList();
            _systemImages.ImageSize = new Size(SmallSystemImageWidth, SmallSystemImageHeight);
            _systemImages.ColorDepth = ColorDepth.Depth32Bit;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the only instance of SystemIconManager
        /// </summary>
        public static SystemIconManager Instance
        {
            get { return _systemIconManager; }
        }

        /// <summary>
        /// Gets the list of currently retrieved system images
        /// </summary>
        public ImageList SystemImages
        {
            get { return _systemImages; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the system image associated to the specified file name
        /// </summary>
        /// <param name="fileName">The file name from which to retrieve the associated system image.</param>
        /// <returns>The image associated to the specified file name or null if file name is invalid.</returns>
        public Image GetFileSystemImage(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (File.Exists(fileName))
                {
                    string fileExtension = Path.GetExtension(fileName);

                    if (!string.IsNullOrEmpty(fileExtension))
                    {
                        if (_systemImages.Images.ContainsKey(fileExtension))
                        {
                            return _systemImages.Images[fileExtension];
                        }
                        else
                        {
                            uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES |
                                         Shell32.SHGFI_SMALLICON;

                            AddSystemImage(fileName, fileExtension, Shell32.FILE_ATTRIBUTE_NORMAL, flags);

                            if (_systemImages.Images.ContainsKey(fileExtension))
                            {
                                return _systemImages.Images[fileExtension];
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get the system image associated to the specified path
        /// </summary>
        /// <param name="path">The path from which to retrieve the associated system image.</param>
        /// <returns>The image associated to the specified path or null if path is invalid.</returns>
        public Image GetDirectorySystemImage(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (_systemImages.Images.ContainsKey(path))
                {
                    return _systemImages.Images[path];
                }
                else
                {
                    uint flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES |
                                 Shell32.SHGFI_SMALLICON;

                    AddSystemImage(path, path, Shell32.FILE_ATTRIBUTE_DIRECTORY, flags);

                    if (_systemImages.Images.ContainsKey(path))
                    {
                        return _systemImages.Images[path];
                    }
                }
            }

            return null;
        }

        private void AddSystemImage(string path, string key, uint fileAttribute, uint flags)
        {
            Shell32.SHFILEINFO shfi = new Shell32.SHFILEINFO();
            IntPtr retVal = Shell32.SHGetFileInfo(path, fileAttribute, ref shfi, (uint)Marshal.SizeOf(shfi), flags);

            if (retVal != IntPtr.Zero)
            {
                _systemImages.Images.Add(key, (Icon)Icon.FromHandle(shfi.hIcon).Clone());
                User32.DestroyIcon(shfi.hIcon);
            }
        }

        #endregion
    }
}
