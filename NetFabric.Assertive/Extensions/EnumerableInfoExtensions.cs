using NetFabric.Reflection;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    static class EnumerableInfoExtensions
    {
        public static object InvokeGetEnumerator(this EnumerableInfo info, object enumerable)
        {
            try
            {
                return info.GetEnumerator.Invoke(enumerable, Array.Empty<object>());
            }
            catch (TargetInvocationException ex)
            {
                throw new AssertionException($"Unhandled exception in {info.EnumerableType.Name}.GetEnumerator().", ex.InnerException);
            }
        }

        public static object InvokeGetAsyncEnumerator(this EnumerableInfo info, object enumerable)
        {
            try
            {
                return info.GetEnumerator.Invoke(enumerable, Array.Empty<object>());
            }
            catch (TargetInvocationException ex)
            {
                throw new AssertionException($"Unhandled exception in {info.EnumerableType.Name}.GetAsyncEnumerator().", ex.InnerException);
            }
        }

        public static object InvokeGetAsyncEnumerator(this EnumerableInfo info, object enumerable, CancellationToken token)
        {
            try
            {
                return info.GetEnumerator.Invoke(enumerable, new object[] { token });
            }
            catch (TargetInvocationException ex)
            {
                throw new AssertionException($"Unhandled exception in {info.EnumerableType.Name}.GetAsyncEnumerator(CancellationToken).", ex.InnerException);
            }
        }

        public static object InvokeCurrent(this EnumerableInfo info, object enumerator)
        {
            try
            {
                return info.Current.GetValue(enumerator, Array.Empty<object>());
            }
            catch (Exception ex)
            {
                throw new AssertionException($"Unhandled exception in {info.EnumeratorType.Name}.Current.", ex.InnerException);
            }
        }

        public static bool InvokeMoveNext(this EnumerableInfo info, object enumerator)
        {
            try
            {
                return (bool)info.MoveNext.Invoke(enumerator, Array.Empty<object>());
            }
            catch (TargetInvocationException ex)
            {
                throw new AssertionException($"Unhandled exception in {info.EnumerableType.Name}.MoveNext().", ex.InnerException);
            }
        }

        public static bool InvokeMoveNextAsync(this EnumerableInfo info, object enumerator)
        {
            try
            {
                return ((ValueTask<bool>)info.MoveNext.Invoke(enumerator, Array.Empty<object>()))
                    .GetAwaiter()
                    .GetResult();
            }
            catch (TargetInvocationException ex)
            {
                throw new AssertionException($"Unhandled exception in {info.EnumerableType.Name}.MoveNextAsync().", ex.InnerException);
            }
        }

        public static void InvokeDispose(this EnumerableInfo info, object enumerator)
        {
            if (info.Dispose is null)
                return;

            try
            {
                switch (info.Dispose.Name)
                {
                    case "Dispose":
                        info.Dispose.Invoke(enumerator, Array.Empty<object>());
                        break;

                    case "DisposeAsync":
                        ((ValueTask)info.Dispose.Invoke(enumerator, Array.Empty<object>())).GetAwaiter().GetResult();
                        break;
                }
            }
            catch (TargetInvocationException ex)
            {
                throw new AssertionException($"Unhandled exception in {info.Dispose.DeclaringType.Name}.{info.Dispose.Name}().", ex.InnerException);
            }
        }
    }
}
