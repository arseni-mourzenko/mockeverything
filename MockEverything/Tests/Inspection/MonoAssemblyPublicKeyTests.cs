namespace MockEverythingTests.Inspection
{
    using System;
    using System.IO;
    using CommonStubs;
    using Demo;
    using DemoSigned;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection.MonoCecil;

    [TestClass]
    public class MonoAssemblyPublicKeyTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestReplacePublicKeyWrongType()
        {
            new Assembly(this.FindAssemblyPathOf<SimpleClass>()).ReplacePublicKey(new AssemblyStub("StubAssembly"));
        }

        [TestMethod]
        [TestCategory("System tests")]
        public void TestReplacePublicKey()
        {
            var originalKey = typeof(SimpleClass).Assembly.GetName().GetPublicKey();
            var newKey = typeof(ClassFromSignedAssembly).Assembly.GetName().GetPublicKey();
            CollectionAssert.AreNotEqual(originalKey, newKey);

            var tempPath = Path.GetTempFileName();
            var assembly = new Assembly(this.FindAssemblyPathOf<SimpleClass>());
            var signedAssembly = new Assembly(this.FindAssemblyPathOf<ClassFromSignedAssembly>());
            assembly.ReplacePublicKey(signedAssembly);
            assembly.Save(tempPath);

            var actual = System.Reflection.Assembly.Load(File.ReadAllBytes(tempPath)).GetName().GetPublicKey();
            var expected = newKey;
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("System tests")]
        public void TestReplacePublicKeyToken()
        {
            var originalKey = typeof(SimpleClass).Assembly.GetName().GetPublicKeyToken();
            var newKey = typeof(ClassFromSignedAssembly).Assembly.GetName().GetPublicKeyToken();
            CollectionAssert.AreNotEqual(originalKey, newKey);

            var tempPath = Path.GetTempFileName();
            var assembly = new Assembly(this.FindAssemblyPathOf<SimpleClass>());
            var signedAssembly = new Assembly(this.FindAssemblyPathOf<ClassFromSignedAssembly>());
            assembly.ReplacePublicKey(signedAssembly);
            assembly.Save(tempPath);

            var actual = System.Reflection.Assembly.Load(File.ReadAllBytes(tempPath)).GetName().GetPublicKeyToken();
            var expected = newKey;
            CollectionAssert.AreEqual(expected, actual);
        }

        private string FindAssemblyPathOf<T>()
        {
            var codeBase = typeof(T).Assembly.CodeBase;
            return Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
        }
    }
}
