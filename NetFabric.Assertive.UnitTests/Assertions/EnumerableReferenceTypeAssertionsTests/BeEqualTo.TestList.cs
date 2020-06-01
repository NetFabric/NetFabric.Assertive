using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestList, int[]> BeEqualTo_TestList_EqualData =>
            new TheoryData<TestList, int[]>
            {
                { new TestList(TestData.Empty),    TestData.Empty },
                { new TestList(TestData.Single),   TestData.Single },
                { new TestList(TestData.Multiple), TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestList_EqualData))]
        public void BeEqualTo_TestList_With_Equal_Should_NotThrow(TestList actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestList, int[], int[], string> BeEqualTo_TestList_NotEqualPrivateIndexerData =>
            new TheoryData<TestList, int[], int[], string>
            {
                { new TestList(TestData.Empty,    TestData.Single,                 TestData.Empty,    TestData.Empty),    TestData.Single,                 TestData.Empty,     $"Actual has more items when using the indexer IList`1[System.Int32].Item[System.Int32].{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestList(TestData.Single,   TestData.Empty,                  TestData.Single,   TestData.Single),   TestData.Empty,                  TestData.Single,    $"Actual has less items when using the indexer IList`1[System.Int32].Item[System.Int32].{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestList(TestData.Single,   TestData.Multiple,               TestData.Single,   TestData.Single),   TestData.Multiple,               TestData.Single,    $"Actual differs at index 0 when using the indexer IList`1[System.Int32].Item[System.Int32].{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.Empty,                  TestData.Multiple, TestData.Multiple), TestData.Empty,                  TestData.Multiple,  $"Actual has less items when using the indexer IList`1[System.Int32].Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.Single,                 TestData.Multiple, TestData.Multiple), TestData.Single,                 TestData.Multiple,  $"Actual differs at index 0 when using the indexer IList`1[System.Int32].Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.MultipleNotEqualFirst,  TestData.Multiple, TestData.Multiple), TestData.MultipleNotEqualFirst,  TestData.Multiple,  $"Actual differs at index 0 when using the indexer IList`1[System.Int32].Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.MultipleNotEqualMiddle, TestData.Multiple, TestData.Multiple), TestData.MultipleNotEqualMiddle, TestData.Multiple,  $"Actual differs at index 2 when using the indexer IList`1[System.Int32].Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.MultipleNotEqualLast,   TestData.Multiple, TestData.Multiple), TestData.MultipleNotEqualLast,   TestData.Multiple,  $"Actual differs at index 4 when using the indexer IList`1[System.Int32].Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestList_NotEqualPrivateIndexerData))]
        public void BeEqualTo_TestList_With_NotEqualPrivateIndexer_Should_Throw(TestList source, int[] actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => source.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<ListWrapper<int>, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<TestList, int[], int[], string> BeEqualTo_TestList_NotEqualPublicIndexerData =>
            new TheoryData<TestList, int[], int[], string>
            {
                { new TestList(TestData.Empty,    TestData.Empty,    TestData.Single,                 TestData.Empty),    TestData.Single,                 TestData.Empty,     $"Actual has more items when using the indexer NetFabric.Assertive.UnitTests.TestList.Item[System.Int32].{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestList(TestData.Single,   TestData.Single,   TestData.Empty,                  TestData.Single),   TestData.Empty,                  TestData.Single,    $"Actual has less items when using the indexer NetFabric.Assertive.UnitTests.TestList.Item[System.Int32].{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestList(TestData.Single,   TestData.Single,   TestData.Multiple,               TestData.Single),   TestData.Multiple,               TestData.Single,    $"Actual differs at index 0 when using the indexer NetFabric.Assertive.UnitTests.TestList.Item[System.Int32].{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.Multiple, TestData.Empty,                  TestData.Multiple), TestData.Empty,                  TestData.Multiple,  $"Actual has less items when using the indexer NetFabric.Assertive.UnitTests.TestList.Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.Multiple, TestData.Single,                 TestData.Multiple), TestData.Single,                 TestData.Multiple,  $"Actual differs at index 0 when using the indexer NetFabric.Assertive.UnitTests.TestList.Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.Multiple, TestData.MultipleNotEqualFirst,  TestData.Multiple), TestData.MultipleNotEqualFirst,  TestData.Multiple,  $"Actual differs at index 0 when using the indexer NetFabric.Assertive.UnitTests.TestList.Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.Multiple, TestData.MultipleNotEqualMiddle, TestData.Multiple), TestData.MultipleNotEqualMiddle, TestData.Multiple,  $"Actual differs at index 2 when using the indexer NetFabric.Assertive.UnitTests.TestList.Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { new TestList(TestData.Multiple, TestData.Multiple, TestData.MultipleNotEqualLast,   TestData.Multiple), TestData.MultipleNotEqualLast,   TestData.Multiple,  $"Actual differs at index 4 when using the indexer NetFabric.Assertive.UnitTests.TestList.Item[System.Int32].{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestList_NotEqualPublicIndexerData))]
        public void BeEqualTo_TestList_With_NotEqualPublicIndexer_Should_Throw(TestList source, int[] actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => source.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<IndexerWrapper<TestList, int>, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}