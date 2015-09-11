namespace MockEverythingTests.Engine.Browsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Attributes;
    using MockEverything.Engine.Browsers;
    using MockEverything.Inspection;

    [TestClass]
    public class TypeMatchingTests
    {
        [TestMethod]
        public void TestFindMatch()
        {
            var actual = new TypeMatching().FindMatch(new ProxyType(), new TargetAssembly()).Name;
            var expected = "TargetType";
            Assert.AreEqual(expected, actual);
        }

        private class TargetAssembly : IAssembly
        {
            public string FilePath
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string FullName
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public void AlterVersion(Version version)
            {
                throw new NotImplementedException();
            }

            public IType FindType(string fullName)
            {
                return new TargetType();
            }

            public IEnumerable<IType> FindTypes(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
            {
                throw new NotImplementedException();
            }

            public void ReplacePublicKey(IAssembly model)
            {
                throw new NotImplementedException();
            }

            public void Save(string path)
            {
                throw new NotImplementedException();
            }
        }

        private class TargetType : IType
        {
            public string FullName
            {
                get
                {
                    return "TargetType";
                }
            }

            public IEnumerable<string> GenericTypes
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string Name
            {
                get
                {
                    return typeof(TargetType).Name;
                }
            }

            public TAttribute FindAttribute<TAttribute>() where TAttribute : Attribute
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> FindAttributeValues<TAttribute>() where TAttribute : Attribute
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IMethod> FindMethods(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
            {
                throw new NotImplementedException();
            }
        }

        private class ProxyType : IType
        {
            public string FullName
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerable<string> GenericTypes
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string Name
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public TAttribute FindAttribute<TAttribute>() where TAttribute : Attribute
            {
                return (TAttribute)(object)new ProxyOfAttribute(typeof(TargetType));
            }

            public IEnumerable<object> FindAttributeValues<TAttribute>() where TAttribute : Attribute
            {
                yield return typeof(TargetType);
            }

            public IEnumerable<IMethod> FindMethods(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
            {
                throw new NotImplementedException();
            }
        }
    }
}
