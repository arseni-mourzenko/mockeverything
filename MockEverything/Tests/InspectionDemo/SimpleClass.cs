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
    }
}
