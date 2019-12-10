using System;
using NetFabric.Reflection;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        [Fact]
        public void Enumerable_BeEqualTo_With_ExceptionOnGetEnumerator_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnGetEnumeratorEnumerable<int>();
            var expected = new int[0];

            // Act
            Action action = () => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionOnGetEnumeratorEnumerable`1.GetEnumerator().", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void Enumerable_BeEqualTo_With_ExceptionOnCurrent_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnCurrentEnumerable<int>();
            var expected = new int[0];

            // Act
            Action action = () => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionOnCurrentEnumerable`1.Current.", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void Enumerable_BeEqualTo_With_ExceptionOnMoveNext_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnMoveNextEnumerable<int>();
            var expected = new int[0];

            // Act
            Action action = () => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionOnMoveNextEnumerable`1.MoveNext().", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        [Fact]
        public void Enumerable_BeEqualTo_With_ExceptionOnDispose_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionOnDisposeEnumerable<int>();
            var expected = new int[0];

            // Act
            Action action = () => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in IDisposable.Dispose().", exception.Message);
            Assert.NotNull(exception.InnerException);
        }

        public static TheoryData<RangeEnumerable, int[]> Enumerable_EqualData =>
            new TheoryData<RangeEnumerable, int[]> 
            {
                { null, null },
                { new RangeEnumerable(0), new int[] { } },
                { new RangeEnumerable(1), new int[] { 0 } },
                { new RangeEnumerable(3), new int[] { 0, 1, 2 } },
                { new RangeNonGenericEnumerable(3, 0), new int[] { 0, 1, 2 } },
            };

        [Theory]
        [MemberData(nameof(Enumerable_EqualData))]
        public void Enumerable_BeEqualTo_With_Equal_Should_NotThrow(RangeEnumerable actual, int[] expected)
        {
            // Arrange

            // Act
            actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<RangeEnumerable, int[], string> Enumerable_NotEqualData =>
            new TheoryData<RangeEnumerable, int[], string> 
            {
                { new RangeEnumerable(0), new int[] { 0 }, $"Actual has less items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0}}{Environment.NewLine}Actual: {{}}" },
                { new RangeEnumerable(1), new int[] { }, $"Actual has more items when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: {{0}}" },
                { new RangeEnumerable(3), new int[] { 0, 5, 2 }, $"Actual differs at index 1 when using 'NetFabric.Assertive.UnitTests.RangeEnumerable.GetEnumerator()'.{Environment.NewLine}Expected: {{0, 5, 2}}{Environment.NewLine}Actual: {{0, 1, 2}}" },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualData))]
        public void Enumerable_BeEqualTo_With_NotEqual_Should_Throw(RangeEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<RangeEnumerable, int, int[]>>(action);
            Assert.Same(actual, exception.Actual.Instance);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }

        public static TheoryData<RangeEnumerable, int[], string> Enumerable_NotEqualNullData =>
            new TheoryData<RangeEnumerable, int[], string>
            {
                { null, new int[] { }, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: <null>" },
                { new RangeEnumerable(0), null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualNullData))]
        public void Enumerable_BeEqualTo_With_NotEqual_Null_Should_Throw(RangeEnumerable actual, int[] expected, string message)
        {
            // Arrange

            // Act
            Action action = () => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<RangeEnumerable, int[]>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}