namespace MockEverythingTests.Inspection
{
    using System;
    using System.Collections.Generic;
    using MockEverything.Inspection;

    internal class TypeStub : IType
    {
        private readonly ICollection<IMethod> methods;

        public TypeStub(string name, params IMethod[] methods)
        {
            this.Name = name;
            this.methods = methods;
        }

        public string FullName { get; set; }

        public string Name { get; private set; }

        public TAttribute FindAttribute<TAttribute>() where TAttribute : Attribute
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMethod> FindMethods(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
        {
            return this.methods;
        }
    }
}
