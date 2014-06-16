// <copyright file="ScriptConfigLoader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Common.Logging;
    using ScriptCs;
    using ScriptCs.Contracts;
    using ScriptCs.Engine.Roslyn;

    public class ScriptConfigLoader
    {
        private static readonly ILog log = LogManager.GetCurrentClassLogger();
        private readonly Assembly[] references;

        public ScriptConfigLoader(params Assembly[] references)
        {
            this.references = (references ?? Enumerable.Empty<Assembly>()).ToArray();
        }

        public object LoadFromFile(ISimpleConfig config, string path)
        {
            var fileSystem = new FileSystem { CurrentDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
            log.InfoFormat(CultureInfo.InvariantCulture, "Executing '{0}'", fileSystem.GetFullPath(path));
            log.DebugFormat(CultureInfo.InvariantCulture, "The current directory is {0}", fileSystem.CurrentDirectory);

            var scriptCsLog = LogManager.GetLogger("ScriptCs");
            var lineProcessors = new ILineProcessor[]
            {
                new LoadLineProcessor(fileSystem),
                new ReferenceLineProcessor(fileSystem),
                new UsingLineProcessor(),
            };

            var filePreProcessor = new FilePreProcessor(fileSystem, scriptCsLog, lineProcessors);
            var engine = new RoslynScriptInMemoryEngine(new ConfigRScriptHostFactory(config), scriptCsLog);
            var executor = new ConfigRScriptExecutor(fileSystem, filePreProcessor, engine, scriptCsLog);
            executor.AddReferenceAndImportNamespaces(new[] { typeof(Config), typeof(IScriptHost) });
            executor.AddReferences(this.references);

            ScriptResult result;
            executor.Initialize(new string[0], new IScriptPack[0]);
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
                log.ErrorFormat(CultureInfo.InvariantCulture, "Failed to compile {0}", result.CompileExceptionInfo, scriptPath);
                result.CompileExceptionInfo.Throw();
            }

            if (result.ExecuteExceptionInfo != null)
            {
                // HACK: waiting on https://github.com/scriptcs/scriptcs/issues/545
                if (!result.ExecuteExceptionInfo.SourceException.StackTrace.Trim()
                    .StartsWith("at Submission#", StringComparison.OrdinalIgnoreCase))
                {
                    log.Warn(
                        "Roslyn failed to execute the scripts. Any configuration in this script will not be available",
                        result.ExecuteExceptionInfo.SourceException);
                }
                else
                {
                    log.ErrorFormat(CultureInfo.InvariantCulture, "Failed to execute {0}", result.ExecuteExceptionInfo, scriptPath);
                    result.ExecuteExceptionInfo.Throw();
                }
            }
        }
    }
}
