using NetFabric.Reflection;
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

                string @string => $"\"{@string}\"",

                Exception exception => exception.GetType().ToString(),

                Type type => type.ToString(),

                IEnumerable enumerable => enumerable.ToFriendlyString(),

                _ => DefaultToFriendlyString(@object),
            };

        static string DefaultToFriendlyString(object @object)
        {
            var type = @object.GetType();

            if (type.IsEnumerable(out var enumerableInfo))
            {
                var wrapperType = typeof(EnumerableWrapper<>).MakeGenericType(type);
                var wrapper = (IEnumerable)Activator.CreateInstance(wrapperType, @object, enumerableInfo);
                return wrapper.ToFriendlyString();
            }

            if (type.IsAsyncEnumerable(out enumerableInfo))
            {
                var wrapperType = typeof(AsyncEnumerableWrapper<>).MakeGenericType(type);
                var wrapper = (IEnumerable)Activator.CreateInstance(wrapperType, @object, enumerableInfo);
                return wrapper.ToFriendlyString();
            }

            return @object.ToString();
        }
    }
}