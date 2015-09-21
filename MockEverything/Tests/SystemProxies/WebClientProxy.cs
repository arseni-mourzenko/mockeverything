namespace MockEverythingTests.SystemProxies
{
    using System;
    using System.Net;
    using MockEverything.Attributes;

    [ProxyOf(typeof(WebClient))]
    public static class WebClientProxy
    {
        [ProxyMethod(TargetMethodType.Instance)]
        public static string DownloadString(Uri address)
        {
            return "Hello, World!";
        }
    }
}