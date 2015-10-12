namespace MockEverythingTests.Engine.Discovery
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine;
    using MockEverything.Inspection;
    using MockEverything.Inspection.MonoCecil;
    using CommonStubs;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    [TestClass]
    public class InstanceProxyExceptionTests
    {
        [TestInitialize]
        public void SetUp()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        [TestMethod]
        public void TestMessageEmptyConstructor()
        {
            var actual = new InstanceProxyException().Message;
            var expected = "Exception of type 'MockEverything.Engine.InstanceProxyException' was thrown.";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMessageConstructorWithMessage()
        {
            var actual = new InstanceProxyException("Hello, World!").Message;
            var expected = "Hello, World!";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMessageConstructorWithTypes()
        {
            var actual = new InstanceProxyException(this.SampleTypes).Message;
            var expected = "The proxy assembly contains the following instance classes declared as proxies: System.Demo, System.Other. Either declare those classes as static or remove the proxy attribute.";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMessageConstructorWithSingleType()
        {
            var actual = new InstanceProxyException(this.SampleTypes.Take(1).ToList()).Message;
            var expected = "The proxy assembly contains the following instance class declared as proxy: System.Demo. Either declare this class as static or remove the proxy attribute.";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestTypesEmptyConstructor()
        {
            var actual = new InstanceProxyException().NamesOfTypes;
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TestTypesConstructorWithMessage()
        {
            var actual = new InstanceProxyException("Hello, World!").NamesOfTypes;
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void TestTypesConstructorWithTypes()
        {
            var actual = new InstanceProxyException(this.SampleTypes).NamesOfTypes.ToList();
            var expected = new[] { "System.Demo", "System.Other" };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSerializationOfTypes()
        {
            var exception = new InstanceProxyException(this.SampleTypes);
            using (var buffer = new MemoryStream())
            {
                var serializingFormatter = new BinaryFormatter();
                serializingFormatter.Serialize(buffer, exception);
                buffer.Seek(0, SeekOrigin.Begin);

                var deserializingFormatter = new BinaryFormatter();
                var result = (InstanceProxyException)deserializingFormatter.Deserialize(buffer);
                var actual = result.NamesOfTypes.ToList();
                var expected = new[] { "System.Demo", "System.Other" };
                CollectionAssert.AreEqual(expected, actual);
            }
        }

        private Collection<IType> SampleTypes
        {
            get
            {
                var types = new[]
                {
                    new TypeStub("Demo", "System.Demo"),
                    new TypeStub("Other", "System.Other"),
                };

                return new Collection<IType>(types);
            }
        }
    }
}