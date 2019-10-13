using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    static class AsyncEqualityComparer
    {
        public static void AssertEquality<TActual, TActualItem, TExpected, TExpectedItem>(TActual actual, EnumerableInfo enumerableInfo, TExpected expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
            where TExpected : IEnumerable<TExpectedItem>
        {
            var getEnumeratorDeclaringType = enumerableInfo.GetEnumerator.DeclaringType;
            if (!getEnumeratorDeclaringType.IsInterface)
            {
                var actualItemType = enumerableInfo.Current.PropertyType;
                var wrapped = new AsyncEnumerableWrapper<TActualItem>(actual, enumerableInfo);

                (var result, var index) = wrapped.CompareAsync(expected, equalityComparison).GetAwaiter().GetResult(); ;
                switch (result)
                {
                    case EqualityResult.NotEqualAtIndex:
                        {
                            throw new EqualToAssertionException<TActual, TExpected>(
                                actual,
                                expected,
                                $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' that differs at index {index} when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                        }

                    case EqualityResult.LessItem:
                        {
                            throw new EqualToAssertionException<TActual, TExpected>(
                                actual,
                                expected,
                                $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with less items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                        }

                    case EqualityResult.MoreItems:
                        {
                            throw new EqualToAssertionException<TActual, TExpected>(
                                actual,
                                expected,
                                $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with more items when using '{getEnumeratorDeclaringType}.GetEnumerator()'.");
                        }
                }
            }

            foreach (var @interface in typeof(TActual).GetInterfaces())
            {
                if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                {
                    var interfaceItemType = interfaceEnumerableInfo.Current.PropertyType;
                    var wrapped = new AsyncEnumerableWrapper<TActualItem>(actual, interfaceEnumerableInfo);

                    (var result, var index) = wrapped.CompareAsync(expected, equalityComparison).GetAwaiter().GetResult();
                    switch (result)
                    {
                        case EqualityResult.NotEqualAtIndex:
                            {
                                throw new EqualToAssertionException<TActual, TExpected>(
                                    actual,
                                    expected,
                                    $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' that differs at index {index} when using '{@interface}.GetEnumerator()'.");
                            }

                        case EqualityResult.LessItem:
                            {
                                throw new EqualToAssertionException<TActual, TExpected>(
                                    actual,
                                    expected,
                                    $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with less items when using '{@interface}.GetEnumerator()'.");
                            }

                        case EqualityResult.MoreItems:
                            {
                                throw new EqualToAssertionException<TActual, TExpected>(
                                    actual,
                                    expected,
                                    $"Expected '{expected.ToFriendlyString()}' but found '{wrapped.ToFriendlyString()}' with more items when using '{@interface}.GetEnumerator()'.");
                            }
                    }
                }
            }
        }

        static async Task<(EqualityResult Result, int Index)> CompareAsync<TActualItem, TExpectedItem>(this IAsyncEnumerable<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
            await using var actualEnumerator = actual.GetAsyncEnumerator();
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = ! await actualEnumerator.MoveNextAsync();
                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return (EqualityResult.Equal, index);

                    if (isActualCompleted)
                        return (EqualityResult.LessItem, index);

                    if (isExpectedCompleted)
                        return (EqualityResult.MoreItems, index);

                    if (!equalityComparison(actualEnumerator.Current, expectedEnumerator.Current))
                        return (EqualityResult.NotEqualAtIndex, index);
                }
            }
        }
    }
}