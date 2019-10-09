using System;

namespace NetFabric.Assertive
{
    public class NullException<TActual> : ActualAssertionException<TActual>
        where TActual : class
    {
        public NullException()
            : base(null, $"Expected not '<null>' but found '<null>'.")
        {
        }
    }
}