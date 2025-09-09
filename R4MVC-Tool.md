# R4MVC Tool

R4MVC.Tools is now available as a .NET Global Tool for easy installation and use.

## Installation

Install globally:

```bash
dotnet tool install -g R4Mvc.Tools --add-source https://nuget.pkg.github.com/ledgerscope/index.json
```

## Usage

Once installed, you can use the tool from anywhere:

```bash
r4mvc help                    # Show help
r4mvc generate               # Generate R4MVC files for the current project
r4mvc generate -p MyProject.csproj  # Generate for a specific project
r4mvc remove                 # Remove generated R4MVC files
r4mvc vsinstances           # List available MSBuild instances
```

## Features

- Cross-platform support (.NET 8+)
- Easy global installation
- Strongly typed helpers for ASP.NET Core MVC
- Support for Areas, Controllers, Actions, and Views
- Support for Razor Pages
- Configurable via `r4mvc.json` file

## Development Notes

The tool is packaged using the .NET Tool packaging system. The current implementation works best when installed globally due to dependency resolution complexities in the .NET tooling infrastructure.

## Migration from R4Mvc.Tools.Cli

The new R4Mvc.Tools package replaces the Windows-only R4Mvc.Tools.Cli package and provides cross-platform support.