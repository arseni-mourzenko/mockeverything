namespace MockEverythingTests.Engine.Browsers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine;
    using MockEverything.Engine.Browsers;
    using Stubs;

    [TestClass]
    public class MethodMatchSearchTests
    {
        [TestMethod]
        public void TestFindMatch()
        {
            var actual = new MethodMatchSearch().FindMatch(
                new MethodStub("DemoMethod"),
                new TypeStub("TargetType", new MethodStub("DemoMethod"))).Name;

            var expected = "DemoMethod";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void TestFindMatchMissing()
        {
            new MethodMatchSearch().FindMatch(
                new MethodStub("DemoMethod"),
                new TypeStub("TargetType", new MethodStub("HelloWorld")));
        }
    }
}