using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

using LuaEdit.HelperDialogs;
using LuaEdit.Interfaces;
using LuaEdit.Rpc;
using Fireball.Syntax;

namespace LuaEdit.Documents.DocumentUtils
{
    public enum HitCountConditions
    {
        BreakAlways,
        BreakEqualTo,
        BreakMultipleOf,
        BreakGreaterOrEqualTo
    };

    public class Breakpoint : IBreakpoint
    {
        #region Members

        private ILuaEditDocumentEditable _editableDoc = null;
        private string _fileName = string.Empty;
        private Row _row = null;
        private bool _enabled = true;
        private string _condition = string.Empty;
        private int _hitCount = 0;
        private HitCountConditions _hitCountCondition = HitCountConditions.BreakAlways;
        private Bitmap _breakpointEnabledImage = null;
        private Bitmap _breakpointDisabledImage = null;
        private Bitmap _breakpointConditionedEnabledImage = null;
        private Bitmap _breakpointConditionedDisabledImage = null;

        public event EventHandler BreakpointChanged = null;

        #endregion

        #region Constructors

        public Breakpoint(string fileName, Row row, int hitCount, Bitmap breakpointEnabledImage,
                          Bitmap breakpointDisabledImage, Bitmap breakpointConditionedEnabledImage,
                          Bitmap breakpointConditionedDisabledImage)
        {
            this.FileName = fileName;
            _row = row;
            _hitCount = hitCount;
            _breakpointEnabledImage = breakpointEnabledImage;
            _breakpointEnabledImage.MakeTransparent(_breakpointEnabledImage.GetPixel(0, 0));
            _breakpointDisabledImage = breakpointDisabledImage;
            _breakpointDisabledImage.MakeTransparent(_breakpointDisabledImage.GetPixel(0, 0));
            _breakpointConditionedEnabledImage = breakpointConditionedEnabledImage;
            _breakpointConditionedEnabledImage.MakeTransparent(_breakpointConditionedEnabledImage.GetPixel(0, 0));
            _breakpointConditionedDisabledImage = breakpointConditionedDisabledImage;
            _breakpointConditionedDisabledImage.MakeTransparent(_breakpointConditionedDisabledImage.GetPixel(0, 0));
        }

        public Breakpoint(SimpleBreakpoint sbp)
        {
            this.FileName = sbp.FileName;
            this.Line = sbp.Line;
            _condition = sbp.Condition;
            _hitCount = sbp.HitCount;
            _hitCountCondition = sbp.HitCountCondition;
        }

        #endregion

