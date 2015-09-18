using MockEverything.Attributes;

namespace MockEverythingTests.Inspection.Demo
{
    public class SimpleClass
    {
        public void SimpleMethod()
        {
        }

        private void PrivateMethod()
        {
        }

        protected void ProtectedMethod()
        {
        }

        internal void InternalMethod()
        {
        }

        public static void StaticMethod()
        {
        }

        private static void PrivateStaticMethod()
        {
        }

        protected static void ProtectedStaticMethod()
        {
        }

        internal static void InternalStaticMethod()
        {
        }

        [Demo]
        [DemoSecond]
        public void DecoratedMethod()
        {
        }

        public string SimpleProperty { get; set; }

        public static string StaticProperty { get; set; }

        public string PropertyPrivateGetter { private get; set; }

        private string PrivateProperty { get; set; }

        [Demo]
        [DemoSecond]
        public string DecoratedProperty { get; set; }

        public string PropertyDecoratedGetter
        {
            [Demo]
            [DemoSecond]
            get;

            set;
        }

        public void GenericSample<T1, T2>(T1 hello, T2 world)
        {
        }

        public void GenericSampleWithConstraints<T1, T2>(T1 hello, T2 world) where T2 : System.Exception
        {
        }

        public void GenericSampleWithStructContraint<T1, T2>(T1 hello, T2 world) where T2 : struct
        {
        }

        public void GenericSampleNew<T1, T2>(T1 hello, T2 world) where T2 : new()
        {
        }

        public void GenericSampleMultiple<T1, T2>(T1 hello, T2 world) where T2 : System.IComparable, new()
        {
        }

        public void WithParameters(string one, int two)
        {
        }

        public void WithOutParameters(out string one)
        {
            throw new System.NotImplementedException();
        }

        public void WithRefParameters(ref string one)
        {
        }

        public void WithInfiniteParams(params string[] lines)
        {
        }

        [ProxyMethod(TargetMethodType.Instance)]
        public void MethodWithProxyAttribute()
        {
        }

        [ProxyMethod(TargetMethodType.Instance, name: "CustomName")]
        public void MethodWithNameInProxyAttribute()
        {
        }
    }
}
