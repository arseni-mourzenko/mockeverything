namespace MockEverythingExample1.Proxies
{
    using System;
    using System.Net;
    using Exchanger;
    using MockEverything.Attributes;

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
