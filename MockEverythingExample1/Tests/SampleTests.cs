namespace MockEverythingExample1.Tests
{
    using Exchanger;
    using Library;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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