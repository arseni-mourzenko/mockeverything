namespace MockEverythingTests.CommonStubs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using MockEverything.Inspection;

    public class TypeStub : IType
    {
        private readonly ICollection<IMethod> methods;

        public TypeStub(string name, string fullName, params IMethod[] methods)
        {
            Contract.Requires(name != null);
            Contract.Requires(!name.Contains("."));
            Contract.Requires(fullName != null);
            Contract.Requires(name.Contains("."));

            this.Name = name;
            this.FullName = fullName;
            this.methods = methods;
            this.GenericTypes = Enumerable.Empty<string>();
        }

        public string FullName { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<string> GenericTypes { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IType))
            {
                return false;
            }

            var other = (IType)obj;
            return this.FullName == other.FullName && this.GenericTypes.SequenceEqual(other.GenericTypes);
        }

        public override int GetHashCode()
        {
            return 0;
        }

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
