using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeReadOnlyCollection, int[]> ReadOnlyCollection_EqualData =>
            new TheoryData<RangeReadOnlyCollection, int[]> 
            {
                { null, null },
                { new RangeReadOnlyCollection(0, 0, 0, 0), new int[] { } },
                { new RangeReadOnlyCollection(1, 1, 1, 1), new int[] { 0 } },
                { new RangeReadOnlyCollection(3, 3, 3, 3), new int[] { 0, 1, 2 } },
                { new RangeReadOnlyList(3, 3, 3, 3, 0), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(ReadOnlyCollection_EqualData))]
        public void ReadOnlyCollection_BeEqualTo_With_Equal_Should_NotThrow(RangeReadOnlyCollection actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeReadOnlyCollection, int[], string> ReadOnlyCollection_NotEqualData =>
            new TheoryData<RangeReadOnlyCollection, int[], string> 
            {
                { new RangeReadOnlyCollection(0, 0, 0, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },
                { new RangeReadOnlyCollection(1, 0, 0, 0), new int[] { }, "Expected '' but found '0' with more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },

                { new RangeReadOnlyCollection(1, 0, 0, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'System.Collections.IEnumerable.GetEnumerator()'." },
                { new RangeReadOnlyCollection(0, 1, 0, 0), new int[] { }, "Expected '' but found '0' with more items when using 'System.Collections.IEnumerable.GetEnumerator()'." },

                { new RangeReadOnlyCollection(1, 1, 0, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'." },
                { new RangeReadOnlyCollection(0, 0, 1, 0), new int[] { }, "Expected '' but found '0' with more items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'." },

                { new RangeReadOnlyCollection(1, 1, 1, 0), new int[] { 0 }, "Expected '0' to have count value of 1 but found 0." },
                { new RangeReadOnlyCollection(0, 0, 0, 1), new int[] { }, "Expected '' to have count value of 0 but found 1." },
            };

        [Theory]
        [MemberData(nameof(ReadOnlyCollection_NotEqualData))]
        public void RangeReadOnlyCollection_BeEqualTo_With_NotEqual_Should_Throw(RangeReadOnlyCollection actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeReadOnlyCollection, int[]>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}