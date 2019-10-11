using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeEnumerable, int[]> RangeEnumerable_EqualData =>
            new TheoryData<RangeEnumerable, int[]> 
            {
                { null, null },
                { new RangeEnumerable(0), new int[] { } },
                { new RangeEnumerable(1), new int[] { 0 } },
                { new RangeEnumerable(3), new int[] { 0, 1, 2 } },
                { new RangeNonGenericEnumerable(3, 0), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(RangeEnumerable_EqualData))]
        public void RangeEnumerable_BeEqualTo_With_Equal_Should_NotThrow(RangeEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeEnumerable, int[], string> RangeEnumerable_NotEqualData =>
            new TheoryData<RangeEnumerable, int[], string> 
            {
                { new RangeEnumerable(0), new int[] { 0 }, "Expected '' to be equal to '0' but it has less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },
                { new RangeEnumerable(1), new int[] { }, "Expected '0' to be equal to '' but it has more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },
            };

        [Theory]
        [MemberData(nameof(RangeEnumerable_NotEqualData))]
        public void RangeEnumerable_BeEqualTo_With_NotEqual_Should_Throw(RangeEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeEnumerable, IEnumerable<int>>>(action);
            Assert.Equal(message, exception.Message);
        }
    }
}