using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Documents;
using LuaEdit.Documents.DocumentUtils;
using LuaEdit.Interfaces;
using LuaEdit.LuaEditDebugger;
using LuaEdit.LuaEditDebugger.DebugCommands;
using LuaEdit.Managers;

using DotNetLib.Controls;
using LuaInterface;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Forms
{
    internal delegate void UpdateCallStackDelegate();

    public partial class CallStackDlg : DockContent
    {
        #region Members

        private int _currentStackLevel = 0;

        #endregion

        #region Constructors

        public CallStackDlg()
        {
            InitializeComponent();

            ClientDebugManager.Instance.CallStackChanged += OnCallStackChanged;
            ClientDebugManager.Instance.DebuggingStopped += OnDebuggingStopped;
            ClientDebugManager.Instance.BreakChanged += OnBreakChanged;

            UpdateCallStack();
        }

        #endregion

        #region Properties

        public int CurrentStackLevel
        {
            get { return _currentStackLevel; }
        }

        #endregion

        #region Events

        private void OnCallStackChanged(object sender, EventArgs e)
        {
            UpdateCallStack();
        }

        private void OnDebuggingStopped(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(OnDebuggingStopped), new object[] { sender, e });
                return;
            }

            ClearCallStack();
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
                ClearCallStack();
            }
        }

        #endregion

        #region Methods

        private void ClearCallStack()
        {
            tlvwCallStack.Items.Clear();
        }

        private void UpdateCallStack()
        {
            if (tlvwCallStack.InvokeRequired)
            {
                tlvwCallStack.Invoke(new UpdateCallStackDelegate(UpdateCallStack));
                return;
            }

            ClearCallStack();

            if (ClientDebugManager.Instance.CallStack != null)
            {
                AddFunctionCallsToItem(tlvwCallStack.RootItem, ClientDebugManager.Instance.CallStack);
                ResetCallStackImages();
            }
        }

        private void AddFunctionCallsToItem(TreeListViewItem item, LuaCall[] callStack)
        {
            int currentDepth = 1;
            int callStackDepth = callStack.Length;

            foreach (LuaCall fctCall in callStack)
            {
                if ((ClientDebugManager.Instance.IsInError && currentDepth != callStackDepth - 1) ||
                    !ClientDebugManager.Instance.IsInError)
                {
                    TreeListViewItem tlvi = FunctionCallToTreeListViewItem(fctCall);
                    item.Items.Insert(0, tlvi);
                }

                ++currentDepth;
            }
        }

        private TreeListViewItem FunctionCallToTreeListViewItem(LuaCall call)
        {
            // Create main item with name
            TreeListViewItem tlvi = new TreeListViewItem(string.Empty);
            tlvi.Tag = call;
            tlvi.ForeColor = call.FunctionSource != "Lua" ? Color.LightGray : Color.Black;

            // Create subitem with name
            TreeListViewSubItem tlvsi = new TreeListViewSubItem(0);

            if (call.IsLineCall)
            {
                ILuaEditDocument doc = DocumentsManager.Instance.OpenDocument(call.FileName, false);

                if (doc is LuaScriptDocument)
                {
                    LuaScriptDocument luaDoc = doc as LuaScriptDocument;

                    if (call.FunctionLineCall < luaDoc.Document.Lines.Length)
                    {
                        tlvsi.Object = string.Format("{0}  Line {1}", luaDoc.Document.Lines[call.FunctionLineCall - 1].TrimStart(new char[] { ' ', '\t' }), call.FunctionLineCall);
                    }
                }
            }
            else
            {
                if (call.FunctionName == "[EntryPoint]")
                {
                    tlvsi.Object = call.FunctionName;
                }
                else
                {
                    tlvsi.Object = string.Format("{0}({1})  Line {2}", call.FunctionName, call.ParamString, call.FunctionLineCall);
                }
            }

            tlvi.SubItems.Add(tlvsi);

            // Create subitem with language
            tlvi.SubItems.Add(new TreeListViewSubItem(1, call.FunctionSource));

            // Create subitem with filename
            tlvi.SubItems.Add(new TreeListViewSubItem(2, call.FileName));

            return tlvi;
        }

        private void ResetCallStackImages()
        {
            for (int x = 0; x < tlvwCallStack.Items.Count; ++x)
            {
                TreeListViewItem tlvi = tlvwCallStack.Items[x];
                LuaCall call = (LuaCall)tlvi.Tag;

                if (!call.Equals(null))
                {
                    if (x == 0)
                    {
                        Breakpoint bp = BreakpointManager.Instance.GetBreakpointAtLine(call.FileName, call.FunctionLineCall);

                        if (bp != null)
                        {
                            tlvi.ImageIndex = bp.Enabled ? 1 : 2;
                            tlvi.SelectedImageIndex = bp.Enabled ? 1 : 2;
                        }
                        else
                        {
                            tlvi.ImageIndex = 0;
                            tlvi.SelectedImageIndex = 0;
                        }
                    }
                    else if (x == _currentStackLevel)
                    {
                        tlvi.ImageIndex = 3;
                        tlvi.SelectedImageIndex = 3;
                    }
                    else
                    {
                        tlvi.ImageIndex = -1;
                        tlvi.SelectedImageIndex = -1;
                    }
                }
            }
        }

        #endregion
    }
}