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

        public string Name { get; set; }

        public IEnumerable<IMethod> FindTypes(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
        {
            throw new NotImplementedException();
        }
    }
}
