using System;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    public class NotEqualToAssertionException<TActual, TNotExpected>
        : ActualAssertionException<TActual>
    {
        public NotEqualToAssertionException([AllowNull]TActual actual, [AllowNull]TNotExpected notExpected)
            : this(actual, notExpected, $"Expected to be not equal but it is.")
        {
        }

        public NotEqualToAssertionException([AllowNull]TActual actual, [AllowNull]TNotExpected notExpected, string message)
            : base(actual, 
                  $"{message}{Environment.NewLine}Not Expected: {ObjectExtensions.ToFriendlyString(notExpected)}")
        {
            NotExpected = notExpected;
        }

        [AllowNull]
        public TNotExpected NotExpected { get; }
    }
}