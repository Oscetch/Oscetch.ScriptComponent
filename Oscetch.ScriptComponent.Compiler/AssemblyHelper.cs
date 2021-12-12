﻿using Oscetch.ScriptComponent.Compiler.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Oscetch.ScriptComponent.Compiler
{
    public static class AssemblyHelper
    {
        public static List<Assembly> GetAssemblies(IEnumerable<string> dllPaths, out List<string> errorMessages)
        {
            var references = typeof(object).LoadAllReferences();
            errorMessages = new List<string>();
            foreach (var dll in dllPaths)
            {
                try
                {
                    var owningAssembly = Assembly.LoadFrom(dll);
                    var assemblyList = new List<Assembly> { owningAssembly };
                    references.AddRange(owningAssembly.GetReferencedAssemblies()
                        .Select(x => Assembly.Load(x))
                        .Where(x => x?.Location != null && !references.Contains(x)));
                }
                catch (Exception e)
                {
                    var errorMessage = $"Failed to load references from:\n{dll}\nError:\n{e.Message}";
                    Debug.WriteLine(errorMessage);
                    errorMessages.Add(errorMessage);
                }
            }

            return references;
        }
    }
}
