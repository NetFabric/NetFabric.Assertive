# NetFabric.Assertive

This is a minimal assertions library that performs full coverage on any enumerable type.

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

- `BeEqualTo<TExpectedItem>(IEnumerable<TExpectedItem> expected)` - asserts that the actual enumerable object contains the same items and in the same order as `expected`. It tests all the enumeration forms implemented by the type `TActual`, passed in to `Must<TActual>()`.

It can handle any of the following enumerable implementations:

### No enumerable interfaces

A collection, to be enumerable by a `foreach` loop, does not have to implement any interface. It just needs to have a `GetEnumerator()` method that returns a type that has a property `Current` with a getter and a parameterless method `MoveNext()`that returns `bool`.

Here's the [minimal implementation of an enumerable](https://sharplab.io/#v2:EYLgtghgzgLgpgJwDQxAgrgOwD4AEBMAjALABQZuAzAAQHUCiYADjAJ72bpiITAA2cADwAVAHxkA3mWozaNRi3aduCXgJGjqAcTgwOXHjAD2CABQBKagF5NMABYBLKAG4y02VWrDqAYXQIEOEwYa00AEzgAMwh0PhhXclJZOWpgIyM+agBZIwA3OAA5OAAPGAtQ6mi+KDgEgF83UgIaSXcZAnxqNuopJOTZSJM4CABjO1NciARqSb50OGoHTGpMOAB3BmY2fRU1ISWYUQtzbuSASFxCAE4JiDm4cwTkhtI6oA===):

```csharp
public class EmptyEnumerable<T>
{
    public EmptyEnumerable<T> GetEnumerator() => this;

    public T Current => default;

    public bool MoveNext() => false;
}
```

Here's an [implementation of an enumerable that returns values from 0 to `count`](https://sharplab.io/#v2:EYLgtghgzgLgpgJwDQxAgrgOwD4AEBMAjALABQZuAzAAQJwQAmA9pgDYCe1sGAxjNQFl2AJQiYA5nDIBvMtXm16zNpwCWmfjyZYYAbjkKD8qoJFjJACnWbtGgJRHqs0gtfUYAC1VQAdFp3UALzU/hr6LgoAvo6OJnSMLBzUAKKY6GCIEDBMCNQA4nAwqemZ2QgWdkEAfNSYcADuKWkZCFk5FqEwduGusTTc6HxNJa1ljs5uCvHKSdYhtnqOrnM86Ah0YUuGEZMmxS1t5SsLDjtuE5Nunt5+C0HzOj2XrqvrcBr3ALSET5PRZ70AQo4kpEmoPgBhNYbfiBGqvGG/NxbYw0YBMJisQRMABucAAcnAAB4wCrVagAagpCPe/AAPA9Nmd/v8KPgaDJYvh8NRxijqAAzHL0HgeCw4iC5CWsdBwajqWoNUyiCRwCyEAAMdlOzwAkLhCABOcUQGVwbqOFlAA):

```csharp
public readonly struct MyRange
{
    readonly int count;
    
    public MyRange(int count)
    {
        this.count = count;
    }
    
    public readonly Enumerator GetEnumerator() => new Enumerator(count);
    
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
```

_NOTE: The advantage here is performance. The enumerator is a value type so the method calls are not virtual. The use of interfaces would box the enumerator and turn method calls into virtual. The enumerator is not disposable, so the `foreach` does not generate a `try`/`finally` clause, making it inlinable. The example also uses the C# 8 'struct read-only members' feature to avoid defensive copies._

### Enumerable interfaces

Collections can have multiple forms of enumeration. For example; a collection that implements `IReadOnlyList<T>` can be enumerated using the indexer, using `IEnumerable<T>.GetEnumerator()`, using `IEnumerable.GetEnumerator()` and using a public `GetEnumerator()` that is not an override of any of these interfaces. There's no guarantee that they all are correctly implemented. The `Count` property can also return the wrong value.

