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

        [ExitHook]
        public static void Exit(string className, string methodName, string methodSignature, object value)
        {
            Console.WriteLine(className + "." + methodName + " finished with value " + (value ?? "null"));
        }

        [ProxyMethod(TargetMethodType.Static)]
        public static string SayHello(string name, int something)
        {
            return string.Format("AOP hello, {0}! {1}", name, something);
        }

        [ProxyMethod(TargetMethodType.Static)]
        public static void DoStuff(string name)
        {
            Console.WriteLine("AOP direct hello, {0}!", name);
        }
    }
}