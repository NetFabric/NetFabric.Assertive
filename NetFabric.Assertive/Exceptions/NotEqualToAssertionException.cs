using System;

namespace NetFabric.Assertive
{
    public class NotEqualToAssertionException<TActual, TNotExpected>
        : ActualAssertionException<TActual>
    {
        public NotEqualToAssertionException(TActual actual, TNotExpected notExpected)
            : this(actual, notExpected, $"Expected to be not equal but it is.")
        {
        }

        public NotEqualToAssertionException(TActual actual, TNotExpected notExpected, string message)
            : base(actual, 
                  $"{message}{Environment.NewLine}Not Expected: {ObjectExtensions.ToFriendlyString(notExpected)}")
        {
            NotExpected = notExpected;
        }

        public TNotExpected NotExpected { get; }
    }
}