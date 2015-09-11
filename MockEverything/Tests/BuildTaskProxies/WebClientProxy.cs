namespace MockEverythingTests.BuildTaskProxies
{
    using System;
    using System.Net;
    using MockEverything.Attributes;
    using BuildTaskExchanger;

    [ProxyOf(typeof(WebClient))]
    public static class WebClientProxy
    {
        [ProxyMethod(TargetMethodType.Instance)]
        public static string DownloadString(Uri address)
        {
            return string.Format("Hello, {0}!", WebClientExchanger.PersonName);
        }
    }
}