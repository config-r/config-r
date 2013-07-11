// <copyright file="IndexModule.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Testing.Website
{
    using System.Globalization;
    using ConfigR;
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            this.Get["/"] = parameters =>
            {
                // NOTE (adamralph): in a real world app you probably wouldn't use configuration directly within a HTTP module in this way
                // in a real world app you'd typically use configuration to configure your IoC container
                // in the case of a Nancy app, this would be done in your custom bootstrapper
                var greeting = string.Format(
                    CultureInfo.InvariantCulture, "{0}, I'm built for {1}!", Configurator.Get<string>("greeting"), Configurator.Get<string>("builtfor"));

                var model = new { Greeting = greeting };
                return View["index", model];
            };
        }
    }
}