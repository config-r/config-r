// <copyright file="DynamicReferencesFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Xbehave;

    public static class DynamicReferencesFeature
    {
        [Scenario]
        public static void AddingADynamicReferenceToAnAssemblyOnDisk(Assembly reference, object result)
        {
            dynamic config = null;

            "Given an assembly on disk which defines a Foo type"
                .f(c =>
                {
                    var code =
$@"namespace {c.Step.Scenario.ScenarioOutline.Method.Name}
{{
    public class Foo
    {{
        public string Bar {{ get; set; }}
    }}
}}";

                    var compilation = CSharpCompilation.Create(
                            c.Step.Scenario.ScenarioOutline.Method.Name,
                            new[] { CSharpSyntaxTree.ParseText(code) },
                            null,
                            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                        .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

                    var emitResult = compilation.Emit(compilation.AssemblyName + ".dll");
                    emitResult.Success.Should().BeTrue(
                        "The diagnostics should indicate no errors:{0}{1}",
                        Environment.NewLine,
                        string.Join(Environment.NewLine, emitResult.Diagnostics.Select(diagnostic => diagnostic.ToString())));
                });

            "And config file with Foo with a Bar of 'baz'"
                .f(c =>
                {
                    var code =
$@"using {c.Step.Scenario.ScenarioOutline.Method.Name};
Config.Foo = new Foo {{ Bar = ""baz"" }};
";
                    ConfigFile.Create(code).Using(c);
                });

            "When I load the assembly"
                .f(c => reference = Assembly.Load(c.Step.Scenario.ScenarioOutline.Method.Name));

            "And I load the config using the assembly as a reference"
                .f(async () => config = await new Config().UseRoslynCSharpLoader().AddReferences(reference).Load());

            "And I get the value"
                .f(() => result = config.Foo);

            "Then the Foo Bar should be 'baz'"
                .f(() => ((string)((dynamic)result).Bar).Should().Be("baz"));
        }

        [Scenario]
        public static void AddingADynamicReferenceToAnInMemoryAssembly(Assembly reference, MetadataReference metadataReference, object result)
        {
            dynamic config = null;

            "Given an assembly in memory which defines a Foo type"
                .f(c =>
                {
                    var code =
$@"namespace {c.Step.Scenario.ScenarioOutline.Method.Name}
{{
    public class Foo
    {{
        public string Bar {{ get; set; }}
    }}
}}";

                    var compilation = CSharpCompilation.Create(
                            c.Step.Scenario.ScenarioOutline.Method.Name,
                            new[] { CSharpSyntaxTree.ParseText(code) },
                            null,
                            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                        .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

                    metadataReference = compilation.ToMetadataReference();

                    using (var stream = new MemoryStream())
                    {
                        var emitResult = compilation.Emit(stream);
                        emitResult.Success.Should().BeTrue(
                            "The diagnostics should indicate no errors:{0}{1}",
                            Environment.NewLine,
                            string.Join(Environment.NewLine, emitResult.Diagnostics.Select(diagnostic => diagnostic.ToString())));

                        stream.Seek(0, SeekOrigin.Begin);
                        reference = Assembly.Load(stream.ToArray());
                    }
                });

            "And config file with Foo with a Bar of 'baz'"
                .f(c =>
                {
                    var code =
$@"using {c.Step.Scenario.ScenarioOutline.Method.Name};
Config.Foo = new Foo {{ Bar = ""baz"" }};
";
                    ConfigFile.Create(code).Using(c);
                });

            "And I load the config using the assembly as a reference"
                .f(async () => config = await new Config().UseRoslynCSharpLoader()
                    .AddReferences(reference)
                    .AddReferences(metadataReference)
                    .Load());

            "And I get the value"
                .f(() => result = config.Foo);

            "Then the Foo Bar should be 'baz'"
                .f(() => ((string)((dynamic)result).Bar).Should().Be("baz"));
        }
    }
}
