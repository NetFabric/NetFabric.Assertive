using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ActionAssertions
    {
        internal ActionAssertions(Action actual) 
        {
            Actual = actual;
        }

        public Action Actual { get; }

        public ExceptionAssertions<TException> Throw<TException>() 
            where TException : Exception
        {
            var actualException = (TException)null; 
            try
            {
                Actual.Invoke();
            }
            catch (TException expected)
            {
                actualException = expected;
            }
            catch (Exception notExpected)
            {
                throw new AssertionException($"Expected exception '{typeof(TException)}' but exception '{notExpected.GetType()}' was thrown instead.");
            }

            if (actualException is null)
                throw new AssertionException($"Expected exception '{typeof(TException)}' but not exception was thrown.");

            return new ExceptionAssertions<TException>(actualException);
        }
    }
}