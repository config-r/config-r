// <copyright file="AnonymousValuesFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xbehave;
    using Xunit;

    public static class AnonymousValuesFeature
    {
        [Background]
        public static void Background()
        {
            "Given no configuration has been loaded"
                .f(() => Config.Global.Reset());
        }

        [Scenario]
        public static void RetrievingAnAnonymousValue(Foo result)
        {
            "Given a local config file containing an anonymous Foo with a Bar of 'baz'"
                .f(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the Foo"
                .f(() => result = Config.Global.Get<Foo>());

            "Then the Foo has a Bar of 'baz'"
                .f(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void RetrievingANamedValueAnonymously(Foo result)
        {
            "Given a local config file containing a named Foo with a Bar of 'baz'"
                .f(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get the Foo"
                .f(() => result = Config.Global.Get<Foo>());

            "Then the Foo has a Bar of 'baz'"
                .f(() => result.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void RetrievingAnAnonymousValueFromMultipleValues(int result)
        {
            "Given a local config file containing multiple values"
                .f(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.WriteLine(@"Add(""stringId"", ""34"");");
                        writer.WriteLine(@"Add(""id"", 12);");
                        writer.WriteLine(@"Add(""foo 2"", new Foo { Bar = ""baz 2"" });");
                        writer.WriteLine(@"Add(""code"", 15);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get int item"
                .f(() => result = Config.Global.Get<int>());

            "Then it should be '12'"
                .f(() => result.Should().Be(12));
        }

        [Scenario]
        public static void TryingToRetrieveAnAnonymousValue(Foo value, bool result)
        {
            "Given a local config file containing a named Foo with a Bar of 'baz'"
                .f(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I try to get a Foo"
                .f(() => result = Config.Global.TryGetValue(out value));

            "Then the result is true"
                .f(() => result.Should().BeTrue());

            "And the Foo has a Bar of 'baz'"
                .f(() => value.Bar.Should().Be("baz"));
        }

        [Scenario]
        public static void RetrievingANonexistentAnonymousValue(Exception ex)
        {
            "Given a local config file not containing any string item"
                .f(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.WriteLine(@"Add(""id"", 12);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I try to get a string item"
                .f(() => ex = Record.Exception(() => Config.Global.Get<string>()));

            "Then an exception is thrown"
                .f(() => ex.Should().NotBeNull());
        }

        [Scenario]
        public static void RetrievingANonexistentAnonymousValueOrDefault(string result)
        {
            "Given a local config file not containing any string item"
                .f(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.WriteLine(@"Add(""id"", 12);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I get a string item or default"
                .f(() => result = Config.Global.GetOrDefault<string>());

            "Then the result should be the default string"
                .f(() => result.Should().Be(default(string)));
        }

        [Scenario]
        public static void TryingToRetrieveANonexistentAnonymousValue(string value, bool result)
        {
            "Given a local config file not containing any string item"
                .f(() =>
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", "Test.config");
                    using (var writer = new StreamWriter(LocalScriptFileConfig.Path))
                    {
                        writer.WriteLine(@"#r ""ConfigR.Features.dll""");
                        writer.WriteLine(@"using ConfigR.Features;");
                        writer.WriteLine(@"Add(""foo"", new Foo { Bar = ""baz"" });");
                        writer.WriteLine(@"Add(""id"", 12);");
                        writer.Flush();
                    }
                })
                .Teardown(() => File.Delete(LocalScriptFileConfig.Path));

            "When I try to get a string item"
                .f(() => result = Config.Global.TryGetValue(out value));

            "Then the result should be false"
                .f(() => result.Should().BeFalse());
        }
    }
}
