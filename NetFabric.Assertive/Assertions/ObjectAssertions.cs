using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class ObjectAssertionsExtensions
    {
        public static ObjectAssertions<TActual> Must<TActual>(this TActual actual) 
            => new ObjectAssertions<TActual>(actual); 
    }

    [DebuggerNonUserCode]
    public class ObjectAssertions<TActual> 
    {
        protected readonly TActual actual;

        internal ObjectAssertions(TActual actual) 
        {
            this.actual = actual;
        }

        public ObjectAssertions<TActual> BeNull() 
        {
            if (actual is object)
                throw new NullException<object>(actual);
                
            return this;
        }

        public ObjectAssertions<TActual> NotBeNull() 
        {
            if (actual is null)
                throw new NotNullException();

            return this;
        }

        public ObjectAssertions<TActual> Equal(TActual expected)
            => Equal(expected, (actual, expected) => actual.Equals(expected));

        public ObjectAssertions<TActual> Equal<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
        {
            if (actual is null)
            {
                if (expected is object)
                    throw new NotNullException();
            }
            else
            {
                if (expected is null)
                    throw new NullException<object>(actual);

                if (!comparer(actual, expected))
                    throw new ExpectedAssertionException<object, object>(actual, expected, $"Expected {actual.ToFriendlyString()} to be equal");
            }
                
            return this;
        }

        public EnumerableObjectAssertions<TActual, TActualItem> BeEnumerable<TActualItem>()
        {
            var actualType = typeof(TActual);
            if (!actualType.IsEnumerable(out var enumerableInfo))
            {
                if (enumerableInfo.GetEnumerator is null)
                    throw new AssertionException($"Expected {actualType} to be an enumerable but it's missing a valid 'GetEnumerator' method.");
                if (enumerableInfo.Current is null)
                    throw new AssertionException($"Expected {enumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'Current' property.");
                if (enumerableInfo.MoveNext is null)
                    throw new AssertionException($"Expected {enumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'MoveNext' method.");
            }

            var actualItemType = enumerableInfo.Current.PropertyType;
            if (!typeof(TActualItem).IsAssignableFrom(actualItemType))
                throw new AssertionException($"Expected {actualType} to be an enumerable of {typeof(TActualItem)} but found an enumerable of {actualItemType}.");

            return new EnumerableObjectAssertions<TActual, TActualItem>(actual, enumerableInfo);
        }
    }
}