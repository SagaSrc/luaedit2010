using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Fireball.Syntax;

using LuaEdit.Interfaces;
using LuaEdit.Win32;

namespace LuaEdit
{
    public class FrameworkManager : IDisposable
    {
        #region Members

        private static readonly FrameworkManager _frameworkManager;
        private delegate void FlashMainDialogDelegate();
        private delegate DialogResult ShowMessageBoxDelegate(string msg, MessageBoxButtons buttons, MessageBoxIcon icon);
        private Form _mainDialog;

        private bool _mainDialogWasMaxed;
        private int _mainDialogWidth;
        private int _mainDialogHeight;

        public event StatusMessageResquestHandler StatusMessageRequested;

        #endregion

        #region Contructors

        static FrameworkManager()
        {
            _frameworkManager = new FrameworkManager();
        }

        private FrameworkManager()
        {
            // Initialize framework settings
            LoadFrameworkSettings();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get the only instance of FrameworkManager
        /// </summary>
        public static FrameworkManager Instance
        {
            get { return _frameworkManager; }
        }

        /// <summary>
        /// Get the application data path for this application
        /// </summary>
        public static string ApplicationDataPath
        {
            get
            {
                string userAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string appData = Path.Combine(userAppData, Application.ProductName);

                if (!Directory.Exists(appData))
                    Directory.CreateDirectory(appData);

                return appData;
            }
        }

        public static string ApplicationRegistryKeyName
        {
            get { return @"Software\LuaEdit"; }
        }

        /// <summary>
        /// Get the software's main dialog
        /// </summary>
        public Form MainDialog
        {
            get { return _mainDialog; }
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            SaveFrameworkSettings();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load the framework settings
        /// </summary>
        private void LoadFrameworkSettings()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(ApplicationRegistryKeyName);

            try
            {
                if (regKey != null && regKey.ValueCount > 0)
                {
                    _mainDialogWasMaxed = Convert.ToBoolean(regKey.GetValue("WasMaxed", false));
                    _mainDialogWidth = Convert.ToInt32(regKey.GetValue("Width", 800));
                    _mainDialogHeight = Convert.ToInt32(regKey.GetValue("Height", 600));
                }
                else
                {
                    _mainDialogWasMaxed = false;
                    _mainDialogWidth = 800;
                    _mainDialogHeight = 600;
                }
            }
            catch (Exception e)
            {
                string msg = string.Format("An error occured while loading framework settings: {0}\n\n{1}", e.Message, e.StackTrace);
                FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (regKey != null)
                    regKey.Close();
            }
        }

        /// <summary>
        /// Load the framework settings
        /// </summary>
        private void SaveFrameworkSettings()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(ApplicationRegistryKeyName, true);

            try
            {
                if (regKey == null)
                {
                    regKey = Registry.CurrentUser.CreateSubKey(ApplicationRegistryKeyName);
                }

                if (regKey != null && _mainDialog != null)
                {
                    regKey.SetValue("WasMaxed", _mainDialog.WindowState == FormWindowState.Maximized);
                    regKey.SetValue("Width", _mainDialog.Width);
                    regKey.SetValue("Height", _mainDialog.Height);
                    regKey.Flush();
                }
            }
            catch (Exception e)
            {
                string msg = string.Format("An error occured while saving framework settings: {0}\n\n{1}", e.Message, e.StackTrace);
                FrameworkManager.ShowMessageBox(msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (regKey != null)
                    regKey.Close();
            }
        }

        /// <summary>
        /// Request to display the specified status message using the provided back and fore colors
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="backColor">The color to use for the background</param>
        /// <param name="foreColor">The color to use for the font</param>
        public void RequestStatusMessage(string message, Color backColor, Color foreColor)
        {
            if (StatusMessageRequested != null)
            {
                StatusMessageRequested(this, new StatusMessageResquestArgs(message, backColor, foreColor));
            }
        }

        /// <summary>
        /// Register the specified dialog as the main dialog of the program
        /// </summary>
        /// <param name="mainDialog">The dialog to be registered as the main dialog</param>
        public void RegisterMainDialog(Form mainDialog)
        {
            _mainDialog = mainDialog;
            _mainDialog.WindowState = _mainDialogWasMaxed ? FormWindowState.Maximized : FormWindowState.Normal;

            if (!_mainDialogWasMaxed)
            {
                _mainDialog.Width = _mainDialogWidth;
                _mainDialog.Height = _mainDialogHeight;
            }
        }

        /// <summary>
        /// Makes the main dialog flash in the taskbar
        /// </summary>
        public void FlashMainDialog()
        {
            if (_mainDialog.InvokeRequired)
            {
                _mainDialog.BeginInvoke(new FlashMainDialogDelegate(FlashMainDialog));
                return;
            }

            if (_mainDialog != null && (!_mainDialog.Focused))
            {
                User32.FLASHWINFO fInfo = new User32.FLASHWINFO();
                fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
                fInfo.hwnd = _mainDialog.Handle;
                fInfo.dwFlags = User32.FLASHW_ALL | User32.FLASHW_TIMERNOFG; //stopFlashing ? User32.FLASHW_STOP | User32.FLASHW_ALL : User32.FLASHW_ALL;
                fInfo.uCount = 100;
                fInfo.dwTimeout = 0;
                User32.FlashWindowEx(ref fInfo);
            }
        }

        /// <summary>
        /// Shows a message box standardized to the application using the main dialog as the owner
        /// </summary>
        /// <param name="msg">The message to display</param>
        /// <param name="buttons">The buttons to use</param>
        /// <param name="icon">The icon to use</param>
        /// <returns>The resulted DialogResult</returns>
        public static DialogResult ShowMessageBox(string msg, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (Instance.MainDialog != null)
            {
                if (Instance.MainDialog.InvokeRequired)
                {
                    return (DialogResult)Instance.MainDialog.Invoke(new ShowMessageBoxDelegate(ShowMessageBox),
                                                                    new object[] { msg, buttons, icon });
                }

                return MessageBox.Show(Instance.MainDialog, msg, "LuaEdit", buttons, icon);
            }

            return DialogResult.None;
        }

        /// <summary>
        /// Get a new unique item file name for the parent context
        /// </summary>
        /// <param name="parentContext">The parent in which ensuring the file name is unique</param>
        /// <param name="baseName">The base file name used for generated a new one</param>
        /// <param name="extension">The file name extension to use</param>
        /// <returns>The unique new item file name</returns>
        public string GetItemNewFileName(ILuaEditDocumentGroup parentContext, string baseName, string extension)
        {
            bool isNameValid = false;
            string newName = string.Empty;
            int counter = 0;

            while (!isNameValid)
            {
                ++counter;
                isNameValid = true;
                newName = baseName + counter + extension;

                if (parentContext != null)
                {
                    foreach (ILuaEditDocument doc in parentContext.Documents)
                    {
                        if (Path.GetFileName(doc.FileName).ToLower() == newName.ToLower())
                        {
                            isNameValid = false;
                            break;
                        }
                    }
                }
            }

            return newName;
        }

        #endregion
    }

    public delegate void StatusMessageResquestHandler(object sender, StatusMessageResquestArgs e);
    public class StatusMessageResquestArgs : EventArgs
    {
        #region Members

        private string _message = string.Empty;
        private Color _backColor = Color.Empty;
        private Color _foreColor = Color.Empty;

        #endregion

        #region Constructors

        public StatusMessageResquestArgs(string message, Color backColor, Color foreColor) :
            base()
        {
            _message = message;
            _backColor = backColor;
            _foreColor = foreColor;
        }

        #endregion

        #region Properties

        public string Message
        {
            get { return _message; }
        }

        public Color BackColor
        {
            get { return _backColor; }
        }

        public Color ForeColor
        {
            get { return _foreColor; }
        }

        #endregion
    }
}
