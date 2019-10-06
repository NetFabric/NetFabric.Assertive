using System;

namespace NetFabric.Assertive
{
    public class NotNullException<TActual> : ActualAssertionException<TActual>
    {
        public NotNullException(TActual actual)
            : base(actual, $"Expected to be <null> but found {actual.ToFriendlyString()}.")
        {
        }
    }
}