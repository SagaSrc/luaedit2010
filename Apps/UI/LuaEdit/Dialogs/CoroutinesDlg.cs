using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using DotNetLib.Controls;
using LuaEdit.Documents;
using LuaEdit.Documents.DocumentUtils;
using LuaEdit.Interfaces;
using LuaEdit.LuaEditDebugger;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Forms
{
    public partial class CoroutinesDlg : DockContent
    {
        #region Constructors

        public CoroutinesDlg()
        {
            InitializeComponent();

            ClientDebugManager.Instance.ThreadsChanged += OnThreadChanged;
            ClientDebugManager.Instance.DebuggingStopped += OnDebuggingStopped;
            ClientDebugManager.Instance.BreakChanged += OnBreakChanged;
        }

        #endregion

        #region Event Handlers

        private void CoroutinesDlg_Shown(object sender, EventArgs e)
        {
            UpdateThreads();
        }

        private void OnDebuggingStopped(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(OnDebuggingStopped), new object[] { sender, e });
                return;
            }

            ClearThreads();
        }

        private void OnBreakChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(OnBreakChanged), new object[] { sender, e });
                return;
            }

            if (!ClientDebugManager.Instance.IsBreaked)
            {
                foreach (TreeListViewItem tlvi in tlvwThreads.Items)
                {
                    tlvi.ForeColor = SystemColors.Control;
                    tlvi.ImageIndex = -1;
                    tlvi.SelectedImageIndex = -1;
                }
            }
        }

        private void OnThreadChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(OnThreadChanged), new object[] { sender, e });
                return;
            }

            UpdateThreads();
        }

        #endregion

        #region Methods

        public void ClearThreads()
        {
            tlvwThreads.Items.Clear();
        }

        private void UpdateThreads()
        {
            tlvwThreads.BeginUpdate();

            try
            {
                ClearThreads();

                if (ClientDebugManager.Instance.LuaThreads != null)
                {
                    foreach (LuaThread luaThread in ClientDebugManager.Instance.LuaThreads)
                    {
                        LuaCall call = luaThread.CallStackTopCall;
                        TreeListViewItem tlvi = new TreeListViewItem();
                        tlvi.Tag = luaThread;
                        tlvi.SubItems.Add(new TreeListViewSubItem(0));

                        TreeListViewSubItem tlvsiID = new TreeListViewSubItem(1, luaThread.ThreadID);
                        tlvi.SubItems.Add(tlvsiID);

                        if (call != null)
                        {
                            if (call.IsLineCall)
                            {
                                ILuaEditDocument doc = DocumentsManager.Instance.OpenDocument(call.FileName, false);
                                LuaScriptDocument luaDoc = doc as LuaScriptDocument;

                                if (luaDoc != null)
                                {
                                    string location = string.Format("{0}   (Lua)   Line {1}", luaDoc.Document.Lines[call.FunctionLineCall - 1].TrimStart(new char[] { ' ', '\t' }), call.FunctionLineCall);
                                    TreeListViewSubItem tlvsiLocation = new TreeListViewSubItem(2, location);
                                    tlvi.SubItems.Add(tlvsiLocation);
                                }
                            }
                            else
                            {
                                string location = string.Format("{0}   ({1})   Line {2}", call.FunctionName, call.FunctionSource, call.LineCalled);
                                TreeListViewSubItem tlvsiLocation = new TreeListViewSubItem(2, location);
                                tlvi.SubItems.Add(tlvsiLocation);
                            }
                        }

                        if (luaThread.ThreadID == ClientDebugManager.Instance.CurrentThreadID)
                        {
                            tlvi.ImageIndex = 0;
                            tlvi.SelectedImageIndex = 0;
                        }
                        else
                        {
                            tlvi.ImageIndex = -1;
                            tlvi.SelectedImageIndex = -1;
                        }

                        tlvwThreads.Items.Add(tlvi);
                    }
                }
            }
            finally
            {
                tlvwThreads.EndUpdate();
            }
        }

        #endregion
    }
}