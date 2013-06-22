// <copyright file="FileConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;
    using ScriptCs.Engine.Roslyn;

    public class FileConfigurator : IConfigurator
    {
        private static readonly ILog log = Common.Logging.LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, dynamic> configuration = new Dictionary<string, dynamic>();
        private readonly string path;

        public FileConfigurator(string path)
        {
            this.path = path;
        }

        public string Path
        {
            get { return this.path; }
        }

        public IDictionary<string, dynamic> Configuration
        {
            get { return this.configuration; }
        }

        public dynamic this[string key]
        {
            get { return this.configuration[key]; }
        }

        public IConfigurator Load()
        {
            log.Debug("Clearing configuration");
            this.configuration.Clear();
            log.DebugFormat(CultureInfo.InvariantCulture, "Loading configuration script {0}", this.path);
            var code = System.IO.File.ReadAllText(this.path);
            var engine = new RoslynScriptEngine(new ScriptHostFactory(), log);
            log.DebugFormat(CultureInfo.InvariantCulture, "Compiling and executing configuration script {0}", this.path);
#if DEBUG
            var result = engine.Execute(code, new string[0], new[] { "System.dll", "ConfigR.dll" }, new[] { "System", "ConfigR" }, new ScriptPackSession(new IScriptPack[0]));
#else
            var result = engine.Execute(code, new string[0], new[] { "System.dll" }, new[] { "System", "ConfigR" }, new ScriptPackSession(new IScriptPack[0]));
#endif

            if (result.CompileException != null)
            {
                log.ErrorFormat(CultureInfo.InvariantCulture, "Error compiling configuration script {0}", result.CompileException, this.path);
                throw new InvalidOperationException("Failed to compile " + this.path, result.CompileException);
            }

            if (result.ExecuteException != null)
            {
                log.ErrorFormat(CultureInfo.InvariantCulture, "Error executing configuration script {0}", result.ExecuteException, this.path);
                throw new InvalidOperationException("Failed to execute " + this.path, result.ExecuteException);
            }

            return this;
        }

        public IConfigurator Add(string key, dynamic value)
        {
            log.InfoFormat(CultureInfo.InvariantCulture, "Adding configuration item with key '{0}', value {1}.", key, value);
            this.configuration.Add(key, value);
            return this;
        }
    }
}
