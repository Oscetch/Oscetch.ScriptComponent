using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Oscetch.ScriptComponent.Compiler.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Loads a <see cref="Assembly"/> instance along with all its referenced assemblies
        /// </summary>
        /// <param name="typeInAssembly"></param>
        /// <returns></returns>
        public static List<Assembly> LoadAllReferences(this Type typeInAssembly)
        {
            var owningAssembly = Assembly.GetAssembly(typeInAssembly);
            var assemblyList = new List<Assembly> { owningAssembly };
            assemblyList.AddRange(owningAssembly.GetReferencedAssemblies()
                .Select(x => Assembly.Load(x))
                .Where(x => x?.Location != null));

            // I thought this would always be included but apparently isn't in some cases..
            var mscorlib = typeof(object).Assembly;
            if (assemblyList.All(x => x.FullName != mscorlib.FullName))
            {
                assemblyList.Add(mscorlib);
            }

            return assemblyList;
        }
    }
}
