using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LuaEdit.LuaEditDebugger;
using LuaEdit.LuaEditDebugger.DebugCommands;

using DotNetLib.Controls;
using LuaInterface;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Forms
{
    internal delegate void UpdateLocalsDelegate();
    internal delegate void TableDetailsUpdatedDelegate(object sender, TableDetailsUpdatedEventArgs e);

    public partial class LocalsDlg : DockContent
    {
        #region Members

        private const string EmptyTableElementName = "";
        private Dictionary<string, TreeListViewItem> _nodesByFullName;
        private LuaVariable _searchingVar = null;

        #endregion

        #region Constructors

        public LocalsDlg()
        {
            InitializeComponent();

            _nodesByFullName = new Dictionary<string, TreeListViewItem>();

            ClientDebugManager.Instance.DebuggingStopped += OnDebuggingStopped;
            ClientDebugManager.Instance.LocalsChanged += OnLocalsChanged;
            ClientDebugManager.Instance.BreakChanged += OnBreakChanged;
            ClientDebugManager.Instance.TableDetailsUpdated += OnTableDetailsUpdated;
        }

        #endregion

        #region Events

        private void LocalsDlg_Shown(object sender, EventArgs e)
        {
            if (ClientDebugManager.Instance.Locals.Count > 0)
            {
                OnLocalsChanged(this, EventArgs.Empty);
            }
        }

        private void OnDebuggingStopped(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DebuggingStoppedEventHandler(OnDebuggingStopped), new object[] { sender, e });
                return;
            }

            ClearLocals();
        }

        private void OnLocalsChanged(object sender, EventArgs e)
        {
            UpdateLocals();
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
                foreach (TreeListViewItem tlvi in tlvwLocals.Items)
                {
                    tlvi.ForeColor = SystemColors.Control;
                }
            }
        }

        private void OnTableDetailsUpdated(object sender, TableDetailsUpdatedEventArgs e)
        {
            if (tlvwLocals.InvokeRequired)
            {
                tlvwLocals.BeginInvoke(new TableDetailsUpdatedDelegate(OnTableDetailsUpdated), new object[] { sender, e });
                return;
            }

            if (e.IsLocal && _nodesByFullName.ContainsKey(e.TableNameIn))
            {
                UpdateVariables(_nodesByFullName[e.TableNameIn], e.SubValues);
            }
        }

        private void tlvwLocals_ItemBeforeEdit(object sender, TreeListViewBeforeEditEventArgs e)
        {
            if (ClientDebugManager.Instance.IsBreaked &&
                e.SubItem != null && (e.Item != null && e.Item.Tag is LuaVariable))
            {
                LuaVariable editedVar = e.Item.Tag as LuaVariable;
                TextBox editor = new TextBox();
                editor.BorderStyle = BorderStyle.None;
                e.Editor = editor;
                e.DefaultValue = editedVar.Type == LuaTypes.LUA_TSTRING ? string.Format("\"{0}\"", editedVar.GetPrettyPrintValue()) : editedVar.GetPrettyPrintValue();
            }
        }

        private void tlvwLocals_ItemAfterEdit(object sender, TreeListViewAfterEditEventArgs e)
        {
            if (e.Item != null && (e.SubItem != null && e.Item.Tag is LuaVariable) && e.SubItem.Text != e.NewValue)
            {
                LuaVariable oldVar = e.Item.Tag as LuaVariable;
                LuaVariable updatedVar = new LuaVariable();
                double dummyDouble;

                if (e.NewValue.StartsWith("\""))
                {
                    string strVal = e.NewValue.Substring(1);
                    // Let's consider the new value a string
                    if (e.NewValue.EndsWith("\""))
                    {
                        strVal = strVal.Substring(0, strVal.Length - 1);
                    }

                    updatedVar.Type = LuaTypes.LUA_TSTRING;
                    updatedVar.Value = strVal;
                }
                else if (Double.TryParse(e.NewValue, out dummyDouble))
                {
                    updatedVar.Type = LuaTypes.LUA_TNUMBER;
                    updatedVar.Value = string.Format("{0:0.000000}", Convert.ToDouble(e.NewValue));
                }
                else if (e.NewValue.ToLower() == "nil")
                {
                    updatedVar.Type = LuaTypes.LUA_TNIL;
                    updatedVar.Value = e.NewValue.ToLower();
                }
                else if (e.NewValue.StartsWith("{"))
                {
                    // todo: handle table construct by executing "return {...}" and
                    //       setting the result of execution as the local's value
                }
                else
                {
                    updatedVar.Type = LuaTypes.LUA_TSTRING;
                    updatedVar.Value = e.NewValue;
                }

                updatedVar.Index = oldVar.Index;
                updatedVar.FullNameOut = oldVar.FullNameOut;
                updatedVar.Name = oldVar.Name;

                if (oldVar.Type == LuaTypes.LUA_TTABLE && updatedVar.Type != LuaTypes.LUA_TTABLE)
                {
                    // Remove all child
                    TreeListViewItem tlvi = _nodesByFullName[oldVar.FullNameIn];
                    RemoveTreeListViewItemChildsRecursively(tlvi);
                }

                e.Item.Tag = updatedVar;
                e.Item.SubItems[0].Object = updatedVar.Name;
                e.Item.SubItems[1].Object = updatedVar.GetPrettyPrintValue();
                e.Item.SubItems[2].Object = updatedVar.Type.ToString();

                ClientDebugManager.Instance.AddCommand(new SetLuaVariableCommand(updatedVar));
            }
        }

        private void tlvwLocals_ItemExpanding(object sender, TreeListViewCancelEventArgs e)
        {
            if (e.Item.Items.Count == 1 && e.Item.Items[0].Text == EmptyTableElementName)
            {
                LuaVariable var = e.Item.Tag as LuaVariable;
                ClientDebugManager.Instance.AddCommand(new UpdateTableDetailsCommand(var.FullNameIn, var.FullNameOut, true, null));
            }
        }

        #endregion

        #region Methods

        private bool FindLocalPredicate(LuaVariable var)
        {
            return _searchingVar.FullNameOut == var.FullNameOut;
        }

        private void ClearLocals()
        {
            tlvwLocals.Items.Clear();
            _nodesByFullName.Clear();
        }

        private void UpdateLocals()
        {
            if (tlvwLocals.InvokeRequired)
            {
                tlvwLocals.BeginInvoke(new UpdateLocalsDelegate(UpdateLocals));
                return;
            }

            try
            {
                tlvwLocals.BeginUpdate();
                UpdateVariables(tlvwLocals.RootItem, ClientDebugManager.Instance.Locals);
            }
            finally
            {
                tlvwLocals.EndUpdate();
                tlvwLocals.Refresh();
            }
        }

        private void UpdateVariables(TreeListViewItem parentItem, List<LuaVariable> currentVars)
        {
            int nonDirtyVarsCount = 0;

            // Remove fake node
            if (parentItem.Items.Count == 1 && parentItem.Items[0].Tag == null)
            {
                parentItem.Items.Remove(parentItem.Items[0]);
            }

            for (int x = parentItem.Items.Count - 1; x >= 0; --x)
            {
                TreeListViewItem tlvi = parentItem.Items[x];
                LuaVariable luaVar = tlvi.Tag as LuaVariable;

                if (luaVar != null)
                {
                    _searchingVar = luaVar;
                    LuaVariable updatedVar = currentVars.Find(FindLocalPredicate);

                    if (updatedVar != null && updatedVar.Name != LuaVariable.TemporaryName)
                    {
                        // The local variable still exists, let's see if
                        // its type or value has changed and if so, let's
                        // update it
                        updatedVar.IsDirty = false;
                        ++nonDirtyVarsCount;

                        if (updatedVar.Value != luaVar.Value || updatedVar.Type != luaVar.Type)
                        {
                            tlvi.Tag = updatedVar;
                            tlvi.SubItems[1].Object = updatedVar.GetPrettyPrintValue();
                            tlvi.SubItems[1].ForeColor = Color.Red;
                            tlvi.SubItems[2].Object = updatedVar.Type.ToString();
                            tlvi.SubItems[2].ForeColor = Color.Red;
                        }
                        else
                        {
                            tlvi.SubItems[1].ForeColor = Color.Empty;
                            tlvi.SubItems[2].ForeColor = Color.Empty;
                        }

                        // The local variable was a table (and is still one) and was expanded so let's
                        // update its content as well
                        if (luaVar.Type == LuaTypes.LUA_TTABLE && updatedVar.Type == LuaTypes.LUA_TTABLE &&
                            tlvi.Expanded)
                        {
                            int stackLevel = ClientDebugManager.Instance.IsInError ? 1 : 0;
                            ClientDebugManager.Instance.AddCommand(
                                new UpdateTableDetailsCommand(luaVar.FullNameIn, luaVar.FullNameOut, true, null));
                        }
                    }
                    else
                    {
                        // The local variable in not a local variable anymore
                        // so let's remove it from the list
                        RemoveTreeListViewItemChildsRecursively(tlvi);
                        _nodesByFullName.Remove(luaVar.FullNameIn);
                        parentItem.Items.Remove(tlvi);
                    }
                }
            }

            if (currentVars.Count != nonDirtyVarsCount)
            {
                foreach (LuaVariable var in currentVars)
                {
                    if (var.IsDirty)
                    {
                        TreeListViewItem tlvi = LuaVariableToTreeListViewItem(var);
                        var.IsDirty = false;
                        _nodesByFullName.Add(var.FullNameIn, tlvi);
                        parentItem.Items.Add(tlvi);
                    }
                }
            }
        }

        private void RemoveTreeListViewItemChildsRecursively(TreeListViewItem parentItem)
        {
            for (int x = parentItem.Items.Count - 1; x >= 0; --x)
            {
                if (parentItem.Items[x].Items.Count > 0)
                {
                    RemoveTreeListViewItemChildsRecursively(parentItem.Items[x]);
                }

                if (parentItem.Items[x].Tag != null)
                {
                    _nodesByFullName.Remove((parentItem.Items[x].Tag as LuaVariable).FullNameIn);
                }

                parentItem.Items.Remove(parentItem.Items[x]);
            }
        }

        private void AddLuaVariablesToItem(TreeListViewItem item, LuaVariable[] vars)
        {
            foreach (LuaVariable luaVar in vars)
            {
                TreeListViewItem tlvi = LuaVariableToTreeListViewItem(luaVar);
                item.Items.Add(tlvi);
            }
        }

        private TreeListViewItem LuaVariableToTreeListViewItem(LuaVariable luaVar)
        {
            // Create main item with name
            TreeListViewItem tlvi = new TreeListViewItem(luaVar.Name);
            tlvi.Tag = luaVar;

            // Create subitem with value
            TreeListViewSubItem tlvsi = new TreeListViewSubItem(1, luaVar.GetPrettyPrintValue());
            tlvsi.Tag = luaVar.Value;
            tlvi.SubItems.Add(tlvsi);

            // Create subitem with value type
            tlvsi = new TreeListViewSubItem(2, luaVar.Type.ToString());
            tlvi.SubItems.Add(tlvsi);

            if (luaVar.Type == LuaTypes.LUA_TTABLE)
            {
                tlvi.Items.Add(EmptyTableElementName);
            }

            return tlvi;
        }

        #endregion
    }
}