namespace MockEverythingTests.AopTarget
{
    using System;

    public static class AopDemo
    {
        public static string SayHello(string name, int something)
        {
            return string.Format("Hello, {0}! {1}", name, something);
        }
    }
}
