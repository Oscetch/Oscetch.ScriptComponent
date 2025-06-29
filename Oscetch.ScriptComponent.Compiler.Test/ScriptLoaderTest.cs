using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oscetch.ScriptComponent.Compiler.Extensions;
using Oscetch.ScriptComponent.Interfaces;
using System.Linq;
using System.IO;
using Oscetch.ScriptComponent.Compiler;
using Oscetch.ScriptComponent.Attributes;

namespace Oscetch.ScriptComponent.Test
{
    [TestClass]
    public class ScriptLoaderTest
    {
        public interface ITestScript : IScript
        {
            int Sum(int n1, int n2);
        }

        public class BuiltInScript : IScript
        {
            public int Test { get; } = 1;
        }

        public class BuiltInScriptWithParam : IScript
        {
            [ScriptParameter(name: "A")]
            public int A { get; set; }

            [ScriptParameter("B")]
            public bool B;

            [ScriptParameter("C")]
            public string C;
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

        [TestMethod]
        public void LoadBuiltInScript()
        {
            var type = typeof(BuiltInScript);
            var reference = new ScriptReference("dummy", type.FullName);
            var loadResult = ScriptLoader.TryLoadBuiltInScriptReference<BuiltInScript>(reference, out var script);

            Assert.IsTrue(loadResult);
            Assert.AreEqual(1, script.Test);
        }

        [TestMethod]
        public void LoadParams()
        {
            var type = typeof(BuiltInScriptWithParam);
            var reference = new ScriptReference("", type.FullName, [new ScriptValueParameter("A", "4"), new ScriptValueParameter("B", "true"), new ScriptValueParameter("C", "test")]);
            var loadResult = ScriptLoader.TryLoadBuiltInScriptReference<BuiltInScriptWithParam>(reference, out var script);

            Assert.IsTrue(loadResult);

            Assert.AreEqual(script.A, 4);
            Assert.IsTrue(script.B);
            Assert.AreEqual(script.C, "test");
        }
    }
}
