using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Fireball.Syntax;

using DotNetLib.Controls;
using LuaEdit.Controls;
using LuaEdit.Documents;
using LuaEdit.Documents.DocumentUtils;
using LuaEdit.LuaEditDebugger;
using LuaEdit.Managers;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Forms
{
    public partial class BreakpointsDlg : DockContent
    {
        #region Constructors

        public BreakpointsDlg()
        {
            InitializeComponent();
            UpdateBreakpointsList();

            BreakpointManager.Instance.BreakpointAdded += OnBreakpointAdded;
            BreakpointManager.Instance.BreakpointRemoved += OnBreakpointRemoved;
            BreakpointManager.Instance.BreakpointChanged += OnBreakpointChanged;
            BreakpointManager.Instance.BreakpointsDirty += OnBreakpointsDirty;
        }

        #endregion

        #region Events

        private void OnBreakpointAdded(object sender, Breakpoint bp)
        {
            AddBreakpointItem(bp);
        }

        private void OnBreakpointRemoved(object sender, Breakpoint bp)
        {
            RemoveBreakpointItem(bp);
        }

        private void OnBreakpointChanged(object sender, Breakpoint bp)
        {
            UpdateBreakpoint(bp);
        }

        private void OnBreakpointsDirty(object sender, string fileName)
        {
            tlvwBreakpoints.Refresh();
        }

        private void tlvwBreakpoints_ItemChecked(object sender, TreeListViewEventArgs e)
        {
            Breakpoint bp = e.Item.Tag as Breakpoint;

            if (bp != null)
            {
                bp.Enabled = e.Item.Checked;
            }
        }

        private void btnDeleteAllBreakpoints_Click(object sender, EventArgs e)
        {
            BreakpointManager.Instance.DeleteAllBreakpoints();
        }

        private void btnDisableAllBreakpoints_Click(object sender, EventArgs e)
        {
            BreakpointManager.Instance.DisableAllBreakpoints();
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (tlvwBreakpoints.SelectedItems.Count > 0)
            {
                foreach (TreeListViewItem tlvi in tlvwBreakpoints.SelectedItems)
                {
                    Breakpoint bp = tlvi.Tag as Breakpoint;
                    BreakpointManager.Instance.RemoveBreakpoint(bp.FileName, bp.Row);
                }
            }
        }

        private void btnGoToSourceCode_Click(object sender, EventArgs e)
        {
            if (tlvwBreakpoints.SelectedItems.Count == 1)
            {
                Breakpoint bp = tlvwBreakpoints.SelectedItems[0].Tag as Breakpoint;
                DocumentsManager.Instance.OpenDocument(bp.FileName, false, new PostOpenDocumentOptions(bp.Line));
            }
        }

        private void tlvwBreakpoints_SelectedItemsChanged(object sender, EventArgs e)
        {
            ValidateButtons();
        }

        private void tlvwBreakpoints_DoubleClick(object sender, EventArgs e)
        {
            if (tlvwBreakpoints.SelectedItems.Count == 1)
            {
                btnGoToSourceCode_Click(sender, e);
            }
        }

        private void _breakpointsContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (tlvwBreakpoints.SelectedItems.Count != 1)
            {
                e.Cancel = true;
            }
            else
            {
                if (tlvwBreakpoints.SelectedItems.Count == 1)
                {
                    Breakpoint bp = tlvwBreakpoints.SelectedItems[0].Tag as Breakpoint;
                    conditionToolStripMenuItem.Checked = !string.IsNullOrEmpty(bp.Condition);
                    hitCountToolStripMenuItem.Checked = bp.HitCountCondition != HitCountConditions.BreakAlways;
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tlvwBreakpoints.SelectedItems.Count == 1)
            {
                btnDeleteSelected_Click(sender, e);
            }
        }

        private void gotoSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tlvwBreakpoints.SelectedItems.Count == 1)
            {
                btnGoToSourceCode_Click(sender, e);
            }
        }

        private void conditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tlvwBreakpoints.SelectedItems.Count == 1)
            {
                Breakpoint bp = tlvwBreakpoints.SelectedItems[0].Tag as Breakpoint;
                bp.EditCondition();
            }
        }

        private void hitCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tlvwBreakpoints.SelectedItems.Count == 1)
            {
                Breakpoint bp = tlvwBreakpoints.SelectedItems[0].Tag as Breakpoint;
                bp.EditHitCountCondition();
            }
        }

        #endregion

        #region Methods

        private void ValidateButtons()
        {
            btnDeleteSelected.Enabled = tlvwBreakpoints.SelectedItems.Count > 0;
            btnDeleteAllBreakpoints.Enabled = tlvwBreakpoints.Items.Count > 0;
            btnDisableAllBreakpoints.Enabled = tlvwBreakpoints.Items.Count > 0;
            btnGoToSourceCode.Enabled = tlvwBreakpoints.SelectedItems.Count == 1;
        }

        private void UpdateBreakpointsList()
        {
            foreach (string fileName in BreakpointManager.Instance.Breakpoints.Keys)
            {
                Dictionary<Row, Breakpoint> breakpoints = BreakpointManager.Instance.Breakpoints[fileName];

                foreach (Breakpoint bp in breakpoints.Values)
                {
                    if (!IsInList(bp))
                    {
                        AddBreakpointItem(bp);
                    }
                    else
                    {
                        UpdateBreakpoint(bp);
                    }
                }
            }

            tlvwBreakpoints.Refresh();
            ValidateButtons();
        }

        private TreeListViewItem AddBreakpointItem(Breakpoint bp)
        {
            TreeListViewItem tlvi = new TreeListViewItem(bp);
            tlvi.Tag = bp;
            tlvi.Checked = bp.Enabled;
            tlvi.ImageIndex = GetBreakpointImageIndex(bp); 
            tlvi.SubItems.Add(new TreeListViewSubItem(0, string.IsNullOrEmpty(bp.Condition) ? "(no condition)" : bp.Condition));
            tlvi.SubItems.Add(new TreeListViewSubItem(1, bp.GetHitConditionString()));
            tlvwBreakpoints.Items.Add(tlvi);

            ValidateButtons();

            return tlvi;
        }

        private void UpdateBreakpoint(Breakpoint bp)
        {
            TreeListViewItem tlvi = FindBreakpointItem(bp);

            if (tlvi != null)
            {
                tlvi.Object = bp;
                tlvi.Checked = bp.Enabled;
                tlvi.ImageIndex = GetBreakpointImageIndex(bp);
                tlvi.SubItems[1].Object = bp.Condition == "" ? "(no condition)" : bp.Condition;
                tlvi.SubItems[2].Object = bp.GetHitConditionString();
            }

            tlvwBreakpoints.Refresh(tlvi);
            ValidateButtons();
        }

        private void RemoveBreakpointItem(Breakpoint bp)
        {
            TreeListViewItem tlvi = FindBreakpointItem(bp);

            if (tlvi != null)
            {
                tlvwBreakpoints.Items.Remove(tlvi);
            }
        }

        private bool IsInList(Breakpoint bp)
        {
            foreach (TreeListViewItem tlvi in tlvwBreakpoints.Items)
            {
                if ((tlvi.Tag as Breakpoint) == bp)
                {
                    return true;
                }
            }

            return false;
        }

        private TreeListViewItem FindBreakpointItem(Breakpoint bp)
        {
            foreach (TreeListViewItem tlvi in tlvwBreakpoints.Items)
            {
                if ((tlvi.Tag as Breakpoint) == bp)
                {
                    return tlvi;
                }
            }

            return null;
        }

        private int GetBreakpointImageIndex(Breakpoint bp)
        {
            return bp.Enabled ? bp.IsConditioned ? 2 : 0 : bp.IsConditioned ? 3 : 1;
        }

        #endregion
    }
}