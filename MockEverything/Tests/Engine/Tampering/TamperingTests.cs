namespace MockEverythingTests.Engine
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine.Browsers;
    using MockEverything.Engine.Tampering;
    using MockEverything.Inspection;
    using MockEverything.Inspection.MonoCecil;

    [TestClass]
    public class TamperingTests
    {
        [TestMethod]
        [TestCategory("System tests")]
        public void TestTampering()
        {
            var testsPath = Path.GetDirectoryName(this.CurrentPath);
            var tampering = new Tampering
            {
                Pair = new Pair<IAssembly>(new Assembly(this.FindAssemblyPathOf<FileAccess>()), new Assembly(null)),
                ResultVersion = new Version("2.0.14.2075"),
            };

            tampering.Tamper().Save(@"C:\Git\mockeverything\MockEverything\Tests\ReadFile\bin\Debug\mscorlib.dll");
        }

        private string CurrentPath
        {
            get
            {
                var path = this.FindAssemblyPathOf<TamperingTests>();
                return Path.GetDirectoryName(path);
            }
        }

        private string FindAssemblyPathOf<T>()
        {
            var codeBase = typeof(T).Assembly.CodeBase;
            return Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
        }
    }
}
