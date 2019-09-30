using System;

namespace NetFabric.Assertive
{
    public class NullException<TActual> : ActualAssertionException<TActual>
    {
        public NullException(TActual actual)
            : base(actual, $"Expected to be <null> but found {actual.ToFriendlyString()}.")
        {
        }
    }
}