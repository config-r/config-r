// <copyright file="IsolatedConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Acceptance
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class IsolatedConfigurationFeature
    {
        [Scenario]
        public static void RetrievingTheSameValueFromTwoFiles(string foo1, string foo2)
        {
            "Given a config file containing a Foo of 'baz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.WriteLine(@"Add(""foo"", ""baz"");");
                    }
                }).Teardown(() => File.Delete("foo1.csx"));

            "And a config file containing a Foo of 'bazzz'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo2.csx"))
                    {
                        writer.WriteLine(@"Add(""foo"", ""bazzz"");");
                    }
                }).Teardown(() => File.Delete("foo2.csx"));

            "When I get the foo from the first file"
                .f(() => foo1 = new ScriptFileConfig("foo1.csx").Load().Get<string>("foo"));

            "And I get the foo from the second file"
                .f(() => foo2 = new ScriptFileConfig("foo2.csx").Load().Get<string>("foo"));

            "Then the first Foo is 'baz'"
                .f(() => foo1.Should().Be("baz"));

            "And the second Foo is 'bazzz'"
                .f(() => foo2.Should().Be("bazzz"));
        }
    }
}
