using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class AsyncEnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestAsyncEnumerable, int[]> BeEqualTo_AsyncEnumerable_EqualData =>
            new TheoryData<TestAsyncEnumerable, int[]>
            {
                { null, null },

                { new TestAsyncEnumerable(TestData.Empty), TestData.Empty },
                { new TestAsyncEnumerable(TestData.Single), TestData.Single },
                { new TestAsyncEnumerable(TestData.Multiple), TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_AsyncEnumerable_EqualData))]
        public void BeEqualTo_AsyncEnumerable_With_Equal_Should_NotThrow(TestAsyncEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestCancellableAsyncEnumerable, int[]> BeEqualTo_AsyncCancellableEnumerable_EqualData =>
            new TheoryData<TestCancellableAsyncEnumerable, int[]>
            {
                { null, null },

                { new TestCancellableAsyncEnumerable(TestData.Empty), TestData.Empty },
                { new TestCancellableAsyncEnumerable(TestData.Single), TestData.Single },
                { new TestCancellableAsyncEnumerable(TestData.Multiple), TestData.Multiple },

                { new TestGenericAsyncEnumerable(TestData.Empty), TestData.Empty },
                { new TestGenericAsyncEnumerable(TestData.Single), TestData.Single },
                { new TestGenericAsyncEnumerable(TestData.Multiple), TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_AsyncCancellableEnumerable_EqualData))]
        public void BeEqualTo_AsyncCancellableEnumerable_With_Equal_Should_NotThrow(TestCancellableAsyncEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeAsyncEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }
    }
}