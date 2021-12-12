using Osctech.ScriptToolExample.Helpers;
using Osctech.ScriptToolExample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Osctech.ScriptToolExample.Dialogs
{
    public partial class SyntaxColorOptionDialog : Form
    {
        private Dictionary<string, SyntaxDisplayOptions> _syntaxDisplayOptions;

        public SyntaxColorOptionDialog(Dictionary<string, SyntaxDisplayOptions> syntaxDisplayOptions)
        {
            InitializeComponent();
            _syntaxDisplayOptions = new Dictionary<string, SyntaxDisplayOptions>(syntaxDisplayOptions
                .Select(x => new KeyValuePair<string, SyntaxDisplayOptions>(x.Key, new SyntaxDisplayOptions(x.Value.Classification) 
                {
                    IsBold = x.Value.IsBold,
                    TextColor = x.Value.TextColor
                })));
            classifierComboBox.Items.AddRange(
                SyntaxClassifierHelper.GetClassifierTypeNames().ToArray());
            classifierComboBox.SelectedIndex = 0;
            UpdateLoremIpsom();
        }

        public Dictionary<string, SyntaxDisplayOptions> GetDisplayOptions()
        {
            return _syntaxDisplayOptions;
        }

        private SyntaxDisplayOptions GetCurrent()
        {
            if(classifierComboBox.SelectedItem is not string classifier)
            {
                Debug.WriteLine($"{nameof(SyntaxColorOptionDialog)}.{nameof(GetCurrent)}s " +
                    "combobox did not have a selected item");
                return new SyntaxDisplayOptions("?");
            }

            if(_syntaxDisplayOptions.TryGetValue(classifier, out var options))
            {
                return options;
            }

            _syntaxDisplayOptions[classifier] = options = new SyntaxDisplayOptions(classifier);
            return options;
        }

        private void IsBoldCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var current = GetCurrent();
            current.IsBold = isBoldCheckBox.Checked;
            UpdateLoremIpsum(current);
        }

        private void UpdateLoremIpsom() => UpdateLoremIpsum(GetCurrent());

        private void UpdateLoremIpsum(SyntaxDisplayOptions options)
        {
            loremIpsum.Font = new Font(loremIpsum.Font, options.IsBold ? FontStyle.Bold : FontStyle.Regular);
            loremIpsum.ForeColor = options.TextColor;
        }

        private void ChangeColorButton_Click(object sender, EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                if(colorDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var current = GetCurrent();
                current.TextColor = colorDialog.Color;
                UpdateLoremIpsom();
            }
        }

        private void ClassifierComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var current = GetCurrent();
            isBoldCheckBox.Checked = current.IsBold;
            UpdateLoremIpsom();
        }
    }
}
