namespace MockEverythingTests.Inspection
{
    using System;
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection.MonoCecil;
    using Demo;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using DemoSigned;
    using System.IO;

    [TestClass]
    public class MonoAssemblyPublicKeyTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestReplacePublicKeyWrongType()
        {
            new Assembly(this.GetAssemblyPath<SimpleClass>()).ReplacePublicKey(new AssemblyStub("StubAssembly"));
        }

        [TestMethod]
        public void TestReplacePublicKey()
        {
            var originalKey = typeof(SimpleClass).Assembly.GetName().GetPublicKey();
            var newKey = typeof(ClassFromSignedAssembly).Assembly.GetName().GetPublicKey();
            CollectionAssert.AreNotEqual(originalKey, newKey);

            var tempPath = Path.GetTempFileName();
            var assembly = new Assembly(this.GetAssemblyPath<SimpleClass>());
            var signedAssembly = new Assembly(this.GetAssemblyPath<ClassFromSignedAssembly>());
            assembly.ReplacePublicKey(signedAssembly);
            assembly.Save(tempPath);

            var actual = System.Reflection.Assembly.Load(File.ReadAllBytes(tempPath)).GetName().GetPublicKey();
            var expected = newKey;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestReplacePublicKeyToken()
        {
            var originalKey = typeof(SimpleClass).Assembly.GetName().GetPublicKeyToken();
            var newKey = typeof(ClassFromSignedAssembly).Assembly.GetName().GetPublicKeyToken();
            CollectionAssert.AreNotEqual(originalKey, newKey);

            var tempPath = Path.GetTempFileName();
            var assembly = new Assembly(this.GetAssemblyPath<SimpleClass>());
            var signedAssembly = new Assembly(this.GetAssemblyPath<ClassFromSignedAssembly>());
            assembly.ReplacePublicKey(signedAssembly);
            assembly.Save(tempPath);

            var actual = System.Reflection.Assembly.Load(File.ReadAllBytes(tempPath)).GetName().GetPublicKeyToken();
            var expected = newKey;
            CollectionAssert.AreEqual(expected, actual);
        }

        private string GetAssemblyPath<T>()
        {
            var codeBase = typeof(T).Assembly.CodeBase;
            return Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
        }
    }
}
