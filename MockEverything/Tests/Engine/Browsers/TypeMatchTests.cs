namespace MockEverythingTests.Engine.Browsers
{
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine.Browsers;
    using MockEverything.Inspection;

    [TestClass]
    public class TypeMatchTests
    {
        [TestMethod]
        public void TestGetProxy()
        {
            var actual = new Pair<IType>(
                new TypeStub("SampleProxy", "Demo.SampleProxy"),
                new TypeStub("SampleTarget", "Demo.SampleTarget")).Proxy.Name;

            var expected = "SampleProxy";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetTarget()
        {
            var actual = new Pair<IType>(
                new TypeStub("SampleProxy", "Demo.SampleProxy"),
                new TypeStub("SampleTarget", "Demo.SampleTarget")).Target.Name;

            var expected = "SampleTarget";

            Assert.AreEqual(expected, actual);
        }
    }
}
