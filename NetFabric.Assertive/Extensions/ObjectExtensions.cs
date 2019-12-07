using NetFabric.Reflection;
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class ObjectExtensions
    {
        public static string ToFriendlyString(object @object)
            => @object switch
            {
                null => "<null>",

                string @string => $"\"{@string}\"",

                Exception exception => exception.GetType().ToString(),

                Type type => type.ToString(),

                IEnumerable enumerable => ToFriendlyString(enumerable),

                _ => DefaultToFriendlyString(@object),
            };

        public static string ToFriendlyString(IEnumerable enumerable)
        {
            var builder = new StringBuilder();
            builder.Append('{');
            var enumerator = enumerable.GetEnumerator();
            var first = true;
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            while (enumerator.MoveNext())
            {
                if (!first)
                {
                    builder.Append(separator);
                    builder.Append(' ');
                }
                builder.Append(ToFriendlyString(enumerator.Current));
                first = false;
            }
            builder.Append('}');
            return builder.ToString();
        }

        static string DefaultToFriendlyString(object @object)
        {
            var type = @object.GetType();

            if (type.IsEnumerable(out var enumerableInfo))
            {
                var wrapperType = typeof(EnumerableWrapper<>).MakeGenericType(type);
                var wrapper = (IEnumerable)Activator.CreateInstance(wrapperType, @object, enumerableInfo);
                return ToFriendlyString(wrapper);
            }

            if (type.IsAsyncEnumerable(out enumerableInfo))
            {
                var wrapperType = typeof(AsyncEnumerableWrapper<>).MakeGenericType(type);
                var wrapper = (IEnumerable)Activator.CreateInstance(wrapperType, @object, enumerableInfo);
                return ToFriendlyString(wrapper);
            }

            return @object.ToString();
        }
    }
}