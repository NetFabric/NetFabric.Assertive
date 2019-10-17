using System;

namespace NetFabric.Assertive
{
    public class CountAssertionException
        : EqualToAssertionException<int, int>
    {
        public CountAssertionException(int actual, int expected)
            : this(actual, expected, $"Expected collections to have same count value.")
        {
        }

        public CountAssertionException(int actual, int expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}