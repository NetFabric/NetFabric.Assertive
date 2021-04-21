using System;

namespace NetFabric.Assertive
{
    public class NonNullableActualAssertionException<TActual>
        : AssertionException
        where TActual : notnull
    {
        public NonNullableActualAssertionException(TActual actual, string message)
            : base($"{message}{Environment.NewLine}Actual: {actual.ToFriendlyString()}") 
            => Actual = actual;

        public TActual Actual { get; }
    }
}