namespace MockEverythingTests.Aop
{
    using System;
    using AopTarget;

    public static class Program
    {
        public static void Main(string[] args)
        {
            switch (args[0])
            {
                case "with-result":
                    var result = AopDemo.SayHello("Jeff", 123);
                    Console.Write(result);
                    break;

                case "void-method":
                    AopDemo.DoStuff("Alice");
                    break;

                default:
                    Console.WriteLine("The mode is not specified.");
                    break;
            }
        }
    }
}
