namespace MockEverythingTests.Engine.Browsers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine.Browsers;

    [TestClass]
    public class PairTests
    {
        [TestMethod]
        public void TestGetProxy()
        {
            var actual = new Pair<int>(5, 6).Proxy;
            var expected = 5;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTarget()
        {
            var actual = new Pair<int>(5, 6).Target;
            var expected = 6;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEqualsAllSame()
        {
            var first = new Pair<int>(5, 6);
            var second = new Pair<int>(5, 6);
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
        }

        [TestMethod]
        public void TestEqualsNull()
        {
            var first = new Pair<int>(5, 6);
            Assert.IsFalse(first.Equals(null));
        }

        [TestMethod]
        public void TestEqualsWrongType()
        {
            var first = new Pair<int>(5, 6);
            Assert.IsFalse(first.Equals(27));
        }

        [TestMethod]
        public void TestEqualsProxyDifferent()
        {
            var first = new Pair<int>(5, 6);
            var second = new Pair<int>(14, 6);
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }

        [TestMethod]
        public void TestEqualsTargetDifferent()
        {
            var first = new Pair<int>(5, 6);
            var second = new Pair<int>(5, 14);
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }
    }
}
