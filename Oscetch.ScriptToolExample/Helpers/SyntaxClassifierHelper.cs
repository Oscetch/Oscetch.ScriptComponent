using Microsoft.CodeAnalysis.Classification;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Oscetch.ScriptToolExample.Helpers
{
    public static class SyntaxClassifierHelper
    {
        private static List<string> _classifierTypeNames;

        public static IReadOnlyList<string> GetClassifierTypeNames()
        {
            // super safe and reliable.. I promise
            _classifierTypeNames ??= [.. typeof(ClassificationTypeNames)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (x.GetRawConstantValue()?.ToString()))];

            return _classifierTypeNames;
        }
    }
}
