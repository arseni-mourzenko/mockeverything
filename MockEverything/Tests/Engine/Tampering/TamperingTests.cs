namespace MockEverythingTests.Engine
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using FileProxies;
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
            var testsPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(this.CurrentPath)));
            var readFileDirectoryPath = Path.Combine(testsPath, @"ReadFile\bin\Debug");
            var readFileExePath = Path.Combine(readFileDirectoryPath, "MockEverythingTests.ReadFile.exe");
            var tampering = new Tampering
            {
                Pair = new Pair<IAssembly>(new Assembly(this.FindAssemblyPathOf<FileAccess>()), new Assembly(this.FindAssemblyPathOf<FileProxy>())),
                ResultVersion = new Version("2.0.14.2075"),
            };

            tampering.Tamper().Save(Path.Combine(readFileDirectoryPath, "mscorlib.dll"));

            var info = new ProcessStartInfo(readFileExePath)
            {
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (var process = Process.Start(info))
            {
                process.WaitForExit();
                var actual = process.StandardOutput.ReadToEnd();
                var expected = "Hello, World!";
                Assert.AreEqual(expected, actual);
            }
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
