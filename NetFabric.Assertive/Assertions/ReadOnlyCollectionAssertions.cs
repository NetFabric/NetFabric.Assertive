using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ReadOnlyCollectionAssertions<T>
        : EnumerableOfTypeAssertions<T>
    {
        readonly IReadOnlyCollection<T> actual;

        internal ReadOnlyCollectionAssertions(IReadOnlyCollection<T> actual) 
            : base(actual) 
        {
            this.actual = actual;
        }

        protected override void EqualityComparison(EnumerableWrapper actual, IEnumerable expected, Func<object, object, bool> equalityComparison)
        {
            base.EqualityComparison(actual, expected, equalityComparison);

            // TODO: compare Count
        }
    }
}