using System;

namespace NetFabric.Assertive
{
    public class NotNullException : AssertionException
    {
        public NotNullException()
            : base($"Expected not to be <null> but found <null>.")
        {
        }
    }
}