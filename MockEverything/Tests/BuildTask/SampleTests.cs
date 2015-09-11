namespace MockEverythingTests.BuildTask
{
    using BuildTaskLibrary;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BuildTaskExchanger;

    [TestClass]
    public class SampleTests
    {
        [TestMethod]
        public void TestFindName()
        {
            WebClientExchanger.PersonName = "Jeff";
            var actual = new Demo().FindName();
            var expected = "Jeff";
            Assert.AreEqual(expected, actual);
        }
    }
}