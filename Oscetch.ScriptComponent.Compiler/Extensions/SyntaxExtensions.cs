using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Oscetch.ScriptComponent.Compiler.Extensions
{
    public static class SyntaxExtensions
    {
        /// <summary>
        /// Creates a <see cref="ScriptReference"/> for this <see cref="SyntaxTree"/>
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <param name="dllPath">The location of the dll this <see cref="SyntaxTree"/> can be found in</param>
        /// <returns></returns>
        public static IEnumerable<ScriptReference> GetScriptReferences(this SyntaxTree syntaxTree, string dllPath)
        {
            return syntaxTree.GetClassNames().Select(x => new ScriptReference(dllPath, x));
        }

        /// <summary>
        /// Gets all class names declared in this <see cref="SyntaxTree"/>
        /// </summary>
        /// <param name="syntaxTree"></param>
        /// <returns>A list of class names</returns>
        public static IEnumerable<string> GetClassNames(this SyntaxTree syntaxTree)
        {
            var classDeclarationSyntaxes = syntaxTree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .ToList();

            foreach (var classSyntax in classDeclarationSyntaxes)
            {
                if (classSyntax.TryGetParentSyntax(out NamespaceDeclarationSyntax namespaceSyntax))
                {
                    yield return $"{namespaceSyntax.Name}.{classSyntax.Identifier}";
                }
                else
                {
                    yield return classSyntax.Identifier.ToString();
                }
            }
        }

        /// <summary>
        /// Attempts to find a parent node of the <typeparamref name="T"/> type
        /// </summary>
        /// <typeparam name="T">A <see cref="SyntaxNode"/> type</typeparam>
        /// <param name="syntaxNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetParentSyntax<T>(this SyntaxNode syntaxNode, out T result) where T : SyntaxNode
        {
            result = null;

            if (syntaxNode == null)
            {
                return false;
            }

            try
            {
                syntaxNode = syntaxNode.Parent;

                if (syntaxNode == null)
                {
                    return false;
                }

                if (syntaxNode.GetType() == typeof(T))
                {
                    result = syntaxNode as T;
                    return true;
                }

                return TryGetParentSyntax(syntaxNode, out result);
            }
            catch
            {
                return false;
            }
        }
    }
}
