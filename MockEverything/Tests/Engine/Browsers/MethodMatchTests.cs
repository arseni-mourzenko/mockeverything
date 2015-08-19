namespace MockEverythingTests.Engine.Browsers
{
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine.Browsers;
    using MockEverything.Inspection;

    [TestClass]
    public class MethodMatchTests
    {
        [TestMethod]
        public void TestGetProxy()
        {
            var actual = new Match<IMethod>(new MethodStub("SampleProxy"), new MethodStub("SampleTarget")).Proxy.Name;
            var expected = "SampleProxy";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTarget()
        {
            var actual = new Match<IMethod>(new MethodStub("SampleProxy"), new MethodStub("SampleTarget")).Target.Name;
            var expected = "SampleTarget";

            Assert.AreEqual(expected, actual);
        }
    }
}
