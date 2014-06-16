// <copyright file="Config.Global.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public partial class Config
    {
        private static readonly IList<Assembly> globalAutoLoadingReferences = new List<Assembly>();

        private static readonly Config global = new Config();

        public static bool GlobalAutoLoadingEnabled { get; set; }

        public static IList<Assembly> GlobalAutoLoadingReferences
        {
            get { return globalAutoLoadingReferences; }
        }

        public static IConfig Global
        {
            get { return GlobalAutoLoadingEnabled ? global : global.EnsureLoaded(globalAutoLoadingReferences.ToArray()); }
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
