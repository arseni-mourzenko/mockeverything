namespace MockEverythingTests.Engine.Browsers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Attributes;
    using MockEverything.Engine.Browsers;
    using MockEverything.Inspection;

    [TestClass]
    public class AssemblyBrowserTests
    {
        [TestMethod]
        public void TestFindTypes()
        {
            var proxy = AssemblyStub.FromNames(string.Empty, "Hello", "World");
            var target = new AssemblyStub(string.Empty);
            var search = new TypeMatchSearchStub();

            var actual = this.ProcessMatches(new AssemblyBrowser(proxy, target, search));
            var expected = new[] { "Hello↔HelloTarget", "World↔WorldTarget" };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindTypesSameTarget()
        {
            var proxy = AssemblyStub.FromNames(string.Empty, "Hello", "World");
            var target = new AssemblyStub(string.Empty);
            var search = new TypeMatchSearchConstStub();

            var actual = this.ProcessMatches(new AssemblyBrowser(proxy, target, search));
            var expected = new[] { "Hello↔ThisIsATarget", "World↔ThisIsATarget" };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindTypesTargetInvocationTypeArgument()
        {
            var proxy = new ProxyAssemblyMock();
            new AssemblyBrowser(proxy, new AssemblyStub(string.Empty), new TypeMatchSearchConstStub()).FindTypes().ToList();

            var actual = proxy.Type;
            var expected = MemberType.Static;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindTypesTargetInvocationAttributesArgument()
        {
            var proxy = new ProxyAssemblyMock();
            new AssemblyBrowser(proxy, new AssemblyStub(string.Empty), new TypeMatchSearchConstStub()).FindTypes().ToList();

            var actual = proxy.ExpectedAttributes;
            var expected = new[] { typeof(ProxyOfAttribute) };
            CollectionAssert.AreEqual(expected, actual);
        }

        private List<string> ProcessMatches(IEnumerable<Pair<IType>> matches)
        {
            Contract.Requires(matches != null);

            return matches.Select(m => m.Proxy.Name + "↔" + m.Target.Name).ToList();
        }

        private List<string> ProcessMatches(AssemblyBrowser browser)
        {
            Contract.Requires(browser != null);

            var matches = browser.FindTypes();
            return this.ProcessMatches(matches);
        }

        private class TypeMatchSearchStub : IMatching<IType, IAssembly>
        {
            public IType FindMatch(IType proxy, IAssembly targetAssembly)
            {
                return new TypeStub(proxy.Name + "Target", "Demo." + proxy.Name + "Target");
            }
        }

        private class TypeMatchSearchConstStub : IMatching<IType, IAssembly>
        {
            public IType FindMatch(IType proxy, IAssembly targetAssembly)
            {
                return new TypeStub("ThisIsATarget", "Demo.ThisIsATarget");
            }
        }

        private class ProxyAssemblyMock : IAssembly
        {
            public MemberType Type { get; private set; }

            public Type[] ExpectedAttributes { get; private set; }

            public string FullName
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerable<IType> FindTypes(MemberType type = MemberType.All, params Type[] expectedAttributes)
            {
                this.Type = type;
                this.ExpectedAttributes = expectedAttributes;

                return Enumerable.Empty<IType>();
            }

            public IType FindType(string fullName)
            {
                throw new NotImplementedException();
            }

            public void AlterVersion(Version version)
            {
                throw new NotImplementedException();
            }
        }
    }
}
