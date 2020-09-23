using System;
using System.Diagnostics.CodeAnalysis;

namespace NetFabric.Assertive
{
    public class ActualAssertionException<TActual>
        : AssertionException
    {
        public ActualAssertionException(TActual? actual, string message)
            : base($"{message}{Environment.NewLine}Actual: {ObjectExtensions.ToFriendlyString(actual)}") 
            => Actual = actual;

        public TActual? Actual { get; }
    }
}