namespace MockEverythingTests.Inspection
{
    using System.Linq;
    using Demo;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection;
    using MockEverything.Inspection.MonoCecil;
    using CommonStubs;

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

        [TestMethod]
        public void TestEqualsAllSame()
        {
            var first = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "SimpleMethod");

            var second = new MethodStub("SimpleMethod");

            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
        }

        [TestMethod]
        public void TestGetParameters()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "WithParameters")
                .Parameters
                .ToList();

            var expected = new[]
            {
                new Parameter(ParameterVariant.In, new TypeStub("String") { FullName = "System.String" }),
                new Parameter(ParameterVariant.In, new TypeStub("Int32") { FullName = "System.Int32" }),
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetParametersOut()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "WithOutParameters")
                .Parameters
                .ToList();

            var expected = new[] { new Parameter(ParameterVariant.Out, new TypeStub("String") { FullName = "System.String" }) };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetParametersRef()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "WithRefParameters")
                .Parameters
                .ToList();

            var expected = new[] { new Parameter(ParameterVariant.Ref, new TypeStub("String") { FullName = "System.String" }) };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetParametersInfinite()
        {
            var actual = this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == "SimpleClass")
                .FindMethods()
                .Single(m => m.Name == "WithInfiniteParams")
                .Parameters
                .ToList();

            var expected = new[] { new Parameter(ParameterVariant.Params, new TypeStub("String") { FullName = "System.String" }) };

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
