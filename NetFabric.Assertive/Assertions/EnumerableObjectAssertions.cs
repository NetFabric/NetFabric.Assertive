using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableObjectAssertions<TActual, TActualItem> 
    {
        internal EnumerableObjectAssertions(TActual actual, EnumerableInfo enumerableInfo) 
        {
            Actual = actual;
            EnumerableInfo = enumerableInfo;
        }

        public TActual Actual { get; }
        public EnumerableInfo EnumerableInfo { get; }

        public EnumerableObjectAssertions<TActual, TActualItem> BeEmpty()
            => BeEqualTo(Enumerable.Empty<TActualItem>());

        public EnumerableObjectAssertions<TActual, TActualItem> BeEqualTo(IEnumerable<TActualItem> expected)
            => BeEqualTo<TActualItem>(expected);

        public EnumerableObjectAssertions<TActual, TActualItem> BeEqualTo<TExpectedItem>(IEnumerable<TExpectedItem> expected)
            => BeEqualTo(expected, (actual, expected) => actual.Equals(expected));

        public EnumerableObjectAssertions<TActual, TActualItem> BeEqualTo<TExpectedItem>(IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
            if (Actual is null)
            {
                if (expected is object)
                    throw new NotNullException<object>(Actual);
            }
            else
            {
                if (expected is null)
                    throw new NullException();

                if (EnumerableInfo.GetEnumerator is null)
                    throw new AssertionException($"Expected {typeof(TActual)} to be an enumerable but it's missing a valid 'GetEnumerator' method.");
                if (EnumerableInfo.Current is null)
                    throw new AssertionException($"Expected {EnumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'Current' property.");
                if (EnumerableInfo.MoveNext is null)
                    throw new AssertionException($"Expected {EnumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'MoveNext' method.");

#if !NETSTANDARD2_1
                // 'Current' may return by-ref but reflection only supports its invocation on netstandard 2.1
                if (EnumerableInfo.Current.PropertyType.IsByRef)
                    return this; // what should we do here?????
#endif

                if (!EnumerableInfo.GetEnumerator.DeclaringType.IsInterface)
                {
                    var actualItemType = EnumerableInfo.Current.PropertyType;
                    if (!typeof(TActualItem).IsAssignableFrom(actualItemType))
                        throw new AssertionException($"Expected {typeof(TActual)} to be an enumerable of {typeof(TActualItem)} but found an enumerable of {actualItemType}.");

                    EqualityComparison(new EnumerableWrapper<TActualItem>(Actual, EnumerableInfo), expected, equalityComparison, EnumerableInfo.GetEnumerator.DeclaringType);
                }

                foreach (var @interface in Actual.GetType().GetInterfaces())
                {
                    if (@interface.IsEnumerable(out var interfaceEnumerableInfo))
                    {
                        var interfaceItemType = interfaceEnumerableInfo.Current.PropertyType;
                        if (!typeof(TActualItem).IsAssignableFrom(interfaceItemType))
                            throw new AssertionException($"Expected {typeof(TActual)} to be an enumerable of {typeof(TActualItem)} but found an enumerable of {interfaceItemType}.");

                        var wrapped = new EnumerableWrapper<TActualItem>(Actual, interfaceEnumerableInfo);
                        var readOnlyCollectionType = typeof(IReadOnlyCollection<>).MakeGenericType(interfaceItemType);
                        var readOnlyListType = typeof(IReadOnlyList<>).MakeGenericType(interfaceItemType);

                        if (@interface == readOnlyCollectionType)
                        {
                            var actualCount = ((IReadOnlyCollection<TActualItem>)Actual).Count;
                            var expectedCount = wrapped.Count();
                            if (actualCount != expectedCount)
                                throw new AssertionException($"Expected {Actual.ToFriendlyString()} to have count value of {expectedCount} but found {actualCount}.");
                        } 
                        else if (@interface == readOnlyListType)
                        {
                            EqualityComparison((IReadOnlyList<TActualItem>)Actual, expected, equalityComparison);
                        }
                        else
                        {
                            EqualityComparison(wrapped, expected, equalityComparison, @interface);
                        }
                    }
                }
            }

            return this;
        }

        protected virtual void EqualityComparison<TExpectedItem>(IEnumerable<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison, Type type)
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
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has less items when using {type}.GetEnumerator().");

                        if (isExpectedCompleted)
                            throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                                actual, 
                                expected,
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has more items when using {type}.GetEnumerator().");
                    }

                    if (!equalityComparison(actualEnumerator.Current, expectedEnumerator.Current))
                        throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                            actual, 
                            expected,
                            $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but if differs at index {index} when using {type}.GetEnumerator().");
                }
            }
        }

        protected virtual void EqualityComparison<TExpectedItem>(IReadOnlyList<TActualItem> actual, IEnumerable<TExpectedItem> expected, Func<TActualItem, TExpectedItem, bool> equalityComparison)
        {
            using var expectedEnumerator = expected.GetEnumerator();
            checked
            {
                for (var index = 0; true; index++)
                {
                    var isActualCompleted = index == actual.Count;
                    var isExpectedCompleted = !expectedEnumerator.MoveNext();

                    if (isActualCompleted && isExpectedCompleted)
                        return;

                    if (isActualCompleted ^ isExpectedCompleted)
                    {
                        if (isActualCompleted)
                            throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                                actual, 
                                expected,
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has less items when using the indexer.");

                        if (isExpectedCompleted)
                            throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                                actual, 
                                expected,
                                $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but it has more items when using the indexer.");
                    }

                    if (!equalityComparison(actual[index], expectedEnumerator.Current))
                        throw new ExpectedAssertionException<IEnumerable<TActualItem>, IEnumerable<TExpectedItem>>(
                            actual, 
                            expected,
                            $"Expected {actual.ToFriendlyString()} to be equal to {expected.ToFriendlyString()} but if differs at index {index} when using the indexer.");
                }
            }
        }
    }
}