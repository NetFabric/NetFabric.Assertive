using System;

namespace NetFabric.Assertive
{
    public class NotEqualToAssertionException<TActual, TNotExpected>
        : ActualAssertionException<TActual>
    {
        public NotEqualToAssertionException(TActual actual, TNotExpected notExpected)
            : this(actual, notExpected, $"Expected '{notExpected.ToFriendlyString()}' to be not equivalent to '{actual.ToFriendlyString()}' but it is.")
        {
        }

        public NotEqualToAssertionException(TActual actual, TNotExpected notExpected, string message)
            : base(actual, message)
        {
            NotExpected = notExpected;
        }

        public TNotExpected NotExpected { get; }
    }
}