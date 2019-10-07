using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ValueTypeAssertions<TActual> 
        where TActual : struct
    {
        internal ValueTypeAssertions(TActual actual) 
        {
            Actual = actual;
        }

        public TActual Actual { get; }

        public ValueTypeAssertions<TActual> BeEqualTo(TActual expected)
            => BeEqualTo(expected, (actual, expected) => actual.Equals(expected));

        public ValueTypeAssertions<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
        {
            if (!comparer(Actual, expected))
                throw new EqualToAssertionException<TActual, TExpected>(Actual, expected);
                
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