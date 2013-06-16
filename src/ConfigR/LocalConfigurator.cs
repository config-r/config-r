// <copyright file="LocalConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;
    using ScriptCs.Engine.Roslyn;

    public class LocalConfigurator : IConfigurator
    {
        private static readonly ILog log = Common.Logging.LogManager.GetCurrentClassLogger();
        private static readonly string path = System.IO.Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) + ".csx";
        private readonly Dictionary<string, dynamic> configuration = new Dictionary<string, dynamic>();

        private bool loaded;

        public static string Path
        {
            get { return path; }
        }

        public IDictionary<string, dynamic> Configuration
        {
            get
            {
                this.EnsureLoaded();
                return this.configuration;
            }
        }

        public dynamic this[string key]
        {
            get
            {
                this.EnsureLoaded();
                return this.configuration[key];
            }
        }

        public void Add(string key, dynamic value)
        {
            log.InfoFormat(CultureInfo.InvariantCulture, "Adding configuration item with key '{0}', value {1}.", key, value);
            this.configuration.Add(key, value);
        }

        public void Load()
        {
            log.Debug("Clearing configuration");
            this.configuration.Clear();
            log.Debug("Loading configuration script " + path);
            var code = System.IO.File.ReadAllText(path);
            var engine = new RoslynScriptEngine(new ScriptHostFactory(), log);
            log.Debug("Compiling and executing configuration script " + path);
            var result = engine.Execute(code, new string[0], new[] { "System.dll" }, new[] { "System", "ConfigR" }, new ScriptPackSession(new IScriptPack[0]));
            if (result.CompileException != null)
            {
                log.Error("Error compiling configuration script " + path, result.CompileException);
                throw new InvalidOperationException("Failed to compile " + path, result.CompileException);
            }

            if (result.ExecuteException != null)
            {
                log.Error("Error executing configuration script " + path, result.ExecuteException);
                throw new InvalidOperationException("Failed to execute " + path, result.ExecuteException);
            }

            this.loaded = true;
        }

        public dynamic Get(string key)
        {
            this.EnsureLoaded();
            return this[key];
        }

        public dynamic GetOrDefault(string key)
        {
            this.EnsureLoaded();
            dynamic value;
            return this.configuration.TryGetValue(key, out value)
                ? value
                : default(dynamic);
        }

        public T Get<T>(string key)
        {
            this.EnsureLoaded();
            var value = this[key];
            return (T)value;
        }

        public T GetOrDefault<T>(string key)
        {
            this.EnsureLoaded();
            dynamic value;
            return this.configuration.TryGetValue(key, out value)
                ? (T)value
                : default(T);
        }
        
        private void EnsureLoaded()
        {
            if (this.loaded)
            {
                return;
            }

            this.Load();
        }
    }
}
