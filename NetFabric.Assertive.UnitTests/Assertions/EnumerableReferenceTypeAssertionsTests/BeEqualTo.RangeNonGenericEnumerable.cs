using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public class RangeNonGenericEnumerable : RangeEnumerable, IEnumerable
    {
        readonly int count;

        public RangeNonGenericEnumerable(int enumerableCount, int nonGenericEnumerableCount)
            : base(enumerableCount)
        {
            count = nonGenericEnumerableCount;
        }

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(count);

        new class Enumerator : IEnumerator
        {
            readonly int count;
            int current;

            public Enumerator(int count)
            {
                this.count = count;
                current = -1;
            }

            public object Current => current;

            public bool MoveNext() => ++current < count;

            public void Reset() => current = -1;

            public void Dispose() {}
        }
    }

    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeNonGenericEnumerable, int[]> RangeNonGenericEnumerable_EqualData =>
            new TheoryData<RangeNonGenericEnumerable, int[]> 
            {
                { null, null },
                { new RangeNonGenericEnumerable(0, 0), new int[] { } },
                { new RangeNonGenericEnumerable(1, 1), new int[] { 0 } },
                { new RangeNonGenericEnumerable(3, 3), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(RangeNonGenericEnumerable_EqualData))]
        public void RangeNonGenericEnumerable_BeEqualTo_With_Equal_Should_NotThrow(RangeNonGenericEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeNonGenericEnumerable, int[], string> RangeNonGenericEnumerable_NotEqualData =>
            new TheoryData<RangeNonGenericEnumerable, int[], string> 
            {
                { new RangeNonGenericEnumerable(0, 0), new int[] { 0 }, "Expected '' to be equal to '0' but it has less items when using 'NetFabric.Assertive.UnitTests.RangeNonGenericEnumerable.GetEnumerator()'." },
                { new RangeNonGenericEnumerable(1, 1), new int[] { }, "Expected '0' to be equal to '' but it has more items when using 'NetFabric.Assertive.UnitTests.RangeNonGenericEnumerable.GetEnumerator()'." },
                { new RangeNonGenericEnumerable(1, 1), new int[] { 0, 1 }, "Expected '0' to be equal to '0, 1' but it has less items when using 'NetFabric.Assertive.UnitTests.RangeNonGenericEnumerable.GetEnumerator()'." },
            };

        [Theory]
        [MemberData(nameof(RangeNonGenericEnumerable_NotEqualData))]
        public void RangeNonGenericEnumerable_BeEqualTo_With_NotEqual_Should_NotThrow(RangeNonGenericEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeNonGenericEnumerable, IEnumerable<int>>>(action);
            Assert.Equal(message, exception.Message);
        }
    }
}