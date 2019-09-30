using System;

namespace NetFabric.Assertive
{
    public class ActualAssertionException<TActual>
        : AssertionException
    {
        public ActualAssertionException(TActual actual, string message)
            : base(message, (Exception)null)
        {
            Actual = actual;
        }

        protected ActualAssertionException(TActual actual, string message, Exception innerException)
            : base(message, innerException)
        {
            Actual = actual;
        }

        protected ActualAssertionException(TActual actual, string message, string stackTrace)
            : base(message)
        {
            Actual = actual;
        }

        public TActual Actual { get; }
   }
}