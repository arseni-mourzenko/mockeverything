namespace MockEverythingTests.ReadFile
{
    using System;
    using System.IO;

    public class Program
    {
        public static void Main()
        {
            var contents = File.ReadAllText(@"H:\file\which\does\not\exist");
            Console.WriteLine(contents);
        }
    }
}
