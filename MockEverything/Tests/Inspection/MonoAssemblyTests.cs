namespace MockEverythingTests.Inspection
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection.MonoCecil;
    using System;
    using System.Linq;
    using Reflection = System.Reflection;

    [TestClass]
    public class MonoAssemblyTests
    {
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