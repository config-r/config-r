require 'albacore'
require 'fileutils'

version = IO.read("src/ConfigR/Properties/AssemblyInfo.cs").split(/AssemblyInformationalVersion\("/, 2)[1].split(/"/).first
xunit_command = "src/packages/xunit.runners.1.9.2/tools/xunit.console.clr4.exe"
nuget_command = "src/packages/NuGet.CommandLine.2.8.0/tools/NuGet.exe"
solution = "src/ConfigR.sln"
output = "bin"

specs = [
]

features = [
  { :command => xunit_command, :assembly => "src/test/ConfigR.Features/bin/Debug/ConfigR.Features.dll" },
]

samples = [
]

nuspec = "src/ConfigR/ConfigR.csproj"

Albacore.configure do |config|
  config.log_level = :verbose
end

desc "Execute default tasks"
task :default => [:spec, :feature, :pack]

desc "Restore NuGet packages"
exec :restore do |cmd|
  cmd.command = nuget_command
  cmd.parameters "restore #{solution}"
end

desc "Clean solution"
msbuild :clean do |msb|
  FileUtils.rmtree output
  msb.properties = { :configuration => :Release }
  msb.targets = [:Clean]
  msb.solution = solution
end

desc "Build solution"
msbuild :build => [:clean, :restore] do |msb|
  msb.properties = { :configuration => :Release }
  msb.targets = [:Build]
  msb.solution = solution
end

desc "Execute specs"
task :spec => [:build] do
  execute_xunit specs
end

desc "Execute features"
task :feature => [:build] do
  execute_xunit features
end

desc "Create the nuget package"
exec :pack => [:build] do |cmd|
  FileUtils.mkpath output
  cmd.command = nuget_command
  cmd.parameters "pack " + nuspec + " -Version " + version + " -OutputDirectory " + output + " -Properties Configuration=Release"
end

desc "Execute samples"
task :sample => [:build] do
  execute_xunit samples
end

def execute_xunit(tests)
  tests.each do |test|
    xunit = XUnitTestRunner.new
    xunit.command = test[:command]
    xunit.assembly = test[:assembly]
    xunit.options "/html", File.expand_path(test[:assembly] + ".TestResults.html"), "/xml", File.expand_path(test[:assembly] + ".TestResults.xml")
    xunit.execute  
  end
end
