namespace MockEverythingTests.Engine.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine.Discovery;
    using MockEverything.Inspection;

    [TestClass]
    public class DirectoryBasedDiscoveryTests
    {
        [TestMethod]
        public void TestEmptyDirectory()
        {
            var actual = new DirectoryBasedDiscovery(new AccessStub(p => false), string.Empty).FindAssemblies().Count();
            var expected = 0;
            Assert.AreEqual(expected, actual);
        }

        private class AccessStub : IDirectoryAccess
        {
            private readonly Func<string, bool> fileExists;

            private readonly string[] paths;

            public AccessStub(Func<string, bool> fileExists, params string[] paths)
            {
                this.fileExists = fileExists;
                this.paths = paths;
            }

            public bool FileExists(string fullPath)
            {
                return this.fileExists(fullPath);
            }

            public IEnumerable<string> ListFilesEndingBy(string directoryPath, string suffix)
            {
                return this.paths;
            }

            public IAssembly LoadAssembly(string fullPath)
            {
                return AccessStub.CreateStub(fullPath);
            }

            public static IAssembly CreateStub(string text)
            {
                return new AssemblyStub(text);
            }
        }
    }
}
