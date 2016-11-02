// <copyright file="ILoader.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Sdk
{
    using System.Threading.Tasks;

    public interface ILoader
    {
        Task<DynamicDictionary> Load(DynamicDictionary config);
    }
}
