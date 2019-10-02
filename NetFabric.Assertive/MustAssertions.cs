using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class MustAssertions
    {
        public static ObjectAssertions<object> Must(this object actual) 
            => new ObjectAssertions<object>(actual); 

        public static ActionAssertions<Action> Must(this Action actual) 
            => new ActionAssertions<Action>(actual); 

        public static EnumerableAssertions<IEnumerable> Must(this IEnumerable actual) 
            => new EnumerableAssertions<IEnumerable>(actual); 

        public static EnumerableOfTypeAssertions<IEnumerable<T>, T> Must<T>(this IEnumerable<T> actual) 
            => new EnumerableOfTypeAssertions<IEnumerable<T>, T>(actual); 

        public static ReadOnlyCollectionAssertions<IReadOnlyCollection<T>, T> Must<T>(this IReadOnlyCollection<T> actual) 
            => new ReadOnlyCollectionAssertions<IReadOnlyCollection<T>, T>(actual); 

        public static ReadOnlyListAssertions<IReadOnlyList<T>, T> Must<T>(this IReadOnlyList<T> actual) 
            => new ReadOnlyListAssertions<IReadOnlyList<T>, T>(actual); 
    }
}