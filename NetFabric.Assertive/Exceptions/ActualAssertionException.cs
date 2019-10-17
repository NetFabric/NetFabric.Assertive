using System;

namespace NetFabric.Assertive
{
    public class ActualAssertionException<TActual>
        : AssertionException
    {
        public ActualAssertionException(TActual actual, string message)
            : base($"{message}{Environment.NewLine}Actual: {actual.ToFriendlyString()}")
        {
            Actual = actual;
        }

        public TActual Actual { get; }
    }
}