using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Immutable;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oscetch.ScriptToolExample.Wrappers;
using Oscetch.ScriptToolExample.Models;
using Oscetch.ScriptComponent.Compiler;

namespace Oscetch.ScriptToolExample.Controls
{
    public partial class ScriptControl : UserControl
    {
        private Document _document;
        private int _spacesInTabs = 4;
        private Settings.Settings _optionSettings;
        private string _tabString = "    ";

        public bool UseTabs { get; set; }
        public int SpacesInTabs 
        { 
            get => _spacesInTabs;
            set
            {
                _spacesInTabs = 4;
                _tabString = string.Empty;
                for(var i = 0; i < _spacesInTabs; i++)
                {
                    _tabString += ' ';
                }
            }
        }

        public ScriptControl()
        {
            InitializeComponent();
        }

        public string GetCode()
        {
            return codeTextBox.Text;
        }

        public void SetCode(string code)
        {
            codeTextBox.Text = code;
        }

        public void SetSelection(int position, int selectionLength = 0)
        {
            codeTextBox.SelectionStart = position;
            codeTextBox.SelectionLength = selectionLength;
        }

        public async Task SetReferences(Settings.Settings settings)
        {
            _optionSettings = settings;
            var references = AssemblyHelper.GetAssemblies(settings.References, out var errors);
            foreach(var error in errors)
            {
                MessageBox.Show(error);
            }
            _document = OscetchCompiler.CreateDocument(settings.AssemblyName, references);
            await ReHighlight();
        }

