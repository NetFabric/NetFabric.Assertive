using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public abstract class AsyncDelegateAssertions<TActual>
        : ReferenceTypeAssertionsBase<Delegate>
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
            try
            {
                InvokeAsync().GetAwaiter().GetResult();
            }
            catch (TException actualException)
            {
                if (actualException.GetType() != typeof(TException))
                    throw new AssertionException($"The exception type is not the expected.");

                return new ExceptionAssertions<TException>(actualException);
            }
            catch (Exception)
            {
                throw new AssertionException($"The exception type is not the expected.");
            }

            throw new AssertionException($"No exception was thrown.");
        }

        public ExceptionAssertions<TException> ThrowAny<TException>() 
            where TException : Exception
        {
            try
            {
                InvokeAsync().GetAwaiter().GetResult();
            }
            catch (TException actualException)
            {
                return new ExceptionAssertions<TException>(actualException);
            }
            catch (Exception)
            {
                throw new AssertionException($"The exception type is not the expected.");
            }

            throw new AssertionException($"No exception was thrown.");
        }
    }
}