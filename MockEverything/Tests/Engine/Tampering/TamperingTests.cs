namespace MockEverythingTests.Engine
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using ArgumentsProxies;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Engine.Browsers;
    using MockEverything.Engine.Tampering;
    using MockEverything.Inspection;
    using MockEverything.Inspection.MonoCecil;
    using ArgumentsTarget;
    using SystemProxies;

    [TestClass]
    public class TamperingTests
    {
        [TestMethod]
        [TestCategory("System tests")]
        public void TestTampering()
        {
            var testsPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(this.CurrentPath)));
            var readFileDirectoryPath = Path.Combine(testsPath, @"DownloadString\bin\Debug");
            var readFileExePath = Path.Combine(readFileDirectoryPath, "MockEverythingTests.DownloadString.exe");
            var tampering = new Tampering
            {
                Pair = new Pair<IAssembly>(
                    proxy: new Assembly(this.FindAssemblyPathOf(typeof(WebClientProxy))),
                    target: new Assembly(this.FindAssemblyPathOf<WebClient>())),
                ResultVersion = new Version("2.0.14.2075"),
            };

            tampering.Tamper().Save(Path.Combine(readFileDirectoryPath, "System.dll"));

            var info = new ProcessStartInfo(readFileExePath)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            using (var process = Process.Start(info))
            {
                process.WaitForExit();
                var actual = process.StandardOutput.ReadToEnd();
                var expected = "Hello, World!" + Environment.NewLine;
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        [TestCategory("System tests")]
        public void TestTamperingOfConstructor()
        {
            var testsPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(this.CurrentPath)));
            var readFileDirectoryPath = Path.Combine(testsPath, @"InitializeEventArgs\bin\Debug");
            var readFileExePath = Path.Combine(readFileDirectoryPath, "MockEverythingTests.InitializeEventArgs.exe");
            var tampering = new Tampering
            {
                Pair = new Pair<IAssembly>(
                    proxy: new Assembly(this.FindAssemblyPathOf(typeof(FileSystemEventArgsProxy))),
                    target: new Assembly(this.FindAssemblyPathOf<FileSystemEventArgs>())),
                ResultVersion = new Version("2.0.14.2075"),
            };

            tampering.Tamper().Save(Path.Combine(readFileDirectoryPath, "System.dll"));

            var info = new ProcessStartInfo(readFileExePath)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            using (var process = Process.Start(info))
            {
                process.WaitForExit();
                var actual = process.StandardOutput.ReadToEnd();
                var expected = "<null>" + Environment.NewLine;
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        [TestCategory("System tests")]
        public void TestArgumentsInTampering()
        {
            var testsPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(this.CurrentPath)));
            var readFileDirectoryPath = Path.Combine(testsPath, @"Arguments\bin\Debug");
            var readFileExePath = Path.Combine(readFileDirectoryPath, "MockEverythingTests.Arguments.exe");
            var tampering = new Tampering
            {
                Pair = new Pair<IAssembly>(
                    proxy: new Assembly(this.FindAssemblyPathOf(typeof(DemoProxy))),
                    target: new Assembly(this.FindAssemblyPathOf<DemoInstance>())),
            };

            var resultPath = Path.Combine(readFileDirectoryPath, "MockEverythingTests.ArgumentsTarget.dll");
            tampering.Tamper().Save(resultPath);

            var info = new ProcessStartInfo(readFileExePath)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };

            using (var process = Process.Start(info))
            {
                process.WaitForExit();
                var actual = process.StandardOutput.ReadToEnd();
                var expected = "Tampered hello, Jeff!" + Environment.NewLine;
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
            return this.FindAssemblyPathOf(typeof(T));
        }

        private string FindAssemblyPathOf(System.Type type)
        {
            var codeBase = type.Assembly.CodeBase;
            return Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
        }
    }
}
