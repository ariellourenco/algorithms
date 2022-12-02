using Playground.Stack;

namespace Playground.Tests.Stack;

public sealed class StackTests
{
    [Fact]
    public void CanInitializeStack()
    {
        var capacity = 10;
        var stack = new MyStack<string>(capacity);
        
        Assert.Equal(0, stack.Size);
        Assert.Equal(capacity, stack.Capacity);
        Assert.True(stack.IsEmpty());
    }

    [Fact]
    public void NegativeCapacity_ThrowsArgumentOutOfRangeException() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => new MyStack<string>(-1));

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    public void PopElements_RemoveThemFromStack(int capacity)
    {
        // Arrange
        var stack = StackFactory(capacity);
        
        // Act
        while (!stack.IsEmpty())
            stack.Pop();

        // Assert
        Assert.Equal(0, stack.Size);
    }
    
    [Fact]
    public void Pop_OnEmptyStack_ThrowsInvalidOperationException() =>
        Assert.Throws<InvalidOperationException>(() => new Stack<string>().Pop());

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    public void Push_CanResizeStack_ForTwiceItsOriginalCapacity(int capacity)
    {
        // Arrange
        var newCapacity = capacity * 2;
        var seed = capacity * 17;
        var stack = new MyStack<string>(10);

        // Act
        // We keep pushing items to the stack until it reaches
        // its full capacity + 1.
        for (var i = 0; i < capacity + 1; i++)
            stack.Push(CreateItem(seed++));

        // Assert
        Assert.Equal(newCapacity, stack.Capacity);
        Assert.Equal(capacity + 1, stack.Size);
    }

    [Theory]
    [InlineData(20)]
    public void RespectTheFirstIn_LastOut_Order(int capacity)
    {
        // Arrange
        var seed = capacity * 17;
        var items = new List<string>(capacity);
        var stack = new MyStack<string>(capacity);

        for (var i = 0; i < capacity + 1; i++)
            items.Add(CreateItem(seed++));
        
        items.ForEach(item => stack.Push(item));
        items.Reverse();

        // Act & Arrange
        items.ForEach(item => Assert.Equal(item, stack.Pop()));
    }

    [Fact]
    public void ModificationDuringEnumeration_ThrowsInvalidOperationException()
    {
        // Arrange
        var stack = StackFactory(10);
        using var iterator = stack.GetEnumerator();
        
        stack.Pop();
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => iterator.MoveNext());
    }
    
    private static MyStack<string> StackFactory(int capacity)
    {
        var seed = capacity * 17;
        var stack = new MyStack<string>(capacity);

        for (var i = 0; i < capacity; i++)
            stack.Push(CreateItem(seed++));

        return stack;
    }

    private static string CreateItem(int seed)
    {
        var stringLength = seed % 10 + 5;
        var rand = new Random(seed);
        var bytes = new byte[stringLength];
        
        rand.NextBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}