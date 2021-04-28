namespace NetFabric.Assertive
{
    public class CopyToAssertionException<TActualItem, TExpected>
        : EqualToAssertionException<CopyToWrapper<TActualItem>, TExpected>
    {
        public CopyToAssertionException(CopyToWrapper<TActualItem> actual, TExpected expected)
            : this(actual, expected, $"Expected collections to have same items.")
        {
        }

        public CopyToAssertionException(CopyToWrapper<TActualItem> actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}