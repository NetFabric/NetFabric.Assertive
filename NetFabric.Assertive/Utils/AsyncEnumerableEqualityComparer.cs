using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class AsyncEnumerableEqualityComparer
    {
        public static async Task<(EqualityResult Result, int Index)> CompareAsync<TActualItem, TExpectedItem>(this IAsyncEnumerable<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
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