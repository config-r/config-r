// <copyright file="LocalScriptFileConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Globalization;
    using System.IO;
    using Common.Logging;
    using ConfigR.Scripting;
    using IOPath = System.IO.Path;

    public class LocalScriptFileConfig : BasicConfig
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private static readonly string standardPath =
            IOPath.ChangeExtension(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, "csx");

        private static readonly string visualStudioHostSuffix = ".vshost";
        private static readonly int visualStudioHostSuffixLength = ".vshost".Length;

        private readonly bool tolerateFileNotFound;

        public LocalScriptFileConfig(bool tolerateFileNotFound)
        {
            this.tolerateFileNotFound = tolerateFileNotFound;
        }

        public LocalScriptFileConfig()
            : this(false)
        {
        }

        public static string Path
        {
            get { return Coerce(standardPath).Path; }
        }

        protected override string Source
        {
            get { return Path; }
        }

        public override ISimpleConfig Load()
        {
            var coercion = Coerce(standardPath);
            if (!File.Exists(coercion.Path))
            {
                if (!this.tolerateFileNotFound)
                {
                    throw new FileNotFoundException("Local file script not found.", coercion.Path);
                }

                return this;
            }

            if (coercion.Occurred)
            {
                log.WarnFormat(
                    CultureInfo.InvariantCulture,
                    "'{0}' not found. Loading '{1}' instead.",
                    IOPath.GetFileName(standardPath),
                    IOPath.GetFileName(coercion.Path));
            }

            new ScriptConfigLoader().LoadFromFile(this, coercion.Path);
            return this;
        }

        private static Coercion Coerce(string path)
        {
            if (!File.Exists(path))
            {
                var fileNameWithoutScriptExtension = IOPath.GetFileNameWithoutExtension(path);
                var fileNameWithoutAssemblyExtension = IOPath.GetFileNameWithoutExtension(fileNameWithoutScriptExtension);
                if (fileNameWithoutAssemblyExtension.EndsWith(visualStudioHostSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    var fileNameWithoutHostSuffix = string.Concat(
                        fileNameWithoutAssemblyExtension.Substring(
                            0, fileNameWithoutAssemblyExtension.Length - visualStudioHostSuffixLength),
                        IOPath.GetExtension(fileNameWithoutScriptExtension),
                        IOPath.GetExtension(path));

                    var pathWithoutHostSuffix = IOPath.Combine(IOPath.GetDirectoryName(path), fileNameWithoutHostSuffix);
                    if (File.Exists(pathWithoutHostSuffix))
                    {
                        return new Coercion { Path = pathWithoutHostSuffix, Occurred = true };
                    }
                }
            }

            return new Coercion { Path = path, Occurred = false };
        }

        private class Coercion
        {
            public string Path { get; set; }

            public bool Occurred { get; set; }
        }
    }
}
