// <copyright file="Config.Global.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    public partial class Config
    {
        private static readonly Config global = new Config();

        public static bool GlobalAutoLoadingEnabled { get; set; }

        public static IConfig Global
        {
            get { return GlobalAutoLoadingEnabled ? global : global.EnsureLoaded(); }
        }

        public static IConfig DisableGlobalAutoLoading()
        {
            GlobalAutoLoadingEnabled = true;
            return global;
        }

        public static IConfig EnableGlobalAutoLoading()
        {
            GlobalAutoLoadingEnabled = false;
            return Global;
        }
    }
}
