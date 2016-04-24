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
            // HACK (adamralph): workaround for https://github.com/scriptcs/scriptcs/issues/1022
            var originalCurrentDirectory = Environment.CurrentDirectory;
            try
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
                var engine = new CSharpScriptInMemoryEngine(new ConfigRScriptHostFactory(config), scriptCsLog);
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
            finally
            {
                Environment.CurrentDirectory = originalCurrentDirectory;
            }
        }

        private static void RethrowExceptionIfAny(ScriptResult result, string scriptPath)
        {
            if (result.CompileExceptionInfo != null)
            {
                // HACK: ScriptCs.Engine.Roslyn.CSharpScriptInMemoryEngine fails miserably with no-op files
                if ((result.CompileExceptionInfo.SourceException.StackTrace == null && string.IsNullOrEmpty(result.CompileExceptionInfo.SourceException.Message)) ||
                    !result.CompileExceptionInfo.SourceException.StackTrace.Trim().StartsWith("at Submission#", StringComparison.OrdinalIgnoreCase))
                {
                    log.WarnException(
                        "scriptcs failed to execute '{0}'. Any configuration in this script will not be available",
                        result.CompileExceptionInfo.SourceException,
                        scriptPath);
                }
                else
                {
                    result.CompileExceptionInfo.Throw();
                }
            }

            if (result.ExecuteExceptionInfo != null)
            {
                result.ExecuteExceptionInfo.Throw();
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
