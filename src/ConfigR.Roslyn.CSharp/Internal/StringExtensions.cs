// <copyright file="StringExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using System;
    using System.IO;
    using ConfigR.Sdk;

    public static class StringExtensions
    {
        public static string ResolveScriptPath(this string scriptPath)
        {
            scriptPath = scriptPath ??
                Path.ChangeExtension(AppDomain.CurrentDomain.SetupInformation.VSHostingAgnosticConfigurationFile(), "csx");

            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, scriptPath);
        }
    }
}
