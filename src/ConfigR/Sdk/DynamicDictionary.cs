// <copyright file="DynamicDictionary.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using ConfigR.Internal;
    using static System.FormattableString;

    public partial class DynamicDictionary : DynamicObject
    {
        private static readonly Expression<Func<object, object>> castForRetreivalExample =
            @object => ObjectExtensions.CastForRetreival<object>(@object, null);

        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Guard.AgainstNullArgument(nameof(binder), binder);

            this.values[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Guard.AgainstNullArgument(nameof(binder), binder);

            return this.values.TryGetValue(binder.Name, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            Guard.AgainstNullArgument(nameof(binder), binder);

            var csharpBinder =
                binder.GetType().GetInterface("Microsoft.CSharp.RuntimeBinder.ICSharpInvokeOrInvokeMemberBinder");

            var genericTypeArgument =
                (csharpBinder.GetProperty("TypeArguments").GetValue(binder, null) as IList<Type>).FirstOrDefault();

            if (!this.values.TryGetValue(binder.Name, out result))
            {
                if (args != null && args.Any())
                {
                    if (!genericTypeArgument.IsInstanceOfType(args[0]))
                    {
                        throw new InvalidOperationException(
                            Invariant($"The specified default is not an instance of '{genericTypeArgument.FullName}'."));
                    }

                    result = args[0];
                    return true;
                }

                throw new InvalidOperationException(Invariant($"'{binder.Name}' does not exist."));
            }

            var castForRetreival = ((MethodCallExpression)castForRetreivalExample.Body)
                .Method.GetGenericMethodDefinition().MakeGenericMethod(genericTypeArgument);

            try
            {
                result = castForRetreival.Invoke(null, new object[] { result, binder.Name });
            }
            catch (Exception ex)
            {
                while (true)
                {
                    var tiex = ex as TargetInvocationException;
                    if (tiex == null)
                    {
                        break;
                    }

                    ex = tiex.InnerException;
                }

                ExceptionDispatchInfo.Capture(ex).Throw();
            }

            return true;
        }
    }
}
