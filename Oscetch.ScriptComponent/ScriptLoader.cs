using Oscetch.ScriptComponent.Attributes;
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
        private static readonly Dictionary<string, Assembly> _loadedAssemblies = [];

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

        private static void SetParameters<T>(T instance, List<ScriptValueParameter> parameters)
        {
            var dict = parameters.ToDictionary(x => x.Name);
            var fields = instance.GetType().GetFields();
            foreach (var field in fields)
            {
                foreach (var attribute in field.GetCustomAttributes<ScriptParameter>())
                {
                    if (!dict.TryGetValue(attribute.Name, out var param))
                    {
                        Debug.WriteLine($"Unable to find {attribute.Name} value for field {field.Name}");
                        continue;
                    }

                    var typedValue = Convert.ChangeType(param.Value, field.FieldType);
                    field.SetValue(instance, typedValue);
                }
            }
            var properties = instance.GetType().GetProperties();
            foreach (var property in properties)
            {
                foreach (var attribute in property.GetCustomAttributes<ScriptParameter>())
                {
                    if (!dict.TryGetValue(attribute.Name, out var param))
                    {
                        Debug.WriteLine($"Unable to find {attribute.Name} value for property {property.Name}");
                        continue;
                    }

                    var typedValue = Convert.ChangeType(param.Value, property.PropertyType);
                    property.SetValue(instance, typedValue);
                }
            }
        }

        private static bool TryInstantiateScript<T>(Assembly assembly, string scriptClassName, List<ScriptValueParameter> parameters, out T script)
            where T : IScript
        {
            var scriptType = assembly.GetTypes().FirstOrDefault(x => x.FullName == scriptClassName);
            if (scriptType != null)
            {
                script = (T)Activator.CreateInstance(scriptType);
                SetParameters(script, parameters);
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
                    && TryInstantiateScript(assembly, scriptReference.ScriptClassName, scriptReference.Params, out script)
                : (_loadedAssemblies.TryGetValue(scriptReference.DllPath, out assembly)
                    || TryLoadAssembly(scriptReference.DllPath, out assembly))
                        && TryInstantiateScript(assembly, scriptReference.ScriptClassName, scriptReference.Params, out script);
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
                    .FirstOrDefault(x => x.GetType(scriptReference.ScriptClassName) != null);
                if (assembly == null)
                {
                    return false;
                }

                _loadedAssemblies[scriptReference.DllPath] = assembly;
            }

            return TryInstantiateScript(assembly, scriptReference.ScriptClassName, scriptReference.Params, out script);
        }
    }
}
