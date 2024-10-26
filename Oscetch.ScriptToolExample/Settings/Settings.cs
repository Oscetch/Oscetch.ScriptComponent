using Microsoft.CodeAnalysis.Classification;
using Oscetch.ScriptToolExample.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Oscetch.ScriptToolExample.Settings
{
    public class Settings
    {
        public List<string> References { get; set; } = [];
        public string AssemblyName { get; set; } = "Default";
        public string BuildDirectory { get; set; } = Environment.CurrentDirectory;
        public string OutputPath { get; set; } = "Default.dll";
        public string CurrentSavePath { get; set; } = $"{Guid.NewGuid()}.cs";
        public Dictionary<string, SyntaxDisplayOptions> SyntaxDisplayOptions { get; set; } = [];

        public void SetDefaultSyntaxDisplayOptions()
        {
            SyntaxDisplayOptions.Clear();
            SyntaxDisplayOptions.Add(ClassificationTypeNames.EnumName,
                new Models.SyntaxDisplayOptions(ClassificationTypeNames.EnumName)
                {
                    IsBold = true,
                    TextColor = Color.Brown
                });
            SyntaxDisplayOptions.Add(ClassificationTypeNames.Keyword,
                new Models.SyntaxDisplayOptions(ClassificationTypeNames.Keyword)
                {
                    IsBold = false,
                    TextColor = Color.DarkBlue
                });
            SyntaxDisplayOptions.Add(ClassificationTypeNames.ClassName,
                new Models.SyntaxDisplayOptions(ClassificationTypeNames.ClassName)
                {
                    IsBold = true,
                    TextColor = Color.DarkCyan
                });
            SyntaxDisplayOptions.Add(ClassificationTypeNames.StringLiteral,
                new Models.SyntaxDisplayOptions(ClassificationTypeNames.StringLiteral)
                {
                    IsBold = false,
                    TextColor = Color.Red
                });
        }
    }
}
