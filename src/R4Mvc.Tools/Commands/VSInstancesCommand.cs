using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using R4Mvc.Tools.Commands.Core;
using R4Mvc.Tools.Services;

namespace R4Mvc.Tools.Commands
{
    public class VSInstancesCommand : ICommand
    {
        public bool IsGlobal => true;
        public byte Order => 100;
        public string Key => "vsinstances";
        public string Summary => "List the available VS/MSBuild instances";
        public string Description => Summary + @"
Usage: vsinstances [showPath]
showpath:
    Should this tool output the path for the VS/MSBuild instance. Defaults to false";

        public Type GetCommandType() => typeof(Runner);

        public class Runner : ICommandRunner
        {
            private readonly IVsLocatorService _vsLocatorService;

            public Runner(IVsLocatorService vsLocatorService)
            {
                _vsLocatorService = vsLocatorService;
            }

            public Task Run(string projectPath, IConfiguration configuration, string[] args)
            {
                // var showPath = configuration.GetValue<bool?>("showPath") ?? false;

                // var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
                var instances = _vsLocatorService.GetInstances();
                if (instances.Length == 0)
                {
                    Console.WriteLine("No Visual Studio / MSBuild instances found.");
                    return Task.CompletedTask;
                }

                Console.WriteLine("Available Visual Studio / MSBuild instances:");
                string instancesDescription = _vsLocatorService.GetInstancesDescription(instances);
                Console.WriteLine(instancesDescription);

                return Task.CompletedTask;
            }
        }
    }
}