        #region Properties

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (value != _fileName)
                {
                    _fileName = value;
                    _editableDoc = DocumentsManager.Instance.OpenDocument(_fileName, false) as ILuaEditDocumentEditable;

                    if (BreakpointChanged != null)
                        BreakpointChanged(this, new EventArgs());
                }
            }
        }

        public Row Row
        {
            get { return _row; }
            set { _row = Row; }
        }

        public int Line
        {
            get { return _row.Index + 1; }
            set
            {
                if (value != _row.Index + 1)
                {
                    if (_editableDoc != null)
                    {
                        _row = _editableDoc.Document.LineToRow(value);
                    }

                    if (BreakpointChanged != null)
                        BreakpointChanged(this, new EventArgs());
                }
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;
                    if (BreakpointChanged != null)
                        BreakpointChanged(this, new EventArgs());
                }
            }
        }

        public string Condition
        {
            get { return _condition; }
            set
            {
                if (value != _condition)
                {
                    _condition = value;
                    if (BreakpointChanged != null)
                        BreakpointChanged(this, new EventArgs());
                }
            }
        }

        public int HitCount
        {
            get { return _hitCount; }
            set
            {
                if (value != _hitCount)
                {
                    _hitCount = value;
                    if (BreakpointChanged != null)
                        BreakpointChanged(this, new EventArgs());
                }
            }
        }

        public HitCountConditions HitCountCondition
        {
            get { return _hitCountCondition; }
            set
            {
                if (value != _hitCountCondition)
                {
                    _hitCountCondition = value;
                    if (BreakpointChanged != null)
                        BreakpointChanged(this, new EventArgs());
                }
            }
        }

        [XmlIgnore]
        public Bitmap BreakpointEnabledImage
        {
            get
            {
                return this.IsConditioned ? _breakpointConditionedEnabledImage : _breakpointEnabledImage;
            }
        }

        [XmlIgnore]
        public Bitmap BreakpointDisabledImage
        {
            get { return this.IsConditioned ? _breakpointConditionedDisabledImage : _breakpointDisabledImage; }
        }

        [XmlIgnore]
        public bool IsConditioned
        {
            get { return !(string.IsNullOrEmpty(_condition) && _hitCountCondition == HitCountConditions.BreakAlways); }
        }

        #endregion

        #region Methods

        public void EditCondition()
        {
            BreakpointConditionDlg conditionDlg = new BreakpointConditionDlg();
            conditionDlg.Condition = this.Condition;

            if (conditionDlg.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(conditionDlg.Condition) || !conditionDlg.ConditionChecked)
                {
                    this.Condition = string.Empty;
                }
                else
                {
                    this.Condition = conditionDlg.Condition;
                }
            }
        }

        public void EditHitCountCondition()
        {
            BreakpointHitCountDlg hitCountDlg = new BreakpointHitCountDlg();
            hitCountDlg.HitCount = _hitCount;
            hitCountDlg.HitCountCondition = this.HitCountCondition;

            if (hitCountDlg.ShowDialog() == DialogResult.OK)
            {
                if (hitCountDlg.HitCountCondition == HitCountConditions.BreakAlways)
                {
                    _hitCount = 0;
                }
                else
                {
                    _hitCount = hitCountDlg.HitCount;
                }

                _hitCountCondition = hitCountDlg.HitCountCondition;

                if (BreakpointChanged != null)
                {
                    BreakpointChanged(this, new EventArgs());
                }
            }
        }

        public void FlagDirty()
        {
            if (BreakpointChanged != null)
            {
                BreakpointChanged(this, new EventArgs());
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, line {1}", this.FileName, this.Line);
        }

        public SimpleBreakpoint ToSimpleBreakpoint()
        {
            return new SimpleBreakpoint(_fileName.ToLower(), this.Line, _condition, _hitCount, _hitCountCondition, _enabled);
        }

        public string GetHitConditionString()
        {
            switch (_hitCountCondition)
            {
                case HitCountConditions.BreakEqualTo: return string.Format("when hit count is equal to {0}", this.HitCount);
                case HitCountConditions.BreakMultipleOf: return string.Format("when hit count is a multiple of {0}", this.HitCount);
                case HitCountConditions.BreakGreaterOrEqualTo: return string.Format("when hit count is greater than or equal to {0}", this.HitCount);
                default: return "break always";
            }
        }

        #endregion
    }

    public class SimpleBreakpoint : IRPCSerializableData
    {
        #region Members

        public string FileName;
        public int Line;
        public string Condition;
        public int HitCount;
        public HitCountConditions HitCountCondition;
        public bool IsEnabled;

        #endregion

        #region Constructors

        public SimpleBreakpoint(string fileName, int line, string condition, int hitCount,
                                HitCountConditions hitCountCondition, bool isEnabled)
        {
            FileName = fileName;
            Line = line;
            Condition = condition;
            HitCount = hitCount;
            HitCountCondition = hitCountCondition;
            IsEnabled = isEnabled;
        }

        #endregion

        #region Methods

        public int GetSerializedDataSize()
        {
            int totalSize = 0;
            totalSize += RPCCommand.GetStringDataSize(FileName);
            totalSize += RPCCommand.GetIntegerDataSize();
            totalSize += RPCCommand.GetStringDataSize(Condition);
            totalSize += RPCCommand.GetIntegerDataSize();
            totalSize += RPCCommand.GetIntegerDataSize();
            totalSize += RPCCommand.GetBooleanDataSize();
            return totalSize;
        }

        public void DeserializeData(byte[] data, ref int offset)
        {
            RPCCommand.DeserializeString(data, ref offset, ref FileName);
            RPCCommand.DeserializeInteger(data, ref offset, ref Line);
            RPCCommand.DeserializeString(data, ref offset, ref Condition);
            RPCCommand.DeserializeInteger(data, ref offset, ref HitCount);

            int hitCountCondition = (int)HitCountCondition;
            RPCCommand.DeserializeInteger(data, ref offset, ref hitCountCondition);
            HitCountCondition = (HitCountConditions)hitCountCondition;

            RPCCommand.DeserializeBoolean(data, ref offset, ref IsEnabled);
        }

        public void SerializeData(ref byte[] data, ref int offset)
        {
            RPCCommand.SerializeString(ref data, ref offset, FileName);
            RPCCommand.SerializeInteger(ref data, ref offset, Line);
            RPCCommand.SerializeString(ref data, ref offset, Condition);
            RPCCommand.SerializeInteger(ref data, ref offset, HitCount);
            RPCCommand.SerializeInteger(ref data, ref offset, (int)HitCountCondition);
            RPCCommand.SerializeBoolean(ref data, ref offset, IsEnabled);
        }

        #endregion
    }
}
