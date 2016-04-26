// <copyright file="Global.asax.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Smoke.Website
{
    using System;
    using System.Web;
    using ConfigR;

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Config.Global
                .LoadLocalScriptFile()
#if DEBUG
                .LoadScriptFile("Web.Debug.csx");
#else
                .LoadScriptFile("Web.Release.csx");
#endif
        }
    }
}
