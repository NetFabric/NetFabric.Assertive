using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class AsyncEnumerableExtensions
    {
        static readonly string Separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        public static async Task<string> ToFriendlyStringAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            var builder = StringBuilderPool.Get();
            try
            {
                _ = builder.Append('{');
                var enumerator = enumerable.GetAsyncEnumerator();
                await using (enumerator.ConfigureAwait(false))
                {
                    var first = true;
                    while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                    {
                        if (!first)
                        {
                            _ = builder.Append(Separator).Append(' ');
                        }
                        _ = builder.Append(ObjectExtensions.ToFriendlyString(enumerator.Current));
                        first = false;
                    }
                }
                _ = builder.Append('}');
                return builder.ToString();
            }
            finally
            {
                StringBuilderPool.Return(builder);
            }
        }
    }
}