using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Osctech.ScriptToolExample.Dialogs
{
    public partial class FilePathDialog : Form
    {
        private readonly string _filter;

        public string Path => pathTextBox.Text;

        public FilePathDialog(string filter, string currentPath)
        {
            InitializeComponent();
            _filter = filter;
            pathTextBox.Text = currentPath;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using(var saveFileDialog = new SaveFileDialog { Filter = _filter })
            {
                if(saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                pathTextBox.Text = saveFileDialog.FileName;
            }
        }
    }
}
