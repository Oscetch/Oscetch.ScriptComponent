using Microsoft.CodeAnalysis.Completion;
using Oscetch.ScriptToolExample.Controls;
using System;
using System.Windows.Forms;

namespace Oscetch.ScriptToolExample.Controls
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
            _scriptControl.SetSelection(_completionItem.Span.Start, _completionItem.Span.End - _completionItem.Span.Start);
            _scriptControl.InsertToCode(_completionItem.Span.Start, _completionItem.DisplayText);
            _scriptControl.SetSelection(_completionItem.Span.Start + _completionItem.DisplayText.Length);
        }
    }
}