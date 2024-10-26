using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oscetch.ScriptComponent.Compiler.Extensions;
using Oscetch.ScriptComponent.Interfaces;
using System.Linq;
using System.IO;

namespace Oscetch.ScriptComponent.Test
{
    [TestClass]
    public class ScriptLoaderTest
    {
        public interface ITestScript : IScript
        {
            int Sum(int n1, int n2);
        }

        private const string TEST_CODE = @"
using Oscetch.ScriptComponent.Test;

namespace SomeAssembly
{
    public class Test : ScriptLoaderTest.ITestScript
    {
        public int Sum(int n1, int n2)
        {
            return n1 + n2;
        }
    }
}
";

        [TestMethod]
        public void TestScript()
        {
            var execReferences = typeof(ITestScript).LoadAllReferences().ToMetadata();
            var syntaxTree = CSharpSyntaxTree.ParseText(TEST_CODE);
            var compileResult = OscetchCompiler.Compile("TestAssembly", [syntaxTree], execReferences,
                out var tempPath, out _);

            Assert.IsTrue(compileResult);

            var scriptReference = new ScriptReference(tempPath, 
                syntaxTree.GetClassNames().First());

            var loadResult = ScriptLoader.TryLoadScriptReference<ITestScript>(scriptReference, out var script);
            File.Delete(tempPath);

            Assert.IsTrue(loadResult);

            var n1 = 1;
            var n2 = 2;
            var expected = n1 + n2;
            var testResult = script.Sum(n1, n2);

            Assert.AreEqual(expected, testResult);
        }
    }
}
