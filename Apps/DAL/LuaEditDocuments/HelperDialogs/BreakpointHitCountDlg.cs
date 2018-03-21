using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using LuaEdit.Documents.DocumentUtils;

namespace LuaEdit.HelperDialogs
{
    public partial class BreakpointHitCountDlg : Form
    {
        #region Constructors

        public BreakpointHitCountDlg()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public HitCountConditions HitCountCondition
        {
            get { return (HitCountConditions)cboHitCountCondition.SelectedIndex; }
            set { cboHitCountCondition.SelectedIndex = (int)value; }
        }

        public int HitCount
        {
            get { return Convert.ToInt32(nudHitCount.Value); }
            set
            {
                Decimal val = Convert.ToDecimal(value);

                if (val >= nudHitCount.Minimum)
                {
                    nudHitCount.Value = val;
                }
                else
                {
                    nudHitCount.Value = nudHitCount.Minimum;
                }
            }
        }

        #endregion

        #region Event Handlers

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

        private void BreakpointHitCountDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                int hitCountVal = Convert.ToInt32(nudHitCount.Value);

                if (hitCountVal <= 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("The specified hit count target is not valid.", "LuaEdit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cboHitCountCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            nudHitCount.Visible = cboHitCountCondition.SelectedIndex != 0;
        }

        #endregion
    }
}