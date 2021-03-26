namespace NetFabric.Assertive.UnitTests
{
    public static class TestData
    {
        public const int EmptyCount = 0;
        public static readonly int[] Empty = { };

        public const int SingleCount = 1;
        public static readonly int[] Single = { 5 };
        public static readonly int[] SingleNotEqual = { 10 };

        public const int MultipleCount = 5;
        public static readonly int[] Multiple = { 3, 4, 5, 6, 7 };
        public static readonly int[] MultipleNotEqualFirst = { 10, 4, 5, 6, 7 };
        public static readonly int[] MultipleNotEqualMiddle = { 3, 4, 10, 6, 7 };
        public static readonly int[] MultipleNotEqualLast = { 3, 4, 5, 6, 10 };
    }
}
