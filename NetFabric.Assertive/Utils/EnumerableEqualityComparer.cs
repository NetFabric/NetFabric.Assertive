using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using NetFabric.Reflection;
using static NetFabric.Expressions.ExpressionEx;
using static System.Linq.Expressions.Expression;

namespace NetFabric.Assertive
{
    //[DebuggerNonUserCode]
    static partial class EnumerableEqualityComparer
    {
        public static (EqualityResult, int, TActualItem?, TActualItem?) Compare<TActual, TActualItem, TExpected>(this TActual actual, TExpected expected)
            where TActual : IEnumerable<TActualItem>
            where TExpected : IEnumerable<TActualItem>
        {
            var actualEnumerator = actual.GetEnumerator();
            var expectedEnumerator = expected.GetEnumerator();
            try
            {
                checked
                {
                    for (var index = 0; true; index++)
                    {
                        var isActualCompleted = !actualEnumerator.MoveNext();
                        var isExpectedCompleted = !expectedEnumerator.MoveNext();

                        if (isActualCompleted && isExpectedCompleted)
                            return (EqualityResult.Equal, index, default, default);

                        if (isActualCompleted)
                            return (EqualityResult.LessItem, index, default, default);

                        if (isExpectedCompleted)
                            return (EqualityResult.MoreItems, index, default, default);

                        if (!EqualityComparer<TActualItem>.Default.Equals(actualEnumerator.Current, expectedEnumerator.Current))
                            return (EqualityResult.NotEqualAtIndex, index, actualEnumerator.Current, expectedEnumerator.Current);
                    }
                }
            }
            finally
            {
                if (actualEnumerator is IDisposable actualDisposable)
                    actualDisposable.Dispose();

                if (expectedEnumerator is IDisposable expectedDisposable)
                    expectedDisposable.Dispose();
            }
        } 

