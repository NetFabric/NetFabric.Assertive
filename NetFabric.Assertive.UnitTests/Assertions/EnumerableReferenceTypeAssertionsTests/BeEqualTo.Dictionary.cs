using System;
using System.Collections.Generic;
using Xunit;

namespace NetFabric.Assertive.UnitTests
{
    public partial class EnumerableReferenceTypeAssertionsTests
    {
        public static TheoryData<KeyValuePair<int, int>[]> BeEqualTo_Dictionary_EqualData =>
            new TheoryData<KeyValuePair<int, int>[]>
            {
                {
                    new KeyValuePair<int, int>[] { }
                },
                { 
                    new KeyValuePair<int, int>[] 
                    { 
                        new KeyValuePair<int, int>(5, 5),
                    } 
                },
                {
                    new KeyValuePair<int, int>[]
                    {
                        new KeyValuePair<int, int>(1, 1),
                        new KeyValuePair<int, int>(2, 2),
                        new KeyValuePair<int, int>(3, 3),
                    }
                },
            };

        [Theory]
        [MemberData(nameof(BeEqualTo_Dictionary_EqualData))]
        public void BeEqualTo_Dictionary_With_Equal_Should_NotThrow(KeyValuePair<int, int>[] pairs)
        {
            // Arrange
            var actual = new Dictionary<int, int>();
            foreach (var pair in pairs)
                actual.Add(pair.Key, pair.Value);

            // Act
            _ = actual.Must().BeEnumerableOf<KeyValuePair<int, int>>().BeEqualTo(pairs, testNonGeneric: false);

            // Assert
        }
    }
}