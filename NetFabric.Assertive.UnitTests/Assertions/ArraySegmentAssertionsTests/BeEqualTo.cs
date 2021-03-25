using System;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class ArraySegmentAssertionsTests
    {
        public static readonly ArraySegment<int> EmptyArraySegment = new(TestData.Empty);

        public static readonly ArraySegment<int> SingleArraySegment = new(TestData.Single);
        public static readonly ArraySegment<int> SingleArraySegmentNotEqual = new(TestData.SingleNotEqual);

        public static readonly ArraySegment<int> MultipleArraySegment = new(TestData.Multiple);
        public static readonly ArraySegment<int> MultipleArraySegmentNotEqualFirst = new(TestData.MultipleNotEqualFirst);
        public static readonly ArraySegment<int> MultipleArraySegmentNotEqualMiddle = new(TestData.MultipleNotEqualMiddle);
        public static readonly ArraySegment<int> MultipleArraySegmentNotEqualLast = new(TestData.MultipleNotEqualLast);

        public static TheoryData<ArraySegment<int>> EmptyArraySegments =>
            new()
            {
                { EmptyArraySegment },
                { new ArraySegment<int>(TestData.Single, 0, 0) },
                { new ArraySegment<int>(TestData.Multiple, 2, 0) },
            };

        public static TheoryData<ArraySegment<int>> SingleArraySegments =>
            new()
            {
                { SingleArraySegment },
                { new ArraySegment<int>(TestData.Multiple, 0, 1) },
                { new ArraySegment<int>(TestData.Multiple, 2, 1) },
            };

        public static TheoryData<ArraySegment<int>> MultipleArraySegments =>
            new()
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
            new()
            {
                { SingleArraySegment,                    EmptyArraySegment,       $"Actual collection has more items.{Environment.NewLine}Expected: {EmptyArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {SingleArraySegment.ToFriendlyString()}" },
                { EmptyArraySegment,                     SingleArraySegment,      $"Actual collection has less items.{Environment.NewLine}Expected: {SingleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {EmptyArraySegment.ToFriendlyString()}" },
                { SingleArraySegmentNotEqual,            SingleArraySegment,      $"Collections differ at index 0.{Environment.NewLine}Expected: {SingleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {SingleArraySegmentNotEqual.ToFriendlyString()}" },
                { MultipleArraySegment,                  SingleArraySegment,      $"Collections differ at index 0.{Environment.NewLine}Expected: {SingleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {MultipleArraySegment.ToFriendlyString()}" },
                { MultipleArraySegmentNotEqualFirst,     MultipleArraySegment,    $"Collections differ at index 0.{Environment.NewLine}Expected: {MultipleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {MultipleArraySegmentNotEqualFirst.ToFriendlyString()}" },
                { MultipleArraySegmentNotEqualMiddle,    MultipleArraySegment,    $"Collections differ at index 2.{Environment.NewLine}Expected: {MultipleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {MultipleArraySegmentNotEqualMiddle.ToFriendlyString()}" },
                { MultipleArraySegmentNotEqualLast,      MultipleArraySegment,    $"Collections differ at index 4.{Environment.NewLine}Expected: {MultipleArraySegment.ToFriendlyString()}{Environment.NewLine}Actual: {MultipleArraySegmentNotEqualLast.ToFriendlyString()}" },
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
