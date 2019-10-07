using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReferenceTypeAssertions<TActual> 
        : BaseAssertions<TActual>
        where TActual : class
    {
        internal ReferenceTypeAssertions(TActual actual) 
            : base(actual)
        {
        }

        public ReferenceTypeAssertions<TActual> BeNull() 
        {
            if (Actual is object)
                throw new EqualToAssertionException<TActual, TActual>(Actual, null);
                
            return this;
        }

        public ReferenceTypeAssertions<TActual> BeNotNull() 
        {
            if (Actual is null)
                throw new NullException();

            return this;
        }

        public ReferenceTypeAssertions<TActual> BeEqualTo(TActual expected)
            => BeEqualTo(expected, (actual, expected) => actual.Equals(expected));

        public ReferenceTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
        {
            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);

                if (!comparer(Actual, expected))
                    throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);
            }
                
            return this;
        }
    }
}