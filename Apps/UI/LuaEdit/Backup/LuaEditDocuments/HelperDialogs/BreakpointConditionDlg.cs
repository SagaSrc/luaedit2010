using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LuaEdit.HelperDialogs
{
    public partial class BreakpointConditionDlg : Form
    {
        #region Constructors

        public BreakpointConditionDlg()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public string Condition
        {
            get { return txtCondition.Text; }
            set { txtCondition.Text = value; }
        }

        public bool ConditionChecked
        {
            get { return chkCondition.Checked; }
        }

        #endregion

        #region Event Handlers

        private void chkCondition_CheckedChanged(object sender, EventArgs e)
        {
            txtCondition.Enabled = chkCondition.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}