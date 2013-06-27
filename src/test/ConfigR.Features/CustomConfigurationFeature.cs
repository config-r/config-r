// <copyright file="CustomConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Xbehave;

    public static class CustomConfigurationFeature
    {
        [Scenario]
        public static void RetreivingAnObject(IConfigurator configurator, string result)
        {
            "Given a custom configurator containing a string of 'bar' keyed by 'foo'"
                .Given(() => configurator = new CustomConfigurator());

            "When I load the configurator"
                .When(() => Configurator.Load(configurator));

            "And I get the Foo"
                .And(() => result = Configurator.Get<string>("foo"))
                .Teardown(() => Configurator.Unload());

            "Then the result should be 'bar'"
                .Then(() => result.Should().Be("bar"));
        }

        public class Foo
        {
            public string Bar { get; set; }
        }

        public class CustomConfigurator : IConfigurator
        {
            private readonly Dictionary<string, dynamic> configuration = new Dictionary<string, dynamic> { { "foo", "bar" } };

            public IEnumerable<KeyValuePair<string, dynamic>> Items
            {
                get { return this.configuration.Select(item => item); }
            }

            public dynamic this[string key]
            {
                get { return this.configuration[key]; }
            }

            public IConfigurator Load()
            {
                return this;
            }

            public IConfigurator Add(string key, dynamic value)
            {
                this.configuration.Add(key, value);
                return this;
            }

            public bool TryGet(string key, out dynamic value)
            {
                return this.configuration.TryGetValue(key, out value);
            }
        }
    }
}
