using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using LuaEdit.Win32;

namespace LuaEdit.HelperDialogs
{
    public partial class AttachToMachineDialog : Form
    {
        #region Members

        private const int RecentMachinesMax = 15;

        #endregion

        #region Constructors

        public AttachToMachineDialog()
        {
            InitializeComponent();
            LoadRecentMachines();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the specified machine
        /// </summary>
        public string MachineName
        {
            get { return cboMachine.Text; }
        }

        /// <summary>
        /// Gets the IP of the specified machine
        /// </summary>
        public IPAddress MachineIP
        {
            get
            {
                IPAddress machineIp;

                if (IPAddress.TryParse(this.MachineName, out machineIp))
                {
                    return machineIp;
                }
                else
                {
                    IPHostEntry ihe = Dns.GetHostEntry(this.MachineName);

                    if (ihe.AddressList.Length > 0)
                    {
                        return ihe.AddressList[0];
                    }
                }

                return IPAddress.None;
            }
        }

        /// <summary>
        /// Gets the port at which to connect on the specified machine
        /// </summary>
        public int Port
        {
            get { return Convert.ToInt32(nudPort.Value); }
        }

        #endregion

        #region Events

        private void btnBrowseMachine_Click(object sender, EventArgs e)
        {
            IntPtr pidlRet = IntPtr.Zero;
            IntPtr pidlRoot = IntPtr.Zero;
            Shell32.SHGetSpecialFolderLocation(this.Handle, (int)Shell32.FolderID.NetworkNeighborhood, out pidlRoot);

            try
            {
                Shell32.BROWSEINFO bi = new Shell32.BROWSEINFO();
                IntPtr buffer = Marshal.AllocHGlobal(Win32Utils.MAX_PATH);

                bi.hwndOwner = this.Handle;
                bi.pidlRoot = pidlRoot;
                bi.lpszTitle = "Browse for Machine";
                bi.pszDisplayName = buffer;
                bi.ulFlags = (int)(Shell32.BrowseInfoFlags.BIF_BROWSEFORCOMPUTER |
                                   Shell32.BrowseInfoFlags.BIF_NONEWFOLDERBUTTON);
                pidlRet = Shell32.SHBrowseForFolder(ref bi);

                if (pidlRet != IntPtr.Zero)
                {
                    string machineName = Marshal.PtrToStringAuto(bi.pszDisplayName).ToUpper();
                    cboMachine.Text = machineName;
                    InsertRecentMachine(machineName);
                }

                // Free the buffer we've allocated on the global heap
                Marshal.FreeHGlobal(buffer);
            }
            finally
            {
                Shell32.IMalloc malloc;
                Shell32.SHGetMalloc(out malloc);
                malloc.Free(pidlRoot);

                if (pidlRet != IntPtr.Zero)
                {
                    malloc.Free(pidlRet);
                }
            }
        }

        private void cboMachine_TextChanged(object sender, EventArgs e)
        {
            ValidateAttachButton();
        }

        private void nudPort_ValueChanged(object sender, EventArgs e)
        {
            ValidateAttachButton();
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            SaveRecentMachines();
        }

        #endregion

        #region Methods

        private void ValidateAttachButton()
        {
            btnAttach.Enabled = !string.IsNullOrEmpty(cboMachine.Text) && nudPort.Value > 0;
        }

        private void InsertRecentMachine(string machineName)
        {
            int machineIndex = cboMachine.Items.IndexOf(machineName);

            if (machineIndex >= 0)
            {
                cboMachine.Items.RemoveAt(machineIndex);
            }

            cboMachine.Items.Insert(0, machineName);

            if (cboMachine.Items.Count > RecentMachinesMax)
            {
                cboMachine.Items.RemoveAt(cboMachine.Items.Count - 1);
            }
        }

        private void LoadRecentMachines()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(FrameworkManager.ApplicationRegistryKeyName);

            try
            {
                if (regKey != null && regKey.ValueCount > 0)
                {
                    // Manage recent machines list
                    regKey = Registry.CurrentUser.OpenSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentMachines");
                    if (regKey != null && regKey.ValueCount > 0)
                    {
                        Dictionary<int, string> indexedList = new Dictionary<int, string>();

                        foreach (string valueName in regKey.GetValueNames())
                        {
                            if (!string.IsNullOrEmpty(valueName))
                            {
                                int index = Convert.ToInt32(regKey.GetValue(valueName));
                                indexedList.Add(index, valueName);
                            }
                        }

                        for (int x = 0; x < indexedList.Count; ++x)
                        {
                            cboMachine.Items.Insert(x, indexedList[x]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string msg = string.Format("An error occured while loading recent machines list: {0}", e.Message);
                MessageBox.Show(msg, "LuaEdit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (regKey != null)
                {
                    regKey.Close();
                }
            }
        }

        private void SaveRecentMachines()
        {
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(FrameworkManager.ApplicationRegistryKeyName, true);

            try
            {
                // Save recent files
                Registry.CurrentUser.DeleteSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentMachines", false);
                regKey = Registry.CurrentUser.CreateSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentMachines");
                if (regKey == null)
                {
                    regKey = Registry.CurrentUser.CreateSubKey(FrameworkManager.ApplicationRegistryKeyName + @"\RecentMachines");
                }

                for (int x = 0; x < cboMachine.Items.Count; ++x)
                {
                    regKey.SetValue(cboMachine.Items[x].ToString(), x);
                }

                regKey.Flush();
            }
            catch (Exception e)
            {
                string msg = string.Format("An error occured while saving recent machines list: {0}", e.Message);
                MessageBox.Show(msg, "LuaEdit", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (regKey != null)
                {
                    regKey.Close();
                }
            }
        }

        #endregion
    }
}