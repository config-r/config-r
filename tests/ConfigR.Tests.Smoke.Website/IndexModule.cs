// <copyright file="IndexModule.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Smoke.Website
{
    using System.Diagnostics.CodeAnalysis;
    using Nancy;

    public class IndexModule : NancyModule
    {
        [SuppressMessage(
            "StyleCop.CSharp.SpacingRules",
            "SA1013:ClosingCurlyBracketsMustBeSpacedCorrectly",
            Justification = "Bug in StyleCop - see https://stylecop.codeplex.com/workitem/7725.")]
        public IndexModule()
        {
            this.Get["/"] = _ => this.View["index", new { Greeting = Global.Config.Get<string>("Greeting"), BuiltFor = Global.Config.Get<string>("BuiltFor") }];
        }
    }
}
