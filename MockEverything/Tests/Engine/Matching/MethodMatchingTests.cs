﻿namespace MockEverythingTests.Engine.Browsers
{
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine;
    using MockEverything.Engine.Browsers;

    [TestClass]
    public class MethodMatchingTests
    {
        [TestMethod]
        public void TestFindMatch()
        {
            var actual = new MethodMatching().FindMatch(
                new MethodStub("DemoMethod"),
                new TypeStub("TargetType", "Demo.TargetType", new MethodStub("DemoMethod"))).Name;

            var expected = "DemoMethod";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void TestFindMatchMissing()
        {
            new MethodMatching().FindMatch(
                new MethodStub("DemoMethod"),
                new TypeStub("TargetType", "Demo.TargetType", new MethodStub("HelloWorld")));
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void TestFindMatchDifferentReturnTypes()
        {
            var type1 = new TypeStub("Type1", "Stubs.Type1");
            var type2 = new TypeStub("Type2", "Stubs.Type2");

            new MethodMatching().FindMatch(
                new MethodStub("DemoMethod", returnType: type1),
                new TypeStub("TargetType", "Demo.TargetType", new MethodStub("DemoMethod", returnType: type2)));
        }

        [TestMethod]
        public void TestFindMatchGenerics()
        {
            var actual = new MethodMatching().FindMatch(
                new MethodStub("DemoMethod", genericTypes: new[] { "System.IComparable" }),
                new TypeStub("TargetType", "Demo.TargetType", new MethodStub("DemoMethod", genericTypes: new[] { "System.IComparable" }))).Name;

            var expected = "DemoMethod";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void TestFindMatchDifferentGenerics()
        {
            new MethodMatching().FindMatch(
                new MethodStub("DemoMethod", genericTypes: new[] { "System.IComparable" }),
                new TypeStub("TargetType", "Demo.TargetType", new MethodStub("DemoMethod")));
        }
    }
}