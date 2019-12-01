using NetFabric.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public abstract class AssertionsBase<TActual> 
    {
        internal AssertionsBase(TActual actual) 
        {
            Actual = actual;
        }

        public TActual Actual { get; }

        protected void AssertIsEnumerable<TActualItem>(out EnumerableInfo enumerableInfo)
        {
            var actualType = typeof(TActual);
            if (actualType == typeof(TActualItem[])) // convert TActualItem[] to IList<TActualItem>
                actualType = typeof(IList<>).MakeGenericType(typeof(TActualItem));
            actualType.IsEnumerable(out enumerableInfo);

            if (enumerableInfo.GetEnumerator is null)
                throw new ActualAssertionException<TActual>(Actual, $"Expected to be an enumerable but it's missing a valid 'GetEnumerator' method.");
            if (enumerableInfo.Current is null)
                throw new ActualAssertionException<TActual>(Actual, $"Expected to be an enumerator but it's missing a valid 'Current' property.");
            if (enumerableInfo.MoveNext is null)
                throw new ActualAssertionException<TActual>(Actual, $"Expected to be an enumerator but it's missing a valid 'MoveNext' method.");

            var actualItemType = enumerableInfo.ItemType;
            if (actualItemType.IsByRef)
            {
                if (!actualItemType.IsAssignableTo(typeof(TActualItem).MakeByRefType()))
                    throw new ActualAssertionException<TActual>(Actual, $"Expected to be an enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
            }
            else
            {
                if (!actualItemType.IsAssignableTo(typeof(TActualItem)))
                    throw new ActualAssertionException<TActual>(Actual, $"Expected to be an enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
            }
        }

        protected void AssertIsAsyncEnumerable<TActualItem>(out EnumerableInfo enumerableInfo)
        {
            var actualType = typeof(TActual);
            actualType.IsAsyncEnumerable(out enumerableInfo);

            if (enumerableInfo.GetEnumerator is null)
                throw new ActualAssertionException<TActual>(Actual, $"Expected to be an async enumerable but it's missing a valid 'GetAsyncEnumerator' method.");
            if (enumerableInfo.Current is null)
                throw new ActualAssertionException<TActual>(Actual, $"Expected to be an async enumerator but it's missing a valid 'Current' property.");
            if (enumerableInfo.MoveNext is null)
                throw new ActualAssertionException<TActual>(Actual, $"Expected to be an async enumerator but it's missing a valid 'MoveNextAsync' method.");

            var actualItemType = enumerableInfo.ItemType;
            if (actualItemType.IsByRef)
            {
                if (!actualItemType.IsAssignableTo(typeof(TActualItem).MakeByRefType()))
                    throw new ActualAssertionException<TActual>(Actual, $"Expected to be an async enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
            }
            else
            {
                if (!actualItemType.IsAssignableTo(typeof(TActualItem)))
                    throw new ActualAssertionException<TActual>(Actual, $"Expected to be an async enumerable of '{typeof(TActualItem)}' but found an enumerable of '{actualItemType}'.");
            }
        }
    }
}