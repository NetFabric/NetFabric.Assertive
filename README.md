# NetFabric.Assertive

This is a minimal assertions library that performs full coverage on any enumerable type and checks edge scenarios that many developers are not aware of.

## Syntax

```csharp
source.Must()
    .BeNotNull()
    .BeEnumerable<int>()
    .BeEqualTo(new[] {0, 1, 2, 3, 4});
```

## Enumerables

The combination of the methods `BeEnumerable<>()` and `BeEqualTo<>()` allow the test of any type of enumerable in a single assertion:

- `BeEnumerable<TActualItem>()` - asserts that the type `TActual`, passed in to `Must<TActual>()`, is an enumerable that returns a stream of items of type `TActualItem`.

- `BeAsyncEnumerable<TActualItem>()` - asserts that the type `TActual`, passed in to `Must<TActual>()`, is an asynchronous enumerable that returns a stream of items of type `TActualItem`.

- `BeEqualTo<TExpectedItem>(IEnumerable<TExpectedItem> expected)` - asserts that the actual enumerable object contains the same items and in the same order as `expected`. It tests all the enumeration forms implemented by the type `TActual`, passed in to `Must<TActual>()`.

It can handle any of the following enumerable implementations:

### No enumerable interfaces

A collection, to be enumerable by a `foreach` loop, does not have to implement any interface. It just needs to have a `GetEnumerator()` method that returns a type that has a property `Current` with a getter and a parameterless method `MoveNext()`that returns `bool`.

The same applies to [async streams](https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/generate-consume-asynchronous-stream) that, to be enumerable by a `await foreach` loop, don't have to implement any interface. They just needs to have a `GetAsyncEnumerator()` method that returns a type that has a property `Current` with a getter and a parameterless method `MoveNextAsync()`that returns `ValueTask<bool>`.

Here's the minimal implementations of both types of enumerables:

``` csharp
public class EmptyEnumerable<T>
{
    public EmptyEnumerable<T> GetEnumerator() 
        => this;

    public T Current 
        => default;

    public bool MoveNext() 
        => false;
}
```

``` csharp
public class EmptyAsyncEnumerable<T>
{
    public EmptyAsyncEnumerable<T> GetAsyncEnumerator() 
        => this;

    public T Current 
        => default;

    public ValueTask<bool> MoveNextAsync() 
        => new ValueTask<bool>(Task.FromResult(false));
}
```

Here's the implementation, of both types of enumerable, returning values from 0 to `count`:

```csharp
public readonly struct RangeEnumerable
{
    readonly int count;
    
    public RangeEnumerable(int count)
    {
        this.count = count;
    }
    
    public readonly Enumerator GetEnumerator() 
        => new Enumerator(count);
    
    public struct Enumerator
    {
        readonly int count;
        int current;
        
        public Enumerator(int count)
        {
            this.count = count;
            current = -1;
        }
        
        public readonly int Current => current;
        
        public bool MoveNext() => ++current < count;
    }
}

readonly struct RangeAsyncEnumerable
{
    readonly int count;

    public RangeAsyncEnumerable(int count)
    {
        this.count = count;
    }

    public Enumerator GetAsyncEnumerator(CancellationToken token = default)
        => new Enumerator(count, token);

    public struct Enumerator 
    {
        readonly int count;
        readonly CancellationToken token;
        int current;

        public Enumerator(int count, CancellationToken token)
        {
            this.count = count;
            this.token = token;
            current = -1;
        }

        public readonly int Current => current;

        public ValueTask<bool> MoveNextAsync()
        {
            token?.ThrowIfCancellationRequested();

            return new ValueTask<bool>(Task.FromResult<bool>(++current < count));
        }
    }
}
```

_NOTE: The advantage here is performance. The enumerator is a value type so the method calls are not virtual. The use of interfaces would box the enumerator and turn method calls into virtual. The enumerator is not disposable, so the `foreach` does not generate a `try`/`finally` clause, making it inlinable. The example also uses the C# 8 'struct read-only members' feature to avoid defensive copies._

### Enumerable interfaces

Collections can have multiple forms of enumeration. For example; a collection that implements `IReadOnlyList<T>` can be enumerated using the indexer, using `IEnumerable<T>.GetEnumerator()`, using `IEnumerable.GetEnumerator()` and using a public `GetEnumerator()` that is not an override of any of these interfaces. There's no guarantee that they all are correctly implemented. The `Count` property can also return the wrong value.

Here's an example of a collection with multiple possible enumerations:

