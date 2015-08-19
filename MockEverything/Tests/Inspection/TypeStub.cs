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

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            var other = (Type)obj;
            return other.FullName == this.FullName;
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