        private void CodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.L when e.Control:
                    LoadRequest?.Invoke(this, EventArgs.Empty);
                    return;
                case Keys.S when e.Control:
                    SaveRequest?.Invoke(this, EventArgs.Empty);
                    return;
                case Keys.N when e.Control:
                    CreateNewRequest?.Invoke(this, EventArgs.Empty);
                    return;
                case Keys.Tab when e.Shift:
                    InvertTab();
                    e.Handled = e.SuppressKeyPress = true;
                    return;
                case Keys.Tab:
                    SetTab();
                    e.Handled = e.SuppressKeyPress = true;
                    return;
                case Keys.Enter when e.Alt:
                    ShowSuggestions();
                    e.Handled = e.SuppressKeyPress = true;
                    return;
                case Keys.Enter:
                    SetNewLineIndentation();
                    e.Handled = e.SuppressKeyPress = true;
                    return;
            }
        }
        private void InvertTab()
        {
            User32Wrapper.BeginUpdate(codeTextBox.Handle);

            var currentPosition = codeTextBox.SelectionStart;
            var preceedingSpaces = GetPreceedingSpaces(currentPosition);
            codeTextBox.SelectionStart = preceedingSpaces.Start.Value;
            codeTextBox.SelectionLength = preceedingSpaces.End.Value - preceedingSpaces.Start.Value;
            var previousClipboard = Clipboard.GetText();
            codeTextBox.Cut();
            Clipboard.SetText(previousClipboard);

            if (preceedingSpaces.Start.Value == 0 
                || codeTextBox.Text[preceedingSpaces.Start.Value - 1] == '\n')
            {
                codeTextBox.SelectionStart = preceedingSpaces.Start.Value;
            }
            else
            {
                codeTextBox.SelectionStart = preceedingSpaces.Start.Value + 1;
            }

            codeTextBox.SelectionLength = 0;

            User32Wrapper.EndUpdate(codeTextBox.Handle);
            codeTextBox.Invalidate();
        }

        public void InsertToCode(int start, string code)
        {
            var previousClipboard = Clipboard.GetText();
            if(codeTextBox.SelectedText.Length > 0)
            {
                codeTextBox.Cut();
            }

            codeTextBox.SelectionStart = start;
            Clipboard.SetText(code);
            codeTextBox.Paste();
            Clipboard.SetText(previousClipboard);
        }

        private void SetTab()
        {
            User32Wrapper.BeginUpdate(codeTextBox.Handle);

            var currentPosition = codeTextBox.SelectionStart;

            InsertToCode(currentPosition, _tabString);
            codeTextBox.SelectionStart = currentPosition + _tabString.Length;

            User32Wrapper.EndUpdate(codeTextBox.Handle);
            codeTextBox.Invalidate();
        }

        private void SetNewLineIndentation()
        {
            var currentSelection = codeTextBox.SelectionStart;
            User32Wrapper.BeginUpdate(codeTextBox.Handle);
            var previousClipboard = Clipboard.GetText();

            if(codeTextBox.SelectedText.Length > 0)
            {
                codeTextBox.Cut();
            }

            var nextLine = GetNextLine(currentSelection);
            var wasNoSpaces = nextLine.Length == 1;
            if(wasNoSpaces)
            {
                nextLine += " ";
            }
            Clipboard.SetText(nextLine);
            codeTextBox.Paste();

            // because rich text box is the dumbest thing ever made
            if (wasNoSpaces)
            {
                codeTextBox.SelectionStart = currentSelection + nextLine.Length - 1;
                codeTextBox.SelectionLength = 1;
                codeTextBox.Cut();
                nextLine = "\n";
            }

            Clipboard.SetText(previousClipboard);

            codeTextBox.SelectionStart = currentSelection + nextLine.Length;
            codeTextBox.SelectionLength = 0;

            User32Wrapper.EndUpdate(codeTextBox.Handle);
            codeTextBox.Invalidate();
        }

        private System.Range GetPreceedingSpaces(int currentSelection)
        {
            if(currentSelection == 0)
            {
                return new System.Range(0, 0);
            }

            var start = currentSelection - 1;
            var end = start;

            for (var i = start; i >= 0; i--)
            {
                if(start - end >= 4)
                {
                    return new System.Range(end, start);
                }

                switch (codeTextBox.Text[i])
                {
                    case ' ':
                        end = i;
                        continue;
                    case '\n':
                        return new System.Range(end, start);
                    default:
                        start = end = i;
                        continue;
                }
            }

            return new System.Range(end, start + 1);
        }

        private string GetNextLine(int currentSelection)
        {
            var nextLineSb = new StringBuilder();
            for(var i = currentSelection - 1; i >= 0; i--)
            {
                switch (codeTextBox.Text[i])
                {
                    case ' ':
                        nextLineSb.Append(' ');
                        continue;
                    case '\n':
                        return nextLineSb.Insert(0, '\n').ToString();
                    default:
                        nextLineSb.Clear();
                        continue;
                }
            }

            return nextLineSb.Insert(0, '\n').ToString();
        }

        private void UpdateDocument()
        {
            _document = _document.WithText(SourceText.From(codeTextBox.Text));
        }

        private void ShowSuggestions()
        {
            var currentPos = codeTextBox.SelectionStart;
            UpdateDocument();
            var completionService = CompletionService.GetService(_document);

            if(completionService == null)
            {
                return;
            }

            var result = completionService.GetCompletionsAsync(_document, currentPos).Result;
            if(result?.ItemsList == null)
            {
                return;
            }

            var filterText = codeTextBox.Text[result.Span.Start..result.Span.End];
            var items = completionService.FilterItems(_document, [.. result.ItemsList], filterText);
            ShowSuggestions(currentPos, items);
        }

        private void ShowSuggestions(int currentPos, ImmutableArray<CompletionItem> items)
        {
            suggestionsMenuStrip.Items.Clear();
            foreach (var completionResult in items)
            {
                var item = new AutoCompleteToolStripItem(completionResult, this);
                suggestionsMenuStrip.Items.Add(item);
            }

            suggestionsMenuStrip.Show(this, codeTextBox.GetPositionFromCharIndex(currentPos));
        }


        private async void CodeTextBox_TextChanged(object sender, EventArgs e)
        {
            await UpdateScriptHighlightingAsync();
        }

        public async Task UpdateScriptHighlightingAsync()
        {
            var sourceText = SourceText.From(codeTextBox.Text);
            var oldDocument = _document;
            _document = _document.WithText(sourceText);

            var changes = await _document.GetTextChangesAsync(oldDocument);

            var currentSelection = codeTextBox.SelectionStart;
            User32Wrapper.BeginUpdate(codeTextBox.Handle);

            foreach (var change in changes)
            {
                var span = new TextSpan(change.Span.Start, change.NewText.Length);
                var changeClassifiedSpans =
                    await Classifier.GetClassifiedSpansAsync(_document, span);
                var classifiedRanges = changeClassifiedSpans.Select(classifiedSpan =>
                    new Models.Range(classifiedSpan, sourceText.GetSubText(classifiedSpan.TextSpan).ToString()));
                foreach (var range in classifiedRanges)
                {
                    codeTextBox.SelectionStart = range.TextSpan.Start;
                    codeTextBox.SelectionLength = range.TextSpan.End - range.TextSpan.Start;
                    var options = GetSyntaxDisplayOptions(range.ClassificationType);
                    codeTextBox.SelectionColor = options.TextColor;
                    if (options.IsBold)
                    {
                        codeTextBox.SelectionFont = new Font(codeTextBox.Font, FontStyle.Bold);
                    }
                    else
                    {
                        codeTextBox.SelectionFont = new Font(codeTextBox.Font, FontStyle.Regular);
                    }
                }
            }

            codeTextBox.SelectionStart = currentSelection;
            codeTextBox.SelectionLength = 0;

            User32Wrapper.EndUpdate(codeTextBox.Handle);
            codeTextBox.Invalidate();
        }

        public async Task ReHighlight()
        {
            if (codeTextBox.Text == "") return;
            User32Wrapper.BeginUpdate(Handle);
            var sourceText = SourceText.From(codeTextBox.Text);
            var classifiedSpans =
                await Classifier.GetClassifiedSpansAsync(_document, TextSpan.FromBounds(0, sourceText.Length));
            var ranges = classifiedSpans.Select(classifiedSpan =>
                new Models.Range(classifiedSpan, sourceText.GetSubText(classifiedSpan.TextSpan).ToString()));

            var currentSelection = codeTextBox.SelectionStart;
            var currentSelectionLength = codeTextBox.SelectionLength;

            codeTextBox.SelectionStart = 0;
            codeTextBox.SelectionLength = codeTextBox.Text.Length;
            codeTextBox.SelectionColor = Color.Black;
            codeTextBox.SelectionFont = new Font(codeTextBox.Font, FontStyle.Regular);

            foreach (var range in ranges)
            {
                codeTextBox.SelectionStart = range.TextSpan.Start;
                codeTextBox.SelectionLength = range.TextSpan.End - range.TextSpan.Start;
                var options = GetSyntaxDisplayOptions(range.ClassificationType);
                codeTextBox.SelectionColor = options.TextColor;
                if (options.IsBold)
                {
                    codeTextBox.SelectionFont = new Font(codeTextBox.Font, FontStyle.Bold);
                }
                else
                {
                    codeTextBox.SelectionFont = new Font(codeTextBox.Font, FontStyle.Regular);
                }
            }

            codeTextBox.SelectionStart = currentSelection;
            codeTextBox.SelectionLength = currentSelectionLength;

            User32Wrapper.EndUpdate(Handle);
            codeTextBox.Invalidate();
        }

        private SyntaxDisplayOptions GetSyntaxDisplayOptions(string classification)
        {
            if(_optionSettings.SyntaxDisplayOptions.TryGetValue(classification, out var syntaxDisplayOptions))
            {
                return syntaxDisplayOptions;
            }

            return new SyntaxDisplayOptions(classification);
        }

        public event EventHandler SaveRequest;
        public event EventHandler LoadRequest;
        public event EventHandler CreateNewRequest;
    }
}
