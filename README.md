# ConfigR

Write your .NET configuration files in C# :sunglasses:.

- [Quickstart](https://github.com/config-r/config-r/wiki/Quickstart)
- [Samples](https://github.com/config-r/config-r-samples)

Fed up with XML soup? Frustrated that app settings can only be strings? Want to do more in your configuration file than just define app settings? Then ConfigR is for you!

Get it at [NuGet](https://nuget.org/packages/ConfigR/ "ConfigR on Nuget").

Powered by [Roslyn](https://github.com/dotnet/roslyn).

[![Source Browser](https://img.shields.io/badge/Browse-Source-green.svg)](http://sourcebrowser.io/Browse/config-r/config-r)

**Status**

 - Dev [![AppVeyor branch](https://img.shields.io/appveyor/ci/filipw/config-r/dev.svg)](https://ci.appveyor.com/project/filipw/config-r/branch/dev) 
 - Master [![AppVeyor branch](https://img.shields.io/appveyor/ci/filipw/config-r/master.svg)](https://ci.appveyor.com/project/filipw/config-r/branch/master) 

## Features

Checkout the [quickstart](https://github.com/config-r/config-r/wiki/Quickstart) to get an idea of the basics.

ConfigR does plenty more! Features include the ability to specify the path of your configuration file(s), multiple cascading configuration files and custom configurators. Checkout the [samples](https://github.com/config-r/config-r-samples) for more info.

TIP: you can write **any C# you like** in your 'configuration file' :wink:. The Roslyn [#load](https://github.com/dotnet/roslyn/wiki/Interactive-Window#load) and [#r](https://github.com/dotnet/roslyn/wiki/Interactive-Window#r) features are both supported for loading scripts and referencing assemblies.

## Updates

Releases will be pushed regularly to [NuGet](https://nuget.org/packages/ConfigR/).

To build manually, clone or fork this repository and see ['How to build'](#how-to-build).

## Can I help to improve it and/or fix bugs? ##

Absolutely! Please feel free to raise issues, fork the source code, send pull requests, etc.

No pull request is too small. Even whitespace fixes are appreciated. Before you contribute anything make sure you read [CONTRIBUTING.md](https://github.com/config-r/config-r/blob/master/CONTRIBUTING.md).

## How to build

Navigate to your clone root folder and execute `build.cmd`. The only prerequisite you need is MSBuild 14, which is also included in Visual Studio 2015.

`build.cmd` executes the default build targets which include compilation, test execution and packaging. After the build has completed, the build artifacts will be located in `artifacts/output/`.

You can also build the solution using Visual Studio 2015 or later. At the time of writing the build is only confirmed to work on Windows using the Microsoft .NET framework.

### Extras

* View the full list of build targets:

    `build.cmd -T`

* Run a specific target:

    `build.cmd build`

* Run multiple targets:

    `build.cmd build pack`

* View the full list of options:

    `build.cmd -?`

## What do the version numbers mean? ##

ConfigR uses [Semantic Versioning](http://semver.org/).
