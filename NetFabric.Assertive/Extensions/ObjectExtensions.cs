using System;
using System.Collections;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class ObjectExtensions
    {
        public static string ToFriendlyString(this object @object)
            => @object switch
            {
                null => "<null>",

                IEnumerable enumerable => enumerable.ToFriendlyString(),

                _ => @object.GetType().IsEnumerable(out var info) ?  
                    new EnumerableWrapper(@object, info).ToFriendlyString() :
                    @object.ToString(),
            };
     }
}