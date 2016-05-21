// <copyright file="DynamicDictionary.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
            @object => ObjectExtensions.CastForRetrieval<object>(@object, null);

        private readonly Dictionary<string, object> values;

        public DynamicDictionary()
            : this(null)
        {
        }

        public DynamicDictionary(IDictionary<string, object> seed)
        {
            this.values = seed == null ? new Dictionary<string, object>() : new Dictionary<string, object>(seed);
        }

        public DynamicDictionary(object seed)
        {
            this.values = new Dictionary<string, object>();
            if (seed != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(seed.GetType()))
                {
                    this.values.Add(property.Name, property.GetValue(seed));
                }
            }
        }

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
            catch (TargetInvocationException ex)
            {
                Exception actualException = ex;
                while (actualException is TargetInvocationException)
                {
                    actualException = ex.InnerException;
                }

                ExceptionDispatchInfo.Capture(actualException).Throw();
            }

            return true;
        }
    }
}
