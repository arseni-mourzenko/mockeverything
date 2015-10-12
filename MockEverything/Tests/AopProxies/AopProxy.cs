namespace MockEverythingTests.AopProxies
{
    using System;
    using AopTarget;
    using MockEverything.Attributes;

    [ProxyOf(typeof(AopDemo))]
    public static class AopProxy
    {
        [EntryHook]
        public static void Entry(string name, object[] args)
        {
            Console.WriteLine(name + "(" + string.Join(", ", args) + ")");
        }

        [ProxyMethod(TargetMethodType.Static)]
        public static void SayHello(string name, int something)
        {
            ////Entry("SayHello", new object[] { name, something });
            Console.WriteLine("AOP hello, {0}! {1}", name, something);
        }
    }
}