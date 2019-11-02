using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ObservableAssertions<TActualItem> 
        : AssertionsBase<IObservable<TActualItem>> 
    {
        internal ObservableAssertions(IObservable<TActualItem> actual) 
            : base(actual)
        {
        }

        public ObservableAssertions<TActualItem> BeEqualTo<TExpected>(TExpected expected)
            where TExpected : IEnumerable<TActualItem>
            => BeEqualTo<TExpected, TActualItem>(expected, (actual, expected) => EqualityComparer<TActualItem>.Default.Equals(actual, expected));

        public ObservableAssertions<TActualItem> BeEqualTo<TExpected, TExpectedItem>(TExpected expected, Func<TActualItem, TExpectedItem, bool> comparer)
            where TExpected : IEnumerable<TExpectedItem>
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            if (Actual is null)
            {
                if (expected is object)
                    throw new EqualToAssertionException<IObservable<TActualItem>, TExpected>(Actual, expected);
            }
            else
            {
                if (expected is null)
                    throw new EqualToAssertionException<IObservable<TActualItem>, TExpected>(Actual, expected);

                switch (Actual.ToEnumerable().Compare(expected, comparer, out var index))
                {
                    case EqualityResult.NotEqualAtIndex:
                        throw new ExpectedAssertionException<IObservable<TActualItem>, TExpected>(
                            Actual,
                            expected,
                            $"Actual differs at index {index}.");

                    case EqualityResult.LessItem:
                        throw new ExpectedAssertionException<IObservable<TActualItem>, TExpected>(
                            Actual,
                            expected,
                            $"Actual has less items.");

                    case EqualityResult.MoreItems:
                        throw new ExpectedAssertionException<IObservable<TActualItem>, TExpected>(
                            Actual,
                            expected,
                            $"Actual has more items.");
                }
            }

            return this;
        }
    }
}