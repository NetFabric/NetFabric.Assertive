using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeReadOnlyList, int[]> ReadOnlyList_EqualData =>
            new TheoryData<RangeReadOnlyList, int[]> 
            {
                { null, null },
                { new RangeReadOnlyList(0, 0, 0, 0, 0), new int[] { } },
                { new RangeReadOnlyList(1, 1, 1, 1, 1), new int[] { 0 } },
                { new RangeReadOnlyList(3, 3, 3, 3, 3), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(ReadOnlyList_EqualData))]
        public void ReadOnlyList_BeEqualTo_With_Equal_Should_NotThrow(RangeReadOnlyList actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeReadOnlyList, int[], string> ReadOnlyList_NotEqualData =>
            new TheoryData<RangeReadOnlyList, int[], string> 
            {
                { new RangeReadOnlyList(0, 0, 0, 0, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },
                { new RangeReadOnlyList(1, 0, 0, 0, 0), new int[] { }, "Expected '' but found '0' with more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },

                { new RangeReadOnlyList(1, 0, 0, 0, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'System.Collections.IEnumerable.GetEnumerator()'." },
                { new RangeReadOnlyList(0, 1, 0, 0, 0), new int[] { }, "Expected '' but found '0' with more items when using 'System.Collections.IEnumerable.GetEnumerator()'." },

                { new RangeReadOnlyList(1, 1, 0, 0, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'." },
                { new RangeReadOnlyList(0, 0, 1, 0, 0), new int[] { }, "Expected '' but found '0' with more items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'." },

                { new RangeReadOnlyList(1, 1, 1, 0, 0), new int[] { 0 }, "Expected '0' to have count value of 1 but found 0." },
                { new RangeReadOnlyList(0, 0, 0, 1, 0), new int[] { }, "Expected '' to have count value of 0 but found 1." },

                { new RangeReadOnlyList(1, 1, 1, 1, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using the indexer." },
                { new RangeReadOnlyList(0, 0, 0, 0, 1), new int[] { }, "Expected '' but found '0' with more items when using the indexer." },
            };

        [Theory]
        [MemberData(nameof(ReadOnlyList_NotEqualData))]
        public void ReadOnlyList_BeEqualTo_With_NotEqual_Should_Throw(RangeReadOnlyList actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeReadOnlyList, IEnumerable<int>>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}