// <copyright file="RoslynScriptEngine.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Scripting.Shims
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using Common.Logging;
    using Roslyn.Scripting;
    using Roslyn.Scripting.CSharp;
    using ScriptCs;
    using ScriptCs.Contracts;

    [CLSCompliant(false)]
    public class RoslynScriptEngine : IScriptEngine
    {
        public const string SessionKey = "Session";
        protected readonly ScriptEngine ScriptEngine;
        private readonly IScriptHostFactory scriptHostFactory;

        public RoslynScriptEngine(IScriptHostFactory scriptHostFactory, ILog logger)
        {
            this.ScriptEngine = new ScriptEngine();
            this.ScriptEngine.AddReference(typeof(ScriptExecutor).Assembly);
            this.scriptHostFactory = scriptHostFactory;
            this.Logger = logger;
        }

        public string BaseDirectory
        {
            get { return this.ScriptEngine.BaseDirectory; }
            set { this.ScriptEngine.BaseDirectory = value; }
        }

        public string FileName { get; set; }

        protected ILog Logger { get; private set; }

        public ScriptResult Execute(string code, string[] scriptArgs, IEnumerable<string> references, IEnumerable<string> namespaces, ScriptPackSession scriptPackSession)
        {
            Guard.AgainstNullArgument("scriptPackSession", scriptPackSession);

            this.Logger.Debug("Starting to create execution components");
            this.Logger.Debug("Creating script host");

            var distinctReferences = references.Union(scriptPackSession.References).Distinct().ToList();
            SessionState<Session> sessionState;

            if (!scriptPackSession.State.ContainsKey(SessionKey))
            {
                var host = this.scriptHostFactory.CreateScriptHost(new ScriptPackManager(scriptPackSession.Contexts), scriptArgs);
                this.Logger.Debug("Creating session");
                var session = this.ScriptEngine.CreateSession(host, host.GetType());

                foreach (var reference in distinctReferences)
                {
                    this.Logger.DebugFormat("Adding reference to {0}", reference);
                    session.AddReference(reference);
                }

                foreach (var @namespace in namespaces.Union(scriptPackSession.Namespaces).Distinct())
                {
                    this.Logger.DebugFormat("Importing namespace {0}", @namespace);
                    session.ImportNamespace(@namespace);
                }

                sessionState = new SessionState<Session> { References = distinctReferences, Session = session };
                scriptPackSession.State[SessionKey] = sessionState;
            }
            else
            {
                this.Logger.Debug("Reusing existing session");
                sessionState = (SessionState<Session>)scriptPackSession.State[SessionKey];

                var newReferences = sessionState.References == null || !sessionState.References.Any() ? distinctReferences : distinctReferences.Except(sessionState.References);
                if (newReferences.Any())
                {
                    foreach (var reference in newReferences)
                    {
                        this.Logger.DebugFormat("Adding reference to {0}", reference);
                        sessionState.Session.AddReference(reference);
                    }

                    sessionState.References = newReferences;
                }
            }

            this.Logger.Debug("Starting execution");
            var result = this.Execute(code, sessionState.Session);
            this.Logger.Debug("Finished execution");
            return result;
        }

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