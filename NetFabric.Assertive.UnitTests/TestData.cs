namespace NetFabric.Assertive.UnitTests
{
    public static class TestData
    {
        public static readonly int[] Empty = new int[] { };

        public static readonly int[] Single = new int[] { 5 };
        public static readonly int[] SingleNotEqual = new int[] { 10 };

        public static readonly int[] Multiple = new int[] { 3, 4, 5, 6, 7 };
        public static readonly int[] MultipleNotEqualFirst = new int[] { 10, 4, 5, 6, 7 };
        public static readonly int[] MultipleNotEqualMiddle = new int[] { 3, 4, 10, 6, 7 };
        public static readonly int[] MultipleNotEqualLast = new int[] { 3, 4, 5, 6, 10 };
    }
}
