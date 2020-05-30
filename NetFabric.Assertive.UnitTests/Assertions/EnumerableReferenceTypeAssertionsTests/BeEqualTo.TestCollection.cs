using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestCollection, int[]> TestCollection_EqualData =>
            new TheoryData<TestCollection, int[]>
            {
                { new TestCollection(TestData.Empty), TestData.Empty },
                { new TestCollection(TestData.Single), TestData.Single },
                { new TestCollection(TestData.Multiple), TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(TestCollection_EqualData))]
        public void BeEqualTo_TestCollection_With_Equal_Should_NotThrow(TestCollection actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestCollection, int[], string> BeEqualTo_TestCollection_NotEqualData =>
            new TheoryData<TestCollection, int[], string> 
            {
                { new TestCollection(0, 0, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestCollection(1, 0, 0, 0, 0), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new TestCollection(1, 0, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestCollection(0, 1, 0, 0, 0), new int[] { }, $"Actual has more items when using 'System.Collections.IEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },

                { new TestCollection(1, 1, 0, 0, 0), new int[] { 0 }, $"Actual has less items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestCollection(0, 0, 1, 0, 0), new int[] { }, $"Actual has more items when using 'System.Collections.Generic.IEnumerable`1[System.Int32].GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestCollection_NotEqualData))]
        public void BeEqualTo_TestCollection_With_NotEqual_Should_Throw(TestCollection actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<TestCollection, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<TestCollection, int[], string> BeEqualTo_TestCollection_NotEqualCountData =>
            new TheoryData<TestCollection, int[], string>
            {
                { new TestCollection(1, 1, 1, 0, 1), new int[] { 0 }, $"Expected collections to have same count value.{Environment.NewLine}Expected: 1{Environment.NewLine}Actual: 0" },
                { new TestCollection(0, 0, 0, 1, 0), new int[] { }, $"Expected collections to have same count value.{Environment.NewLine}Expected: 0{Environment.NewLine}Actual: 1" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestCollection_NotEqualCountData))]
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

        public static TheoryData<TestCollection, int[], string> BeEqualTo_TestCollection_CopyToThrowsData =>
            new TheoryData<TestCollection, int[], string>
            {
                { new TestCollection(2, 2, 2, 2, 0), new int[] { 0, 1 }, $"Actual differs at index 0 when using the CopyTo.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new TestCollection(2, 2, 2, 2, 1), new int[] { 0, 1 }, $"Actual differs at index 1 when using the CopyTo.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestCollection_CopyToThrowsData))]
        public void BeEqualTo_With_CopyToThrows_Should_Throw(TestCollection actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<ICollection<int>, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<TestCollection, int[], string> BeEqualTo_TestCollection_NotEqualCopyToData =>
            new TheoryData<TestCollection, int[], string>
            {
                { new TestCollection(2, 2, 2, 2, 3), new int[] { 0, 1 }, $"Actual has more items when using the CopyTo.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestCollection_NotEqualCopyToData))]
        public void BeEqualTo_With_NotEqualCopyTo_Should_Throw(TestCollection actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<CopyToAssertionException<int, int[]>>(action);
            Assert.Equal(actual, exception.Actual.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}