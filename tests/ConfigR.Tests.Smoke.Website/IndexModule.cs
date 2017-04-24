// <copyright file="IndexModule.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Smoke.Website
{
    using System.Diagnostics.CodeAnalysis;
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            this.Get["/"] = _ => this.View["index", new { Greeting = Global.Config.Get<string>("Greeting"), BuiltFor = Global.Config.Get<string>("BuiltFor") }];
        }
    }
}
