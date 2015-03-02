// <copyright file="LocalScriptFileConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using Common.Logging;
    using IOPath = System.IO.Path;

    public class LocalScriptFileConfig : ScriptConfig
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private static readonly string visualStudioHostSuffix = ".vshost";
        private static readonly int visualStudioHostSuffixLength = ".vshost".Length;

        private readonly bool tolerateFileNotFound;

        public LocalScriptFileConfig(bool tolerateFileNotFound, params Assembly[] references)
            : this(references)
        {
            this.tolerateFileNotFound = tolerateFileNotFound;
        }

        public LocalScriptFileConfig(params Assembly[] references)
            : base(references)
        {
        }

        public static string Path
        {
            get
            {
                var path = IOPath.ChangeExtension(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, "csx");
                if (!File.Exists(path))
                {
                    var fileNameWithoutScriptExtension = IOPath.GetFileNameWithoutExtension(path);
                    var fileNameWithoutAssemblyExtension = IOPath.GetFileNameWithoutExtension(fileNameWithoutScriptExtension);
                    if (fileNameWithoutAssemblyExtension != null &&
                        fileNameWithoutAssemblyExtension.EndsWith(visualStudioHostSuffix, StringComparison.OrdinalIgnoreCase))
                    {
                        var fileNameWithoutHostSuffix = string.Concat(
                            fileNameWithoutAssemblyExtension.Substring(
                                0, fileNameWithoutAssemblyExtension.Length - visualStudioHostSuffixLength),
                            IOPath.GetExtension(fileNameWithoutScriptExtension),
                            IOPath.GetExtension(path));

                        var directoryName = IOPath.GetDirectoryName(path);
                        var pathWithoutHostSuffix = directoryName == null
                            ? fileNameWithoutHostSuffix
                            : IOPath.Combine(directoryName, fileNameWithoutHostSuffix);
                        
                        if (File.Exists(pathWithoutHostSuffix))
                        {
                            return pathWithoutHostSuffix;
                        }
                    }
                }

                return path;
            }
        }

        protected override string Source
        {
            get { return Path; }
        }

        protected override void Load(string scriptPath)
        {
            if (!File.Exists(scriptPath))
            {
                if (!this.tolerateFileNotFound)
                {
                    throw new FileNotFoundException("Local file script not found.", scriptPath);
                }

                return;
            }

            var path = Path;
            if (scriptPath != path)
            {
                log.WarnFormat(
                    CultureInfo.InvariantCulture,
                    "'{0}' not found. Loading '{1}' instead.",
                    IOPath.GetFileName(path),
                    IOPath.GetFileName(scriptPath));
            }

            base.Load(scriptPath);
        }

        protected override string GetScriptPath()
        {
            return Path;
        }
    }
}
