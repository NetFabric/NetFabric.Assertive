using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<RangeEnumerable> BeSameAs_SameData =>
            new TheoryData<RangeEnumerable>
            {
                { null },
                { new RangeEnumerable(0) },
            };

        [Theory]
        [MemberData(nameof(BeSameAs_SameData))]
        public void BeSameAs_With_Equal_Should_NotThrow(RangeEnumerable value)
        {
            // Arrange

            // Act
            value.Must()
                .BeEnumerable<int>()
                .BeSameAs(value);

            // Assert
        }

        public static TheoryData<RangeEnumerable, RangeEnumerable, string> BeSameAs_NotSameData =>
            new TheoryData<RangeEnumerable, RangeEnumerable, string>
            {
                { new RangeEnumerable(0), new RangeEnumerable(0), "Expected '' to be same as '' but it's not." },
            };

        [Theory]
        [MemberData(nameof(BeSameAs_NotSameData))]
        public void BeSameAs_With_NotSame_Should_Throw(RangeEnumerable actual, RangeEnumerable expected, string message)
        {
            // Arrange

            // Act
            void action() => actual.Must()
                .BeEnumerable<int>()
                .BeSameAs(expected);

            // Assert
            var exception = Assert.Throws<ExpectedAssertionException<RangeEnumerable, RangeEnumerable>>(action);
            Assert.Same(actual, exception.Actual);
            Assert.Same(expected, exception.Expected);
            Assert.Equal(exception.Message, message);
        }
    }
}
