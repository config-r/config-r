// <copyright file="LocalConfigurator.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR
{
    using System;

    public class LocalConfigurator : FileConfigurator
    {
        private static readonly string path = System.IO.Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile) + ".csx";

        public LocalConfigurator()
            : base(path)
        {
        }
    }
}
