using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReferenceTypeAssertions<TActual> 
        where TActual : class
    {
        internal ReferenceTypeAssertions(TActual actual) 
        {
            Actual = actual;
        }

        public TActual Actual { get; }

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

        public EnumerableObjectAssertions<TActual, TActualItem> BeEnumerable<TActualItem>()
        {
            var actualType = typeof(TActual);
            var enumerableInfo = actualType.GetEnumerableInfo();

            if (enumerableInfo.GetEnumerator is null)
                throw new AssertionException($"Expected {actualType} to be an enumerable but it's missing a valid 'GetEnumerator' method.");
            if (enumerableInfo.Current is null)
                throw new AssertionException($"Expected {enumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'Current' property.");
            if (enumerableInfo.MoveNext is null)
                throw new AssertionException($"Expected {enumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'MoveNext' method.");

            var actualItemType = enumerableInfo.Current.PropertyType;
            if (!typeof(TActualItem).IsAssignableFrom(actualItemType))
                throw new AssertionException($"Expected {actualType} to be an enumerable of {typeof(TActualItem)} but found an enumerable of {actualItemType}.");

            return new EnumerableObjectAssertions<TActual, TActualItem>(Actual, enumerableInfo);
        }
    }
}