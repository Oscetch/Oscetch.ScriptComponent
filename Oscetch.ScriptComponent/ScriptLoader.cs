using Oscetch.ScriptComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Oscetch.ScriptComponent
{
    public static class ScriptLoader
    {
        private static readonly Dictionary<string, Assembly> _loadedAssemblies = new();

        private static bool TryLoadAssembly(string dllPath, out Assembly assembly)
        {
            try
            {
                if (File.Exists(dllPath))
                {
                    assembly = Assembly.Load(File.ReadAllBytes(dllPath));
                    _loadedAssemblies[dllPath] = assembly;
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            assembly = null;
            return false;
        }

        private static bool TryInstantiateScript<T>(Assembly assembly, string scriptClassName, out T script)
            where T : IScript
        {
            var scriptType = assembly.GetTypes().FirstOrDefault(x => x.FullName == scriptClassName);
            if (scriptType != null)
            {
                script = (T)Activator.CreateInstance(scriptType);
                return true;
            }

            script = default;
            return false;
        }

        /// <summary>
        /// Loads a new instance of the script class referenced by the <see cref="ScriptReference"/>
        /// </summary>
        /// <typeparam name="T">The script type</typeparam>
        /// <param name="scriptReference"></param>
        /// <param name="script"></param>
        /// <param name="forceReload">True if you want to ignore cached assemblies and reload the dll from disk</param>
        /// <returns></returns>
        public static bool TryLoadScriptReference<T>(ScriptReference scriptReference, out T script, bool forceReload = false)
            where T : IScript
        {
            script = default;
            return forceReload
                ? TryLoadAssembly(scriptReference.DllPath, out var assembly)
                    && TryInstantiateScript(assembly, scriptReference.ScriptClassName, out script)
                : (_loadedAssemblies.TryGetValue(scriptReference.DllPath, out assembly)
                    || TryLoadAssembly(scriptReference.DllPath, out assembly))
                        && TryInstantiateScript(assembly, scriptReference.ScriptClassName, out script);
        }

        /// <summary>
        /// Loads scripts that are already defined in the current domain, i.e. scripts that are present in the project running this method.
        /// It will not try to instantiate another instance of the assembly. 
        /// forceReload is just for updating the internal dictionary of already loaded assemblies
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scriptReference"></param>
        /// <param name="script"></param>
        /// <param name="forceReload"></param>
        /// <returns></returns>
        public static bool TryLoadBuiltInScriptReference<T>(ScriptReference scriptReference, out T script, bool forceReload = false)
            where T : IScript
        {
            script = default;
            if (!_loadedAssemblies.TryGetValue(scriptReference.DllPath, out var assembly) || forceReload)
            {
                assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => Path.GetFileName(x.Location) == scriptReference.DllPath);
                if (assembly == null)
                {
                    return false;
                }

                _loadedAssemblies[scriptReference.DllPath] = assembly;
            }

            return TryInstantiateScript(assembly, scriptReference.ScriptClassName, out script);
        }
    }
}
