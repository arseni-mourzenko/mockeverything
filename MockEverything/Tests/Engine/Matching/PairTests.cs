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
    }
}
