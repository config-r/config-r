// <copyright file="IConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Sdk
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    public interface IConfig
    {
        IConfig UseLoader(ILoader loader);

        Task<dynamic> Load();

        Task<dynamic> Load(object seed);

        [SuppressMessage(
            "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "'Advanced' feature.")]
        Task<IDictionary<string, object>> LoadDictionary();

        [SuppressMessage(
            "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "'Advanced' feature.")]
        Task<IDictionary<string, object>> LoadDictionary(object seed);
    }
}
