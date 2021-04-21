using System;
using NetFabric.Reflection;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<TestEnumerableRef, int[]> BeEqualTo_TestEnumerableRef_EqualData =>
            new()
            {
                { new TestEnumerableRef(TestData.Empty), TestData.Empty },
                { new TestEnumerableRef(TestData.Single), TestData.Single },
                { new TestEnumerableRef(TestData.Multiple), TestData.Multiple },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestEnumerableRef_EqualData))]
        public void BeEqualTo_TestEnumerableRef_With_Equal_Should_NotThrow(TestEnumerableRef actual, int[] expected)
        {
            // Arrange

            // Act
            _ = actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
        }

        public static TheoryData<TestEnumerableRef, int[], string> BeEqualTo_TestEnumerableRef_NotEqualData =>
            new()
            {
                { new TestEnumerableRef(TestData.Empty), TestData.Single, "Actual has less items when using 'NetFabric.Assertive.UnitTests.TestEnumerableRef.GetEnumerator()'.\r\nExpected: { 5 }\r\nActual: { }" },
                { new TestEnumerableRef(TestData.Single), TestData.Multiple, "Actual differs at index 0 when using 'NetFabric.Assertive.UnitTests.TestEnumerableRef.GetEnumerator()'.\r\nExpected: { 3, 4, 5, 6, 7 }\r\nActual: { 5 }" },
                { new TestEnumerableRef(TestData.Single), TestData.Empty, "Actual has more items when using 'NetFabric.Assertive.UnitTests.TestEnumerableRef.GetEnumerator()'.\r\nExpected: { }\r\nActual: { 5 }" },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_TestEnumerableRef_NotEqualData))]
        public void BeEqualTo_TestEnumerableRef_With_NotEqual_Should_Throw(TestEnumerableRef actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerableAssertionException<TestEnumerableRef, int[]>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);        
        }

        public static TheoryData<TestEnumerable, int[]> BeEqualTo_TestEnumerable_EqualData =>
            new()
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
            var actual = new ExceptionInGetEnumeratorEnumerable<int>();
            var expected = Array.Empty<int>();

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionInGetEnumeratorEnumerable`1.GetEnumerator().", exception.Message);
            Assert.NotNull(exception.InnerException);
            var innerException = Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
            Assert.Equal("An exception!...", innerException.Message);
            Assert.Null(innerException.InnerException);
        }

        [Fact]
        public void BeEqualTo_With_ExceptionOnCurrent_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionInCurrentEnumerable<int>();
            var expected = new[] { 0 };

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionInCurrentEnumerable`1.Current.", exception.Message);
            Assert.NotNull(exception.InnerException);
            var innerException = Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
            Assert.Equal("An exception!...", innerException.Message);
            Assert.Null(innerException.InnerException);
        }

        [Fact]
        public void BeEqualTo_With_ExceptionOnMoveNext_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionInMoveNextEnumerable<int>();
            var expected = Array.Empty<int>();

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in ExceptionInMoveNextEnumerable`1.MoveNext().", exception.Message);
            Assert.NotNull(exception.InnerException);
            var innerException = Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
            Assert.Equal("An exception!...", innerException.Message);
            Assert.Null(innerException.InnerException);
        }

        [Fact]
        public void BeEqualTo_With_ExceptionOnDispose_Should_Throw()
        {
            // Arrange
            var actual = new ExceptionInDisposeEnumerable<int>();
            var expected = Array.Empty<int>();

            // Act
            void action() => actual.Must().BeEnumerableOf<int>().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EnumerationException>(action);
            Assert.Equal("Unhandled exception in IDisposable.Dispose().", exception.Message);
            Assert.NotNull(exception.InnerException);
            var innerException = Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
            Assert.Equal("An exception!...", innerException.Message);
            Assert.Null(innerException.InnerException);
        }

        public static TheoryData<TestEnumerable, int[], string> BeEqualTo_TestEnumerable_NotEqualData =>
            new()
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
            var exception = Assert.Throws<EnumerableAssertionException<TestEnumerable, int[]>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}