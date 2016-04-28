// <copyright file="ConfigExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using ConfigR.Roslyn.CSharp.Internal;
    using ConfigR.Sdk;

    public static class ConfigExtensions
    {
        public static IConfig UseRoslynCSharpLoader(this IConfig config)
        {
            return config?.UseLoader(new Loader());
        }
    }
}
