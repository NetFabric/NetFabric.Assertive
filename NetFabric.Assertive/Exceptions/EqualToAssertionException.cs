namespace NetFabric.Assertive
{
    public class EqualToAssertionException<TActual, TExpected>
        : ExpectedAssertionException<TActual, TExpected>
    {
        public EqualToAssertionException(TActual actual, TExpected expected)
            : this(actual, expected, $"Expected to be equal but it's not.")
        {
        }

        public EqualToAssertionException(TActual actual, TExpected expected, string message)
            : base(actual, expected, message)
        {
        }
    }
}