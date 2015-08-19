namespace MockEverythingTests.Engine.Browsers.Stubs
{
    using System.Collections.Generic;
    using System.Linq;
    using MockEverything.Inspection;

    public class MethodStub : IMethod
    {
        public MethodStub(string name, IType returnType = null, IEnumerable<Parameter> parameters = null, IEnumerable<string> genericTypes = null)
        {
            this.Name = name;
            this.ReturnType = returnType ?? new TypeStub("MethodReturn") { FullName = "Stubs.MethodReturn" };
            this.Parameters = parameters ?? Enumerable.Empty<Parameter>();
            this.GenericTypes = genericTypes ?? Enumerable.Empty<string>();
        }

        public IEnumerable<string> GenericTypes { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<Parameter> Parameters { get; private set; }

        public IType ReturnType { get; private set; }
    }
}
