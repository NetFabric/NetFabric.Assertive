using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ArraySegmentAssertionsTests
    {
        public static readonly ArraySegment<int> EmptyArraySegment = new ArraySegment<int>(TestData.Empty);

        public static readonly ArraySegment<int> SingleArraySegment = new ArraySegment<int>(TestData.Single);
        public static readonly ArraySegment<int> SingleArraySegmentNotEqual = new ArraySegment<int>(TestData.SingleNotEqual);

        public static readonly ArraySegment<int> MultipleArraySegment = new ArraySegment<int>(TestData.Multiple);
        public static readonly ArraySegment<int> MultipleArraySegmentNotEqualFirst = new ArraySegment<int>(TestData.MultipleNotEqualFirst);
        public static readonly ArraySegment<int> MultipleArraySegmentNotEqualMiddle = new ArraySegment<int>(TestData.MultipleNotEqualMiddle);
        public static readonly ArraySegment<int> MultipleArraySegmentNotEqualLast = new ArraySegment<int>(TestData.MultipleNotEqualLast);

        public static TheoryData<ArraySegment<int>> EmptyArraySegments =>
            new TheoryData<ArraySegment<int>>
            {
                { EmptyArraySegment },
                { new ArraySegment<int>(TestData.Single, 0, 0) },
                { new ArraySegment<int>(TestData.Multiple, 2, 0) },
            };

        public static TheoryData<ArraySegment<int>> SingleArraySegments =>
            new TheoryData<ArraySegment<int>>
            {
                { SingleArraySegment },
                { new ArraySegment<int>(TestData.Multiple, 0, 1) },
                { new ArraySegment<int>(TestData.Multiple, 2, 1) },
            };

        public static TheoryData<ArraySegment<int>> MultipleArraySegments =>
            new TheoryData<ArraySegment<int>>
            {
                { MultipleArraySegment },
                { new ArraySegment<int>(TestData.Multiple, 0, 3) },
                { new ArraySegment<int>(TestData.Multiple, 2, 3) },
            };

        [Theory]
        [MemberData(nameof(EmptyArraySegments))]
        [MemberData(nameof(SingleArraySegments))]
        [MemberData(nameof(MultipleArraySegments))]
        public void BeEqualTo_With_Equal_Should_NotThrow(ArraySegment<int> value)
        {
            // Arrange

            // Act
            _ = value.Must().BeEqualTo(value);

            // Assert
        }

        public static TheoryData<ArraySegment<int>, ArraySegment<int>, string> NotEqualNullData =>
            new TheoryData<ArraySegment<int>, ArraySegment<int>, string>
            {
                { SingleArraySegment,                    EmptyArraySegment,       $"Actual array segment has more items.{Environment.NewLine}Expected: {EmptyArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {SingleArraySegment.ToFriendlyString()}" },
                { EmptyArraySegment,                     SingleArraySegment,      $"Actual array segment has less items.{Environment.NewLine}Expected: {SingleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {EmptyArraySegment.ToFriendlyString()}" },
                { SingleArraySegmentNotEqual,            SingleArraySegment,      $"Array segments differ at index 0.{Environment.NewLine}Expected: {SingleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {SingleArraySegmentNotEqual.ToFriendlyString()}" },
                { MultipleArraySegment,                  SingleArraySegment,      $"Array segments differ at index 0.{Environment.NewLine}Expected: {SingleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {MultipleArraySegment.ToFriendlyString()}" },
                { MultipleArraySegmentNotEqualFirst,     MultipleArraySegment,    $"Array segments differ at index 0.{Environment.NewLine}Expected: {MultipleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {MultipleArraySegmentNotEqualFirst.ToFriendlyString()}" },
                { MultipleArraySegmentNotEqualMiddle,    MultipleArraySegment,    $"Array segments differ at index 2.{Environment.NewLine}Expected: {MultipleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {MultipleArraySegmentNotEqualMiddle.ToFriendlyString()}" },
                { MultipleArraySegmentNotEqualLast,      MultipleArraySegment,    $"Array segments differ at index 4.{Environment.NewLine}Expected: {MultipleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {MultipleArraySegmentNotEqualLast.ToFriendlyString()}" },
            };

        [Theory]
        [MemberData(nameof(NotEqualNullData))]
        public void BeEqualTo_With_NotEqual_Should_Throw(ArraySegment<int> actual, ArraySegment<int> expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must().BeEqualTo(expected);

            // Assert
            var exception = Assert.Throws<EqualToAssertionException<ArraySegment<int>, ArraySegment<int>>>(action);
            Assert.Equal(actual, exception.Actual);
            Assert.Equal(expected, exception.Expected);
            Assert.Equal(message, exception.Message);
        }
    }
}
