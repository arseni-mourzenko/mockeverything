namespace MockEverythingTests.Attributes
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Attributes;

    [TestClass]
    public class ProxyMethodAttributeTests
    {
        [TestMethod]
        public void TestGetMethodType()
        {
            var actual = new ProxyMethodAttribute(TargetMethodType.Instance).MethodType;
            var expected = TargetMethodType.Instance;

            Assert.AreEqual(expected, actual);
        }
    }
}
