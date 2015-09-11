namespace MockEverythingExample1.Sample
{
    using System;
    using Exchanger;
    using Library;

    public static class Program
    {
        public static void Main()
        {
            WebClientExchanger.PersonName = "Jeff";
            var actual = new Demo().FindName();
            Console.WriteLine(actual);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}