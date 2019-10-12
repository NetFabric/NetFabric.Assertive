using System;

namespace NetFabric.Assertive
{
    public partial class ReferenceTypeAssertions<TActual> 
    {
        public AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem> BeAsyncEnumerable<TActualItem>()
        {
            TypeExtensions.AssertIsAsyncEnumerable<TActual, TActualItem>(Actual, out var enumerableInfo);

            return new AsyncEnumerableReferenceTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }
    }
}