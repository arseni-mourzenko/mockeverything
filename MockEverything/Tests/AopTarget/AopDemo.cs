namespace MockEverythingTests.AopTarget
{
    using System;

    public static class AopDemo
    {
        public static string SayHello(string name, int something)
        {
            return string.Format("Hello, {0}! {1}", name, something);
        }

        public static void DoStuff(string name)
        {
            Console.WriteLine("Direct hello, {0}!", name);
        }
    }
}
