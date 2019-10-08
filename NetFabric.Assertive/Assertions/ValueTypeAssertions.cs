using System;
using System.Collections.Generic;
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
        {
            if (!EqualityComparer<TActual>.Default.Equals(Actual, expected))
                throw new EqualToAssertionException<TActual, TActual>(Actual, expected);
                
            return this;
        }

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