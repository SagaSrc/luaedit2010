using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using Fireball.Syntax;
using LuaEdit.LuaEditDebugger;
using LuaEdit.Controls;
using WeifenLuo.WinFormsUI.Docking;

namespace LuaEdit.Forms
{
    public partial class OutputDlg : DockContent
    {
        #region Embeded Classes

        public class OutputChannel
        {
            #region Members

            private SyntaxDocument _channelDoc = new SyntaxDocument();
            private string _channelName = string.Empty;
            private OutputType _outputType;

            #endregion

            #region Constructors

            public OutputChannel(string channelName, OutputType outputType)
            {
                _channelName = channelName;
                _outputType = outputType;
                _channelDoc.Text = string.Empty;
                _channelDoc.MaxUndoBufferSize = 0;
                _channelDoc.Folding = false;
            }

            #endregion

            #region Properties

            public SyntaxDocument ChannelDocument
            {
                get { return _channelDoc; }
            }

            public string ChannelName
            {
                get { return _channelName; }
                set { _channelName = value; }
            }

            public OutputType ChannelOutputType
            {
                get { return _outputType; }
            }

            #endregion

            #region Methods

            public override string ToString()
            {
                return _channelName;
            }

            #endregion
        }

        #endregion

        #region Members

        private delegate void UpdateOutputEventHandler();
        private Thread _outputThread = null;
        private Dictionary<OutputType, string> _outputBuffers = new Dictionary<OutputType,string>();
        private Dictionary<OutputType, string> _outputBuffersToUpdate = new Dictionary<OutputType,string>();
        private Dictionary<OutputType, OutputChannel> _channels = new Dictionary<OutputType,OutputChannel>();

        #endregion

        #region Constructors

        public OutputDlg()
        {
            InitializeComponent();

            foreach (string enumName in Enum.GetNames(typeof(OutputType)))
            {
                OutputType outputType = (OutputType)Enum.Parse(typeof(OutputType), enumName);
                _channels.Add(outputType, new OutputChannel(enumName, outputType));
                tscboOutputTypes.Items.Add(_channels[outputType]);
                _outputBuffers.Add(outputType, string.Empty);
                _outputBuffersToUpdate.Add(outputType, string.Empty);
            }

            tscboOutputTypes.SelectedIndex = 0;
            _outputThread = new Thread(UpdateOutput);
            _outputThread.Start();
            ClientDebugManager.Instance.DebuggingStarted += OnDebuggingStarted;
            ClientDebugManager.Instance.OutputOccured += OnOutputOccured;
        }

        #endregion

        #region Event Handlers

        private void OnDebuggingStarted(object sender, EventArgs e)
        {
            txtOutput.Document.Clear();
        }

        private void OnOutputOccured(object sender, OutputOccuredEventArgs e)
        {
            lock (_outputBuffers)
            {
                _outputBuffers[e.OutputType] += e.Output;
            }
        }

        private void OnUpdateOutput()
        {
            lock (_outputBuffersToUpdate)
            {
                foreach (string enumName in Enum.GetNames(typeof(OutputType)))
                {
                    OutputType outputType = (OutputType)Enum.Parse(typeof(OutputType), enumName);
                    bool isSelected = ((OutputChannel)tscboOutputTypes.SelectedItem).ChannelOutputType == outputType;

                    if (_outputBuffersToUpdate[outputType] != string.Empty)
                    {
                        bool forceGotoLastLine = isSelected && txtOutput.Caret.CurrentRow.Index == txtOutput.Document.Lines.Length - 1;
                        _channels[outputType].ChannelDocument.Text += _outputBuffersToUpdate[outputType];

                        if (forceGotoLastLine)
                        {
                            txtOutput.GotoLine(txtOutput.Document.Lines.Length - 1);
                        }

                        if (isSelected)
                        {
                            txtOutput.Refresh();
                        }

                        _outputBuffersToUpdate[outputType] = string.Empty;
                    }
                }
            }
        }

        private void tscboOutputTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtOutput.SuspendLayout();
            txtOutput.GotoLine(0);
            txtOutput.Document = ((OutputChannel)tscboOutputTypes.SelectedItem).ChannelDocument;
            txtOutput.GotoLine(txtOutput.Document.Lines.Length - 1);
            txtOutput.ResumeLayout();
        }

        private void tsbClearAll_Click(object sender, EventArgs e)
        {
            txtOutput.Document.Clear();
        }

        private void tsbToggleWordWrap_Click(object sender, EventArgs e)
        {
            // todo
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (disposing && _outputThread != null)
            {
                _outputThread.Abort();
                _outputThread = null;
            }

            base.Dispose(disposing);
        }

        private void UpdateOutput()
        {
            bool needsUpdate = false;

            while (true)
            {
                lock (_outputBuffers)
                {
                    foreach (string enumName in Enum.GetNames(typeof(OutputType)))
                    {
                        OutputType outputType = (OutputType)Enum.Parse(typeof(OutputType), enumName);

                        if (_outputBuffers[outputType] != string.Empty)
                        {
                            lock (_outputBuffersToUpdate)
                            {
                                _outputBuffersToUpdate[outputType] = _outputBuffers[outputType];
                            }

                            _outputBuffers[outputType] = string.Empty;
                            needsUpdate = true;
                        }
                    }

                    if (needsUpdate)
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new UpdateOutputEventHandler(OnUpdateOutput));
                        }

                        needsUpdate = false;
                    }
                }

                Thread.Sleep(100);
            }
        }

        #endregion
    }
}