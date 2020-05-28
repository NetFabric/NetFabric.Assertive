using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestList, int[], string> BeEqualTo_List_NotEqualData =>
            new TheoryData<TestList, int[], string> 
            {
                { new TestList(0, 0, 0, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestList(1, 0, 0, 0, 0, 0), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new TestList(1, 0, 0, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestList(0, 1, 0, 0, 0, 0), new int[] { }, $"Actual has more items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new TestList(1, 1, 0, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestList(0, 0, 1, 0, 0, 0), new int[] { }, $"Actual has more items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_List_NotEqualData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(TestList actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<TestList, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }


        public static TheoryData<TestCollection, int[], string> BeEqualTo_List_NotEqualCountData =>
            new TheoryData<TestCollection, int[], string>
            {
                { new TestList(1, 1, 1, 0, 0, 0), new int[] { 0 }, $"Expected collections to have same count value.{Environment.NewLine}Expected: 1{Environment.NewLine}Actual: 0" },
                { new TestList(0, 0, 0, 1, 0, 0), new int[] { }, $"Expected collections to have same count value.{Environment.NewLine}Expected: 0{Environment.NewLine}Actual: 1" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_List_NotEqualCountData))]
        public void BeEqualTo_With_NotEqualCount_Should_Throw(TestCollection actual, int[] expected, string message)
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


        public static TheoryData<TestList, int[], string> BeEqualTo_List_NotEqualIndexerData =>
            new TheoryData<TestList, int[], string>
            {
                { new TestList(1, 1, 1, 1, 1, 0), new int[] { 0 }, $"Actual has less items when using the indexer.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestList(0, 0, 0, 0, 0, 1), new int[] { }, $"Actual has more items when using the indexer.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_List_NotEqualIndexerData))]
        public void BeEqualTo_With_NotEqualIndexer_Should_Throw(TestList actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<ReadOnlyListAssertionException<int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}