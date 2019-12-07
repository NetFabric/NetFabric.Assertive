using System.Collections;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class EnumerableExtensions
    {
        public static int Count(this IEnumerable enumerable)
        {
            var count = 0;
            var enumerator = enumerable.GetEnumerator();
            checked
            {
                while(enumerator.MoveNext())
                    count++;
            }
            return count;
        }

        public static bool Any(this IEnumerable enumerable)
            => enumerable.GetEnumerator().MoveNext();
    }
}