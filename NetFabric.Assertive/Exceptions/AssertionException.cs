using System;
using System.Text;

namespace NetFabric.Assertive
{
    public class AssertionException
        : Exception
    {
        readonly string stackTrace;

        public AssertionException(string message)
            : this(message, (Exception)null)
        {
        }

        protected AssertionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AssertionException(string message, string stackTrace)
            : this(message)
        {
            this.stackTrace = stackTrace;
        }

        public override string StackTrace 
            => stackTrace ?? base.StackTrace; 

        public override string ToString()
        {
            var result = new StringBuilder(GetType().ToString());
 
            if (!string.IsNullOrEmpty(Message))
            {
                result.Append(": ");
                result.Append(Message);
            }

            if (!string.IsNullOrEmpty(StackTrace))
            {
                result.AppendLine();
                result.Append(StackTrace);
            }

            return result.ToString();
        }
    }
}