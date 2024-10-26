using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Oscetch.ScriptToolExample.Dialogs
{
    public partial class ReferenceDialog : Form
    {
        public List<string> References
        {
            get
            {
                var references = new List<string>();
                foreach(ListViewItem item in referencesListView.Items)
                {
                    references.Add(item.Text);
                }

                return references;
            }
        }

        public ReferenceDialog(IEnumerable<string> references)
        {
            InitializeComponent();

            referencesListView.View = View.List;

            foreach(var reference in references)
            {
                referencesListView.Items.Add(reference);
            }
        }

        private void AddReferenceButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog 
            {
                Filter = "DLL files(*.dll)|*.dll",
                Multiselect = true
            };
            if(openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            foreach(var fileName in openFileDialog.FileNames)
            {
                referencesListView.Items.Add(fileName);
            }
        }

        private void RemoveRerferenceButton_Click(object sender, EventArgs e)
        {
            if(referencesListView.SelectedItems.Count == 0)
            {
                return;
            }

            foreach(ListViewItem selectedItem in referencesListView.SelectedItems)
            {
                referencesListView.Items.RemoveAt(selectedItem.Index);
            }
        }
    }
}
