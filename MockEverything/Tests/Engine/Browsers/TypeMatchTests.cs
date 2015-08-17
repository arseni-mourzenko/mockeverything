namespace MockEverythingTests.Engine.Browsers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine.Browsers;
    using Stubs;

    [TestClass]
    public class TypeMatchTests
    {
        [TestMethod]
        public void TestGetProxy()
        {
            var actual = new TypeMatch(new TypeStub("SampleProxy"), new TypeStub("SampleTarget")).Proxy.Name;
            var expected = "SampleProxy";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTarget()
        {
            var actual = new TypeMatch(new TypeStub("SampleProxy"), new TypeStub("SampleTarget")).Target.Name;
            var expected = "SampleTarget";

            Assert.AreEqual(expected, actual);
        }
    }
}
