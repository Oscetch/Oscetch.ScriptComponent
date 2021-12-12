using Microsoft.CodeAnalysis.Completion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Osctech.ScriptToolExample.Controls
{
    public class AutoCompleteToolStripItem : ToolStripMenuItem
    {
        private readonly CompletionItem _completionItem;
        private readonly ScriptControl _scriptControl;

        public AutoCompleteToolStripItem(CompletionItem completionItem, ScriptControl scriptControl)
        {
            _completionItem = completionItem;
            Text = completionItem.DisplayText;
            _scriptControl = scriptControl;
            Click += AutoCompleteToolStripItem_Click;
        }

        private void AutoCompleteToolStripItem_Click(object sender, EventArgs e)
        {
            var currentCode = _scriptControl.GetCode();
            var preString = currentCode.Substring(0, _completionItem.Span.Start);
            var postString = currentCode[_completionItem.Span.End..];

            _scriptControl.SetSelection(_completionItem.Span.Start, _completionItem.Span.End - _completionItem.Span.Start);
            _scriptControl.InsertToCode(_completionItem.Span.Start, _completionItem.DisplayText);
            _scriptControl.SetSelection(_completionItem.Span.Start + _completionItem.DisplayText.Length);
        }
    }
}