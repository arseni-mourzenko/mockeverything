using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace MockEverythingTests.Inspection
{
    using System.Linq;
    using Demo;
    using MockEverything.Inspection.MonoCecil;

    [TestClass]
    public class MonoMethodTests
    {
        [TestMethod]
        public void TestGetName()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "SimpleMethod")
                .Name;

            var expected = "SimpleMethod";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetReturnTypeVoid()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "SimpleMethod")
                .ReturnType
                .FullName;

            var expected = "System.Void";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetReturnTypeCustom()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "get_SimpleProperty")
                .ReturnType
                .FullName;

            var expected = "System.String";
            Assert.AreEqual(expected, actual);
        }

        private Assembly SampleAssembly
        {
            get
            {
                return new Assembly(this.SampleAssemblyPath);
            }
        }

        private string SampleAssemblyPath
        {
            get
            {
                var codeBase = typeof(SimpleClass).Assembly.CodeBase;
                return System.Uri.UnescapeDataString(new System.UriBuilder(codeBase).Path);
            }
        }
    }
}
