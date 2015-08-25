namespace MockEverythingTests.DownloadString
{
    using System;
    using System.Net;

    public static class Program
    {
        public static void Main()
        {
            var contents = new WebClient().DownloadString("http://example.com/");
            Console.WriteLine(contents);
        }
    }
}
