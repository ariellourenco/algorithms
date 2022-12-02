namespace Playground.Stack;

/// <summary>
/// Represents a variable size last-in-first-out (LIFO) collection of instances of the same specified type.
/// </summary>
/// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
public interface IStack<T>
{
    /// <summary>
    /// Inserts an object at the top of the <see cref="MyStack{T}"/>.
    /// </summary>
    /// <param name="item">The object to push onto the <see cref="MyStack{T}"/>.</param>
    void Push(T item);
    
    /// <summary>
    /// Removes and returns the object at the top of the <see cref="MyStack{T}"/>.
    /// </summary>
    /// <returns>The object removed from the top of the <see cref="MyStack{T}"/>.</returns>
    T Pop();

    /// <summary>
    /// Checks whether the <see cref="MyStack{T}"/> is empty.
    /// </summary>
    /// <returns><see langword="true"/> if the <see cref="MyStack{T}"/> has no elements; otherwise, <see langword="false"/>.</returns>
    bool IsEmpty();

    /// <summary>
    /// Gets the number of elements contained in the <see cref="MyStack{T}"/>.
    /// </summary>
    int Size { get; }
}