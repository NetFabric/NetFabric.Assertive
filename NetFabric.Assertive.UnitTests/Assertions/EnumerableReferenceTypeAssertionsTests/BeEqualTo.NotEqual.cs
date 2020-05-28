using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestEnumerable, int[], string> NotEqual_NullData =>
            new TheoryData<TestEnumerable, int[], string>
            {
                { null, TestData.Empty, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: <null>" },
                { new TestEnumerable(TestData.Empty), null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
            };

        [Theory]
        [MemberData(nameof(NotEqual_NullData))]
        public void BeEqualTo_With_NotEqual_Null_Should_Throw(TestEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<TestEnumerable, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }


        public static TheoryData<TestEnumerable, int[], string> BeEqualTo_NotEqualData =>
            new TheoryData<TestEnumerable, int[], string>
            {
                { new TestEnumerable(TestData.Empty), TestData.Single, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{}}" },
                { new TestEnumerable(TestData.Single), TestData.Empty, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestEnumerable(TestData.Single), TestData.SingleNotEqual, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestEnumerable(TestData.Single), TestData.Multiple, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestEnumerable(TestData.Multiple), TestData.Empty, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestEnumerable(TestData.Multiple), TestData.Single, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestEnumerable(TestData.Multiple), TestData.MultipleNotEqualFirst, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestEnumerable(TestData.Multiple), TestData.MultipleNotEqualMiddle, $"Actual differs at index 2 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestEnumerable(TestData.Multiple), TestData.MultipleNotEqualLast, $"Actual differs at index 4 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },

                { new TestNonGenericEnumerable(TestData.Empty), TestData.Single, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestNonGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{}}" },
                { new TestNonGenericEnumerable(TestData.Single), TestData.Empty, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestNonGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestNonGenericEnumerable(TestData.Single), TestData.SingleNotEqual, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestNonGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestNonGenericEnumerable(TestData.Single), TestData.Multiple, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestNonGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestNonGenericEnumerable(TestData.Multiple), TestData.Empty, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestNonGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestNonGenericEnumerable(TestData.Multiple), TestData.Single, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestNonGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestNonGenericEnumerable(TestData.Multiple), TestData.MultipleNotEqualFirst, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestNonGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestNonGenericEnumerable(TestData.Multiple), TestData.MultipleNotEqualMiddle, $"Actual differs at index 2 when using 'NetFabric.Assertive.UnitTests.TestNonGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestNonGenericEnumerable(TestData.Multiple), TestData.MultipleNotEqualLast, $"Actual differs at index 4 when using 'NetFabric.Assertive.UnitTests.TestNonGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },

                { new TestGenericEnumerable(TestData.Empty), TestData.Single, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{}}" },
                { new TestGenericEnumerable(TestData.Single), TestData.Empty, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestGenericEnumerable(TestData.Single), TestData.SingleNotEqual, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestGenericEnumerable(TestData.Single), TestData.Multiple, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestGenericEnumerable(TestData.Multiple), TestData.Empty, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestGenericEnumerable(TestData.Multiple), TestData.Single, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestGenericEnumerable(TestData.Multiple), TestData.MultipleNotEqualFirst, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestGenericEnumerable(TestData.Multiple), TestData.MultipleNotEqualMiddle, $"Actual differs at index 2 when using 'NetFabric.Assertive.UnitTests.TestGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestGenericEnumerable(TestData.Multiple), TestData.MultipleNotEqualLast, $"Actual differs at index 4 when using 'NetFabric.Assertive.UnitTests.TestGenericEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },

                { new TestCollection(TestData.Empty), TestData.Single, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestCollection.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{}}" },
                { new TestCollection(TestData.Single), TestData.Empty, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestCollection.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestCollection(TestData.Single), TestData.SingleNotEqual, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestCollection.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestCollection(TestData.Single), TestData.Multiple, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestCollection.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestCollection(TestData.Multiple), TestData.Empty, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestCollection.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestCollection(TestData.Multiple), TestData.Single, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestCollection.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestCollection(TestData.Multiple), TestData.MultipleNotEqualFirst, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestCollection.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestCollection(TestData.Multiple), TestData.MultipleNotEqualMiddle, $"Actual differs at index 2 when using 'NetFabric.Assertive.UnitTests.TestCollection.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestCollection(TestData.Multiple), TestData.MultipleNotEqualLast, $"Actual differs at index 4 when using 'NetFabric.Assertive.UnitTests.TestCollection.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },

                { new TestList(TestData.Empty), TestData.Single, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestList.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{}}" },
                { new TestList(TestData.Single), TestData.Empty, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestList.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestList(TestData.Single), TestData.SingleNotEqual, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestList.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestList(TestData.Single), TestData.Multiple, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestList.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new TestList(TestData.Multiple), TestData.Empty, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestList.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestList(TestData.Multiple), TestData.Single, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.TestList.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestList(TestData.Multiple), TestData.MultipleNotEqualFirst, $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestList.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestList(TestData.Multiple), TestData.MultipleNotEqualMiddle, $"Actual differs at index 2 when using 'NetFabric.Assertive.UnitTests.TestList.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
                { new TestList(TestData.Multiple), TestData.MultipleNotEqualLast, $"Actual differs at index 4 when using 'NetFabric.Assertive.UnitTests.TestList.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_NotEqualData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(TestEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<TestEnumerable, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

    }
}