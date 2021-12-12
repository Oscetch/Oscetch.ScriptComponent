using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;
using Oscetch.ScriptComponent;
using Oscetch.ScriptComponent.Compiler;
using Oscetch.ScriptComponent.Compiler.Extensions;
using Osctech.ScriptToolExample.Controls;
using Osctech.ScriptToolExample.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Osctech.ScriptToolExample
{
    public partial class ScriptForm : Form
    {
        private readonly Settings.Settings _settings;

        public ScriptForm()
        {
            InitializeComponent();
            try
            {
                _settings = JsonConvert.DeserializeObject<Settings.Settings>(File.ReadAllText("settings.json"));
            }
            catch
            {
                _settings = new Settings.Settings();
                _settings.SetDefaultSyntaxDisplayOptions();
            }
            scriptControl1.SetReferences(_settings).Wait();
            if (File.Exists(_settings.CurrentSavePath))
            {
                var savedCode = File.ReadAllText(_settings.CurrentSavePath);
                scriptControl1.SetCode(savedCode);
            }

            scriptControl1.SaveRequest += ScriptControl1_SaveRequest;
            scriptControl1.LoadRequest += ScriptControl1_LoadRequest;
            scriptControl1.CreateNewRequest += ScriptControl1_CreateNewRequest;
        }

        private void ScriptControl1_CreateNewRequest(object sender, EventArgs e)
        {
            CreateNew();
        }

        private void ScriptControl1_LoadRequest(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void ScriptControl1_SaveRequest(object sender, EventArgs e)
        {
            if (!File.Exists(_settings.CurrentSavePath))
            {
                SaveCurrentCode();
                return;
            }

            File.WriteAllText(_settings.CurrentSavePath, scriptControl1.GetCode());
        }

        private void SaveSettings()
        {
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(_settings));
        }

        private async void AddReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var referenceDialog = new ReferenceDialog(_settings.References);
            if(referenceDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _settings.References = referenceDialog.References;
            await scriptControl1.SetReferences(_settings);
            SaveSettings();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentCode();
        }

        private void SaveCurrentCode()
        {
            using var saveFileDialog = new SaveFileDialog { Filter = "CSharp file (*.cs)|*.cs" };
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _settings.CurrentSavePath = saveFileDialog.FileName;
            SaveSettings();
            File.WriteAllText(saveFileDialog.FileName, scriptControl1.GetCode());
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void LoadFile()
        {
            using var openFileDialog = new OpenFileDialog { Filter = "CSharp file (*.cs)|*.cs" };
            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _settings.CurrentSavePath = openFileDialog.FileName;
            SaveSettings();
            scriptControl1.SetCode(File.ReadAllText(openFileDialog.FileName));
        }

        private void CurrentCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var references = AssemblyHelper.GetAssemblies(_settings.References, out _).ToMetadata();
            if(!OscetchCompiler.Compile(scriptControl1.GetCode(), _settings.AssemblyName, references, 
                out var tempDll, out var diagnostics))
            {
                MessageBox.Show("Compilation failed\n" + string.Join('\n', diagnostics));
                return;
            }

            File.Copy(tempDll, _settings.OutputPath, true);
            File.Delete(tempDll);
        }

        private async void SyntaxHighlightingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var syntaxColorOptionDialog = new SyntaxColorOptionDialog(_settings.SyntaxDisplayOptions))
            {
                if (syntaxColorOptionDialog.ShowDialog() != DialogResult.OK) 
                {
                    return;
                }

                _settings.SyntaxDisplayOptions = syntaxColorOptionDialog.GetDisplayOptions();
                await scriptControl1.SetReferences(_settings);
                SaveSettings();
            }
        }

        private void RegularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var dirDialog = new FolderBrowserDialog { SelectedPath = _settings.BuildDirectory })
            {
                if(dirDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _settings.BuildDirectory = dirDialog.SelectedPath;
                SaveSettings();
                BuildFolder(dirDialog.SelectedPath);
            }
        }

        private List<SyntaxTree> RecursiveSyntaxTreeCheck(string path)
        {
            var syntaxTrees = new List<SyntaxTree>();
            foreach (var filePath in Directory.GetFiles(path, "*.cs"))
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
                syntaxTrees.Add(syntaxTree);
            }

            foreach(var dir in Directory.GetDirectories(path))
            {
                syntaxTrees.AddRange(RecursiveSyntaxTreeCheck(dir));
            }

            return syntaxTrees;
        }

        private void BuildFolder(string path)
        {
            var syntaxTrees = new List<SyntaxTree>();
            foreach(var filePath in Directory.GetFiles(path, "*.cs"))
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
                syntaxTrees.Add(syntaxTree);
            }

            BuildFromSyntaxTrees(syntaxTrees);
        }

        private void BuildFromSyntaxTrees(IEnumerable<SyntaxTree> syntaxTrees)
        {
            var references = AssemblyHelper.GetAssemblies(_settings.References, out _).ToMetadata();
            if (!OscetchCompiler.Compile(_settings.AssemblyName, syntaxTrees, references,
                out var tempDll, out var diagnostics))
            {
                // todo diagnostics
                MessageBox.Show("Compilation failed\n" + string.Join('\n', diagnostics));
                return;
            }

            File.Copy(tempDll, _settings.OutputPath, true);
            File.Delete(tempDll);
        }

        private void RecursiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dirDialog = new FolderBrowserDialog { SelectedPath = _settings.BuildDirectory })
            {
                if (dirDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                _settings.BuildDirectory = dirDialog.SelectedPath;
                SaveSettings();
                BuildFolder(dirDialog.SelectedPath);
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNew();
        }

        private void CreateNew()
        {
            scriptControl1.SetCode(string.Empty);
            _settings.CurrentSavePath = $"{Guid.NewGuid()}.cs";
            SaveSettings();
        }

        private void SetBuildOutputPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var filePathDialog = new FilePathDialog("DLL file(*.dll)|*.dll", _settings.OutputPath);
            if(filePathDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _settings.OutputPath = filePathDialog.Path;
            SaveSettings();
        }
    }
}
