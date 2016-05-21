require 'albacore'

version_suffix = ENV["VERSION_SUFFIX"]  || ""
build_number   = ENV["BUILD_NUMBER"]    || "000000"
build_number_suffix = version_suffix == "" ? "" : "-build" + build_number
version = IO.read("src/CommonAssemblyInfo.cs").split(/AssemblyInformationalVersion\("/, 2)[1].split(/"/).first + version_suffix + build_number_suffix

$msbuild_command = "C:/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe"
$xunit_command = "packages/xunit.runner.console.2.1.0/tools/xunit.console.exe"
nuget_command = ".nuget/NuGet.exe"
$solution = "ConfigR.sln"
output = "artifacts/output"
logs = "artifacts/logs"

acceptance_tests = [
  "tests/ConfigR.Tests.Acceptance.Roslyn.CSharp/bin/Release/ConfigR.Tests.Acceptance.Roslyn.CSharp.dll",
]

nuspecs = [
  { :file => "src/ConfigR/ConfigR.nuspec", :version => version },
  { :file => "src/ConfigR.Roslyn.CSharp/ConfigR.Roslyn.CSharp.nuspec", :version => version },
]

Albacore.configure do |config|
  config.log_level = :verbose
end

desc "Execute default tasks"
task :default => [:accept, :pack]

desc "Restore NuGet packages"
exec :restore do |cmd|
  cmd.command = nuget_command
  cmd.parameters "restore #{$solution}"
end

directory logs

desc "Clean solution"
task :clean => [logs] do
  run_msbuild "Clean"
end

desc "Build solution"
task :build => [:clean, :restore, logs] do
  run_msbuild "Build"
end

desc "Run acceptance tests"
task :accept => [:build] do
  run_tests acceptance_tests
end

directory output

desc "Create the nuget packages"
task :pack => [:build, output] do
  nuspecs.each do |nuspec|
    file = nuspec[:file]
    original_file = "#{file}.original"
    File.rename file, original_file
    original_content = File.read(original_file)
    content = original_content.gsub(/\[0.0.0/, "[#{nuspec[:version]}")
    File.open(file, "w") {|file| file.puts content }
    begin
      cmd = Exec.new
      cmd.command = nuget_command
      cmd.parameters "pack " + file + " -Version " + nuspec[:version] + " -OutputDirectory " + output + " -NoPackageAnalysis"
      cmd.execute
    ensure
      FileUtils.rm file
      FileUtils.mv original_file, file
    end
  end
end

def run_msbuild(target)
  cmd = Exec.new
  cmd.command = $msbuild_command
  cmd.parameters "#{$solution} /target:#{target} /p:configuration=Release /nr:false /verbosity:minimal /nologo /fl /flp:LogFile=artifacts/logs/#{target}.log;Verbosity=Detailed;PerformanceSummary"
  cmd.execute
end

def run_tests(tests)
  tests.each do |test|
    xunit = XUnitTestRunner.new
    xunit.command = $xunit_command
    xunit.assembly = test
    xunit.options "-html", File.expand_path(test + ".TestResults.html"), "-xml", File.expand_path(test + ".TestResults.xml")
    xunit.execute  
  end
end
