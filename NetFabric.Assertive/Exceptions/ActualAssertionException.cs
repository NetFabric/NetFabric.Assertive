using System;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    public class ActualAssertionException<TActual>
        : AssertionException
    {
        public ActualAssertionException([AllowNull]TActual actual, string message)
            : base($"{message}{Environment.NewLine}Actual: {ObjectExtensions.ToFriendlyString(actual)}")
        {
            Actual = actual;
        }

        [AllowNull]
        public TActual Actual { get; }
    }
}