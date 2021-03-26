using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
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

        public static (EqualityResult, int, TActualItem?, TExpectedItem?) Compare<TActualItem, TExpected, TExpectedItem>(this IEnumerable<TActualItem> actual, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
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
