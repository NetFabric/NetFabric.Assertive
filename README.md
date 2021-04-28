[![GitHub last commit (master)](https://img.shields.io/github/last-commit/NetFabric/NetFabric.Assertive/master.svg?style=flat-square&logo=github)](https://github.com/NetFabric/NetFabric.Assertive/commits/master)
[![Build (master)](https://img.shields.io/github/workflow/status/NetFabric/NetFabric.Assertive/.NET%20Core/master.svg?style=flat-square&logo=github)](https://github.com/NetFabric/NetFabric.Assertive/actions)
[![Coverage](https://img.shields.io/coveralls/github/NetFabric/NetFabric.Assertive/master?style=flat-square&logo=coveralls)](https://coveralls.io/github/NetFabric/NetFabric.Assertive)
[![NuGet Version](https://img.shields.io/nuget/v/NetFabric.Assertive.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/NetFabric.Assertive/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/NetFabric.Assertive.svg?style=flat-square&logo=nuget)](https://www.nuget.org/packages/NetFabric.Assertive/) 
[![Gitter](https://img.shields.io/gitter/room/netfabric/netfabric.assertive?style=flat-square&logo=gitter)](https://gitter.im/NetFabric/NetFabric.Assertive)

# NetFabric.Assertive

This is a assertions library that performs full coverage on most enumerable types and checks edge scenarios that many developers are not aware of.

## Syntax

```csharp
source.Must()
    .BeNotNull()
    .BeEnumerableOf<int>()
    .BeEqualTo(new[] {0, 1, 2, 3, 4});
```

## Enumerables

This framework uses fluent syntax and the combination of the following methods allow the testing of any type of enumerable in a single assertion:

- `BeEnumerableOf<TActualItem>()` - asserts that the type `TActual`, passed in to `Must<TActual>()`, is an enumerable that returns a stream of items of type `TActualItem`.

- `BeAsyncEnumerableOf<TActualItem>()` - asserts that the type `TActual`, passed in to `Must<TActual>()`, is an asynchronous enumerable that returns a stream of items of type `TActualItem`.

- `BeEqualTo<TExpectedItem>(IEnumerable<TExpectedItem> expected)` - asserts that the actual enumerable object contains the same items and in the same order as `expected`. It tests all the enumeration forms implemented by the type `TActual`, passed in to `Must<TActual>()`.

Collections can have multiple forms of enumeration. For example; a collection that implements `IReadOnlyList<T>` can be enumerated using the indexer, using `IEnumerable<T>.GetEnumerator()`, using `IEnumerable.GetEnumerator()` and using a public `GetEnumerator()` that is not an override of any of these interfaces. There's no guarantee that they all are correctly implemented. The `Count` property can also return the wrong value.

_NOTE: This project uses [NetFabric.CodeAnalysis](https://github.com/NetFabric/NetFabric.CodeAnalysis) to handle any kind of enumerable or async enumerable. Check its [README](https://github.com/NetFabric/NetFabric.CodeAnalysis/blob/master/README.md) for a detailed description._

Here's an example of a collection with multiple possible enumerations and enumerator implementations:

``` csharp
public readonly struct MyRange : IReadOnlyList<int>, IList<int>
{    
    public MyRange(int count)
    {
        Count = count;
    }
    
    public readonly int Count { get; }
    
    public int this[int index]
    {
    	get
        {
            if (index < 0 || index >= Count)
                ThrowIndexOutOfRangeException();

            return index;

            static void ThrowIndexOutOfRangeException() => throw new IndexOutOfRangeException();
        }
    }
 
    bool ICollection<int>.IsReadOnly => true;

    public bool Contains(int item) => item >= 0 && item < Count;

    public void CopyTo(int[] array, int arrayIndex)
    {
        for (var index = 0; index < Count; index++)
            array[index + arrayIndex] = index;
    }

    void ICollection<int>.Add(int item) => throw new NotSupportedException();
    void ICollection<int>.Clear() => throw new NotSupportedException();
    bool ICollection<int>.Remove(int item) => throw new NotSupportedException();
    
    int IList<int>.this[int index] 
    { 
        get => this[index]; 
        set => throw new NotSupportedException(); 
    }

    public int IndexOf(int item) => item >= 0 && item < Count ? item : -1;

    void IList<int>.Insert(int index, int item) => throw new NotSupportedException();
    void IList<int>.RemoveAt(int index) => throw new NotSupportedException();
    
    public readonly Enumerator GetEnumerator() => new Enumerator(Count);
    readonly IEnumerator<int> IEnumerable<int>.GetEnumerator() => new DisposableEnumerator(Count);
    readonly IEnumerator IEnumerable.GetEnumerator() => new DisposableEnumerator(Count);
    
    public struct Enumerator
    {
        readonly int count;
        int current;
        
        internal Enumerator(int count)
        {
            this.count = count;
            current = -1;
        }
        
        public readonly int Current => current;
        
        public bool MoveNext() => ++current < count;
    }
    
    class DisposableEnumerator : IEnumerator<int>
    {
        readonly int count;
        int current;
        
        internal DisposableEnumerator(int count)
        {
            this.count = count;
            current = -1;
        }
        
        public int Current => current;
        object IEnumerator.Current => current;
        
        public bool MoveNext() => ++current < count;
        
        public void Reset() => current = -1;
        
        public void Dispose() {}
    }
}
```

This example has two enumerators to: improve performance, allow casting to an enumerable interface and allow the use of extension methods for collections (like LINQ). The public enumerator is a value-type so that calls are not virtual. It doesn't implement `IDispose` so that, a `foreach` that calls it, can be inlined.

It implements `IReadOnlyCollection<>` as the number of items is known. It also implements `ICollection<>` so that it performs better when used with LINQ and when converted to an array or a `List<>`.

It implements `IReadOnlyList<>` so, the indexer can be used. The indexer performs much better that enumerators. It also implements `IList<>` so that it can be used on methods that still don't take `IReadOnlyList<>` parameters.

One single call to the `BeEnumerableOf<TActualItem>()` assertion validates if all implementations return the same sequence of items. Returned by the multiple `GetEnumerator()` methods, the `Count` property, the `CopyTo` method and the multiple indexers. It doesn't test `IndexOf()` and only partially tests `Contains()` methods because these would only work for certain sequences.

### No enumerable interfaces

A collection does not have to implement interfaces to be enumerable using `foreach` or `await foreach`. For example, these are two valid enumerables:

``` csharp
public class Enumerable<T>
{
    public Enumerable<T> GetEnumerator() 
        => this;

    public T Current 
        => default;

    public bool MoveNext() 
        => default;
}
```

``` csharp
public class AsyncEnumerable<T>
{
    public AsyncEnumerable<T> GetAsyncEnumerator() 
        => this;

    public T Current 
        => default;

    public ValueTask<bool> MoveNextAsync() 
        => default;
}
```

To be able to handle these, `NetFabric.Assertive` does not cast or constrain the collections to an enumerable interface. It uses [`NetFabric.Reflection`](https://www.nuget.org/packages/NetFabric.Reflection/) to validate if its an enumerable.

Enumerables can also have `ref struct` enumerators. For example:

``` csharp
class Enumerable<T>
{
    readonly Memory<T> source;
    
    public Enumerable(Memory<T> source)
        => this.source = source;
        
    public Enumerator GetEnumerator()
        => new(source);
        
    public ref struct Enumerator // ref struct enumerator
    {
        readonly Span<T> source;
        int index;
        
        internal Enumerator(Memory<T> source)
        {
            this.source = source.Span;
            index = -1;
        }
        
        public T Current
            => source[index];
            
        public bool MoveNext()
            => ++index < source.Length;
    }
}
```

These cannot be boxed, so reflection cannot be used to enumerate these as it tries to cast them to `object`. To support these type of enumerables `NetFabric.Assertive` generates custom code using expression trees and the data collected by [`NetFabric.Reflection`](https://www.nuget.org/packages/NetFabric.Reflection/).

`NetFabric.Assertive` also supports validation of async enumerables. To be able to enumerate these, its required the use of the `async` and `await` keywords so that the compiler generates a complex state machine that enumerates it. Expression trees do not yet support these. For this reason, `NetFabric.Assertive` uses a wrapper that implements `IAsyncEnumerable` and calls the enumerable using reflection.

`await foreach` does not support `ref struct` enumerators so, the use of reflection in this case, is not an issue.

## Limitations

`foreach` and `await foreach` support the return by reference of items. Unfortunately reflection only supports this feature since `netstandard 2.1` and expression trees do not support it at all.

For these reasons, `NetFabric.Assertive` only support return by reference in the case of async enumerables and starting from `netstandard 2.1`.

In the case it's not supported, an exception is thrown. To be able to test the other types of enumeration on the enumerable, it's possible to disable this test by setting the optional parameter `warnRefReturns` to `false`:

``` csharp
result.Must()
    .BeEnumerableOf<int>()
    .BeEqualTo(expected, warnRefReturns: false)
```

This disables this particular test but leaves all the other enabled. You should still compare the enumeration using an alternative method. 

## References

- [Enumeration in .NET](https://blog.usejournal.com/enumeration-in-net-d5674921512e) by Antão Almada
- [Performance of value-type vs reference-type enumerators](https://medium.com/@antao.almada/performance-of-value-type-vs-reference-type-enumerators-820ab1acc291) by Antão Almada

## Credits

The following open-source projects are used to build and test this project:

- [.NET](https://github.com/dotnet)
- [NetFabric.CodeAnalysis](https://github.com/NetFabric/NetFabric.CodeAnalysis)
- [xUnit.net](https://xunit.net/)

## License

This project is licensed under the MIT license. See the [LICENSE](LICENSE) file for more info.
