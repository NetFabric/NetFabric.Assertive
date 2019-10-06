using System;

namespace NetFabric.Assertive
{
    public class NullException : AssertionException
    {
        public NullException()
            : base($"Expected not <null> but found <null>.")
        {
        }
    }
}