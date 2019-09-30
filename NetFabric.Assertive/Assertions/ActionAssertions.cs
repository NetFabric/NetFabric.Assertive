using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ActionAssertions
        : ObjectAssertions
    {
        readonly Action actual;

        internal ActionAssertions(Action actual) 
            : base(actual) 
        {
            this.actual = actual;
        }

        public ExceptionAssertions<TException> ThrowException<TException>() 
            where TException : Exception
        {
            var actualException = (TException)null; 
            try
            {
                actual.Invoke();
            }
            catch (TException expected)
            {
                actualException = expected;
            }
            catch (Exception notExpected)
            {
                throw new AssertionException($"Expected exception {typeof(TException)} but exception {notExpected.GetType()} was thrown instead.");
            }

            if (actualException is null)
                throw new AssertionException($"Expected exception {typeof(TException)} but not exception was thrown.");

            return new ExceptionAssertions<TException>(actualException);
        }

        public ArgumentExceptionAssertions<TException> ThrowArgumentException<TException>() 
            where TException : ArgumentException
        {
            var actualException = (TException)null; 
            try
            {
                actual.Invoke();
            }
            catch (TException expected)
            {
                actualException = expected;
            }
            catch (Exception notExpected)
            {
                throw new AssertionException($"Expected exception {typeof(TException)} but exception {notExpected.GetType()} was thrown instead.");
            }

            if (actualException is null)
                throw new AssertionException($"Expected exception {typeof(TException)} but not exception was thrown.");

            return new ArgumentExceptionAssertions<TException>(actualException);
        }
    }
}