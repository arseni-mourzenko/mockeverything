namespace MockEverythingTests.AopTarget
{
    using System;

    public static class AopDemo
    {
        public static void SayHello(string name, int something)
        {
            Console.WriteLine("Hello, {0}! {1}", name, something);
        }
    }
}
