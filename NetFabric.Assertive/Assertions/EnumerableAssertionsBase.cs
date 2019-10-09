using System;
using System.Diagnostics;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    public class EnumerableAssertionsBase<TActual> 
        : AssertionsBase<TActual> 
    {
        internal EnumerableAssertionsBase(TActual actual, EnumerableInfo enumerableInfo) 
            : base(actual)
        {
            EnumerableInfo = enumerableInfo;
        }

        public EnumerableInfo EnumerableInfo { get; }
    }
}