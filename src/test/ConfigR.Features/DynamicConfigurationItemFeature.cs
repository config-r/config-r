// <copyright file="DynamicConfigurationItemFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class DynamicConfigurationItemFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration is loaded"
                .Given(() => Configurator.Unload());
        }

        [Scenario]
        public static void TryingToGetADynamicConfigurationItem()
        {
            // NOTE (adamralph): can't be a parameter - xunit runner borks
            var result = default(dynamic);

            "Given a config file with an integer of 123 named 'foo'"
                .f(() =>
                {
                    using (var writer = new StreamWriter("foo1.csx"))
                    {
                        writer.WriteLine(@"Add(""foo"", 123);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete("foo1.csx"));

            "When I load the file"
                .When(() => Configurator.Load("foo1.csx"));

            "And I try to get a dynamic named 'foo'"
                .f(() => Configurator.TryGet("foo", out result));

            "Then the result is 123"
                .f(() => ((object)result).Should().Be(123));
        }
    }
}