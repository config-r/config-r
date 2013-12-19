// <copyright file="ScriptIndexingFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;

    public static class ScriptIndexingFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .Given(() => Config.Global.Reset());
        }

        [Scenario]
        public static void AScriptIndexingItself(int value)
        {
            "Given a local config file which indexes itself to set an named 'value' of 123"
                .Given(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"This[""value""] = 123;");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the value"
                .When(() => value = Config.Global.Get<int>("value"));

            "Then the value is 123"
                .Then(() => value.Should().Be(123));
        }
    }
}
