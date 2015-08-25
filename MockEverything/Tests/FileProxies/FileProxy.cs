namespace MockEverythingTests.FileProxies
{
    using System.IO;
    using MockEverything.Attributes;

    [ProxyOf(typeof(File))]
    public static class FileProxy
    {
        [ProxyMethod(TargetMethodType.Static)]
        public static string ReadAllText(string path)
        {
            return "Hello, World!";
        }
    }
}