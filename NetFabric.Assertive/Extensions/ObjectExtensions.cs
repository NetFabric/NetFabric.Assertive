using NetFabric.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class ObjectExtensions
    {
        static readonly string ListSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        public static string ToFriendlyString(this object? @object)
            => @object switch
            {
                null => "<null>",

                string @string => $"\"{@string}\"",

                Exception exception => exception.GetType().ToString(),

                Type type => type.ToString(),

                IEnumerable enumerable => enumerable.ToFriendlyString(),

                _ => @object.DefaultToFriendlyString(),
            };

        public static string ToFriendlyString(this IEnumerable enumerable)
        {
            var builder = StringBuilderPool.Get();
            try
            {
                _ = builder.Append('{').Append(' ');
                var enumerator = enumerable.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    _ = builder.Append(enumerator.Current);
                    while (enumerator.MoveNext())
                    {
                        _ = builder.Append(ListSeparator).Append(' ').Append(ToFriendlyString(enumerator.Current));
                    }
                    _ = builder.Append(' ');
                }
                _ = builder.Append('}');
                return builder.ToString();
            }
            finally
            {
                StringBuilderPool.Return(builder);
            }
        }

        public static async ValueTask<string> ToFriendlyStringAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            var builder = StringBuilderPool.Get();
            try
            {
                _ = builder.Append('{').Append(' ');
                var enumerator = enumerable.GetAsyncEnumerator();
                await using (enumerator.ConfigureAwait(false))
                {
                    if (await enumerator.MoveNextAsync().ConfigureAwait(false))
                    {
                        _ = builder.Append(enumerator.Current);
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            _ = builder.Append(ListSeparator).Append(' ').Append(ToFriendlyString(enumerator.Current));
                        }
                        _ = builder.Append(' ');
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

        public static string ToFriendlyString<T>(this T[] enumerable)
        {
            var builder = StringBuilderPool.Get();
            try
            {
                _ = builder.Append('{').Append(' ');
                if (enumerable.Length != 0)
                {
                    _ = builder.Append(enumerable[0]);
                    for (var index = 1; index < enumerable.Length; index++)
                    {
                        _ = builder.Append(ListSeparator).Append(' ').Append(ToFriendlyString(enumerable[index]));
                    }
                    _ = builder.Append(' ');
                }
                _ = builder.Append('}');
                return builder.ToString();
            }
            finally
            {
                StringBuilderPool.Return(builder);
            }
        }

        public static string ToFriendlyString<T>(this IReadOnlyList<T> enumerable)
        {
            var builder = StringBuilderPool.Get();
            try
            {
                _ = builder.Append('{').Append(' ');
                if (enumerable.Count != 0)
                {
                    _ = builder.Append(enumerable[0]);
                    for (var index = 1; index < enumerable.Count; index++)
                    {
                        _ = builder.Append(ListSeparator).Append(' ').Append(ToFriendlyString(enumerable[index]));
                    }
                    _ = builder.Append(' ');
                }
                _ = builder.Append('}');
                return builder.ToString();
            }
            finally
            {
                StringBuilderPool.Return(builder);
            }
        }

        static string DefaultToFriendlyString(this object @object)
        {
            var type = @object.GetType();

            if (type.IsEnumerable(out var enumerableInfo))
            {
                var wrapperType = typeof(EnumerableWrapper<,>).MakeGenericType(type, typeof(object));
                var wrapper = (IEnumerable)Activator.CreateInstance(wrapperType, @object, enumerableInfo);
                return wrapper.ToFriendlyString();
            }

            if (type.IsAsyncEnumerable(out var asyncEnumerableInfo))
            {
                var wrapperType = typeof(AsyncEnumerableWrapper<,>).MakeGenericType(type, typeof(object));
                var wrapper = (IAsyncEnumerable<object>)Activator.CreateInstance(wrapperType, @object, asyncEnumerableInfo);
                return wrapper.ToFriendlyStringAsync().GetAwaiter().GetResult();
            }

            return @object.ToString();
        }
    }
}