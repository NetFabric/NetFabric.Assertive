using System;
using System.Reactive.Linq;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ObservableAssertionsTests
    {
        public static TheoryData<IObservable<int>, int[], string> NotEqualNullData =>
            new TheoryData<IObservable<int>, int[], string>
            {
                { null, new int[] { }, $"Expected to be equal but it's not.{Environment.NewLine}Expected: {{}}{Environment.NewLine}Actual: <null>" },
                { Observable.Range(0, 0), null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: {{}}" },
            };

        [Theory]
        [MemberData(nameof(NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Null_Should_Throw(IObservable<int> actual, int[] expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<IObservable<int>, int[]>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}