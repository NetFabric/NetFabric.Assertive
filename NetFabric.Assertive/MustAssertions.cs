using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public static class MustAssertions
    {
        public static ObjectAssertions Must(this object actual) 
            => new ObjectAssertions(actual); 

        public static ActionAssertions Must(this Action actual) 
            => new ActionAssertions(actual); 

        public static EnumerableAssertions Must(this IEnumerable actual) 
            => new EnumerableAssertions(actual); 

        public static EnumerableOfTypeAssertions<T> Must<T>(this IEnumerable<T> actual) 
            => new EnumerableOfTypeAssertions<T>(actual); 

        public static ReadOnlyCollectionAssertions<T> Must<T>(this IReadOnlyCollection<T> actual) 
            => new ReadOnlyCollectionAssertions<T>(actual); 

        public static ReadOnlyListAssertions<T> Must<T>(this IReadOnlyList<T> actual) 
            => new ReadOnlyListAssertions<T>(actual); 
    }
}