namespace MockEverythingTests.InitializeEventArgs
{
    using System;
    using System.IO;

    public static class Program
    {
        public static void Main()
        {
            var name = new FileSystemEventArgs(WatcherChangeTypes.Created, @"C:\Hello", "World.txt").Name;
            Console.WriteLine(name ?? "<null>");
        }
    }
}