namespace MockEverythingTests.Engine.Browsers
{
    using System;
    using System.Linq;
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine;
    using MockEverything.Engine.Browsers;
    using MockEverything.Inspection;
    using MockEverything.Inspection.MonoCecil;

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

        [TestMethod]
        public void TestFindMatchParameters()
        {
            var stringType = new TypeStub("String", "System.String");
            var actual = new MethodMatching().FindMatch(
                new MethodStub("DemoMethod", parameters: new[] { new Parameter(ParameterVariant.In, stringType) }),
                new TypeStub(
                    "TargetType",
                    "Demo.TargetType",
                    new MethodStub("DemoMethod", parameters: new[] { new Parameter(ParameterVariant.In, stringType) }))).Name;

            var expected = "DemoMethod";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void TestFindMatchDifferentParameters()
        {
            var stringType = new TypeStub("String", "System.String");
            new MethodMatching().FindMatch(
                new MethodStub("DemoMethod", parameters: new[] { new Parameter(ParameterVariant.In, stringType) }),
                new TypeStub(
                    "TargetType",
                    "Demo.TargetType",
                    new MethodStub("DemoMethod", parameters: new[] { new Parameter(ParameterVariant.Ref, stringType) })));
        }

        [TestMethod]
        public void TestFindMatchParamsParameter()
        {
            var stringType = new TypeStub("String", "System.String");
            var actual = new MethodMatching().FindMatch(
                new MethodStub("DemoMethod", parameters: new[] { new Parameter(ParameterVariant.Params, stringType) }),
                new TypeStub(
                    "TargetType",
                    "Demo.TargetType",
                    new MethodStub("DemoMethod", parameters: new[] { new Parameter(ParameterVariant.Params, stringType) }))).Name;

            var expected = "DemoMethod";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [TestCategory("System tests")]
        public void TestFindMatchParamsParameterReal()
        {
            var firstAssemblyPath = new Uri(typeof(StringFormat).Assembly.CodeBase).AbsolutePath;
            var secondAssemblyPath = new Uri(typeof(string).Assembly.CodeBase).AbsolutePath;

            var methodFromFirst = new Assembly(firstAssemblyPath).FindType("MockEverythingTests.Engine.Browsers.StringFormat").FindMethods(MemberType.Static).Single();
            var typeFromSecond = new Assembly(secondAssemblyPath).FindType("System.String");

            var actual = new MethodMatching().FindMatch(methodFromFirst, typeFromSecond).Name;

            var expected = "Format";
            Assert.AreEqual(expected, actual);
        }
    }
}