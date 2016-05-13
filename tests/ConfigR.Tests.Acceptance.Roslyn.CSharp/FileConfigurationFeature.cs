// <copyright file="FileConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;

    public static class FileConfigurationFeature
    {
        [Scenario]
        public static void RetrievingAnObject(Foo result)
        {
            dynamic config = null;

            "Given a config file containing a Foo with a Bar of 'baz'"
                .f(c =>
                {
                    var code =
@"using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
Config.Foo = new Foo { Bar = ""baz"" };
";

                    ConfigFile.Create(code, "foo.csx").Using(c);
                });

            "When I load the file"
                .f(async () => config = await new Config().UseRoslynCSharpLoader("foo.csx").Load());

            "And I get the Foo"
                .f(() => result = config.Foo<Foo>());

            "Then the Foo has a Bar of 'baz'"
                .f(() => result.Bar.Should().Be("baz"));
        }
    }
}
