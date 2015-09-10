// <copyright file="TamperingTask.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.BuildTask
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Engine.Browsers;
    using Engine.Tampering;
    using Inspection;
    using Inspection.MonoCecil;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class TamperingTask : Task
    {
        /// <summary>
        /// Gets or sets the path of the assembly containing the proxy code.
        /// </summary>
        [Required]
        public string ProxyAssemblyPath { get; set; }

        /// <summary>
        /// Gets or sets the path of the assembly containing the proxy code.
        /// </summary>
        [Required]
        public string TargetAssemblyPath { get; set; }

        /// <summary>
        /// Gets or sets the destination path. Usually, this is set to <c>$(TargetPath)</c>.
        /// </summary>
        [Required]
        public string DestinationPath { get; set; }

        /// <summary>
        /// Gets or sets the version of the resulting assembly. If this value is <see langword="null"/>, the version will correspond to the version of the original target assembly.
        /// </summary>
        public string CustomVersion { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns><see langword="true"/> if the task successfully executed; otherwise, <see langword="false"/>.</returns>
        public override bool Execute()
        {
            this.Log.LogWarning("Started.");

            Trace.Listeners.Add(new TaskLoggingTraceListener(this.Log));

            try
            {
                var tampering = new Tampering
                {
                    Pair = new Pair<IAssembly>(
                        proxy: new Assembly(this.ProxyAssemblyPath),
                        target: new Assembly(this.TargetAssemblyPath)),
                    ResultVersion = string.IsNullOrEmpty(this.CustomVersion) ? null : new Version(this.CustomVersion),
                };

                var targetPath = Path.Combine(this.DestinationPath, Path.GetFileName(this.TargetAssemblyPath));
                this.Log.LogWarning("The proxy {0} will be generated.", targetPath);
                tampering.Tamper(this.DestinationPath).Save(targetPath);
                this.Log.LogWarning("Ended: file {0}.", File.Exists(targetPath) ? "exits" : "doesn't exist");

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