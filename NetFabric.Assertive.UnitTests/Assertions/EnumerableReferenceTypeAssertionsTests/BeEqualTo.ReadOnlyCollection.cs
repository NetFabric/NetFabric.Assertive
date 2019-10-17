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
            actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeReadOnlyCollection, int[], string> ReadOnlyCollection_NotEqualData =>
            new TheoryData<RangeReadOnlyCollection, int[], string> 
            {
                { new RangeReadOnlyCollection(0, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeReadOnlyCollection(1, 0, 0, 0), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new RangeReadOnlyCollection(1, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeReadOnlyCollection(0, 1, 0, 0), new int[] { }, $"Actual has more items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new RangeReadOnlyCollection(1, 1, 0, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeReadOnlyCollection(0, 0, 1, 0), new int[] { }, $"Actual has more items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
            };

        [Theory]
        [MemberData(nameof(ReadOnlyCollection_NotEqualData))]
        public void RangeReadOnlyCollection_BeEqualTo_With_NotEqual_Should_Throw(RangeReadOnlyCollection actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<RangeReadOnlyCollection, int[]>>(action);
            Assert.Same(actual, exception.Actual.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<RangeReadOnlyCollection, int[], string> ReadOnlyCollection_NotEqualCountData =>
            new TheoryData<RangeReadOnlyCollection, int[], string>
            {
                { new RangeReadOnlyCollection(1, 1, 1, 0), new int[] { 0 }, $"Expected collections to have same count value.{Environment.NewLine}Expected: 1{Environment.NewLine}Actual: 0" },
                { new RangeReadOnlyCollection(0, 0, 0, 1), new int[] { }, $"Expected collections to have same count value.{Environment.NewLine}Expected: 0{Environment.NewLine}Actual: 1" },
            };

        [Theory]
        [MemberData(nameof(ReadOnlyCollection_NotEqualCountData))]
        public void RangeReadOnlyCollection_BeEqualTo_With_NotEqualCount_Should_Throw(RangeReadOnlyCollection actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<CountAssertionException>(action);
            Assert.Equal(actual.Count, exception.Actual);
            Assert.Equal(expected.Length, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}