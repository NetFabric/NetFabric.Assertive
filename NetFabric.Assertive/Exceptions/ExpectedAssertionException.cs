using System;

namespace NetFabric.Assertive
{
    public class ExpectedAssertionException<TActual, TExpected>
        : ActualAssertionException<TActual>
    {
        public ExpectedAssertionException(TActual actual, TExpected expected, string message)
            : base(actual, message, (Exception)null)
        {
            Expected = expected;
        }

        protected ExpectedAssertionException(TActual actual, TExpected expected, string message, Exception innerException)
            : base(actual, message, innerException)
        {
            Expected = expected;
        }

        protected ExpectedAssertionException(TActual actual, TExpected expected, string message, string stackTrace)
            : base(actual, message, stackTrace)
        {
            Expected = expected;
        }

        public TExpected Expected { get; }
    }
}