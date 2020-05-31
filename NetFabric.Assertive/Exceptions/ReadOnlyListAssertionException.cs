using System;

namespace NetFabric.Assertive
{
    public class ReadOnlyListAssertionException<TActualItem, TExpected>
        : EqualToAssertionException<ReadOnlyListWrapper<TActualItem>, TExpected>
    {
        public ReadOnlyListAssertionException(ReadOnlyListWrapper<TActualItem> actual, TExpected expected)
            : this(actual, expected, $"Expected collections to have same items.")
        {
        }

        public ReadOnlyListAssertionException(ReadOnlyListWrapper<TActualItem> actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}