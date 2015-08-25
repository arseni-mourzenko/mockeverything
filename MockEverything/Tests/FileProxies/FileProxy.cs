namespace MockEverythingTests.FileProxies
{
    using System.IO;
    using MockEverything.Attributes;

    [ProxyOf(typeof(File))]
    public class FileProxy
    {
        [ProxyMethod(TargetMethodType.Static)]
        public string ReadAllText(string path)
        {
            return "Hello, World!";
        }
    }
}