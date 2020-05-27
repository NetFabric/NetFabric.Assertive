using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class AsyncFunctionAssertions
        : AsyncDelegateAssertions<Func<ValueTask>>
    {
        internal AsyncFunctionAssertions(Func<ValueTask> actual)
            : base(actual) 
            => Actual = actual;

        public new Func<ValueTask> Actual { get; }

        protected override ValueTask InvokeAsync()
            => Actual();
    }
}