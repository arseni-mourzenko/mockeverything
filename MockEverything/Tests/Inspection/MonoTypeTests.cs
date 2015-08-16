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
            var assembly = new Assembly(this.SampleAssemblyPath);
            var result = assembly.FindTypes().Single(t => t.Name == "SimpleClass").FindTypes().Select(m => m.Name).Contains("SimpleMethod");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFindMethodsMissing()
        {
            var assembly = new Assembly(this.SampleAssemblyPath);
            var result = assembly.FindTypes().Single(t => t.Name == "SimpleClass").FindTypes().Select(m => m.Name).Contains("MissingMethod");

            Assert.IsFalse(result);
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
