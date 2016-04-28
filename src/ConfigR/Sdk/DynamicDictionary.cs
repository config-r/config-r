// <copyright file="DynamicDictionary.cs" company="ConfigR contributors">
//  Copyright (c) ConfigR contributors. (configr.net@gmail.com)
// </copyright>

namespace ConfigR.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using static System.FormattableString;

    public partial class DynamicDictionary : DynamicObject
    {
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
                throw new InvalidOperationException(Invariant($"'{binder.Name}' does not exist."));
            }

            if (!genericTypeArgument.IsInstanceOfType(result))
            {
                throw new InvalidOperationException(
                    Invariant($"'{binder.Name}' is not an instance of '{genericTypeArgument.FullName}'."));
            }

            return true;
        }
    }
}
