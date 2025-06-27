using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;
using Oscetch.ScriptComponent.Compiler.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Oscetch.ScriptComponent.Compiler
{
    /// <summary>
    /// A simple API for interacting with roslyn compilation
    /// </summary>
    public static class OscetchCompiler
    {
        /// <summary>
        /// The platform you want to compile for
        /// </summary>
        public static Platform TargetPlatform { get; set; } = Platform.AnyCpu;

        /// <summary>
        /// Creates a <see cref="Document"/> which can be used for syntax highlighting, suggestions and lots of other things
        /// </summary>
        /// <param name="assemblyName">The name of the assembly</param>
        /// <param name="hostedAssemblies">Referenced dlls</param>
        /// <param name="documentName">Name of the document</param>
        /// <returns></returns>
        public static Document CreateDocument(string assemblyName,
            IEnumerable<Assembly> hostedAssemblies, 
            string documentName = "Script")
        {
            var mefHostRequiredAssemblies = new List<Assembly>
            {
                Assembly.Load("Microsoft.CodeAnalysis"),
                Assembly.Load("Microsoft.CodeAnalysis.CSharp"),
                Assembly.Load("Microsoft.CodeAnalysis.Features"),
                Assembly.Load("Microsoft.CodeAnalysis.CSharp.Features")
            };

            if (hostedAssemblies != null)
            {
                mefHostRequiredAssemblies.AddRange(hostedAssemblies);
            }

            var partTypes = MefHostServices.DefaultAssemblies.Concat(mefHostRequiredAssemblies)
                .Distinct()
                .SelectMany(x => x.GetTypes())
                .ToArray();

            var compositionContext = new ContainerConfiguration()
                .WithParts(partTypes)
                .CreateContainer();

            var workspace = new AdhocWorkspace(MefHostServices.Create(compositionContext));
            var project = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(),
                assemblyName, assemblyName, LanguageNames.CSharp)
                .WithMetadataReferences(mefHostRequiredAssemblies.ToMetadata());

            var documentId = DocumentId.CreateNewId(project.Id);
            var documentInfo = DocumentInfo.Create(documentId, documentName,
                    loader: TextLoader.From(
                        TextAndVersion.Create(
                            SourceText.From(string.Empty), VersionStamp.Create())
                        )
                    );


            return workspace.CurrentSolution.AddProject(project)
                .AddDocument(documentInfo)
                .GetDocument(documentId);
        }

        /// <summary>
        /// Compiles <paramref name="code"/> into a dll located at the <paramref name="tempDllPath"/>
        /// </summary>
        /// <param name="code">The c# code you want to compile</param>
        /// <param name="assemblyName">The name of the assembly</param>
        /// <param name="referensMetadata">Referenced dlls</param>
        /// <param name="tempDllPath">The path to the resulting dll</param>
        /// <param name="diagnostics">Information, Warnings and Errors reported from roslyn</param>
        /// <returns>True if the compilation succeeded, false otherwise</returns>
        public static bool Compile(string code, string assemblyName, 
            IEnumerable<PortableExecutableReference> referensMetadata, 
            out string tempDllPath, out ImmutableArray<Diagnostic> diagnostics)
        {
            var dllPath = Path.Combine(Path.GetTempPath(), $"{assemblyName}.dll");

            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release, platform: TargetPlatform);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var roslynCompilator = CSharpCompilation.Create(assemblyName, 
                [syntaxTree], referensMetadata, compilationOptions);
            var result = roslynCompilator.Emit(dllPath);

            tempDllPath = dllPath;
            diagnostics = result.Diagnostics;
            if (result.Success)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compiles <paramref name="source"/> into a dll located at <paramref name="tempDllPath"/>
        /// </summary>
        /// <param name="assemblyName">Name of the assembly</param>
        /// <param name="source">The code you're compiling</param>
        /// <param name="referensMetadata">Referenced dlls</param>
        /// <param name="tempDllPath">The path to the resulting dll</param>
        /// <param name="diagnostics">Information, Warnings and Errors reported from roslyn</param>
        /// <returns>True if the compilation succeeded, false otherwise</returns>
        public static bool Compile(string assemblyName, IEnumerable<SyntaxTree> source, 
            IEnumerable<PortableExecutableReference> referensMetadata,
            out string tempDllPath, out ImmutableArray<Diagnostic> diagnostics)
        {
            var dllPath = Path.Combine(Path.GetTempPath(), $"{assemblyName}.dll");

            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release, platform: TargetPlatform);

            var roslynCompilator = CSharpCompilation.Create(assemblyName, source, referensMetadata, compilationOptions);
            var result = roslynCompilator.Emit(dllPath);

            tempDllPath = dllPath;

            diagnostics = result.Diagnostics;

            if (result.Success)
            {
                return true;
            }

            return false;
        }
    }
}
