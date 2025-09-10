using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Build.Locator;

namespace R4Mvc.Tools.Services
{
    public interface IVsLocatorService
    {
        VisualStudioInstance[] GetInstances();
    }

    public class VsLocatorService : IVsLocatorService
    {
        public VisualStudioInstance[] GetInstances()
        {
            var instances = MSBuildLocator.QueryVisualStudioInstances()
                .ToList();

            if (tryGetMyInstance(out VisualStudioInstance myInstance))
            {
                if (!instances.Any(i => i.MSBuildPath.Equals(myInstance.MSBuildPath, StringComparison.OrdinalIgnoreCase)))
                {
                    // instances.Insert(0, myInstance);
                    instances.Add(myInstance);
                }
            }

            return instances.ToArray();
        }

        private bool tryGetMyInstance([MaybeNullWhen(false)] out VisualStudioInstance myInstance)
        {
            // It's a known issue that MsBuildLocator doesn't always find the latest VS installation.
            // https://github.com/microsoft/MSBuildLocator/issues/152
            // So we'll try to find it ourselves using vswhere.exe

            string progFilesx86Dir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string vsInstallerPath = Path.Combine(progFilesx86Dir, "Microsoft Visual Studio", "Installer");
            if (!Directory.Exists(vsInstallerPath))
            {
                Console.WriteLine($"Visual Studio Installer path not found: {vsInstallerPath}");
                myInstance = null;
                return false;
            }

            string vsWherePath = Path.Combine(vsInstallerPath, "vswhere.exe");
            if (!File.Exists(vsWherePath))
            {
                Console.WriteLine($"vswhere.exe not found: {vsWherePath}");
                myInstance = null;
                return false;
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = vsWherePath,
                Arguments = "-latest -products * -requires Microsoft.Component.MSBuild", //  -property installationPath
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            string vswhereOutput;
            using (var process = Process.Start(startInfo))
            {
                if (process == null)
                {
                    Console.WriteLine("Failed to start vswhere.exe process.");
                    myInstance = null;
                    return false;
                }
                vswhereOutput = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    Console.WriteLine($"vswhere.exe exited with code {process.ExitCode}");
                    myInstance = null;
                    return false;
                }
            }

            // Debug.Print(vswhereOutput);
            var kvps = GetKeyValuePairs(vswhereOutput);
            var discoveryType = DiscoveryType.VisualStudioSetup;
            string name = kvps.TryGetValue("displayName", out var n) ? n : "Unknown";
            string path = kvps.TryGetValue("installationPath", out var p) ? p : "";
            string versionStr = kvps.TryGetValue("installationVersion", out var v) ? v : "0.0";
            Version version = Version.TryParse(versionStr, out var ver) ? ver : new Version(0, 0);

            myInstance = getNewVisualStudioInstance(name, path, version, discoveryType);
            return true;
        }

        static private VisualStudioInstance getNewVisualStudioInstance(string name, string path, Version version, DiscoveryType discoveryType)
        {
            // Thanks for nothing, MS, for making the VisualStudioInstance instructor internal. 

            var type = typeof(VisualStudioInstance);
            var ctor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                [typeof(string), typeof(string), typeof(Version), typeof(DiscoveryType)],
                null
            );

            if (ctor == null)
                throw new InvalidOperationException("Could not find the internal constructor for VisualStudioInstance.");

            var instance = (VisualStudioInstance)ctor.Invoke([name, path, version, discoveryType]);
            return instance;
        }

        private IReadOnlyDictionary<string, string> GetKeyValuePairs(string input)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var separatorIndex = line.IndexOf(':');
                if (separatorIndex > 0)
                {
                    var key = line.Substring(0, separatorIndex).Trim();
                    var value = line.Substring(separatorIndex + 1).Trim();
                    dict[key] = value;
                }
            }
            return dict;
        }
    }
}
