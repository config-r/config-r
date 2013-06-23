// <copyright file="CustomConfigurationFeature.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Features
{
    using System;
    using System.Collections.Generic;
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
            public IDictionary<string, dynamic> Configuration
            {
                get { return new Dictionary<string, dynamic> { { "foo", "bar" } }; }
            }

            public dynamic this[string key]
            {
                get { throw new NotImplementedException(); }
            }

            public IConfigurator Load()
            {
                return this;
            }

            public IConfigurator Add(string key, dynamic value)
            {
                return this;
            }
        }
    }
}
