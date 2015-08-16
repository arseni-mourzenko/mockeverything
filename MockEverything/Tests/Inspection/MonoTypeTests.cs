namespace MockEverythingTests.Inspection
{
    using Demo;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection.MonoCecil;
    using System;
    using System.Linq;

    [TestClass]
    public class MonoTypeTests
    {
        [TestMethod]
        public void TestFindMethods()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "SimpleMethod"));
        }

        [TestMethod]
        public void TestFindMethodsMissing()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "MissingMethod"));
        }

        [TestMethod]
        public void TestFindMethodsPrivate()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "PrivateMethod"));
        }

        [TestMethod]
        public void TestFindMethodsProtected()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "ProtectedMethod"));
        }

        [TestMethod]
        public void TestFindMethodsInternal()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "InternalMethod"));
        }

        [TestMethod]
        public void TestFindMethodsStatic()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "StaticMethod"));
        }

        [TestMethod]
        public void TestFindMethodsPrivateStatic()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "PrivateStaticMethod"));
        }

        [TestMethod]
        public void TestFindMethodsProtectedStatic()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "ProtectedStaticMethod"));
        }

        [TestMethod]
        public void TestFindMethodsInternalStatic()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "InternalStaticMethod"));
        }


        private bool MethodExists(string typeName, string methodName)
        {
            var assembly = new Assembly(this.SampleAssemblyPath);
            return assembly
                .FindTypes()
                .Single(t => t.Name == typeName)
                .FindTypes()
                .Select(m => m.Name)
                .Contains(methodName);
        }

        private string SampleAssemblyPath
        {
            get
            {
                var codeBase = typeof(SimpleClass).Assembly.CodeBase;
                return Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
            }
        }
    }
}
