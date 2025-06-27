using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oscetch.ScriptComponent.Compiler;
using Oscetch.ScriptComponent.Compiler.Extensions;
using Oscetch.ScriptComponent.Interfaces;
using System.IO;
using System.Reflection;

namespace Oscetch.ScriptComponent.Test
{
    [TestClass]
    public class CompilerTest
    {
        private const string TEST_CODE = @"
namespace SomeAssembly
{
    public class Test
    {
    }
}
";

        private const string TEST_CODE2 = @"
namespace SomeAssembly
{
    public class Test2
    {
    }
}
";

        [TestMethod]
        public void SyntaxTreeCompileTest()
        {
            var execReferences = typeof(IScript).LoadAllReferences().ToMetadata();
            var syntaxTree = CSharpSyntaxTree.ParseText(TEST_CODE);
            var syntaxTree2 = CSharpSyntaxTree.ParseText(TEST_CODE2);
            var result = OscetchCompiler.Compile("SomeAssembly", [syntaxTree, syntaxTree2], execReferences,
                out var tempPath, out _);

            Assert.IsTrue(result);

            var bytes = File.ReadAllBytes(tempPath);
            var assembly = Assembly.Load(bytes);

            Assert.IsTrue(assembly != null);

            File.Delete(tempPath);
        }

        [TestMethod]
        public void CodeCompileTest()
        {
            var execReferences = typeof(IScript).LoadAllReferences().ToMetadata();
            var result = OscetchCompiler.Compile(TEST_CODE, "TestAssembly", execReferences,
                out var tempPath, out _);

            Assert.IsTrue(result);

            var bytes = File.ReadAllBytes(tempPath);
            var assembly = Assembly.Load(bytes);

            Assert.IsTrue(assembly != null);

            File.Delete(tempPath);
        }
    }
}
