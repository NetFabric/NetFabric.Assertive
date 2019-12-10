using NetFabric.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

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

        public static async ValueTask<string> ToFriendlyStringAsync<T>(IAsyncEnumerable<T> enumerable)
        {
            var builder = new StringBuilder();
            builder.Append('{');
            await using (var enumerator = enumerable.GetAsyncEnumerator())
            {
                var first = true;
                var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                while (await enumerator.MoveNextAsync())
                {
                    if (!first)
                    {
                        builder.Append(separator);
                        builder.Append(' ');
                    }
                    builder.Append(ToFriendlyString(enumerator.Current));
                    first = false;
                }
            }
            builder.Append('}');
            return builder.ToString();
        }

        static string DefaultToFriendlyString(object @object)
        {
            var type = @object.GetType();

            if (type.IsEnumerable(out var enumerableInfo))
            {
                var wrapperType = typeof(EnumerableWrapper<,>).MakeGenericType(type, typeof(object));
                var wrapper = (IEnumerable)Activator.CreateInstance(wrapperType, @object, enumerableInfo);
                return ToFriendlyString(wrapper);
            }

            if (type.IsAsyncEnumerable(out var asyncEnumerableInfo))
            {
                var wrapperType = typeof(AsyncEnumerableWrapper<,>).MakeGenericType(type, typeof(object));
                var wrapper = (IAsyncEnumerable<object>)Activator.CreateInstance(wrapperType, @object, asyncEnumerableInfo);
                return ToFriendlyStringAsync(wrapper).GetAwaiter().GetResult();
            }

            return @object.ToString();
        }
    }
}