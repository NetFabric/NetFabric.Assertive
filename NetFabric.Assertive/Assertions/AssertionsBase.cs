using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public abstract class AssertionsBase<TActual> 
    {
        internal AssertionsBase(TActual actual) 
        {
            Actual = actual;
        }

        public TActual Actual { get; }

        public AssertionsBase<TActual> EvaluatesTrue(Func<TActual, bool> func)
            => this.EvaluatesTrue<AssertionsBase<TActual>, TActual>(func);

        public AssertionsBase<TActual> EvaluatesFalse(Func<TActual, bool> func)
            => this.EvaluatesFalse<AssertionsBase<TActual>, TActual>(func);

        public AssertionsBase<TActual> BeEqualTo(TActual expected)
            => this.BeEqualTo<AssertionsBase<TActual>, TActual>(expected);

        public AssertionsBase<TActual> BeEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => this.BeEqualTo<AssertionsBase<TActual>, TActual, TExpected>(expected, comparer);

        public AssertionsBase<TActual> BeNotEqualTo(TActual expected)
            => this.BeNotEqualTo<AssertionsBase<TActual>, TActual>(expected);

        public AssertionsBase<TActual> BeNotEqualTo<TExpected>(TExpected expected, Func<TActual, TExpected, bool> comparer)
            => this.BeNotEqualTo<AssertionsBase<TActual>, TActual, TExpected>(expected, comparer);
    }
}