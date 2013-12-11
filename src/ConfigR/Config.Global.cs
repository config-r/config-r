// <copyright file="Config.Global.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    public partial class Config
    {
        private static readonly Config global = new Config();

        private static bool suppressGlobalLoad;

        public static IConfig Global
        {
            get { return suppressGlobalLoad ? global : global.EnsureLoaded(); }
        }

        public static IConfig SuppressGlobalLoad()
        {
            suppressGlobalLoad = true;
            return global;
        }

        public static IConfig AllowGlobalLoad()
        {
            suppressGlobalLoad = false;
            return Global;
        }
    }
}
