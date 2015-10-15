namespace MockEverythingTests.Inspection
{
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection;

    [TestClass]
    public class ParameterTests
    {
        [TestMethod]
        public void TestGetType()
        {
            var type = new TypeStub("Hello", "Demo.Hello");
            var actual = new Parameter(ParameterVariant.In, type).Type;
            var expected = type;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetVariant()
        {
            var type = new TypeStub("Hello", "Demo.Hello");
            var actual = new Parameter(ParameterVariant.Ref, type).Variant;
            var expected = ParameterVariant.Ref;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEqualsAllSame()
        {
            var type = new TypeStub("Hello", "Demo.Hello");
            var p1 = new Parameter(ParameterVariant.In, type);
            var p2 = new Parameter(ParameterVariant.In, type);
            Assert.IsTrue(p1.Equals(p2));
        }

        [TestMethod]
        public void TestEqualsNull()
        {
            var p1 = new Parameter(ParameterVariant.In, new TypeStub("Hello", "Demo.Hello"));
            Assert.IsFalse(p1.Equals(null));
        }

        [TestMethod]
        public void TestEqualsWrongType()
        {
            var p1 = new Parameter(ParameterVariant.In, new TypeStub("Hello", "Sample.Hello"));
            Assert.IsFalse(p1.Equals(27));
        }

        [TestMethod]
        public void TestEqualsTypeDifferent()
        {
            var p1 = new Parameter(ParameterVariant.In, new TypeStub("Hello", "Sample.Hello"));
            var p2 = new Parameter(ParameterVariant.In, new TypeStub("World", "Sample.World"));
            Assert.IsFalse(p1.Equals(p2));
        }

        [TestMethod]
        public void TestEqualsVariantDifferent()
        {
            var type = new TypeStub("Hello", "Demo.Hello");
            var p1 = new Parameter(ParameterVariant.In, type);
            var p2 = new Parameter(ParameterVariant.Params, type);
            Assert.IsFalse(p1.Equals(p2));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var type = new TypeStub("Hello", "Demo.Hello");
            var p1 = new Parameter(ParameterVariant.In, type);
            var p2 = new Parameter(ParameterVariant.In, type);
            Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
        }
    }
}