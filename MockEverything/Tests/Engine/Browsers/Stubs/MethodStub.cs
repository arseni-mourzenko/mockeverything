namespace MockEverythingTests.Engine.Browsers.Stubs
{
    using MockEverything.Inspection;

    public class MethodStub : IMethod
    {
        public MethodStub(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
