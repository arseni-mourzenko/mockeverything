namespace MockEverythingTests.ArgumentsTarget
{
    using System;

    public class DemoInstance
    {
        public void SayHello(string name, int a, int b, int c, int d, int e)
        {
            Console.WriteLine("Hello, {0}! {1}", name, string.Join(", ", a, b, c, d, e));
        }
    }
}
