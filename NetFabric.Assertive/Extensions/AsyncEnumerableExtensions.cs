using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class AsyncEnumerableExtensions
    {
        public static async Task<string> ToFriendlyStringAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            var builder = new StringBuilder();
            builder.Append('{');
            var enumerator = enumerable.GetAsyncEnumerator();
            var first = true;
            var separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            while(await enumerator.MoveNextAsync())
            {   
                if (!first)
                {
                    builder.Append(separator);
                    builder.Append(' ');
                }
                builder.Append(ObjectExtensions.ToFriendlyString(enumerator.Current));
                first = false;
            }
            builder.Append('}');
            return builder.ToString();
        }
    }
}