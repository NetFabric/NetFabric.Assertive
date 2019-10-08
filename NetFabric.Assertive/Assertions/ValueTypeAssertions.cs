using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ValueTypeAssertions<TActual> 
        : CommonAssertions<TActual>
        where TActual : struct
    {
        internal ValueTypeAssertions(TActual actual) 
            : base(actual)
        {
        }

        public ValueTypeAssertions<TActual> BeEqualTo(TActual expected)
            => BeEqualTo(expected, (actual, expected) => actual.Equals(expected));

        public ValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
        {
            if (!comparer(Actual, expected))
                throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);
                
            return this;
        }

        public EnumerableValueTypeAssertions<TActual, TActualItem> BeEnumerable<TActualItem>()
        {
            typeof(TActual).AssertIsEnumerable<TActualItem>(out var enumerableInfo);

            return new EnumerableValueTypeAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }
    }
}