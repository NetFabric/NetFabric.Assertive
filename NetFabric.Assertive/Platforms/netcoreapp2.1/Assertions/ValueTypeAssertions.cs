using System;

namespace NetFabric.Assertive
{
    public partial class ValueTypeAssertions<TActual> 
    {
        public AsyncEnumerableValueTypeAssertions<TActual, TActualItem> BeAsyncEnumerable<TActualItem>()
        {
            TypeExtensions.AssertIsAsyncEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new AsyncEnumerableValueTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }
    }
}