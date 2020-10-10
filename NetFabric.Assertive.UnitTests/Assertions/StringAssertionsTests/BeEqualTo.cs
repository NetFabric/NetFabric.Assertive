using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class StringAssertionsTests
    {
        [Theory]
        [InlineData(null, null, false)]
        [InlineData("", "", false)]
        [InlineData("", "", true)]
        [InlineData(" ", " ", false)]
        [InlineData(" ", " ", true)]
        [InlineData("a", "a", false)]
        [InlineData("a", "a", true)]
        [InlineData("a", "A", true)]
        public void BeEqualTo_With_Equal_Should_NotThrow(string value, string expected, bool ignoreCase)
        {
            // Arrange

            // Act
            _ = value.Must().BeEqualTo(expected, ignoreCase);

            // Assert
        }

        public static TheoryData<string, string, string> Enumerable_NotEqualNullData =>
            new TheoryData<string, string, string>
            {
                { null, "", $"Expected to be equal but it's not.{Environment.NewLine}Expected: <empty>{Environment.NewLine}Actual: <null>" },
                { "", null, $"Expected to be equal but it's not.{Environment.NewLine}Expected: <null>{Environment.NewLine}Actual: <empty>" },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Null_Should_Throw(string actual, string expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<string, string>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }


        public static TheoryData<string, string, int, string> Enumerable_NotEqualData =>
            new TheoryData<string, string, int, string>
            {
                { "*", "", 0, $"Expected to be equal but it's not at position 0.{Environment.NewLine}Expected: <empty>{Environment.NewLine}  Actual: *{Environment.NewLine}          \u25b2" },
                { "*", "0", 0, $"Expected to be equal but it's not at position 0.{Environment.NewLine}Expected: 0{Environment.NewLine}  Actual: *{Environment.NewLine}          \u25b2" },
                { "0", "012345", 1, $"Expected to be equal but it's not at position 1.{Environment.NewLine}Expected: 012345{Environment.NewLine}  Actual: 0{Environment.NewLine}           \u25b2" },
                { "012345", "0", 1, $"Expected to be equal but it's not at position 1.{Environment.NewLine}Expected: 0{Environment.NewLine}  Actual: 012345{Environment.NewLine}           \u25b2" },
                { "01234*", "012345", 5, $"Expected to be equal but it's not at position 5.{Environment.NewLine}Expected: 012345{Environment.NewLine}  Actual: 01234*{Environment.NewLine}               \u25b2" },
                { "01234*6789", "0123456789", 5, $"Expected to be equal but it's not at position 5.{Environment.NewLine}Expected: 0123456789{Environment.NewLine}  Actual: 01234*6789{Environment.NewLine}               \u25b2" },
                { "0123456789a*", "0123456789ab", 11, $"Expected to be equal but it's not at position 11.{Environment.NewLine}Expected: \u2026123456789ab{Environment.NewLine}  Actual: \u2026123456789a*{Environment.NewLine}                     \u25b2" },
                { "0123456789a*cdefghijkl", "0123456789abcdefghijkl", 11, $"Expected to be equal but it's not at position 11.{Environment.NewLine}Expected: \u2026123456789abcdefghijk\u2026{Environment.NewLine}  Actual: \u2026123456789a*cdefghijk\u2026{Environment.NewLine}                     \u25b2" },
                { " \n\r34*", " \n\r345", 5, $"Expected to be equal but it's not at position 5.{Environment.NewLine}Expected: \u2022\u2193\u2190345{Environment.NewLine}  Actual: \u2022\u2193\u219034*{Environment.NewLine}               \u25b2" },
            };

        [Theory]
        [MemberData(nameof(Enumerable_NotEqualData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(string actual, string expected, int index, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<StringEqualToAssertionException>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(index, exception.Index);
            Assert.Equal(message, exception.Message);
        }
    }
}
