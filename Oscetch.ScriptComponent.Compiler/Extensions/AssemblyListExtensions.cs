using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Oscetch.ScriptComponent.Compiler.Extensions
{
    public static class AssemblyListExtensions
    {
        /// <summary>
        /// Mutates this list of <see cref="Assembly"/> to a list of <see cref="MetadataReference"/>
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static List<PortableExecutableReference> ToMetadata(this IEnumerable<Assembly> assemblies)
        {
            return [.. assemblies
                .Where(x => x?.Location != null)
                .Select(x => MetadataReference.CreateFromFile(x.Location))];
        }
    }
}
