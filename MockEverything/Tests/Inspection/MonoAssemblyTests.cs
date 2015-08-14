namespace MockEverythingTests.Inspection
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection.MonoCecil;
    using System;
    using System.IO;
    using System.Linq;
    using Reflection = System.Reflection;

    [TestClass]
    public class MonoAssemblyTests
    {
        [TestMethod]
        public void ConstructorWithWrongNameLazy()
        {
            // The assembly shouldn't be loaded when a constructor is called, but only when we actually need it. Thus, calling a constructor with a name shouldn't be a problem.
            var assembly = new Assembly(@"not a real path");
            Assert.IsNotNull(assembly);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ConstructorWithWrongName()
        {
            var assembly = new Assembly(@"not a real path");
            assembly.FindTypes().ToList();
        }

        [TestMethod]
        public void TestFindTypes()
        {
            var assembly = new Assembly(this.CurrentAssemblyPath);
            var result = assembly.FindTypes().Select(t => t.Name).Contains("MonoAssemblyTests");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFindTypesMissing()
        {
            var assembly = new Assembly(this.CurrentAssemblyPath);
            var result = assembly.FindTypes().Select(t => t.Name).Contains("MissingType");

            Assert.IsFalse(result);
        }

        private string CurrentAssemblyPath
        {
            get
            {
                var codeBase = Reflection.Assembly.GetExecutingAssembly().CodeBase;
                return Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
            }
        }
    }
}