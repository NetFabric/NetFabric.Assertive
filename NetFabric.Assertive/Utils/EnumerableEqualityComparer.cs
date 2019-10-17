using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
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

        public static EqualityResult Compare<TActualItem, TExpectedItem>(this IEnumerable<TActualItem> actual, IEnumerable<TExpectedItem> expected, out int index)
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

                    if (!actualEnumerator.Current.Equals(expectedEnumerator.Current))
                        return EqualityResult.NotEqualAtIndex;
                }
            }
        }

        public static EqualityResult Compare<TActualItem, TExpectedItem>(this IReadOnlyList<TActualItem> actual, IEnumerable<TExpectedItem> expected, out int index)
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

                    if (!actualItem.Equals(expectedEnumerator.Current))
                        return EqualityResult.NotEqualAtIndex;
                }
            }
        }

        public static EqualityResult Compare<TActualItem, TExpectedItem>(this TActualItem[] actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> comparer, out int index)
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

                    if (!comparer(actual[index], expectedEnumerator.Current))
                        return EqualityResult.NotEqualAtIndex;
                }
            }
        }
    }
}