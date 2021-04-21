using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NetFabric.Reflection;
using static NetFabric.Expressions.ExpressionEx;
using static System.Linq.Expressions.Expression;

namespace NetFabric.Assertive
{
    //[DebuggerNonUserCode]
    static partial class EnumerableEqualityComparer
    {
        public static (EqualityResult, int, T?, T?) Compare<T, TExpected>(this IEnumerable<T> actual, TExpected expected)
            where TExpected : IEnumerable<T>
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

                        if (!EqualityComparer<T>.Default.Equals(actualEnumerator.Current, expectedEnumerator.Current))
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

        // This comparer uses expression trees to generate custom code so that ref struct enumerators can be supported
        public static (EqualityResult, int, TActualItem?, TExpectedItem?) Compare<TActual, TActualItem, TExpected, TExpectedItem>(this TActual actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TActual : notnull
            where TExpected : IEnumerable<TExpectedItem>
        {
            var compare = ComparerCache<TActual, TActualItem, TExpected, TExpectedItem>.Instance.Value;
            return compare(actual, expected, comparer);
        }

        // This caches the comparers for the previous method
        static class ComparerCache<TActual, TActualItem, TExpected, TExpectedItem>
            where TActual : notnull
            where TExpected : IEnumerable<TExpectedItem>
        {
            public static Lazy<Func<TActual, TExpected, Func<TActualItem, TExpectedItem, bool>,
                ValueTuple<EqualityResult, int, TActualItem, TExpectedItem>>> Instance = new (CompileFunc, LazyThreadSafetyMode.ExecutionAndPublication);
                
            static Func<TActual, TExpected, Func<TActualItem, TExpectedItem, bool>, ValueTuple<EqualityResult, int, TActualItem, TExpectedItem>> CompileFunc()
            {
                var actualEnumerableParameter = Parameter(typeof(TActual));
                var expectedEnumerableParameter = Parameter(typeof(TExpected));
                var comparerEnumerableParameter = Parameter(typeof(Func<TActualItem, TExpectedItem, bool>));
                return Lambda<Func<TActual, TExpected, Func<TActualItem, TExpectedItem, bool>, ValueTuple<EqualityResult, int, TActualItem, TExpectedItem>>>(
                        GetExpression(actualEnumerableParameter, expectedEnumerableParameter, comparerEnumerableParameter), 
                        actualEnumerableParameter, 
                        expectedEnumerableParameter, 
                        comparerEnumerableParameter)
                    .Compile();
            }

            static Expression GetExpression(ParameterExpression actualEnumerableParameter, ParameterExpression expectedEnumerableParameter, ParameterExpression comparerEnumerableParameter)
            {
                if (!typeof(TActual).IsEnumerable(out var actualEnumerableInfo))
                    throw new Exception($"{typeof(TActual).Name} is not enumerable!");
        
                if (!typeof(TExpected).IsEnumerable(out var expectedEnumerableInfo))
                    throw new Exception($"{typeof(TExpected).Name} is not enumerable!");

                var actualEnumeratorVariable = Variable(actualEnumerableInfo.GetEnumerator.ReturnType);
                var expectedEnumeratorVariable = Variable(expectedEnumerableInfo.GetEnumerator.ReturnType);
                var indexVariable = Variable(typeof(int));
                var isActualCompletedVariable = Variable(typeof(bool));
                var isExpectedCompletedVariable = Variable(typeof(bool));
                var actualCurrent = Variable(actualEnumerableInfo.EnumeratorInfo.Current.PropertyType);
                var expectedCurrent = Variable(expectedEnumerableInfo.EnumeratorInfo.Current.PropertyType);

                var resultType = typeof(ValueTuple<EqualityResult, int, TActualItem, TExpectedItem>);
                var resultConstructor = resultType.GetConstructor(new[] {typeof(EqualityResult), typeof(int), typeof(TActualItem), typeof(TExpectedItem)})!; 
                var returnTarget = Label(resultType);

                return Block(
                    new[] {actualEnumeratorVariable, expectedEnumeratorVariable, indexVariable},
                    Assign(actualEnumeratorVariable, InvokeGetEnumerator(actualEnumerableParameter, actualEnumerableInfo)),
                    Assign(expectedEnumeratorVariable, InvokeGetEnumerator(expectedEnumerableParameter, expectedEnumerableInfo)),
                    TryFinally(
                        For(
                            Assign(indexVariable, Constant(0)),
                            Constant(true),
                            PostIncrementAssign(indexVariable),
                            Block(
                                new[] {isActualCompletedVariable, isExpectedCompletedVariable, actualCurrent, expectedCurrent},
                                Assign(isActualCompletedVariable, Not(InvokeMoveNext(actualEnumeratorVariable, actualEnumerableInfo))),
                                Assign(isExpectedCompletedVariable, Not(InvokeMoveNext(expectedEnumeratorVariable, expectedEnumerableInfo))),
                                IfThen(
                                    And(isActualCompletedVariable, isExpectedCompletedVariable),
                                    Return(
                                        returnTarget, 
                                        New(
                                            resultConstructor, 
                                            Constant(EqualityResult.Equal), 
                                            indexVariable, 
                                            Constant(default(TActualItem)), 
                                            Constant(default(TExpectedItem))
                                        )
                                    )
                                ),
                                IfThen(
                                    isActualCompletedVariable,
                                    Return(
                                        returnTarget, 
                                        New(
                                            resultConstructor, 
                                            Constant(EqualityResult.LessItem), 
                                            indexVariable, 
                                            Constant(default(TActualItem)), 
                                            Constant(default(TExpectedItem))
                                        )
                                    )
                                ),
                                IfThen(
                                    isExpectedCompletedVariable,
                                    Return(
                                        returnTarget, 
                                        New(
                                            resultConstructor, 
                                            Constant(EqualityResult.MoreItems), 
                                            indexVariable, 
                                            Constant(default(TActualItem)), 
                                            Constant(default(TExpectedItem))
                                        )
                                    )
                                ),
                                Assign(actualCurrent, InvokeCurrent(actualEnumeratorVariable, actualEnumerableInfo)),
                                Assign(expectedCurrent, InvokeCurrent(expectedEnumeratorVariable, expectedEnumerableInfo)),
                                IfThen(
                                    Not(
                                        Invoke(
                                            comparerEnumerableParameter, 
                                            actualCurrent, 
                                            expectedCurrent
                                        )
                                    ),
                                    Return(returnTarget, 
                                        New(
                                            resultConstructor, 
                                            Constant(EqualityResult.NotEqualAtIndex), 
                                            indexVariable, 
                                            actualCurrent, 
                                            expectedCurrent
                                        )
                                    )
                                )
                            )
                        ),
                        Block(
                            InvokeDispose(actualEnumeratorVariable, actualEnumerableInfo),
                            InvokeDispose(expectedEnumeratorVariable, expectedEnumerableInfo)
                        )
                    ),
                    Label(returnTarget, Constant(default(ValueTuple<EqualityResult, int, TActualItem, TExpectedItem>)))
                );
                
                
                static Expression InvokeGetEnumerator(Expression expression, EnumerableInfo enumerableInfo)
                {
                    var getEnumeratorInfo = enumerableInfo.GetEnumerator;
                    var enumeratorType = getEnumeratorInfo.ReturnType;
                    var resultVariable = Variable(enumeratorType);
                    var exceptionVariable = Variable(typeof(Exception));
                    return Block(
                        new[] { resultVariable },
                        TryCatch(
                            Assign(resultVariable, Call(expression, getEnumeratorInfo)),
                            Catch(
                                exceptionVariable,
                                Throw(NewEnumerationException(getEnumeratorInfo, exceptionVariable), enumeratorType)
                            )
                        ),
                        resultVariable
                    );
                }

                static Expression InvokeCurrent(Expression expression, EnumerableInfo enumerableInfo)
                {
                    var currentInfo = enumerableInfo.EnumeratorInfo.Current;
                    var elementType = currentInfo.PropertyType;
                    var resultVariable = Variable(elementType);
                    var exceptionVariable = Variable(typeof(Exception));
                    return Block(
                        new[] { resultVariable },
                        TryCatch(
                            Assign(resultVariable, Property(expression, currentInfo)),
                            Catch(
                                exceptionVariable,
                                Throw(NewEnumerationException(currentInfo, exceptionVariable), elementType)
                            )
                        ),
                        resultVariable
                    );
                }

                static Expression InvokeMoveNext(Expression expression, EnumerableInfo enumerableInfo)
                {
                    var moveNextInfo = enumerableInfo.EnumeratorInfo.MoveNext;
                    var resultVariable = Variable(typeof(bool));
                    var exceptionVariable = Variable(typeof(Exception));
                    return Block(
                        new[] { resultVariable },
                        TryCatch(
                            Assign(resultVariable, Call(expression, moveNextInfo)),
                            Catch(
                                exceptionVariable,
                                Throw(NewEnumerationException(moveNextInfo, exceptionVariable), typeof(bool))
                            )
                        ),
                        resultVariable
                    );
                }

                static Expression InvokeDispose(Expression expression, EnumerableInfo enumerableInfo)
                {
                    var disposeInfo = enumerableInfo.EnumeratorInfo.Dispose;
                    if (disposeInfo is null)
                        return Empty();

                    var exceptionVariable = Variable(typeof(Exception));

                    if (enumerableInfo.EnumeratorInfo.IsByRefLike)
                        return TryCatch(
                            Call(expression, disposeInfo),
                            Catch(
                                exceptionVariable,
                                Throw(NewEnumerationException(disposeInfo, exceptionVariable))
                            )
                        );

                    if (expression.Type.IsValueType)
                        return TryCatch(
                            Call(Convert(expression, typeof(IDisposable)), disposeInfo),
                            Catch(
                                exceptionVariable,
                                Throw(NewEnumerationException(disposeInfo, exceptionVariable))
                            )
                        );
                    
                    return IfThen(NotEqual(expression, Constant(null)),
                        TryCatch(
                            Call(Convert(expression, typeof(IDisposable)), disposeInfo),
                            Catch(
                                exceptionVariable,
                                Throw(NewEnumerationException(disposeInfo, exceptionVariable))
                            )
                        )
                    );
                }

                static Expression NewEnumerationException(MemberInfo memberInfo, Expression innerException)
                {
                    var parentheses = memberInfo.MemberType == MemberTypes.Method
                        ? "()"
                        : string.Empty;
                    return New(
                        typeof(EnumerationException).GetConstructor(new[] {typeof(string), typeof(Exception)})!,
                        Constant($"Unhandled exception in {memberInfo.DeclaringType!.Name}.{memberInfo.Name}{parentheses}."),
                        innerException
                    );
                }

            }
        }

        public static async Task<(EqualityResult, int, TActualItem?, TExpectedItem?)> Compare<TActualItem, TExpected, TExpectedItem>(this IAsyncEnumerable<TActualItem> actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
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

        public static (EqualityResult, int, TActualItem?, TExpectedItem?) Compare<TActualItem, TExpected, TExpectedItem>(this IReadOnlyList<TActualItem> actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
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
