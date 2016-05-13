// <copyright file="Global.asax.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Tests.Smoke.Website
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using ConfigR;

    public class Global : HttpApplication
    {
        [SuppressMessage(
            "StyleCop.CSharp.LayoutRules",
            "SA1500:CurlyBracketsForMultiLineStatementsMustNotShareLine",
            Justification = "Bug in StyleCop - see https://stylecop.codeplex.com/workitem/7723.")]
        public static dynamic Config { get; } = new Config()
            .UseRoslynCSharpLoader()
////#if DEBUG
////            .UseRoslynCSharpLoader("Web.Debug.csx")
////#else
////            .UseRoslynCSharpLoader("Web.Release.csx")
////#endif
            .Load()
            .GetAwaiter().GetResult();
    }
}
