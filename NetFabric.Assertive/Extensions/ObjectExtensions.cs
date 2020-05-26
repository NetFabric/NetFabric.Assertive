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
        public static string ToFriendlyString(object? @object)
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
            var builder = StringBuilderPool.Get();
            try
            {
                _ = builder.Append('{');
                var enumerator = enumerable.GetEnumerator();
                var first = true;
                var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                while (enumerator.MoveNext())
                {
                    if (!first)
                    {
                        _ = builder.Append(separator).Append(' ');
                    }
                    _ = builder.Append(ToFriendlyString(enumerator.Current));
                    first = false;
                }
                _ = builder.Append('}');
                return builder.ToString();
            }
            finally
            {
                StringBuilderPool.Return(builder);
            }
        }

        public static async ValueTask<string> ToFriendlyStringAsync<T>(IAsyncEnumerable<T> enumerable)
        {
            var builder = StringBuilderPool.Get();
            try
            {
                _ = builder.Append('{');
                var enumerator = enumerable.GetAsyncEnumerator();
                await using (enumerator.ConfigureAwait(false))
                {
                    var first = true;
                    var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                    while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                    {
                        if (!first)
                        {
                            _ = builder.Append(separator).Append(' ');
                        }
                        _ = builder.Append(ToFriendlyString(enumerator.Current));
                        first = false;
                    }
                }
                _ = builder.Append('}');
                return builder.ToString();
            }
            finally
            {
                StringBuilderPool.Return(builder);
            }
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