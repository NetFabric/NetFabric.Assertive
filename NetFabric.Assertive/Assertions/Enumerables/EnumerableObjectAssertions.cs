using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableObjectAssertions<TActual, TActualItem> 
    {
        readonly TActual actual;
        readonly EnumerableInfo enumerableInfo;

        internal EnumerableObjectAssertions(TActual actual, EnumerableInfo enumerableInfo) 
        {
            this.actual = actual;
            this.enumerableInfo = enumerableInfo;
        }

        public EnumerableObjectAssertions<TActual, TActualItem> Equal<TExpectedItem>(IEnumerable<TExpectedItem> expected)
            => Equal(expected, (actual, expected) => actual.Equals(expected));

        public EnumerableObjectAssertions<TActual, TActualItem> Equal<TExpectedItem>(IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
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

                if (enumerableInfo.GetEnumerator is null)
                    throw new AssertionException($"Expected {typeof(TActual)} to be an enumerable but it's missing a valid 'GetEnumerator' method.");
                if (enumerableInfo.Current is null)
                    throw new AssertionException($"Expected {enumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'Current' property.");
                if (enumerableInfo.MoveNext is null)
                    throw new AssertionException($"Expected {enumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'MoveNext' method.");

#if !NETSTANDARD2_1
                // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
                if (enumerableInfo.Current.PropertyType.IsByRef)
                    return this; // what should we do here?????
#endif

                if (!enumerableInfo.GetEnumerator.DeclaringType.IsInterface)
                    EqualityComparison(new EnumerableWrapper<TActualItem>(actual, enumerableInfo), expected, equalityComparison);

                foreach (var @interface in actual.GetType().GetInterfaces())
                {
                    if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                        EqualityComparison(new EnumerableWrapper<TActualItem>(actual, interfaceEnumerableInfo), expected, equalityComparison);
                }
            }

            return this;
        }

        protected virtual void EqualityComparison<TExpectedItem>(EnumerableWrapper<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
            using var actualEnumerator = actual.GetEnumerator();
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = !actualEnumerator.MoveNext();
                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return;

                    if (isActualCompleted ^ isExpectedCompleted)
                    {
                        if (isActualCompleted)
                            throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                                actual, 
                                expected,
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has less items when using {actual.DeclaringType}.GetEnumerator().");

                        if (isExpectedCompleted)
                            throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                                actual, 
                                expected,
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has more items when using {actual.DeclaringType}.GetEnumerator().");
                    }

                    if (!equalityComparison(actualEnumerator.Current, expectedEnumerator.Current))
                        throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                            actual, 
                            expected,
                            $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but if differs at index {index} when using {actual.DeclaringType}.GetEnumerator().");
                }
            }
        }
    }
}