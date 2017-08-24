using ConfigR.Tests.Acceptance.Roslyn.CSharp.Support;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbehave;

namespace ConfigR.Tests.Acceptance.Roslyn.CSharp
{
    public static class CSharpLanguageVersionFeature
    {
        [Scenario]
        public static void NamedTuples(Foo foo)
        {
            "Given a local file which assigns property values from a named tuple ('a','b')"
                .x(c => ConfigFile.Create(@"var a = ""a""; var b = ""b""; var tuple = (a,b); Config.Bar = tuple.a; Config.Baz = tuple.b;").Using(c));

            "When I load the config as Foo"
                .x(async () => foo = await new Config().UseRoslynCSharpLoader().Load<Foo>());

            "Then the Foo has a Bar of 'a'"
                .x(() => foo.Bar.Should().Be("a"));

            "And the Foo has a Baz of 'b'"
                .x(() => foo.Bar.Should().Be("a"));
        }
    }
}
