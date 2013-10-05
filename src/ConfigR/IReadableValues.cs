// <copyright file="IReadableValues.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface IReadableValues
    {
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Necessary.")]
        IEnumerable<KeyValuePair<string, dynamic>> Items { get; }

        dynamic this[string key] { get; }

        bool TryGet(string key, out dynamic value);
    }
}
