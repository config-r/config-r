// <copyright file="Global.asax.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.WebApplication
{
    using System;
    using System.Web;
    using ConfigR;

    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // NOTE (adamralph): If you want to load a specific config, e.g. Web.Debug.csx or Web.Release.csx, and you also want Web.csx to be loaded,
            // you need to call LoadLocal() before calling Load("...") otherwise ConfigR will assume that you *only* want to load the specific config.
            // On the other hand, if you are *not* loading any specific config and you only want Web.csx to be loaded,
            // you don't need to call any Load...() methods, ConfigR will do the loading for you as usual.
            // In that case, this entire class could be removed from this sample.
            Configurator
                .LoadLocal()
#if DEBUG
                .Load("Web.Debug.csx");
#else
                .Load("Web.Release.csx");
#endif
        }
    }
}
