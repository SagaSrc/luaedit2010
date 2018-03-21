using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit
{
    public partial class AboutScreen : Form
    {
        #region Members

        [DllImport("rdbg.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetCurrentLuaVersion();

        #endregion

        #region Constructors

        public AboutScreen()
        {
            InitializeComponent();

            object[] assemblyAttribs = Assembly.GetExecutingAssembly().GetCustomAttributes(false);

            if (assemblyAttribs != null && assemblyAttribs.Length > 0)
            {
                foreach (object assemblyAttr in assemblyAttribs)
                {
                    if (assemblyAttr is AssemblyTitleAttribute)
                    {
                        lblAssemblyTitle.Text = (assemblyAttr as AssemblyTitleAttribute).Title;
                    }
                    else if (assemblyAttr is AssemblyCopyrightAttribute)
                    {
                        lblAssemblyCopyright.Text = (assemblyAttr as AssemblyCopyrightAttribute).Copyright;
                    }
                    else if (assemblyAttr is AssemblyFileVersionAttribute)
                    {
                        IntPtr version = GetCurrentLuaVersion();
                        lblAssemblyVersion.Text = string.Format("Version {0} ({1})", (assemblyAttr as AssemblyFileVersionAttribute).Version, Marshal.PtrToStringAnsi(version));
                    }
                }
            }
        }

        #endregion

        #region Event Handlers

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}