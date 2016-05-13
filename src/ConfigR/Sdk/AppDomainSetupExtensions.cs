// <copyright file="AppDomainSetupExtensions.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

#if TESTS_ACCEPTANCE_ROSLYN_CSHARP
namespace ConfigR.Tests.Acceptance.Roslyn.CSharp.Support
#else
namespace ConfigR.Sdk
#endif
{
    using System;
    using System.IO;

    public static class AppDomainSetupExtensions
    {
        private static readonly string visualStudioHostSuffix = ".vshost";
        private static readonly int visualStudioHostSuffixLength = ".vshost".Length;

        public static string VSHostingAgnosticConfigurationFile(this AppDomainSetup setup)
        {
            var path = setup?.ConfigurationFile;
            if (path == null)
            {
                return null;
            }

            var fileNameWithoutConfigExtension = Path.GetFileNameWithoutExtension(path);
            var fileNameWithoutAssemblyTypeExtension = Path.GetFileNameWithoutExtension(fileNameWithoutConfigExtension);
            if (fileNameWithoutAssemblyTypeExtension.EndsWith(visualStudioHostSuffix, StringComparison.OrdinalIgnoreCase))
            {
                var fileNameWithoutHostSuffix = string.Concat(
                    fileNameWithoutAssemblyTypeExtension.Substring(
                        0, fileNameWithoutAssemblyTypeExtension.Length - visualStudioHostSuffixLength),
                    Path.GetExtension(fileNameWithoutConfigExtension),
                    Path.GetExtension(path));

                return Path.Combine(Path.GetDirectoryName(path) ?? string.Empty, fileNameWithoutHostSuffix);
            }

            return path;
        }
    }
}
