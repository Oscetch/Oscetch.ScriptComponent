using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Osctech.ScriptToolExample.Models
{
    public class SyntaxDisplayOptions
    {
        public string Classification { get; }
        public Color TextColor { get; set; } = Color.Black;
        public bool IsBold { get; set; }

        public SyntaxDisplayOptions(string classification)
        {
            Classification = classification;
        }
    }
}
