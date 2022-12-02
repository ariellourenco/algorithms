using System.Collections;
using System.Diagnostics;

namespace Playground.Stack;

/// <summary>
/// A simple generic stack. Internally it is implemented as an array, so as elements are added to a
/// <see cref="MyStack{T}"/>, the capacity is automatically increased as required by reallocating
/// the internal array.
/// </summary>
/// <remarks>
/// This implementation is not memory efficient and has the flaw that some push and pop operations
/// require resizing: this takes time proportional to the size of the stack. If the number of elements is less than
/// the capacity of the stack, Push is an O(1) operation. If the capacity needs to be increased to accommodate the
/// new element, Push becomes an O(n) operation, where n is the number of elements in the <see cref="MyStack{T}"/>.
/// The same rules applies to Pop operation that is an O(1) in most cases but can be O(n) if resizing is required.
/// </remarks>
/// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
public sealed class MyStack<T>: IStack<T>, IEnumerable<T>
{
    private T[] _items;
    private int _size;
    private int _version;

    /// <summary>
    /// Constructs a <see cref="MyStack{T}"/> with a given initial capacity. The <see cref="MyStack{T}"/> is
    /// initially empty, but will have room for the given number of elements before any re-allocations are required.
    /// </summary>
    /// <param name="capacity">The number of elements the Stack can initially store.</param>
    public MyStack(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), 
                "The initial capacity must be a non-negative number");

        _items = new T[capacity];
        _size = 0;
    }

    /// <summary>
    /// Inserts an object at the top of the <see cref="MyStack{T}"/>.
    /// </summary>
    /// <remarks>
    /// The size of the stack is increased by one. If required, the capacity of the <see cref="MyStack{T}"/>
    /// is doubled before adding the new element.
    /// </remarks>
    /// <param name="item">The object to push onto the <see cref="MyStack{T}"/>.</param>
    public void Push(T item)
    {
        if (_size == _items.Length)
            Resize(_size + 1);
        
        _items[_size++] = item;
        _version++;
    }

    /// <summary>
    /// Removes and returns the object at the top of the <see cref="MyStack{T}"/>.
    /// </summary>
    /// <returns>The object removed from the top of the <see cref="MyStack{T}"/>.</returns>
    public T Pop()
    {
        var item = _items[--_size];
        
        // Even though the popped item will never be accessed again the array still holding a reference
        // to the item - the item is effectively an orphan. This condition is known as loitering and,
        // in this case, is easy to avoid, by setting the array entry corresponding to the popped item to null,
        // thus overwriting the unused reference and making it possible for the .NET GC to reclaim the
        // memory associated with the popped item when the client is finished with it.
        _items[_size] = default!;
        _version++;
        
        if ((uint)_size > 0 && _size == _items.Length / 4)
            Resize(_items.Length / 2);

        return item;
    }

    /// <summary>
    /// Checks whether the <see cref="MyStack{T}"/> is empty.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="MyStack{T}"/> has no elements; otherwise, <see langword="false"/>.</returns>
    public bool IsEmpty() => _size == 0;

    /// <summary>
    /// Gets the number of elements contained in the <see cref="MyStack{T}"/>.
    /// </summary>
    public int Size => _size;

    /// <summary>
    /// Gets the capacity of the <see cref="MyStack{T}"/>.
    /// </summary>
    public int Capacity => _items.Length;
    
    private void Resize(int min)
    {
        if (_items.Length >= min) 
            return;
        
        // If the current capacity of the Stack is less than min, the
        // capacity is increased to twice the current capacity or to min,
        // whichever is larger.
        var newCapacity = 2 * _items.Length;

        // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
        // Note that this check works even when _items.Length overflowed thanks to the (uint) cast.
        // https://github.com/dotnet/coreclr/pull/9773
        if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;
        
        // If computed capacity is still less than specified, set to the original argument.
        // Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
        if (newCapacity < min) newCapacity = min;
        
        Array.Resize(ref _items, newCapacity);
    }

    #region IEnumerable

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => 
        new ReverseArrayEnumerator<T>(this);
    
    /// <internalonly/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    private struct ReverseArrayEnumerator<TItem> : IEnumerator<TItem>
    {
        private TItem? _current;
        private int _index;
        private readonly MyStack<TItem> _myStack;

        // The _version field is an implementation detail intended to detect modification during enumeration.
        // An enumerator remains valid as long as the collection remains unchanged. If changes are made to the
        // collection, such as adding, modifying, or deleting elements, the enumerator is irrecoverably invalidated
        // and the next call to MoveNext or Reset throws an InvalidOperationException.
        private readonly int _version;

        public ReverseArrayEnumerator(MyStack<TItem> myStack)
        {
            _current = default;
            _index = -2;
            _myStack = myStack;
            _version = _myStack._version;
        }

        public TItem Current
        {
            get
            {
                if (_index < 0)
                    throw new InvalidOperationException();
                
                return _current!;
            }
        }

        object? IEnumerator.Current => Current;
        
        public bool MoveNext()
        {
            bool hasNext;

            if (_version != _myStack._version)
                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
            
            switch (_index)
            {
                case -2:
                {
                    // First call to the enumerator.
                    _index = _myStack.Size - 1;
                    hasNext = (_index >= 0);

                    if (hasNext)
                        _current = _myStack._items[_index];
                    break;
                }
                case -1:
                    // End of the enumerator.
                    return false;
                default:
                    hasNext = (--_index >= 0);
                    _current = hasNext ? _myStack._items[_index] : default;
                    break;
            }

            return hasNext;
        }

        public void Reset()
        {
            if (_version != _myStack._version)
                throw new InvalidOperationException("Collection was modified; enumeration operation may not execute.");
            
            _index = -2;
            _current = default;
        }

        public void Dispose() => _index = -1;
    }
}
