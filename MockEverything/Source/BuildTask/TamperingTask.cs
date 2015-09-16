// <copyright file="TamperingTask.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.BuildTask
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
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
                    var targetPath = Path.Combine(this.DestinationPath, Path.GetFileName(tampering.Pair.Target.FilePath));
                    Trace.WriteLine(string.Format("The proxy {0} will be generated.", targetPath));
                    tampering.Tamper(this.DestinationPath).Save(targetPath);
                }

                return true;
            }
            catch (Exception ex)
            {
                this.Log.LogError(ex.Message + ex.StackTrace);
                return false;
            }
        }
    }
}