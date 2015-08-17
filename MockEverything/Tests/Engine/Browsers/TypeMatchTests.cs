namespace MockEverythingTests.Engine.Browsers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine.Browsers;
    using MockEverything.Inspection;

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

        private class TypeStub : IType
        {
            public TypeStub(string name)
            {
                this.Name = name;
            }

            public string Name { get; set; }

            public IEnumerable<IMethod> FindTypes(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
            {
                throw new NotImplementedException();
            }
        }
    }
}
