// <copyright file="EmptyConfigFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System.IO;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class EmptyConfigFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        [Example(null)]
        [Example("")]
        [Example(" ")]
        [Example("\n")]
        [Example("//")]
        [Example("// a comment")]
        public static void EmptyConfig(string code, object exception)
        {
            "Given a config file which contains no executable code"
                .f(() =>
                {
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(code);
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I load the config"
                .f(() => exception = Record.Exception(() => Config.Global));

            "Then no exception is thrown"
                .f(() => exception.Should().BeNull());
        }
    }
}
