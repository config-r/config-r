// <copyright file="ScriptGlobals.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp.Internal
{
    using ConfigR.Sdk;

    public class ScriptGlobals
    {
        public ScriptGlobals(DynamicDictionary config)
        {
            this.Config = config;
        }

        public dynamic Config { get; }
    }
}
