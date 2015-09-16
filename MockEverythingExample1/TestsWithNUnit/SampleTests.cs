namespace MockEverythingExample1.TestsWithNUnit
{
    using Exchanger;
    using Library;
    using NUnit.Framework;

    [TestFixture]
    public class SampleTests
    {
        [Test]
        public void TestFindName()
        {
            WebClientExchanger.PersonName = "Jeff";
            var actual = new Demo().FindName();
            var expected = "Jeff";
            Assert.AreEqual(expected, actual);
        }
    }
}
