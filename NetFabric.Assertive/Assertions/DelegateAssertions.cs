using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public abstract class DelegateAssertions<TActual>
        : ReferenceTypeAssertionsBase<DelegateAssertions<TActual>, Delegate>
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
            try
            {
                Invoke();
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
                Invoke();
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