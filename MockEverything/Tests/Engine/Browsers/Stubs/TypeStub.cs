namespace MockEverythingTests.Engine.Browsers.Stubs
{
    using System;
    using System.Collections.Generic;
    using MockEverything.Inspection;

    internal class TypeStub : IType
    {
        public TypeStub(string name)
        {
            this.Name = name;
        }

        public string FullName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name { get; set; }

        public TAttribute FindAttribute<TAttribute>() where TAttribute : Attribute
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IMethod> FindMethods(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
        {
            throw new NotImplementedException();
        }
    }
}
