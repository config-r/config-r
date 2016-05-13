// <copyright file="IConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Sdk
{
    using System.Threading.Tasks;
    using ConfigR.Sdk;

    public interface IConfig
    {
        IConfig UseLoader(ILoader loader);

        Task<dynamic> Load();
    }
}
