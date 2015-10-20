// <copyright file="ScriptConfigLoader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Linq;
    using System.Reflection;
    using ConfigR.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;
    using ScriptCs.Engine.Roslyn;

    public class ScriptConfigLoader
    {
        private static readonly Logging.ILog log = LogProvider.For<ScriptConfigLoader>();
        private readonly Assembly[] references;

        public ScriptConfigLoader(params Assembly[] references)
        {
            this.references = (references ?? Enumerable.Empty<Assembly>()).ToArray();
        }

        public object LoadFromFile(ISimpleConfig config, string path)
        {
            var fileSystem = new FileSystem { CurrentDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
            log.InfoFormat("Executing '{0}'", fileSystem.GetFullPath(path));
            log.DebugFormat("The current directory is {0}", fileSystem.CurrentDirectory);

            var scriptCsLog = new LogProviderAdapter();
            var lineProcessors = new ILineProcessor[]
            {
                new LoadLineProcessor(fileSystem),
                new ReferenceLineProcessor(fileSystem),
                new UsingLineProcessor(),
            };

            var filePreProcessor = new FilePreProcessor(fileSystem, scriptCsLog, lineProcessors);
            var engine = new RoslynScriptInMemoryEngine(new ConfigRScriptHostFactory(config), scriptCsLog);
            var executor = new ScriptExecutor(fileSystem, filePreProcessor, engine, scriptCsLog);
            executor.AddReferenceAndImportNamespaces(new[] { typeof(Config), typeof(IScriptHost) });
            executor.AddReferences(this.references);

            ScriptResult result;
            executor.Initialize(new string[0], new IScriptPack[0]);

            // HACK (adamralph): BaseDirectory is set to bin subfolder in Initialize()!
            executor.ScriptEngine.BaseDirectory = executor.FileSystem.CurrentDirectory;
            try
            {
                result = executor.Execute(path);
            }
            finally
            {
                executor.Terminate();
            }

            RethrowExceptionIfAny(result, path);
            return result.ReturnValue;
        }

        private static void RethrowExceptionIfAny(ScriptResult result, string scriptPath)
        {
            if (result.CompileExceptionInfo != null)
            {
                result.CompileExceptionInfo.Throw();
            }

            if (result.ExecuteExceptionInfo != null)
            {
                // HACK: waiting on https://github.com/scriptcs/scriptcs/issues/545
                if (!result.ExecuteExceptionInfo.SourceException.StackTrace.Trim()
                    .StartsWith("at Submission#", StringComparison.OrdinalIgnoreCase))
                {
                    log.WarnException(
                        "Roslyn failed to execute '{0}'. Any configuration in this script will not be available",
                        result.ExecuteExceptionInfo.SourceException,
                        scriptPath);
                }
                else
                {
                    result.ExecuteExceptionInfo.Throw();
                }
            }
        }

        private class LogProviderAdapter : ScriptCs.Contracts.ILogProvider
        {
            public ScriptCs.Contracts.Logger GetLogger(string name)
            {
                return (logLevel, messageFunc, exception, formatParameters) =>
                    LogProvider.GetLogger(name).Log((Logging.LogLevel)logLevel, messageFunc, exception, formatParameters);
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                return LogProvider.OpenMappedContext(key, value);
            }

            public IDisposable OpenNestedContext(string message)
            {
                return LogProvider.OpenNestedContext(message);
            }
        }
    }
}
