namespace MockEverythingTests.Aop
{
    using System;
    using AopTarget;

    public static class Program
    {
        public static void Main()
        {
            var result = AopDemo.SayHello("Jeff", 123);
            Console.Write(result);
        }
    }
}
