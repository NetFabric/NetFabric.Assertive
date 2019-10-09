using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;

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
        {
            var enumerator = enumerable.GetEnumerator();
            return enumerator.MoveNext();
        }

        public static string ToFriendlyString(this IEnumerable enumerable)
        {
            var builder = new StringBuilder();
            var enumerator = enumerable.GetEnumerator();
            var first = true;
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            while(enumerator.MoveNext())
            {   
                if (!first)
                {
                    builder.Append(separator);
                    builder.Append(' ');
                }
                builder.Append(enumerator.Current.ToString());
                first = false;
            }
            return builder.ToString();
        }
    }
}