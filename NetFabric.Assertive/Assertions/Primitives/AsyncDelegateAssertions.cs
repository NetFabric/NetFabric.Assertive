using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public abstract class AsyncDelegateAssertions<TActual>
        : AssertionsBase<Delegate>
        where TActual : Delegate
    {
        internal AsyncDelegateAssertions(TActual actual) 
            : base(actual)
        {
        }

        protected abstract ValueTask InvokeAsync();

        public ExceptionAssertions<TException> Throw<TException>() 
            where TException : Exception
        {
            var actualException = (TException)null; 
            try
            {
                InvokeAsync().GetAwaiter().GetResult();
            }
            catch (TException expected)
            {
                if (expected.GetType() != typeof(TException))
                    throw new AssertionException($"The exception type is not the expected.");

                actualException = expected;
            }
            catch (Exception)
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
                InvokeAsync().GetAwaiter().GetResult();
            }
            catch (TException expected)
            {
                actualException = expected;
            }
            catch (Exception)
            {
                throw new AssertionException($"The exception type is not the expected.");
            }

            if (actualException is null)
                throw new AssertionException($"No exception was thrown.");

            return new ExceptionAssertions<TException>(actualException);
        }
    }
}