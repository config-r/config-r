// <copyright file="DictionarySyntaxFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance.Roslyn.CSharp
{
    using System.Collections.Generic;
    using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
    using FluentAssertions;
    using Xbehave;

    public static class DictionarySyntaxFeature
    {
        [Scenario]
        public static void UsingDictionarySyntax(int result)
        {
            var config = default(IDictionary<string, object>);

            "Given a local config file with a Foo of 123"
                .f(c => ConfigFile.Create(@"ConfigDictionary[""Foo""] = 123;").Using(c));

            "When I load the config"
                .f(async () => config = await new Config().UseRoslynCSharpLoader().LoadDictionary());

            "And I get Foo"
                .f(() => result = config.Get<int>("Foo"));

            "Then the result is 123"
                .f(() => result.Should().Be(123));
        }

        [Scenario]
        public static void UsingDictionarySyntaxWithADefaultValue(int result)
        {
            var config = default(IDictionary<string, object>);

            "Given a local config file with a Foo of 123"
                .f(c => ConfigFile.Create(@"ConfigDictionary[""Foo""] = 123;").Using(c));

            "When I load the config"
                .f(async () => config = await new Config().UseRoslynCSharpLoader().LoadDictionary());

            "And I get Bar with a default of 456"
                .f(() => result = config.Get("Bar", 456));

            "Then the result is 456"
                .f(() => result.Should().Be(456));
        }
    }
}
