namespace MockEverythingTests.Attributes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Attributes;

    [TestClass]
    public class ProxyOfAttributeTests
    {
        [TestMethod]
        public void TestGetTargetType()
        {
            var actual = new ProxyOfAttribute(typeof(int)).TargetType;
            var expected = typeof(int);

            Assert.AreEqual(expected, actual);
        }
    }
}