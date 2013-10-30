# ConfigR

Write your .NET configuration files in C# :sunglasses:.

[Quickstart](https://github.com/config-r/config-r/wiki/Quickstart)

Fed up with XML soup? Frustrated that app settings can only be strings? Want to do more in your configuration file than just define app settings? Then ConfigR is for you!

Get it at [NuGet](https://nuget.org/packages/ConfigR/ "ConfigR on Nuget").

Powered by [scriptcs](https://github.com/scriptcs/scriptcs) and [Roslyn](http://msdn.microsoft.com/en-gb/roslyn).

Congratulations! You've freed yourself from the shackles of XML and strings! :trophy:

## Advanced Usage

ConfigR does plenty more! Features include the ability to specify the path of your configuration file(s), multiple cascading configuration files and custom configurators. [See the wiki for details](https://github.com/config-r/config-r/wiki).

TIP: you can write **any C# you like** in your 'configuration file' :wink:. The scriptcs [#load](https://github.com/scriptcs/scriptcs/wiki/Writing-a-script#loading-referenced-scripts) and [#r](https://github.com/scriptcs/scriptcs/wiki/Writing-a-script#referencing-assemblies) features are both supported for loading scripts and referencing assemblies.

## Updates

Releases will be pushed regularly to [NuGet](https://nuget.org/packages/ConfigR/). For update notifications, follow [@adamralph](https://twitter.com/#!/adamralph).

To build manually, clone or fork this repository and see ['How to build'](https://github.com/config-r/config-r/blob/master/how_to_build.md).

## Can I help to improve it and/or fix bugs? ##

Absolutely! Please feel free to raise issues, fork the source code, send pull requests, etc.

No pull request is too small. Even whitespace fixes are appreciated. Before you contribute anything make sure you read [CONTRIBUTING.md](https://github.com/config-r/config-r/blob/master/CONTRIBUTING.md).

## What do the version numbers mean? ##

ConfigR uses [Semantic Versioning](http://semver.org/). The current release is 0.x which means 'initial development'. Version 1.0 will follow the release of [scriptcs](https://github.com/scriptcs/scriptcs) version 1.0.

## Sponsors ##
Our build server is kindly provided by [CodeBetter](http://codebetter.com/) and [JetBrains](http://www.jetbrains.com/).

![YouTrack and TeamCity](http://www.jetbrains.com/img/banners/Codebetter300x250.png)
