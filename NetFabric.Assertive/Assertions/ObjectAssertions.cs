using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ObjectAssertions 
    {
        readonly object actual;

        internal ObjectAssertions(object actual) 
        {
            this.actual = actual;
        }

        public ObjectAssertions Equal(object expected)
            => Equal(expected, (actual, expected) => actual.Equals(expected));

        public ObjectAssertions Equal(object expected, Func<object, object, bool> comparer)
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

        public ObjectEnumerableAssertions BeEnumerable()
        {
            var actualType = actual.GetType();
            if (!actualType.IsEnumerable(out var enumerableInfo))
            {
                if (enumerableInfo.GetEnumerator is null)
                    throw new AssertionException($"Expected {actualType} to be an enumerable but it's missing a valid 'GetEnumerator' method.");
                if (enumerableInfo.Current is null)
                    throw new AssertionException($"Expected {enumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'Current' property.");
                if (enumerableInfo.MoveNext is null)
                    throw new AssertionException($"Expected {enumerableInfo.GetEnumerator.ReturnType} to be an enumerator but it's missing a valid 'MoveNext' method.");
            }

            return new ObjectEnumerableAssertions(actual, enumerableInfo);
        }

        public ObjectAssertions BeNull() 
        {
            if (actual is object)
                throw new NullException<object>(actual);
                
            return this;
        }

        public ObjectAssertions NotBeNull() 
        {
            if (actual is null)
                throw new NotNullException();

            return this;
        }
    }
}