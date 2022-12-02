var test = new Playground.Stack.MyStack<string>(10);

test.Push("Item 1");
test.Push("Item 2");
test.Push("Item 3");
test.Push("Item 4");
test.Push("Item 5");
test.Push("Item 6");
test.Push("Item 7");
test.Push("Item 8");
test.Push("Item 9");

foreach(var item in test)
    Console.WriteLine(item);