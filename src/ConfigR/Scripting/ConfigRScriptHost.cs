// <copyright file="ConfigRScriptHost.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Collections.Generic;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public partial class ConfigRScriptHost : ScriptHost, IConfigRScriptHost
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();

        private readonly IDictionary<string, object> dictionary;

        public ConfigRScriptHost(IDictionary<string, object> dictionary, IScriptPackManager scriptPackManager, string[] scriptArgs)
            : base(scriptPackManager, scriptArgs)
        {
            Guard.AgainstNullArgument("dictionary", dictionary);

            this.dictionary = dictionary;
        }

        public IConfigRScriptHost This
        {
            get { return this; }
        }

        public IConfig Global
        {
            get { return ConfigR.Config.Global; }
        }

        public void Add(object value)
        {
            this.dictionary.Add(value);
        }

        public T Get<T>()
        {
            return this.dictionary.Get<T>();
        }

        public IConfigRScriptHost LoadWebScript(Uri uri)
        {
            ConfigR.Config.Global.LoadWebScript(uri);
            return this;
        }

        public IConfigRScriptHost LoadScriptFile(string path)
        {
            ConfigR.Config.Global.LoadScriptFile(path);
            return this;
        }

        public IConfigRScriptHost LoadLocalScriptFile()
        {
            ConfigR.Config.Global.LoadLocalScriptFile();
            return this;
        }

        public IConfigRScriptHost Load(ISimpleConfig config)
        {
            ConfigR.Config.Global.Load(config);
            return this;
        }

        [Obsolete("Deprecated since version 0.9 and will soon be removed. Use LoadWebScript(Uri) instead.")]
        public IConfigRScriptHost Load(Uri uri)
        {
            log.Warn("Load(Uri) in scripts is deprecated since version 0.9 and will soon be removed. Use LoadWebScript(Uri) instead.");
            return this.LoadWebScript(uri);
        }

        [Obsolete("Deprecated since version 0.9 and will soon be removed. Use LoadScriptFile(string) instead.")]
        public IConfigRScriptHost Load(string path)
        {
            log.Warn("Load(string) in scripts is deprecated since version 0.9 and will soon be removed. Use LoadScriptFile(string) instead.");
            return this.LoadScriptFile(path);
        }

        [Obsolete("Deprecated since version 0.9 and will soon be removed. Use LoadLocalScriptFile() instead.")]
        public IConfigRScriptHost LoadLocal()
        {
            log.Warn("LoadLocal() in scripts is deprecated since version 0.9 and will soon be removed. Use LoadLocalScriptFile() instead.");
            return this.LoadLocalScriptFile();
        }
    }
}
