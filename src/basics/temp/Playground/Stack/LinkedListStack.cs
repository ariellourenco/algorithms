using System.Collections;

namespace Playground.Stack;

/// <summary>
/// A simple generic stack implemented as a linked list. So the space required is always proportional
/// to the size of the collection as well as the time per operations.
/// </summary>
/// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
public sealed class LinkedListStack<T> : IStack<T>, IEnumerable<T>
{
    private Node<T>? _first;
    private int _size;

    /// <summary>
    /// Inserts an object at the top of the <see cref="MyStack{T}"/>.
    /// </summary>
    /// <param name="item">The object to push onto the <see cref="MyStack{T}"/>.</param>
    public void Push(T item)
    {
        var next = _first;

        _first = new Node<T>(item, next);
        _size++;
    }

    /// <summary>
    /// Removes and returns the object at the top of the <see cref="MyStack{T}"/>.
    /// </summary>
    /// <returns>The object removed from the top of the <see cref="MyStack{T}"/>.</returns>
    public T Pop()
    {
        if (_first == null || _size == 0)
            throw new InvalidOperationException("The stack is empty.");

        var item = _first.Item;
        _first = _first.Next;
        _size--;
        
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

    #region IEnumerable

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() =>
        new Enumerator(this);

    /// <internalonly/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    private struct Enumerator : IEnumerator<T>
    {
        private T? _current;
        private Node<T>? _node;
        private readonly LinkedListStack<T> _stack;
        
        public Enumerator(LinkedListStack<T> stack)
        {
            _current = default;
            _node = stack._first;
            _stack = stack;
        }
        
        public bool MoveNext()
        {
            if (_node == null)
            {
                return false;
            }

            _current = _node.Item;
            _node = _node.Next;

            return true;
        }

        public void Reset()
        {
            _current = default;
            _node = _stack._first;
        }

        public T Current => _current!;

        object IEnumerator.Current => Current!;

        public void Dispose()
        {
        }
    }

    #endregion
}

internal sealed class Node<T>
{
    public Node(T item, Node<T>? next)
    {
        Item = item;
        Next = next;
    }
        
    public T Item { get; }
        
    public Node<T>? Next { get; }
}