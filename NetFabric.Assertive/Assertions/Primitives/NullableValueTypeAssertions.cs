using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class NullableValueTypeAssertions<TActual>
        : AssertionsBase
        where TActual : struct
    {
        internal NullableValueTypeAssertions(TActual? actual) 
            => Actual = actual;

        public TActual? Actual { get; }

        public NullableValueTypeAssertions<TActual> EvaluateTrue(Func<TActual?, bool> func) 
            => func(Actual) 
                ? this 
                : throw new ActualAssertionException<TActual?>(Actual, $"Evaluates to 'false'.");

        public NullableValueTypeAssertions<TActual> EvaluateFalse(Func<TActual?, bool> func) 
            => func(Actual) 
                ? throw new ActualAssertionException<TActual?>(Actual, $"Evaluates to 'true'.") 
                : this;

        public NullableValueTypeAssertions<TActual> BeEqualTo(TActual? expected) 
            => EqualityComparer<TActual?>.Default.Equals(Actual, expected)
                ? this
                : throw new EqualToAssertionException<TActual?, TActual?>(Actual, expected);

        public NullableValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected? expected, Func<TActual?, TExpected?, bool> comparer) 
            => comparer(Actual, expected) 
                ? this
                : throw new EqualToAssertionException<TActual?, TExpected>(Actual, expected);

        public NullableValueTypeAssertions<TActual> BeNotEqualTo(TActual? expected) 
            => EqualityComparer<TActual?>.Default.Equals(Actual, expected)
                ? throw new NotEqualToAssertionException<TActual?, TActual?>(Actual, expected)
                : this;

        public NullableValueTypeAssertions<TActual> BeNotEqualTo<TExpected>(TExpected? expected, Func<TActual?, TExpected?, bool> comparer) 
            => comparer(Actual, expected) 
                ? throw new NotEqualToAssertionException<TActual?, TExpected>(Actual, expected) 
                : this;

        public NullableValueTypeAssertions<TActual> HaveValue() 
            => Actual.HasValue 
                ? this 
                : throw new NotEqualToAssertionException<TActual?, TActual?>(null, Actual);

        public NullableValueTypeAssertions<TActual> NotHaveValue() 
            => Actual.HasValue 
                ? throw new EqualToAssertionException<TActual?, TActual?>(Actual, null) 
                : this;
    }
}
