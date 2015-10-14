namespace MockEverythingTests.AopProxies
{
    using System;
    using AopTarget;
    using MockEverything.Attributes;

    [ProxyOf(typeof(AopDemo))]
    public static class AopProxy
    {
        [EntryHook]
        public static void Entry(string className, string methodName, string methodSignature, object[] args)
        {
            Console.WriteLine(className + "." + methodName + " called with arguments {" + string.Join(", ", args) + "}");
        }

        [ProxyMethod(TargetMethodType.Static)]
        public static void SayHello(string name, int something)
        {
            Console.WriteLine("AOP hello, {0}! {1}", name, something);
        }
    }
}