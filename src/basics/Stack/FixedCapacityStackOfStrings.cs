using System;

namespace Algorithms.Basics.Stack;

/// <summary>
/// Exercise 1.3.1
/// Add a method IsFull() to FixedCapacityStackOfStrings.
/// </summary>
/// <remarks>The implementation below was taken from the book.</remarks>
public sealed class FixedCapacityStackOfStrings
{
    private string[] _items;
    private int _size;

    /// <summary>
    /// Constructs a <see cref="FixedCapacityStackOfStrings"/> with a given initial capacity. 
    /// The <see cref="FixedCapacityStackOfStrings"/> is initially empty, but will have room for 
    /// the given number of elements.
    /// </summary>
    /// <param name="capacity">The number of elements the Stack can initially store.</param>
    public FixedCapacityStackOfStrings(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), 
                "The initial capacity must be a non-negative number");

        _items = new string[capacity];
    }

    /// <summary>
    /// Inserts an object at the top of the <see cref="FixedCapacityStackOfStrings"/>.
    /// </summary>
    /// <param name="item">The string to push onto the <see cref="FixedCapacityStackOfStrings"/>.</param>
    public void Push(string item)
    {
        if (IsFull())
            throw new OutOfMemoryException("The FixedCapacityStackOfStrings is full.");

        _items[_size++] = item;
    }

    /// <summary>
    /// Removes and returns the string at the top of the <see cref="FixedCapacityStackOfStrings"/>.
    /// </summary>
    /// <returns>The string removed from the top of the <see cref="FixedCapacityStackOfStrings"/>.</returns>
    public string Pop()
    {
        if (IsEmpty())
            throw new InvalidOperationException("The FixedCapacityStackOfStrings is empty.");

        var item = _items[--_size];

        // Even though the popped item will never be accessed again the array still holding a reference
        // to the item - the item is effectively an orphan. This condition is known as loitering and,
        // in this case, is easy to avoid, by setting the array entry corresponding to the popped item to null,
        // thus overwriting the unused reference and making it possible for the .NET GC to reclaim the
        // memory associated with the popped item when the client is finished with it.
        _items[_size] = default!;

        return item;
    }

    /// <summary>
    /// Checks whether the <see cref="FixedCapacityStackOfStrings"/> is full.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="FixedCapacityStackOfStrings"/> reaches its capacity; 
    /// otherwise, <see langword="false"/>.</returns>
    public bool IsFull() => _items.Length == _size;

    /// <summary>
    /// Checks whether the <see cref="FixedCapacityStackOfStrings"/> is empty.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="FixedCapacityStackOfStrings"/> has no elements; 
    /// otherwise, <see langword="false"/>.</returns>
    public bool IsEmpty() => _items.Length == 0;

    /// <summary>
    /// Gets the number of elements contained in the <see cref="FixedCapacityStackOfStrings"/>.
    /// </summary>
    public int Size => _size;
}