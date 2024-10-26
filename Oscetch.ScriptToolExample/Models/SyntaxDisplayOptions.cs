using System.Drawing;

namespace Oscetch.ScriptToolExample.Models
{
    public class SyntaxDisplayOptions(string classification)
    {
        public string Classification { get; } = classification;
        public Color TextColor { get; set; } = Color.Black;
        public bool IsBold { get; set; }
    }
}
