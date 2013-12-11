// <copyright file="ConfigRScriptEngine.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using Common.Logging;
    using Roslyn.Scripting;
    using Roslyn.Scripting.CSharp;
    using ScriptCs;
    using ScriptCs.Contracts;

    public class ConfigRScriptEngine : IScriptEngine
    {
        [SuppressMessage("Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate", Justification = "http://stackoverflow.com/questions/8140142/")]
        private static readonly string SessionKey = "Session";
        private readonly ISimpleConfig config;
        private readonly ScriptEngine roslynScriptEngine;
        private readonly IConfigRScriptHostFactory scriptHostFactory;
        private readonly ILog log;

        [CLSCompliant(false)]
        public ConfigRScriptEngine(ISimpleConfig config, IConfigRScriptHostFactory scriptHostFactory, ILog log)
        {
            Guard.AgainstNullArgument("scriptHostFactory", scriptHostFactory);
            Guard.AgainstNullArgument("log", log);

            this.config = config;
            this.roslynScriptEngine = new ScriptEngine();
            this.roslynScriptEngine.AddReference(typeof(ScriptExecutor).Assembly);
            this.scriptHostFactory = scriptHostFactory;
            this.log = log;
        }

        public string BaseDirectory
        {
            get { return this.roslynScriptEngine.BaseDirectory; }
            set { this.roslynScriptEngine.BaseDirectory = value; }
        }

        public string FileName { get; set; }

        [CLSCompliant(false)]
        public ScriptResult Execute(
            string code, string[] scriptArgs, IEnumerable<string> references, IEnumerable<string> namespaces, ScriptPackSession scriptPackSession)
        {
            Guard.AgainstNullArgument("scriptPackSession", scriptPackSession);

            this.log.Debug("Starting to create execution components");
            this.log.Debug("Creating script host");

            var distinctReferences = references.Union(scriptPackSession.References).Distinct().ToList();
            SessionState<Session> sessionState;

            if (!scriptPackSession.State.ContainsKey(SessionKey))
            {
                var host = this.scriptHostFactory.CreateScriptHost(this.config, new ScriptPackManager(scriptPackSession.Contexts), scriptArgs);
                this.log.Debug("Creating session");
                var session = this.roslynScriptEngine.CreateSession(host);

                foreach (var reference in distinctReferences)
                {
                    this.log.DebugFormat(CultureInfo.InvariantCulture, "Adding reference to {0}", reference);
                    session.AddReference(reference);
                }

                foreach (var @namespace in namespaces.Union(scriptPackSession.Namespaces).Distinct())
                {
                    this.log.DebugFormat(CultureInfo.InvariantCulture, "Importing namespace {0}", @namespace);
                    session.ImportNamespace(@namespace);
                }

                sessionState = new SessionState<Session> { References = distinctReferences, Session = session };
                scriptPackSession.State[SessionKey] = sessionState;
            }
            else
            {
                this.log.Debug("Reusing existing session");
                sessionState = (SessionState<Session>)scriptPackSession.State[SessionKey];

                var newReferences = sessionState.References == null ||
                    !sessionState.References.Any() ? distinctReferences : distinctReferences.Except(sessionState.References);

                if (newReferences.Any())
                {
                    foreach (var reference in newReferences)
                    {
                        this.log.DebugFormat(CultureInfo.InvariantCulture, "Adding reference to {0}", reference);
                        sessionState.Session.AddReference(reference);
                    }

                    sessionState.References = newReferences;
                }
            }

            this.log.Debug("Starting execution");
            var result = this.Execute(code, sessionState.Session);
            this.log.Debug("Finished execution");
            return result;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "ScriptCS does it ;-).")]
        [CLSCompliant(false),]
        protected virtual ScriptResult Execute(string code, Session session)
        {
            Guard.AgainstNullArgument("session", session);

            var result = new ScriptResult();
            try
            {
                var submission = session.CompileSubmission<object>(code);
                try
                {
                    result.ReturnValue = submission.Execute();
                }
                catch (Exception ex)
                {
                    result.ExecuteExceptionInfo = ExceptionDispatchInfo.Capture(ex);
                }
            }
            catch (Exception ex)
            {
                result.UpdateClosingExpectation(ex);
                if (!result.IsPendingClosingChar)
                {
                    result.CompileExceptionInfo = ExceptionDispatchInfo.Capture(ex);
                }
            }

            return result;
        }
    }
}