        public static (EqualityResult, int, TActualItem?, TExpectedItem?) Compare<TActual, TActualItem, TExpected, TExpectedItem>(this TActual actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            // where TActual : IEnumerable<TActualItem>
            // where TExpected : IEnumerable<TExpectedItem>
        {
            /*
            using var actualEnumerator = actual.GetEnumerator();
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = !actualEnumerator.MoveNext();
                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return (EqualityResult.Equal, index, default, default);

                    if (isActualCompleted)
                        return (EqualityResult.LessItem, index, default, default);

                    if (isExpectedCompleted)
                        return (EqualityResult.MoreItems, index, default, default);

                    if (!comparer(actualEnumerator.Current, expectedEnumerator.Current))
                        return (EqualityResult.NotEqualAtIndex, index, actualEnumerator.Current, expectedEnumerator.Current);
                }
            }
            */
            
            _ = typeof(TActual).IsEnumerable(out var actualEnumerableInfo);
            _ = typeof(TExpected).IsEnumerable(out var expectedEnumerableInfo);

            var actualParameter = Parameter(typeof(TActual), "actual");
            var expectedParameter = Parameter(typeof(TExpected), "expected");
            var comparerParameter = Parameter(typeof(Func<TActualItem, TExpectedItem, bool>), "comparer");

            Type actualEnumeratorType;
            Expression actualGetEnumerator;
            PropertyInfo actualCurrentInfo;
            MethodInfo actualMoveNextInfo;
            if (actualParameter.Type.IsArray)
            {
                var enumerableType = typeof(IEnumerable<TActualItem>);
                actualGetEnumerator = Call(Convert(actualParameter, enumerableType), enumerableType.GetPublicMethod(nameof(IEnumerable.GetEnumerator))!);
                actualEnumeratorType = typeof(IEnumerator<TActualItem>);
                actualCurrentInfo = actualEnumeratorType.GetPublicProperty(nameof(IEnumerator.Current))!;
                actualMoveNextInfo = actualEnumeratorType.GetPublicMethod(nameof(IEnumerator.MoveNext))!;
            }
            else
            {
                actualEnumeratorType = actualEnumerableInfo!.GetEnumerator.ReturnType;
                actualGetEnumerator = Call(actualParameter, actualEnumerableInfo!.GetEnumerator);
                actualCurrentInfo = actualEnumerableInfo.EnumeratorInfo.Current;
                actualMoveNextInfo = actualEnumerableInfo.EnumeratorInfo.MoveNext;
            }

            Type expectedEnumeratorType;
            Expression expectedGetEnumerator;
            PropertyInfo expectedCurrentInfo;
            MethodInfo expectedMoveNextInfo;
            if (expectedParameter.Type.IsArray)
            {
                var enumerableType = typeof(IEnumerable<TExpectedItem>);
                expectedGetEnumerator = Call(Convert(expectedParameter, enumerableType), enumerableType.GetPublicMethod(nameof(IEnumerable.GetEnumerator))!);
                expectedEnumeratorType = typeof(IEnumerator<TExpectedItem>);
                expectedCurrentInfo = expectedEnumeratorType.GetPublicProperty(nameof(IEnumerator.Current))!;
                expectedMoveNextInfo = expectedEnumeratorType.GetPublicMethod(nameof(IEnumerator.MoveNext))!;
            }
            else
            {
                expectedEnumeratorType = expectedEnumerableInfo!.GetEnumerator.ReturnType;
                expectedGetEnumerator = Call(expectedParameter, expectedEnumerableInfo!.GetEnumerator);
                expectedCurrentInfo = expectedEnumerableInfo.EnumeratorInfo.Current;
                expectedMoveNextInfo = expectedEnumerableInfo.EnumeratorInfo.MoveNext;
            }
            
            var actualEnumerator = Variable(actualEnumeratorType, "actualEnumerator");
            var expectedEnumerator = Variable(expectedEnumeratorType, "expectedEnumerator");
            var index = Variable(typeof(int), "index");
            var isActualCompleted = Variable(typeof(bool), "isActualCompleted");
            var isExpectedCompleted = Variable(typeof(bool), "isExpectedCompleted");
            
            var resultType = typeof(ValueTuple<EqualityResult, int, TActualItem?, TExpectedItem?>);
            var result = Variable(resultType, "result");
            var resultConstructor = resultType.GetConstructor(new[] {typeof(EqualityResult), typeof(int), typeof(TActualItem), typeof(TExpectedItem)})!;

            var returnTarget = Label();
            var expression = Block(
                new[] { actualEnumerator, expectedEnumerator, index, result },
                // arrays have to be casted so that reflection can find the generics version of GetEnumerator
                Assign(actualEnumerator, actualGetEnumerator),
                Assign(expectedEnumerator, expectedGetEnumerator),
                For(Assign(index, Constant(0)), Constant(true), PostIncrementAssign(index),
                    Block(
                        new[] { isActualCompleted, isExpectedCompleted },
                        Assign(isActualCompleted, Not(Call(actualEnumerator, actualMoveNextInfo))),
                        Assign(isExpectedCompleted, Not(Call(expectedEnumerator, expectedMoveNextInfo))),
                        IfThen(And(isActualCompleted, isExpectedCompleted),
                            Block(
                                Assign(result, New(resultConstructor, Constant(EqualityResult.Equal), index, Default(typeof(TActualItem)), Default(typeof(TExpectedItem)))),
                                Return(returnTarget)    
                            )    
                        ),
                        IfThen(isActualCompleted,
                            Block(
                                Assign(result, New(resultConstructor, Constant(EqualityResult.LessItem), index, Default(typeof(TActualItem)), Default(typeof(TExpectedItem)))),
                                Return(returnTarget)    
                            )    
                        ),
                        IfThen(isExpectedCompleted,
                            Block(
                                Assign(result, New(resultConstructor, Constant(EqualityResult.MoreItems), index, Default(typeof(TActualItem)), Default(typeof(TExpectedItem)))),
                                Return(returnTarget)    
                            )    
                        ),
                        IfThen(Not(Invoke(Constant(comparer), Property(actualEnumerator, actualCurrentInfo), Property(expectedEnumerator, expectedCurrentInfo))),
                            Block(
                                Assign(result, New(resultConstructor, Constant(EqualityResult.NotEqualAtIndex), index, Property(actualEnumerator, actualCurrentInfo), Property(expectedEnumerator, expectedCurrentInfo))),
                                Return(returnTarget)    
                            )    
                        )
                    )
                ),
                Label(returnTarget),
                result
            );
            var func = Lambda<Func<TActual, TExpected, Func<TActualItem, TExpectedItem, bool>, ValueTuple<EqualityResult, int, TActualItem?, TExpectedItem?>>>(
                    expression, 
                    actualParameter, expectedParameter, comparerParameter)
                .Compile();

            return func(actual, expected, comparer);
        }

        public static async Task<(EqualityResult, int, TActualItem?, TExpectedItem?)> CompareAsync<TActual, TActualItem, TExpected, TExpectedItem>(this TActual actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TActual : IAsyncEnumerable<TActualItem>
            where TExpected : IEnumerable<TExpectedItem>
        {
            var actualEnumerator = actual.GetAsyncEnumerator();
            await using (actualEnumerator.ConfigureAwait(false))
            {
                using var expectedEnumerator = expected.GetEnumerator();
                checked
                {
                    for (var index = 0; true; index++)
                    {
                        var isActualCompleted = !await actualEnumerator.MoveNextAsync().ConfigureAwait(false);
                        var isExpectedCompleted = !expectedEnumerator.MoveNext();

                        if (isActualCompleted && isExpectedCompleted)
                            return (EqualityResult.Equal, index, default, default);

                        if (isActualCompleted)
                            return (EqualityResult.LessItem, index, default, default);

                        if (isExpectedCompleted)
                            return (EqualityResult.MoreItems, index, default, default);

                        if (!comparer(actualEnumerator.Current, expectedEnumerator.Current))
                            return (EqualityResult.NotEqualAtIndex, index, actualEnumerator.Current, expectedEnumerator.Current);
                    }
                }
            }
        }

        public static (EqualityResult, int, TActualItem?, TExpectedItem?) Compare<TActual, TActualItem, TExpected, TExpectedItem>(this IReadOnlyList<TActualItem> actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            // where TActual : IReadOnlyList<TActualItem>
            where TExpected : IEnumerable<TExpectedItem>
        {
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = false;
                    var actualItem = default(TActualItem);
                    try
                    {
                        actualItem = actual[index];
                    }
                    catch
                    {
                        isActualCompleted = true;
                    }

                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return (EqualityResult.Equal, index, default, default);

                    if (isActualCompleted)
                        return (EqualityResult.LessItem, index, default, default);

                    if (isExpectedCompleted)
                        return (EqualityResult.MoreItems, index, default, default);

                    if (!comparer(actualItem!, expectedEnumerator.Current))
                        return (EqualityResult.NotEqualAtIndex, index, actualItem, expectedEnumerator.Current);
                }
            }
        }

        public static (EqualityResult, int, TActualItem?, TExpectedItem?) Compare<TActualItem, TExpected, TExpectedItem>(this Span<TActualItem> actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
            => Compare((ReadOnlySpan<TActualItem>)actual, expected, comparer);

        public static (EqualityResult, int, TActualItem?, TExpectedItem?) Compare<TActualItem, TExpected, TExpectedItem>(this ReadOnlySpan<TActualItem> actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = false;
                    isActualCompleted = index == actual.Length;

                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return (EqualityResult.Equal, index, default, default);

                    if (isActualCompleted)
                        return (EqualityResult.LessItem, index, default, default);

                    if (isExpectedCompleted)
                        return (EqualityResult.MoreItems, index, default, default);

                    TActualItem item;
                    try
                    {
                        item = actual[index];
                    }
                    catch
                    {
                        return (EqualityResult.LessItem, index, default, default);
                    }

                    if (!comparer(item, expectedEnumerator.Current))
                        return (EqualityResult.NotEqualAtIndex, index, item, expectedEnumerator.Current);
                }
            }
        }

        public static (bool, int) Compare(this string actual, string expected, bool ignoreCase)
        {
            var end = Math.Min(actual.Length, expected.Length);
            if (ignoreCase)
            {
                for (var index = 0; index < end; index++)
                { 
                    if (char.ToLower(actual[index]) != char.ToLower(expected[index]))
                        return (false, index);
                }
            }
            else
            {
                for (var index = 0; index < end; index++)
                {
                    if (actual[index] != expected[index])
                        return (false, index);
                }
            }

            if (actual.Length < expected.Length)
                return (false, actual.Length);

            if (actual.Length > expected.Length)
                return (false, expected.Length);

            return (true, actual.Length);
        }
    }
}
