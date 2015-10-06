namespace MockEverythingTests.ArgumentsProxies
{
    using System;
    using ArgumentsTarget;
    using MockEverything.Attributes;

    [ProxyOf(typeof(DemoInstance))]
    public static class DemoProxy
    {
        [ProxyMethod(TargetMethodType.Instance)]
        public static void SayHello(string name)
        {
            Console.WriteLine("Tampered hello, {0}!", name);
        }
    }
}