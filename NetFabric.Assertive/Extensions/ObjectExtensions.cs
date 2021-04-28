using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static NetFabric.Expressions.ExpressionEx;
using static System.Linq.Expressions.Expression;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class ObjectExtensions
    {
        static readonly string ListSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        static readonly string NullFriendlyString = "<null>";
        static readonly string EmptyFriendlyString = "<empty>";

        public static string ToFriendlyString(this object? @object)
            => @object switch
            {
                null => NullFriendlyString,

                string { Length: 0 } => EmptyFriendlyString,

                string @string => $"\"{@string}\"",

                Exception exception => exception.GetType().ToString(),

                Type type => type.ToString(),

                _ => @object.DefaultToFriendlyString(),
            };

        static string DefaultToFriendlyString(this object @object)
        {
            var type = @object.GetType();

            if (type.IsEnumerable(out var enumerableInfo))
                return @object.ToFriendlyString(enumerableInfo);

            if (type.IsAsyncEnumerable(out var asyncEnumerableInfo))
            {
                var wrapperType = typeof(AsyncEnumerableWrapper<,>).MakeGenericType(type, typeof(object));
                var wrapper = (IAsyncEnumerable<object>)Activator.CreateInstance(wrapperType, @object, asyncEnumerableInfo)!;
                return wrapper.ToFriendlyStringAsync().GetAwaiter().GetResult();
            }

            return @object.ToString() ?? string.Empty;
        }

        // This method has been replaced by the next one
        // static string ToFriendlyString2(this IEnumerable? enumerable)
        // {
        //     if (enumerable is null)
        //         return NullFriendlyString;
        //
        //     var builder = StringBuilderPool.Get();
        //     var enumerator = enumerable.GetEnumerator();
        //     try
        //     {
        //         _ = builder.Append('{').Append(' ');
        //         if (enumerator.MoveNext())
        //         {
        //             var current = enumerator.Current;
        //             _ = builder.Append(ToFriendlyString(current));
        //
        //             while (enumerator.MoveNext())
        //             {
        //                 current = enumerator.Current;
        //                 _ = builder.Append(ListSeparator).Append(' ').Append(ToFriendlyString(current));
        //             }
        //
        //             _ = builder.Append(' ');
        //         }
        //         _ = builder.Append('}');
        //         return builder.ToString();
        //     }
        //     finally
        //     {
        //         if(enumerator is IDisposable disposable)
        //             disposable.Dispose();
        //         
        //         StringBuilderPool.Return(builder);
        //     }
        // }

        static LazyConcurrentDictionary<Type, Func<object, string>> enumerableToFriendlyStringFuncs = new();

        // This method uses expression trees to generate custom code so that ref struct enumerators can be supported
        static string ToFriendlyString(this object enumerable, EnumerableInfo enumerableInfo)
        {
            var func = enumerableToFriendlyStringFuncs.GetOrAdd(enumerable.GetType(), _ => CompileFunc(enumerableInfo));
            return func(enumerable);

            static Func<object, string> CompileFunc(EnumerableInfo enumerableInfo)
            {
                var enumerableParameter = Parameter(typeof(object));
                return Lambda<Func<object, string>>(GetExpression(enumerableParameter, enumerableInfo), enumerableParameter)
                    .Compile();
            }

            static Expression GetExpression(ParameterExpression enumerable, EnumerableInfo enumerableInfo)
            {
                var stringBuilder = Variable(typeof(StringBuilder));
                var enumerator = Variable(enumerableInfo.GetEnumerator.ReturnType);
                var current = Variable(enumerableInfo.EnumeratorInfo.Current.PropertyType);
                var result = Variable(typeof(string));

                var appendCharInfo = typeof(StringBuilder).GetMethod("Append", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(char) }, null)!;
                var appendStringInfo = typeof(StringBuilder).GetMethod("Append", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string) }, null)!;
                var toFriendlyStringInfo = typeof(ObjectExtensions).GetMethod("ToFriendlyString", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(object) }, null)!;

                return Block(
                    new[] { stringBuilder, enumerator, result },
                    Assign(stringBuilder, Call(typeof(StringBuilderPool).GetMethod("Get", BindingFlags.Public | BindingFlags.Static)!)),
                    Assign(enumerator, Call(Convert(enumerable, enumerableInfo.GetEnumerator.DeclaringType!), enumerableInfo.GetEnumerator)),
                    TryFinally(
                        Block(
                            Assign(stringBuilder, Call(stringBuilder, appendCharInfo, Constant('{'))),
                            Assign(stringBuilder, Call(stringBuilder, appendCharInfo, Constant(' '))),
                            IfThen(
                                Call(enumerator, enumerableInfo.EnumeratorInfo.MoveNext),
                                Block(
                                    new[] {current},
                                    Assign(current, Property(enumerator, enumerableInfo.EnumeratorInfo.Current)),
                                    Assign(stringBuilder, Call(stringBuilder, appendStringInfo, Call(toFriendlyStringInfo, Convert(current, typeof(object))))),
                                    While(
                                        Call(enumerator, enumerableInfo.EnumeratorInfo.MoveNext),
                                        Block(
                                            Assign(current, Property(enumerator, enumerableInfo.EnumeratorInfo.Current)),
                                            Assign(stringBuilder, Call(stringBuilder, appendStringInfo, Constant(ListSeparator))),
                                            Assign(stringBuilder, Call(stringBuilder, appendCharInfo, Constant(' '))),
                                            Assign(stringBuilder, Call(stringBuilder, appendStringInfo, Call(toFriendlyStringInfo, Convert(current, typeof(object)))))
                                        )
                                    ),
                                    Assign(stringBuilder, Call(stringBuilder, appendCharInfo, Constant(' ')))
                                )
                            ),
                            Assign(stringBuilder, Call(stringBuilder, appendCharInfo, Constant('}'))),
                            Assign(
                                result, 
                                Call(
                                    stringBuilder, 
                                    typeof(object).GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null)!
                                )
                            )
                        ),
                        Block(
                            Dispose(enumerator, enumerableInfo),
                            Call(
                                typeof(StringBuilderPool).GetMethod("Return", BindingFlags.Public | BindingFlags.Static, null, new[] {typeof(StringBuilder)}, null)!,
                                stringBuilder
                            )
                        )
                    ),
                    result
                );
            }
            
            static Expression Dispose(Expression expression, EnumerableInfo enumerableInfo)
            {
                var disposeInfo = enumerableInfo.EnumeratorInfo.Dispose;
                if (disposeInfo is null)
                    return Empty();

                if (enumerableInfo.EnumeratorInfo.IsByRefLike)
                    return Call(expression, disposeInfo);

                if (expression.Type.IsValueType)
                    return Call(Convert(expression, typeof(IDisposable)), disposeInfo);
            
                return IfThen(NotEqual(expression, Constant(null)),
                        Call(Convert(expression, typeof(IDisposable)), disposeInfo)
                    );
            }
        }

        public static async ValueTask<string> ToFriendlyStringAsync<T>(this IAsyncEnumerable<T>? enumerable)
        {
            if (enumerable is null)
                return NullFriendlyString;

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

        public static string ToFriendlyString<T>(this T[]? enumerable)
            => enumerable is null
                ? NullFriendlyString
                : ToFriendlyString((ReadOnlySpan<T>)enumerable);

        public static string ToFriendlyString<T>(this ArraySegment<T> enumerable)
            => ToFriendlyString((ReadOnlySpan<T>)enumerable);

        public static string ToFriendlyString<T>(this Memory<T> enumerable)
            => ToFriendlyString((ReadOnlySpan<T>)enumerable.Span);

        public static string ToFriendlyString<T>(this ReadOnlyMemory<T> enumerable)
            => ToFriendlyString(enumerable.Span);

        public static string ToFriendlyString<T>(this Span<T> enumerable)
            => ToFriendlyString((ReadOnlySpan<T>)enumerable);

        public static string ToFriendlyString<T>(this ReadOnlySpan<T> enumerable)
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

        public static string ToFriendlyString<T>(this IReadOnlyList<T>? enumerable)
        {
            if (enumerable is null)
                return NullFriendlyString;

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
    }
}