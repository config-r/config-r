// <copyright file="IRoslynCSharpConfig.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Roslyn.CSharp
{
    using System;
    using System.Reflection;
    using ConfigR.Sdk;
    using Microsoft.CodeAnalysis;

    [CLSCompliant(false)]
    public interface IRoslynCSharpConfig : IConfig
    {
        IRoslynCSharpConfig AddReferences(params Assembly[] references);

        IRoslynCSharpConfig AddReferences(params MetadataReference[] references);
    }
}
