// <copyright file="ConfigRScriptHack.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using ScriptCs.Contracts;

    internal class ConfigRScriptHack : IScriptPack
    {
        public void Initialize(IScriptPackSession session)
        {
            Guard.AgainstNullArgument("session", session);

            session.AddReference(typeof(ConfigRScriptHack).Assembly.Location);
            session.ImportNamespace("ConfigR");
        }

        public IScriptPackContext GetContext()
        {
            return null;
        }

        public void Terminate()
        {
        }
    }
}
