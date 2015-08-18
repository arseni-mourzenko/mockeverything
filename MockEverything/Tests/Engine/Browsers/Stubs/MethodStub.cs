namespace MockEverythingTests.Engine.Browsers.Stubs
{
    using MockEverything.Inspection;

    public class MethodStub : IMethod
    {
        public MethodStub(string name, IType returnType = null)
        {
            this.Name = name;
            this.ReturnType = returnType ?? new TypeStub("MethodReturn") { FullName = "Stubs.MethodReturn" };
        }

        public string Name { get; private set; }

        public IType ReturnType { get; private set; }
    }
}
