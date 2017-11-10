#load "packages/simple-targets-csx.5.3.0/simple-targets.csx"

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static SimpleTargets;

// version
var versionSuffix = Environment.GetEnvironmentVariable("VERSION_SUFFIX") ?? "-adhoc";
var buildNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER") ?? "000000";
var buildNumberSuffix = versionSuffix == "" ? "" : "-build" + buildNumber;
var version = File.ReadAllText("src/CommonAssemblyInfo.cs")
    .Split(new[] { "AssemblyInformationalVersion(\"" }, 2, StringSplitOptions.RemoveEmptyEntries)[1]
    .Split('\"').First() + versionSuffix + buildNumberSuffix;

// locations
var solution = "./ConfigR.sln";
var logs = "./artifacts/logs";
var vswhere = "packages/vswhere.2.2.11/tools/vswhere.exe";
string msBuild;
var nuspecs = new[] { "./src/ConfigR/ConfigR.nuspec", "./src/ConfigR.Roslyn.CSharp/ConfigR.Roslyn.CSharp.nuspec", };
var output = "./artifacts/output";
var nuget = "./.nuget/v4.3.0/NuGet.exe";
var acceptanceTests = Path.GetFullPath("./tests/ConfigR.Tests.Acceptance.Roslyn.CSharp/bin/Release/ConfigR.Tests.Acceptance.Roslyn.CSharp.dll");
var xunit = "./packages/xunit.runner.console.2.1.0/tools/xunit.console.exe";

// targets
var targets = new TargetDictionary();

targets.Add("default", DependsOn("pack", "accept"));

targets.Add("logs", () => Directory.CreateDirectory(logs));

targets.Add("restore", () => Cmd(nuget, $"restore {solution}"));

targets.Add(
    "find-msbuild",
    () => msBuild = $"{ReadCmd(vswhere, "-latest -requires Microsoft.Component.MSBuild -property installationPath").Trim()}/MSBuild/15.0/Bin/MSBuild.exe");

targets.Add(
    "build",
    DependsOn("restore", "logs", "find-msbuild"),
    () => Cmd(
        msBuild,
        $"{solution} /p:Configuration=Release /nologo /m /v:m /nr:false " +
            $"/fl /flp:LogFile={logs}/msbuild.log;Verbosity=Detailed;PerformanceSummary"));

targets.Add("output", () => Directory.CreateDirectory(output));

targets.Add(
    "pack",
    DependsOn("build", "output"),
    () =>
    {
        foreach (var nuspec in nuspecs)
        {
            var originalNuspec = $"{nuspec}.original";
            File.Move(nuspec, originalNuspec);
            var originalContent = File.ReadAllText(originalNuspec);
                var content = originalContent.Replace("[0.0.0]", $"[{version}]");
            File.WriteAllText(nuspec, content);
            try
            {
                Cmd(nuget, $"pack {nuspec} -Version {version} -OutputDirectory {output} -NoPackageAnalysis");
            }
            finally
            {
                File.Delete(nuspec);
                File.Move(originalNuspec, nuspec);
            }
        }
    });

targets.Add(
    "accept",
    DependsOn("build"),
    () => Cmd(
        xunit, $"{acceptanceTests} -html {acceptanceTests}.TestResults.html -xml {acceptanceTests}.TestResults.xml"));

Run(Args, targets);

// helper
public static void Cmd(string fileName, string args)
{
    using (var process = new Process())
    {
        process.StartInfo = new ProcessStartInfo { FileName = $"\"{fileName}\"", Arguments = args, UseShellExecute = false, };
        Console.WriteLine($"Running '{process.StartInfo.FileName} {process.StartInfo.Arguments}'...");
        process.Start();
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"The command exited with code {process.ExitCode}.");
        }
    }
}

public static string ReadCmd(string fileName, string args)
{
    var output = new StringBuilder();
    using (var process = new Process())
    {
        process.StartInfo = new ProcessStartInfo {
            FileName = $"\"{fileName}\"",
            Arguments = args,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        process.OutputDataReceived += (sender, e) => output.AppendLine(e.Data);
        process.ErrorDataReceived += (sender, e) => output.AppendLine(e.Data);

        Console.WriteLine($"Running '{process.StartInfo.FileName} {process.StartInfo.Arguments}'...");
        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"The command exited with code {process.ExitCode}. {output.ToString()}");
        }
    }

    return output.ToString();
}
