namespace MockEverythingTests.ArgumentsProxies
{
    using System;
    using ArgumentsTarget;
    using MockEverything.Attributes;

    [ProxyOf(typeof(DemoInstance))]
    public static class DemoProxy
    {
        [ProxyMethod(TargetMethodType.Instance)]
        public static void SayHello(string name, int a, int b, int c, int d, int e)
        {
            Console.WriteLine("Tampered hello, {0}! {1}", name, string.Join(", ", a, b, c, d, e));
        }
    }
}