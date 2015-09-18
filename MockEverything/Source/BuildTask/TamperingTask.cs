// <copyright file="TamperingTask.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.BuildTask
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using Engine;
    using Engine.Discovery;
    using Engine.Tampering;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Represents the build task which transforms target assemblies corresponding to the proxy assemblies found in a specified directory.
    /// </summary>
    public class TamperingTask : Task
    {
        /// <summary>
        /// Gets or sets the absolute path to the directory containing the proxy assemblies.
        /// </summary>
        [Required]
        public string ProxiesPath { get; set; }

        /// <summary>
        /// Gets or sets the destination path. Usually, this is set to <c>$(TargetPath)</c>.
        /// </summary>
        [Required]
        public string DestinationPath { get; set; }

        /// <summary>
        /// Gets or sets the version of the resulting assemblies. If this value is <see langword="null"/>, the version will correspond to the version of the original target assembly.
        /// </summary>
        public string CustomVersion { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns><see langword="true"/> if the task successfully executed; otherwise, <see langword="false"/>.</returns>
        public override bool Execute()
        {
            Trace.Listeners.Add(new TaskLoggingTraceListener(this.Log));
            Trace.WriteLine("Started tampering of assemblies.");

            var resultVersion = string.IsNullOrEmpty(this.CustomVersion) ? null : new Version(this.CustomVersion);

            try
            {
                var parts = from pair in new DirectoryBasedDiscovery(new DirectoryAccess(), this.ProxiesPath).FindAssemblies()
                            select new Tampering { Pair = pair, ResultVersion = resultVersion };

                foreach (var tampering in parts)
                {
                    this.ProcessProxy(tampering);
                }

                return true;
            }
            catch (ProxyMistakeException ex)
            {
                this.Log.LogError(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                this.Log.LogError(ex.Message + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Tampers a target using a proxy, or does nothing if the resulting assembly is newer than the target, the proxy and the current assembly.
        /// </summary>
        /// <param name="tampering">The tampering to apply.</param>
        private void ProcessProxy(Tampering tampering)
        {
            Contract.Requires(tampering != null);

            var resultPath = Path.Combine(this.DestinationPath, Path.GetFileName(tampering.Pair.Target.FilePath));
            var cachePath = this.GenerateCachePath(tampering);

            if (File.Exists(cachePath))
            {
                Trace.WriteLine(string.Format("The proxying of {0} will be ignored, because its result is already in cache.", resultPath));
                if (!File.Exists(resultPath) || this.GenerateFileHash<SHA1CryptoServiceProvider>(cachePath) != this.GenerateFileHash<SHA1CryptoServiceProvider>(resultPath))
                {
                    File.Copy(cachePath, resultPath, overwrite: true);
                }

                return;
            }

            Trace.WriteLine(string.Format("The proxying of {0} will be applied.", resultPath));
            tampering.Tamper(this.DestinationPath).Save(resultPath);
            File.Copy(resultPath, cachePath);
        }

        /// <summary>
        /// Generates a path which corresponds to the tampering result within the cache directory.
        /// </summary>
        /// <param name="tampering">The tampering which contains information about the paths of the target and the proxy assemblies, as well as the required version.</param>
        /// <returns>The full path of the tampering result within the cache directory.</returns>
        private string GenerateCachePath(Tampering tampering)
        {
            Contract.Requires(tampering != null);
            Contract.Ensures(Contract.Result<string>() != null);

            var fileParts = new[]
            {
                "mockeverything",
                "cache",
                this.GenerateFileHash<SHA1CryptoServiceProvider>(this.CurrentAssemblyPath()),
                this.GenerateFileHash<SHA1CryptoServiceProvider>(tampering.Pair.Target.FilePath),
                this.GenerateFileHash<SHA1CryptoServiceProvider>(tampering.Pair.Proxy.FilePath),
                (tampering.ResultVersion ?? new Version(0, 0, 0, 0)).ToString()
            };

            return Path.Combine(Path.GetTempPath(), string.Join("-", fileParts));
        }

        /// <summary>
        /// Generates a hash of a file.
        /// </summary>
        /// <typeparam name="T">The hash algorithm.</typeparam>
        /// <param name="path">The full path to the file.</param>
        /// <returns>The hexadecimal representation of the hash.</returns>
        /// <example>
        /// <para>The following example generates the SHA1 hash of a file:</para>
        /// <code>
        /// <![CDATA[
        /// var path = @"C:\demo.txt";
        /// var hash = this.GenerateFileHash<SHA1CryptoServiceProvider>(path);
        /// Debug.Assert(hash.Length == 40);
        /// 
        /// // The value of `hash` is: 0a0a9f2a6772942557ab5355d76af442f8f65e01
        /// ]]>
        /// </code>
        /// </example>
        private string GenerateFileHash<T>(string path) where T : HashAlgorithm, new()
        {
            Contract.Requires(path != null);
            Contract.Ensures(Contract.Result<string>() != null);

            using (var stream = File.OpenRead(path))
            {
                using (var sha = new T())
                {
                    var hashData = sha.ComputeHash(stream);
                    return new string(hashData.SelectMany(b => b.ToString("x2")).ToArray());
                }
            }
        }

        /// <summary>
        /// Determines the path of the current assembly.
        /// </summary>
        /// <returns>The full path of the assembly file.</returns>
        private string CurrentAssemblyPath()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
        }
    }
}