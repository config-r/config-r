// <copyright file="LocalConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Globalization;
    using System.IO;
    using Common.Logging;
    using IOPath = System.IO.Path;

    public class LocalConfigurator : FileConfigurator
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly string path = System.IO.Path.ChangeExtension(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, "csx");
        private static readonly string visualStudioHostSuffix = ".vshost";
        private static readonly int visualStudioHostSuffixLength = ".vshost".Length;

        private string coercedPath;

        public LocalConfigurator()
            : base(path)
        {
        }

        public override string Path
        {
            get { return this.coercedPath ?? base.Path; }
        }

        public override IConfigurator Load()
        {
            if (!File.Exists(path))
            {
                var fileNameWithoutScriptExtension = IOPath.GetFileNameWithoutExtension(path);
                var fileNameWithoutAssemblyExtension = IOPath.GetFileNameWithoutExtension(fileNameWithoutScriptExtension);
                if (fileNameWithoutAssemblyExtension.EndsWith(visualStudioHostSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    var fileNameWithoutHostSuffix = string.Concat(
                        fileNameWithoutAssemblyExtension.Substring(0, fileNameWithoutAssemblyExtension.Length - visualStudioHostSuffixLength),
                        IOPath.GetExtension(fileNameWithoutScriptExtension),
                        IOPath.GetExtension(path));

                    var pathWithoutHostSuffix = IOPath.Combine(IOPath.GetDirectoryName(path), fileNameWithoutHostSuffix);
                    if (File.Exists(pathWithoutHostSuffix))
                    {
                        log.InfoFormat(CultureInfo.InvariantCulture, "'{0}' does not exist. Using '{1}' instead.", path, pathWithoutHostSuffix);
                        this.coercedPath = pathWithoutHostSuffix;
                    }
                }
            }

            return base.Load();
        }
    }
}
