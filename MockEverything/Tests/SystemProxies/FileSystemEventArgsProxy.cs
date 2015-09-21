namespace MockEverythingTests.SystemProxies
{
    using System.IO;
    using MockEverything.Attributes;

    [ProxyOf(typeof(FileSystemEventArgs))]
    public static class FileSystemEventArgsProxy
    {
        [ProxyMethod(TargetMethodType.Instance, ".ctor")]
        public static void Init(WatcherChangeTypes changeType, string directory, string name)
        {
        }
    }
}
