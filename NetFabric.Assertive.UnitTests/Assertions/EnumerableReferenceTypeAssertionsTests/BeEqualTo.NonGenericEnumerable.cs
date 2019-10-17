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
            actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeNonGenericEnumerable, int[], string> NonGenericEnumerable_NotEqualData =>
            new TheoryData<RangeNonGenericEnumerable, int[], string> 
            {
                { new RangeNonGenericEnumerable(0, 0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeNonGenericEnumerable(1, 0), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new RangeNonGenericEnumerable(1, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeNonGenericEnumerable(0, 1), new int[] { }, $"Actual has more items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
            };

        [Theory]
        [MemberData(nameof(NonGenericEnumerable_NotEqualData))]
        public void NonGenericEnumerable_BeEqualTo_With_NotEqual_Should_Throw(RangeNonGenericEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<RangeNonGenericEnumerable, int[]>>(action);
            Assert.Same(actual, exception.Actual.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}