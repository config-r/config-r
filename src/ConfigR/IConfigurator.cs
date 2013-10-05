﻿// <copyright file="IConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    public interface IConfigurator : IReadableValues
    {
        IConfigurator Load();

        IConfigurator Add(string key, dynamic value);
    }
}
