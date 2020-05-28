using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    //[DebuggerNonUserCode]
    static partial class EnumerableEqualityComparer
    {
        public static EqualityResult Compare(this IEnumerable actual, IEnumerable expected, out int index)
        {
            var actualEnumerator = actual.GetEnumerator();
            var expectedEnumerator = expected.GetEnumerator();
            try
            {
                checked
                {
                    for (index = 0; true; index++)
                    {
                        var isActualCompleted = !actualEnumerator.MoveNext();
                        var isExpectedCompleted = !expectedEnumerator.MoveNext();

                        if (isActualCompleted && isExpectedCompleted)
                            return EqualityResult.Equal;

                        if (isActualCompleted)
                            return EqualityResult.LessItem;

                        if (isExpectedCompleted)
                            return EqualityResult.MoreItems;

                        if (!actualEnumerator.Current.Equals(expectedEnumerator.Current))
                            return EqualityResult.NotEqualAtIndex;
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

        public static EqualityResult Compare<TActualItem, TExpectedItem>(this IEnumerable<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> comparer, out int index)
        {
            using var actualEnumerator = actual.GetEnumerator();
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (index = 0; true; index++)
                {
                    var isActualCompleted = !actualEnumerator.MoveNext();
                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return EqualityResult.Equal;

                    if (isActualCompleted)
                        return EqualityResult.LessItem;

                    if (isExpectedCompleted)
                        return EqualityResult.MoreItems;

                    if (!comparer(actualEnumerator.Current, expectedEnumerator.Current))
                        return EqualityResult.NotEqualAtIndex;
                }
            }
        }

        public static async ValueTask<(EqualityResult, int)> Compare<TActualItem, TExpectedItem>(this IAsyncEnumerable<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> comparer)
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
                            return (EqualityResult.Equal, index);

                        if (isActualCompleted)
                            return (EqualityResult.LessItem, index);

                        if (isExpectedCompleted)
                            return (EqualityResult.MoreItems, index);

                        if (!comparer(actualEnumerator.Current, expectedEnumerator.Current))
                            return (EqualityResult.NotEqualAtIndex, index);
                    }
                }
            }
        }

        public static EqualityResult CompareCopyTo<TActualItem, TExpected, TExpectedItem>(this ICollection<TActualItem> actual, int offset, TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer, out int index)
            where TExpected : IEnumerable<TExpectedItem>
        {
            var array = new TActualItem[offset + actual.Count];

            try
            {
                actual.CopyTo(array, offset);
            }
            catch (NotSupportedException) // don't test if not supported
            {
                index = default;
                return EqualityResult.Equal; 
            }
            catch
            {
                throw new EqualToAssertionException<ICollection<TActualItem>, TExpected>(actual, expected, "Unhandled exception during call to CopyTo.");
            }

#if NETCORE
            return Compare(array.AsSpan(offset, actual.Count), expected, comparer, out index);
#else
            return Compare(new ArraySegment<TActualItem>(array, offset, actual.Count), expected, comparer, out index);
#endif
        }

        public static EqualityResult Compare<TActualItem, TExpectedItem>(this IReadOnlyList<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> comparer, out int index)
        {
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (index = 0; true; index++)
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
                        return EqualityResult.Equal;

                    if (isActualCompleted)
                        return EqualityResult.LessItem;

                    if (isExpectedCompleted)
                        return EqualityResult.MoreItems;

                    if (!comparer(actualItem!, expectedEnumerator.Current))
                        return EqualityResult.NotEqualAtIndex;
                }
            }
        }

#if NETCORE
        public static EqualityResult Compare<TActualItem, TExpectedItem>(this ReadOnlySpan<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> comparer, out int index)
        {
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (index = 0; true; index++)
                {
                    var isActualCompleted = false;
                    isActualCompleted = index == actual.Length;

                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return EqualityResult.Equal;

                    if (isActualCompleted)
                        return EqualityResult.LessItem;

                    if (isExpectedCompleted)
                        return EqualityResult.MoreItems;

                    TActualItem item;
                    try
                    {
                        item = actual[index];
                    }
                    catch
                    {
                        return EqualityResult.NotEqualAtIndex;
                    }

                    if (!comparer(item, expectedEnumerator.Current))
                        return EqualityResult.NotEqualAtIndex;
                }
            }
        }
#else
        public static EqualityResult Compare<TActualItem, TExpectedItem>(this ArraySegment<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> comparer, out int index)
        {
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (index = 0; true; index++)
                {
                    var isActualCompleted = false;
                    isActualCompleted = index == actual.Count;

                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return EqualityResult.Equal;

                    if (isActualCompleted)
                        return EqualityResult.LessItem;

                    if (isExpectedCompleted)
                        return EqualityResult.MoreItems;

                    TActualItem item;
                    try
                    {
                        item = actual.Array[index];
                    }
                    catch
                    {
                        return EqualityResult.NotEqualAtIndex;
                    }

                    if (!comparer(item, expectedEnumerator.Current))
                        return EqualityResult.NotEqualAtIndex;
                }
            }
        }
#endif

    }
}