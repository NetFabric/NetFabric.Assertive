using System;
using NetFabric.Reflection;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestEnumerableRef, int[]> BeEqualTo_TestEnumerableRef_EqualData =>
            new TheoryData<TestEnumerableRef, int[]>
            {
                { new TestEnumerableRef(TestData.Empty.AsMemory<int>()), TestData.Empty },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestEnumerableRef_EqualData))]
        public void BeEqualTo_TestEnumerableRef_With_Equal_Should_NotThrow(TestEnumerableRef actual, int[] expected)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<AssertionException>(action);
            Assert.Equal("Enumerators declared as 'ref struct' are not supported. Set the 'testRefStructs' parameter to 'false' and use other method of comparison.", exception.Message);
        }

        public static TheoryData<TestEnumerable, int[]> BeEqualTo_TestEnumerable_EqualData =>
            new TheoryData<TestEnumerable, int[]>
            {
                { new TestEnumerable(TestData.Empty),       TestData.Empty },
                { new TestEnumerable(TestData.Single),      TestData.Single },
                { new TestEnumerable(TestData.Multiple),    TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestEnumerable_EqualData))]
        public void BeEqualTo_TestEnumerable_With_Equal_Should_NotThrow(TestEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }
        
        [Fact]
        public void Enumerable_BeEqualTo_With_ExceptionOnGetEnumerator_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnGetEnumeratorEnumerable<int>();
            var expected = new int[0];

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionOnGetEnumeratorEnumerable`1.GetEnumerator().", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void BeEqualTo_With_ExceptionOnCurrent_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnCurrentEnumerable<int>();
            var expected = new int[0];

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionOnCurrentEnumerable`1.Current.", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void BeEqualTo_With_ExceptionOnMoveNext_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnMoveNextEnumerable<int>();
            var expected = new int[0];

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionOnMoveNextEnumerable`1.MoveNext().", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void BeEqualTo_With_ExceptionOnDispose_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnDisposeEnumerable<int>();
            var expected = new int[0];

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in IDisposable.Dispose().", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        public static TheoryData<TestEnumerable, int[], string> BeEqualTo_TestEnumerable_NotEqualData =>
            new TheoryData<TestEnumerable, int[], string>
            {
                { new TestEnumerable(TestData.Single),                 TestData.Empty,     $"Actual has more items when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Empty.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestEnumerable(TestData.Empty),                  TestData.Single,    $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestEnumerable(TestData.SingleNotEqual),         TestData.Single,    $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.SingleNotEqual.ToFriendlyString()}" },
                { new TestEnumerable(TestData.Multiple),               TestData.Single,    $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Single.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Multiple.ToFriendlyString()}" },
                { new TestEnumerable(TestData.Empty),                  TestData.Multiple,  $"Actual has less items when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Empty.ToFriendlyString()}" },
                { new TestEnumerable(TestData.Single),                 TestData.Multiple,  $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.Single.ToFriendlyString()}" },
                { new TestEnumerable(TestData.MultipleNotEqualFirst),  TestData.Multiple,  $"Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualFirst.ToFriendlyString()}" },
                { new TestEnumerable(TestData.MultipleNotEqualMiddle), TestData.Multiple,  $"Actual differs at index 2 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualMiddle.ToFriendlyString()}" },
                { new TestEnumerable(TestData.MultipleNotEqualLast),   TestData.Multiple,  $"Actual differs at index 4 when using 'NetFabric.Assertive.UnitTests.TestEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {TestData.Multiple.ToFriendlyString()}{Environment.NewLine}Actual: {TestData.MultipleNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestEnumerable_NotEqualData))]
        public void BeEqualTo_TestEnumerable_With_NotEqual_Should_Throw(TestEnumerable actual, int[] expected, string message)
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