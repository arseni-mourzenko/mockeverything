namespace MockEverythingTests.Inspection
{
    using System.Linq;
    using Demo;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod]
        public void TestGetGenericTypesWithoutGenerics()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "SimpleMethod")
                .GenericTypes
                .Count();

            var expected = 0;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetGenericTypes()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "GenericSample")
                .GenericTypes
                .ToList();

            var expected = new[] { string.Empty, string.Empty };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetGenericTypesWithConstraints()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "GenericSampleWithConstraints")
                .GenericTypes
                .ToList();

            var expected = new[] { string.Empty, "System.Exception" };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetGenericTypesWithStructConstraint()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "GenericSampleWithStructContraint")
                .GenericTypes
                .ToList();

            // Notice that new() constraint is added automatically.
            var expected = new[] { string.Empty, "new(),System.ValueType" };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetGenericTypesWithNewConstraint()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "GenericSampleNew")
                .GenericTypes
                .ToList();

            var expected = new[] { string.Empty, "new()" };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetGenericTypesWithMultipleConstraints()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "GenericSampleMultiple")
                .GenericTypes
                .ToList();

            var expected = new[] { string.Empty, "new(),System.IComparable" };
            CollectionAssert.AreEqual(expected, actual);
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
