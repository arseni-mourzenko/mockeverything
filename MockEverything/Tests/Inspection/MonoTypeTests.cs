namespace MockEverythingTests.Inspection
{
    using System.Linq;
    using CommonStubs;
    using Demo;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection;
    using MockEverything.Inspection.MonoCecil;

    [TestClass]
    public class MonoTypeTests
    {
        [TestMethod]
        public void TestGetName()
        {
            var actual = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.SimpleClass").Name;
            var expected = "SimpleClass";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGetFullName()
        {
            var actual = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.SimpleClass").FullName;
            var expected = "MockEverythingTests.Inspection.Demo.SimpleClass";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindMethods()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "SimpleMethod"));
        }

        [TestMethod]
        public void TestFindMethodsMissing()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "MissingMethod"));
        }

        [TestMethod]
        public void TestFindMethodsPrivate()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "PrivateMethod"));
        }

        [TestMethod]
        public void TestFindMethodsProtected()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "ProtectedMethod"));
        }

        [TestMethod]
        public void TestFindMethodsInternal()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "InternalMethod"));
        }

        [TestMethod]
        public void TestFindMethodsStatic()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "StaticMethod"));
        }

        [TestMethod]
        public void TestFindMethodsPrivateStatic()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "PrivateStaticMethod"));
        }

        [TestMethod]
        public void TestFindMethodsProtectedStatic()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "ProtectedStaticMethod"));
        }

        [TestMethod]
        public void TestFindMethodsInternalStatic()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "InternalStaticMethod"));
        }

        [TestMethod]
        public void TestFindMethodsStaticWithStaticFilter()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "StaticMethod", MemberType.Static));
        }

        [TestMethod]
        public void TestFindMethodsStaticWithInstanceFilter()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "StaticMethod", MemberType.Instance));
        }

        [TestMethod]
        public void TestFindMethodsInstanceWithStaticFilter()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "SimpleMethod", MemberType.Static));
        }

        [TestMethod]
        public void TestFindMethodsInstanceWithInstanceFilter()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "SimpleMethod", MemberType.Instance));
        }

        [TestMethod]
        public void TestFindMethodsAttributeMissing()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "SimpleMethod", MemberType.Instance, typeof(DemoAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsSingleAttribute()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "DecoratedMethod", MemberType.Instance, typeof(DemoAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsAllAttributes()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "DecoratedMethod", MemberType.Instance, typeof(DemoAttribute), typeof(DemoSecondAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsMoreAttributes()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "DecoratedMethod", MemberType.Instance, typeof(DemoAttribute), typeof(DemoSecondAttribute), typeof(System.SerializableAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsProperty()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "get_SimpleProperty"));
        }

        [TestMethod]
        public void TestFindMethodsPropertyPrivate()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "get_PrivateProperty"));
        }

        [TestMethod]
        public void TestFindMethodsPropertyPrivateGetter()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "get_PropertyPrivateGetter"));
        }

        [TestMethod]
        public void TestFindMethodsPropertyInstanceFilterStatic()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "get_SimpleProperty", MemberType.Static));
        }

        [TestMethod]
        public void TestFindMethodsPropertyStaticFilterStatic()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "get_StaticProperty", MemberType.Static));
        }

        [TestMethod]
        public void TestFindMethodsPropertyStaticFilterInstance()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "get_StaticProperty", MemberType.Instance));
        }

        [TestMethod]
        public void TestFindMethodsPropertyAttributeMissing()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "get_SimpleProperty", MemberType.Instance, typeof(DemoAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsPropertySingleAttribute()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "get_DecoratedProperty", MemberType.Instance, typeof(DemoAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsPropertyAllAttributes()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "get_DecoratedProperty", MemberType.Instance, typeof(DemoAttribute), typeof(DemoSecondAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsPropertyMoreAttributes()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "get_DecoratedProperty", MemberType.Instance, typeof(DemoAttribute), typeof(DemoSecondAttribute), typeof(System.SerializableAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsPropertyGetterSingleAttribute()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "get_PropertyDecoratedGetter", MemberType.Instance, typeof(DemoAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsPropertyGetterSingleAttributeMatchingSetter()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "set_PropertyDecoratedGetter", MemberType.Instance, typeof(DemoAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsPropertyGetterAllAttributes()
        {
            Assert.IsTrue(this.MethodExists("SimpleClass", "get_PropertyDecoratedGetter", MemberType.Instance, typeof(DemoAttribute), typeof(DemoSecondAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsPropertyGetterMoreAttributes()
        {
            Assert.IsFalse(this.MethodExists("SimpleClass", "get_PropertyDecoratedGetter", MemberType.Instance, typeof(DemoAttribute), typeof(DemoSecondAttribute), typeof(System.SerializableAttribute)));
        }

        [TestMethod]
        public void TestFindMethodsIndexers()
        {
            Assert.IsTrue(this.MethodExists("ClassWithIndexer", "get_Item"));
        }

        [TestMethod]
        public void TestFindMethodsIndexersSetter()
        {
            Assert.IsTrue(this.MethodExists("ClassWithIndexer", "set_Item"));
        }

        [TestMethod]
        public void TestFindMethodsIndexerOverloads()
        {
            Assert.IsTrue(this.MethodExists("ClassWithIndexers", "get_Item"));
        }

        [TestMethod]
        public void TestFindMethodsIndexerOverloadsSetter()
        {
            Assert.IsTrue(this.MethodExists("ClassWithIndexers", "set_Item"));
        }

        [TestMethod]
        public void TestFindAttributeCompareTypes()
        {
            var actual = this.SampleAssembly
                .FindType("MockEverythingTests.Inspection.Demo.DecoratedCustomClass")
                .FindAttribute<DemoAttribute>()
                .GetType();

            var expected = typeof(DemoAttribute);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindAttribute()
        {
            var actual = this.SampleAssembly
                .FindType("MockEverythingTests.Inspection.Demo.DecoratedCustomClass")
                .FindAttribute<DemoAttribute>()
                .Text;

            var expected = "Hello, World!";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(AttributeNotFoundException))]
        public void TestFindAttributeMissing()
        {
            this.SampleAssembly
                .FindType("MockEverythingTests.Inspection.Demo.DecoratedCustomClass")
                .FindAttribute<DemoSecondAttribute>();
        }

        [TestMethod]
        public void TestEqualsAllSame()
        {
            var first = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.SimpleClass");
            var second = new TypeStub(string.Empty, "MockEverythingTests.Inspection.Demo.SimpleClass");
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
        }

        [TestMethod]
        public void TestEqualsNull()
        {
            var first = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.SimpleClass");
            Assert.IsFalse(first.Equals(null));
        }

        [TestMethod]
        public void TestEqualsWrongType()
        {
            var first = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.SimpleClass");
            Assert.IsFalse(first.Equals(27));
        }

        [TestMethod]
        public void TestEqualsFullNameDifferent()
        {
            var first = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.SimpleClass");
            var second = new TypeStub(string.Empty, "Something.Else");
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }

        [TestMethod]
        public void TestEqualsWithGenerics()
        {
            var aa = this.SampleAssembly.FindTypes();
            var first = this.SampleAssembly.FindTypes().Single(t => t.Name == "SimpleGeneric`1" && t.GenericTypes.Count() == 1);
            var second = new TypeStub(string.Empty, "MockEverythingTests.Inspection.Demo.SimpleGeneric`1") { GenericTypes = new[] { string.Empty } };
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
        }

        [TestMethod]
        public void TestEqualsWithGenericsDifferentNumber()
        {
            var first = this.SampleAssembly.FindTypes().Single(t => t.Name == "SimpleGeneric`2" && t.GenericTypes.Count() == 2);
            var second = new TypeStub(string.Empty, "MockEverythingTests.Inspection.Demo.SimpleGeneric`2") { GenericTypes = new[] { string.Empty } };
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }

        [TestMethod]
        public void TestEqualsWithGenericsConstraint()
        {
            var aaa = this.SampleAssembly.FindTypes();
            var first = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.GenericsWithConstraints`1");
            var second = new TypeStub(string.Empty, "MockEverythingTests.Inspection.Demo.GenericsWithConstraints`1") { GenericTypes = new[] { "System.IComparable" } };
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
        }

        [TestMethod]
        public void TestEqualsWithGenericsDifferentConstraint()
        {
            var first = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.GenericsWithConstraints`1");
            var second = new TypeStub(string.Empty, "MockEverythingTests.Inspection.Demo.GenericsWithConstraints`1") { GenericTypes = new[] { "System.IFormattable" } };
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }

        [TestMethod]
        public void TestEqualsWithGenericsNewConstraint()
        {
            var first = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.GenericsWithNewConstraint`1");
            var second = new TypeStub(string.Empty, "MockEverythingTests.Inspection.Demo.GenericsWithNewConstraint`1") { GenericTypes = new[] { "new(),System.IComparable" } };
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(second.Equals(first));
        }

        [TestMethod]
        public void TestEqualsWithGenericsDifferentNewConstraint()
        {
            var first = this.SampleAssembly.FindType("MockEverythingTests.Inspection.Demo.GenericsWithNewConstraint`1");
            var second = new TypeStub(string.Empty, "MockEverythingTests.Inspection.Demo.GenericsWithNewConstraint`1") { GenericTypes = new[] { "System.IComparable" } };
            Assert.IsFalse(first.Equals(second));
            Assert.IsFalse(second.Equals(first));
        }

        private bool MethodExists(string typeName, string methodName)
        {
            return this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == typeName)
                .FindMethods()
                .Select(m => m.Name)
                .Contains(methodName);
        }

        private bool MethodExists(string typeName, string methodName, MemberType filter, params System.Type[] attributes)
        {
            return this.SampleAssembly
                .FindTypes()
                .Single(t => t.Name == typeName)
                .FindMethods(filter, attributes)
                .Select(m => m.Name)
                .Contains(methodName);
        }

        private Assembly SampleAssembly
        {
            get
            {
                return new Assembly(this.SampleAssemblyPath);
            }
        }

        private string SampleAssemblyPath
        {
            get
            {
                var codeBase = typeof(SimpleClass).Assembly.CodeBase;
                return System.Uri.UnescapeDataString(new System.UriBuilder(codeBase).Path);
            }
        }
    }
}