using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeNonGenericEnumerable, int[]> NonGenericEnumerable_EqualData =>
            new TheoryData<RangeNonGenericEnumerable, int[]> 
            {
                { null, null },
                { new RangeNonGenericEnumerable(0, 0), new int[] { } },
                { new RangeNonGenericEnumerable(1, 1), new int[] { 0 } },
                { new RangeNonGenericEnumerable(3, 3), new int[] { 0, 1, 2 } },
                { new RangeGenericEnumerable(3, 3, 0), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(NonGenericEnumerable_EqualData))]
        public void NonGenericEnumerable_BeEqualTo_With_Equal_Should_NotThrow(RangeNonGenericEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeNonGenericEnumerable, int[], string> NonGenericEnumerable_NotEqualData =>
            new TheoryData<RangeNonGenericEnumerable, int[], string> 
            {
                { new RangeNonGenericEnumerable(0, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },
                { new RangeNonGenericEnumerable(1, 0), new int[] { }, "Expected '' but found '0' with more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'." },

                { new RangeNonGenericEnumerable(1, 0), new int[] { 0 }, "Expected '0' but found '' with less items when using 'System.Collections.IEnumerable.GetEnumerator()'." },
                { new RangeNonGenericEnumerable(0, 1), new int[] { }, "Expected '' but found '0' with more items when using 'System.Collections.IEnumerable.GetEnumerator()'." },
            };

        [Theory]
        [MemberData(nameof(NonGenericEnumerable_NotEqualData))]
        public void NonGenericEnumerable_BeEqualTo_With_NotEqual_Should_Throw(RangeNonGenericEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerable<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeNonGenericEnumerable, int[]>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}