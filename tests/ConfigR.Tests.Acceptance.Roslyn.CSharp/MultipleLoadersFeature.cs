// <copyright file="MultipleLoadersFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;

    public static class MultipleLoadersFeature
    {
        [Scenario]
        public static void RetrievingAnObjectDefinedInTwoFiles(int foo)
        {
            dynamic config = null;

            "Given a config file with a Foo of 123"
                .f(c => ConfigFile.Create("Config.Foo = 123;", "foo.csx").Using(c));

            "And another config file with a Foo of 456"
                .f(c => ConfigFile.Create("Config.Foo = 456;", "bar.csx").Using(c));

            "When I load the first file followed by the second file"
                .f(async () => config = await new Config()
                    .UseRoslynCSharpLoader("foo.csx").UseRoslynCSharpLoader("bar.csx").Load());

            "And I get Foo"
                .f(() => foo = config.Foo<int>());

            "Then Foo is 456"
                .f(() => foo.Should().Be(456));
        }
    }
}