Here's an example of [a collection with multiple possible enumerations](https://sharplab.io/#v2:EYLgtghgzgLgpgJwD4AEBMBGAsAKBQBgAIUMA6AYQHsAbauAYxgEtKA7KAblwOIwBYuObgGZCCOBAAmbagE9CsBAFdGhALKyAShFYBzOIRCEAkpomSA8qzkAZJrAA8TVjAB8uAN6FvhXD+KiGtp6cAAUzjCE9JRKLgCUfj4eif7eVLGRALxRMS6C/gC+Kb44/iii4lIy8hGE6S6EXvowHIRFpT7F5YS1MAAW9gDatc6ScAAeALrFyR3eAJDNxf6zqWs9AGaE4axj44QOhERISD27E4Su2fUwCXPrawAqfQiUAO7G5+MWSjAWG8F9ABRcb0OAAB2YbFCcUEy3WKAA7Gc9nD7g8UHxCM9Xh8vj8/gCdMDQRCoawYYRMq5CP1cYRWHA3iZ8b9/oC4CCwZCWBTYfDvO1CsUuhVzNVCEDWEowIgIDBKAhCABxOAwKUyuUKhCU6kMpmS6WyhDyxWhG78+6VaTWeTGDXG00IJwuGkkYQutykVXqo1as1xKk0xnMgAi9nBlCgEGAdAd/p1FvyPmtEvtfpN2t4wm9avjmYDQf1YYjUZjcYzTvNuVuye8ooUMGUqnzTpmArE4ttZ0i0QyddStXoSgQ4jyHY73Vb2p2vZrdweqwePn69lIfYa2Q3LQ7/mHo7gm8IAFoMAPhejOpfvN1U93auQR2OsjT98/z1eMaJgJQaOpKAAbnAAByEwwLqNIANSQW+h6RIc24DkKn4+OghDhlAkbRrGnKVlmRjppqBbOhE7j3Euax3nIPY5P2HZDk+cEfvW14BOhpbYRWRFVkO84dhRy6rlA641lStHjqxe6MUep7MW0E6sd0D7SS+UQqXJlDAAAVgwkSEY62oUCpRawRJDyTt+v7UP+QGgeM4GBnq0GmfB4k7qxFnEFiZhQGqEFqQeMlngpX5eexmFRmEgYeMhgq4O03BoKInhdGgaAlCsHYbIqEj0H0oQARASqFdQSgGM4xbqFoxJhBg+BxAu6zzCQACcBUQKVcCWheBRAA):

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

Here's a possible [implementation](https://sharplab.io/#v2:EYLgtghgzgLgpgJwDQxAgrgOwD4AEBMAjALABQuADAAS6EB0AwgPYA2LcAxjAJZOZQBuMpRqEALENLCAzFW6Z4CAGYQOcKgEkAahBbo4AUUzowiCMHYAeACpIq1oybMwmCAHxkAkCFHSbH0k8AdwALRHUHY1MECBcEKh9YDC47DUdo2Nd/MgBvL0w4IPt051cqAHE4GBKYuIAKAEpJAF8yGSoEOAgAEz4WAE8qJPQuKgBZfoAlCEwAc3UyKiWEzR09QyizCzhLeRg7Cem5uDoAEW4oAAcmKHN2Gsz3XOWqL06evsG9qg4mLBhJC9FstcLJDjN5nVvr9/g1gUs8qQXi8YCELnQYQoqABeH5/BSA5atJHIpbwmiyd69TADKgPOIVKr01yNHFuKgFIrMhB1TEwJrkqmfKjnK43O4bJy1MraXT6B7bXYKOyi663bbctx0SrVTbSnkNNkcwoii5qiXc3n4/mEpZCmmDNJ6x5KmDs2h+PZanWWw3Y9mc01i9X3Z31PkCkl2rrU2lOqWPXzaplhll+gMm1XijWpnkR22vKMUoYwZIwOm58mI0nRj4OuRYvkFl7Q9AIToE8lAosvUEVhP1aHWuE95bVmsotFQDHWnF4/7NmscNsd8u4gC0hEXS2JE67IMpMeF3wYK7gWP9PzPndHZNvxeATFY4yYADc4AA5OAADxgrMvADUAHLu257lpY843i8u7LOSfbDKMWYhpKGQMj48aoVkXpVvutaxl8jbWtuDbliBq7EbhxZIRauZQoRsKUeOE5LKi6J8nOTaUS8ZFgXOm7ETBpKUX2J7Xmu7I8VBNZMMAABWnDlhhpQIIwYlGpJAKUcJsiPs+Yxvp+P5/umVBARpVAQZx97aTQYhUJMcBQFU/4SWpG5blp959rgdlIXArI5IJVDEsSwj4LIuRwfg+CFi8THIkorhdBwIR1K+EDxOl6wNsaRTgscdSEBQDQjhOni0AAnGlcpwJG0FkM0QA):

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

## References

- [Enumeration in .NET](https://blog.usejournal.com/enumeration-in-net-d5674921512e) by Antão Almada
- [Performance of value-type vs reference-type enumerators](https://medium.com/@antao.almada/performance-of-value-type-vs-reference-type-enumerators-820ab1acc291) by Antão Almada

## Credits

The following open-source projects are used to build and test this project:

- [.NET](https://github.com/dotnet)
- [xUnit.net](https://xunit.net/)

## License

This project is licensed under the MIT license. See the [LICENSE](LICENSE.txt) file for more info.