```csharp
public readonly struct MyRange : IReadOnlyList<int>
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
    
    public readonly Enumerator GetEnumerator() => new Enumerator(Count);
    readonly IEnumerator<int> IEnumerable<int>.GetEnumerator() => new DisposableEnumerator(Count);
    readonly IEnumerator IEnumerable.GetEnumerator() => new DisposableEnumerator(Count);
    
    public struct Enumerator
    {
        readonly int count;
        int current;
        
        public Enumerator(int count)
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
        
        public DisposableEnumerator(int count)
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

_NOTE: The indexer uses a local function so that the accessor does not throw an exception, making it inlinable. The local function does not add dependencies to the example... ;)_

This example has two enumerators so that it can take advantage of the performance features described above and also; be casted to an interface and take advantage of extension methods for collections (like LINQ). It can also be enumerated using the indexer.

### Custom enumerable interfaces

Custom enumerable interfaces can add even more possible enumerations. Here's an interface that restricts the enumerator to a value type and adds a `GetEnumerator()` that explicitly returns the enumerator type. This allows the use of an enumerable interface without boxing the enumerator:

```csharp
public interface IValueEnumerable<T, TEnumerator>
    : IEnumerable<T>
    where TEnumerator : struct, IEnumerator<T>
{
    new TEnumerator GetEnumerator();
}
```

Here's a possible implementation:

```csharp
public readonly struct MyRange 
    : IValueEnumerable<int, MyRange.DisposableEnumerator>
{    
    readonly int count;
    
    public MyRange(int count)
    {
        this.count = count;
    }
        
    public readonly Enumerator GetEnumerator() => new Enumerator(count);
    readonly DisposableEnumerator IValueEnumerable<int, DisposableEnumerator>.GetEnumerator() => new DisposableEnumerator(count);
    readonly IEnumerator<int> IEnumerable<int>.GetEnumerator() => new DisposableEnumerator(count);
    readonly IEnumerator IEnumerable.GetEnumerator() => new DisposableEnumerator(count);
    
    public struct Enumerator
    {
        readonly int count;
        int current;
        
        public Enumerator(int count)
        {
            this.count = count;
            current = -1;
        }
        
        public readonly int Current => current;
        
        public bool MoveNext() => ++current < count;
    }
    
    public struct DisposableEnumerator : IEnumerator<int>
    {
        readonly int count;
        int current;
        
        public DisposableEnumerator(int count)
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

### Explicit interface implementations

Enumerables usually implement interfaces by overriding them using public implementations, or using a mix but, an enumerable can be implemented [using only explicit interface implementations](https://sharplab.io/#v2:EYLgdgpgLgZgHgGiiATgVzAHwAICYCMAsAFDYAMABNvgHQDCA9gDZMQDGUAlg2AM4DcJclXwAWQcSEBmKrgoBZAJ4AlAIZgA5hAogRUgDycwUAHwkA3hSsUS1iigiqAJjyaKKRqBTYMMUCXa21tgySmqaEAAUnt6+xgCUQVbmSXYUUAAWnLw0Pn4UALyxfgHWAL4kqQCSAKJgaAC2ECiqUAwohsYmep2mNADi0HWNza3tkfGF3ZAA7hTDTS1tKJF5CaVWtfWLYyh6A0Pbo8sTUxSz80dL42tQ8Rs2xHZ4lyPXe7pbb7u9Zk/WKX+aXsjhcYDcHmMxWMDzsMTYaBQDhhqUCQLSIVeOxO8Lid1RAIJwMy2VyeMK0P8RLSCKREChRQAtPhYeVqdSYl9se1fvREcivAVurSBayrAxgAArdheLnHdp8ukM4X8+lU9Fo4FWYAMZgUOXvGjyBgANwgADkIHAoKchRQANT2kVqij6Slix5aqiifULeUoGjKCC8aC2lVKwUUZke6nYH1VAAi2QADgxeKpgKwaEneKmQ6dzBV0UWi0JcDILKk8HJUoCvTB2o42BlIibVHs20w0NojOcIHMwuotJF8GR4okNdYAJDUACcrdUXYg91SJaAA=):

```csharp
public class MyRange : IEnumerable<int>
{    
    readonly int count;
    
    public MyRange(int count)
    {
        this.count = count;
    }

    IEnumerator<int> IEnumerable<int>.GetEnumerator() => new Enumerator(count);
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator(count);
    
    class Enumerator : IEnumerator<int>
    {
        readonly int count;
        int current;
        
        public Enumerator(int count)
        {
            this.count = count;
            current = -1;
        }
        
        int IEnumerator<int>.Current => current;
        object IEnumerator.Current => current;
        
        bool IEnumerator.MoveNext() => ++current < count;
        
        void IEnumerator.Reset() => current = -1;
        
        void IDisposable.Dispose() {}
    }
}
```

### Enumerator independence

When a collection is enumerated more than once, using for example two `forech` loops, it is expected that both go through the exact same items.

When a collection is checked for empty using LINQ's `Any()` and later is projected using `Select()`, it is expected the projection is performed on all items.

For this to happen, all enumerators returned by `GetEnumerator()` must not share state.

## References

- [Enumeration in .NET](https://blog.usejournal.com/enumeration-in-net-d5674921512e) by Antão Almada
- [Performance of value-type vs reference-type enumerators](https://medium.com/@antao.almada/performance-of-value-type-vs-reference-type-enumerators-820ab1acc291) by Antão Almada

## Credits

The following open-source projects are used to build and test this project:

- [.NET](https://github.com/dotnet)
- [xUnit.net](https://xunit.net/)

## License

This project is licensed under the MIT license. See the [LICENSE](LICENSE.txt) file for more info.
