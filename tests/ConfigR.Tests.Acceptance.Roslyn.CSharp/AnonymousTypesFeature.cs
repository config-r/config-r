// <copyright file="AnonymousTypesFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;

    public static class AnonymousTypesFeature
    {
        [Scenario]
        public static void RetrievingAnAnonymousType()
        {
            dynamic config = null;
            dynamic result = null;

            "Given a local config file containing an anonymous type with a Bar of 'baz'"
                .x(c =>
                {
                    var code =
@"using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
Config.Foo = new { Bar = ""baz"" };
";

                    ConfigFile.Create(code).Using(c);
                });

            "When I load the config"
                .x(async () => config = await new Config().UseRoslynCSharpLoader().LoadDynamic());

            "When I get the anonymous type"
                .x(() => { result = config.Foo; });

            "Then the anonymous type has a Bar of 'baz'"
                .x(() => ((string)result.Bar).Should().Be("baz"));
        }
    }
}
