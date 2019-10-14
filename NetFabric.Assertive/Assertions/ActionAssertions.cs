using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class ActionAssertions
        : DelegateAssertions<Action>
    {
        internal ActionAssertions(Action actual)
            : base(actual)
        {
            Actual = actual;
        }

        public new Action Actual { get; }

        protected override void Invoke()
            => Actual();

        public ActionAssertions EvaluatesTrue(Func<Action, bool> func)
        {
            if (!func(Actual))
                throw new ActualAssertionException<Action>(Actual,
                    $"Evaluates to 'false'.");

            return this;
        }
    }
}