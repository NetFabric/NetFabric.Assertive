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
            actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeReadOnlyList, int[], string> ReadOnlyList_NotEqualData =>
            new TheoryData<RangeReadOnlyList, int[], string> 
            {
                { new RangeReadOnlyList(0, 0, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeReadOnlyList(1, 0, 0, 0, 0), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new RangeReadOnlyList(1, 0, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeReadOnlyList(0, 1, 0, 0, 0), new int[] { }, $"Actual has more items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new RangeReadOnlyList(1, 1, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeReadOnlyList(0, 0, 1, 0, 0), new int[] { }, $"Actual has more items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
            };

        [Theory]
        [MemberData(nameof(ReadOnlyList_NotEqualData))]
        public void ReadOnlyList_BeEqualTo_With_NotEqual_Should_Throw(RangeReadOnlyList actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<RangeReadOnlyList, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }


        public static TheoryData<RangeReadOnlyCollection, int[], string> ReadOnlyList_NotEqualCountData =>
            new TheoryData<RangeReadOnlyCollection, int[], string>
            {
                { new RangeReadOnlyList(1, 1, 1, 0, 0), new int[] { 0 }, $"Expected collections to have same count value.{Environment.NewLine}Expected: 1{Environment.NewLine}Actual: 0" },
                { new RangeReadOnlyList(0, 0, 0, 1, 0), new int[] { }, $"Expected collections to have same count value.{Environment.NewLine}Expected: 0{Environment.NewLine}Actual: 1" },
            };

        [Theory]
        [MemberData(nameof(ReadOnlyList_NotEqualCountData))]
        public void ReadOnlyList_BeEqualTo_With_NotEqualCount_Should_Throw(RangeReadOnlyCollection actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<CountAssertionException>(action);
            Assert.Equal(actual.Count, exception.Actual);
            Assert.Equal(expected.Length, exception.Expected);
            Assert.Equal(message, exception.Message);
        }


        public static TheoryData<RangeReadOnlyList, int[], string> ReadOnlyList_NotEqualIndexerData =>
            new TheoryData<RangeReadOnlyList, int[], string>
            {
                { new RangeReadOnlyList(1, 1, 1, 1, 0), new int[] { 0 }, $"Actual has less items when using the indexer.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeReadOnlyList(0, 0, 0, 0, 1), new int[] { }, $"Actual has more items when using the indexer.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
            };

        [Theory]
        [MemberData(nameof(ReadOnlyList_NotEqualIndexerData))]
        public void ReadOnlyList_BeEqualTo_With_NotEqualIndexer_Should_Throw(RangeReadOnlyList actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<ReadOnlyListAssertionException<int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}