namespace MockEverythingTests.CommonStubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MockEverything.Inspection;

    public class MethodStub : IMethod
    {
        public MethodStub(string name, IType returnType = null, IEnumerable<Parameter> parameters = null, IEnumerable<string> genericTypes = null, bool isPublic = true)
        {
            this.Name = name;
            this.ReturnType = returnType ?? new TypeStub("Void", "System.Void");
            this.Parameters = parameters ?? Enumerable.Empty<Parameter>();
            this.GenericTypes = genericTypes ?? Enumerable.Empty<string>();
            this.IsPublic = isPublic;
        }

        public IEnumerable<string> GenericTypes { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<Parameter> Parameters { get; private set; }

        public IType ReturnType { get; private set; }

        public bool IsPublic { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IMethod))
            {
                return false;
            }

            var other = (IMethod)obj;
            return
                this.Name == other.Name &&
                this.ReturnType.Equals(other.ReturnType) &&
                this.Parameters.SequenceEqual(other.Parameters) &&
                this.GenericTypes.SequenceEqual(other.GenericTypes);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public void ReplaceBody(IMethod other)
        {
            throw new NotImplementedException();
        }
    }
}
