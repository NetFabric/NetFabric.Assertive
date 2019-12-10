using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public abstract class DelegateAssertions<TActual>
        : AssertionsBase<Delegate>
        where TActual : Delegate
    {
        internal DelegateAssertions(TActual actual) 
            : base(actual)
        {
        }

        protected abstract void Invoke();

        public ExceptionAssertions<TException> Throw<TException>() 
            where TException : Exception
        {
            var actualException = (TException)null; 
            try
            {
                Invoke();
            }
            catch (TException expected)
            {
                if (expected.GetType() != typeof(TException))
                    throw new AssertionException($"The exception type is not the expected.");

                actualException = expected;
            }
            catch (Exception notExpected)
            {
                throw new AssertionException($"The exception type is not the expected.");
            }

            if (actualException is null)
                throw new AssertionException($"No exception was thrown.");

            return new ExceptionAssertions<TException>(actualException);
        }

        public ExceptionAssertions<TException> ThrowAny<TException>() 
            where TException : Exception
        {
            var actualException = (TException)null; 
            try
            {
                Invoke();
            }
            catch (TException expected)
            {
                actualException = expected;
            }
            catch (Exception notExpected)
            {
                throw new AssertionException($"The exception type is not the expected.");
            }

            if (actualException is null)
                throw new AssertionException($"No exception was thrown.");

            return new ExceptionAssertions<TException>(actualException);
        }
    }
}