// <copyright file="IndexModule.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Testing.Website
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            this.Get["/"] = parameters =>
            {
                return View["index"];
            };
        }
    }
}