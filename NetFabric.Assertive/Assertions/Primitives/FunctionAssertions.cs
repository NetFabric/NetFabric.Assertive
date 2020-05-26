using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class FunctionAssertions<TActual>
        : DelegateAssertions<Func<TActual>>
    {
        internal FunctionAssertions(Func<TActual> actual)
            : base(actual) 
            => Actual = actual;

        public new Func<TActual> Actual { get; }

        protected override void Invoke()
            => Actual();
    }
}