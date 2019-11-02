using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reactive.Linq;
using System.Text;

namespace NetFabric.Assertive
{
    [DebuggerNonUserCode]
    static class ObservableExtensions
    {
        public static string ToFriendlyString<T>(this IObservable<T> observable)
            => observable.ToEnumerable().ToFriendlyString();
    }
}