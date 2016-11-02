// <copyright file="VisualStudioHostingFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System;
    using System.IO;
    using System.Linq;
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;

    public static class VisualStudioHostingFeature
    {
        [Scenario]
        public static void RetrievingAnObject(int result)
        {
            dynamic config = null;

            "Given a local config file containing Foo of 42"
                .f(c => ConfigFile.Create("Config.Foo = 42;").Using(c));

            "And we are using Visual Studio hosting"
                .x(() =>
                {
                    var tokens = Path.GetFileName(ConfigFile.GetDefaultPath()).Split('.');
                    var vsHostTokens = tokens
                        .Reverse()
                        .Skip(2)
                        .Reverse()
                        .Concat(new[] { "vshost" })
                        .Concat(tokens.Reverse().Take(2).Reverse());

                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", string.Join(".", vsHostTokens));
                })
                .Teardown(() => AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", Path.GetFileName(ConfigFile.GetDefaultPath())));

            "When I load the config"
                .f(async () => config = await new Config().UseRoslynCSharpLoader().LoadDynamic());

            "And I get Foo"
                .f(() => result = config.Foo<int>());

            "Then Foo is 42"
                .f(() => result.Should().Be(42));
        }
    }
}
