namespace MockEverythingTests.Engine.Browsers.Stubs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using CommonStubs;
    using MockEverything.Inspection;

    internal class AssemblyStub : IAssembly
    {
        private readonly ICollection<IType> types;

        public AssemblyStub(params IType[] types)
        {
            Contract.Requires(types != null);

            this.types = types;
        }

        public static AssemblyStub FromNames(params string[] types)
        {
            Contract.Requires(types != null);

            return new AssemblyStub(types.Select(t => (IType)new TypeStub(t, "Demo." + t)).ToArray());
        }

        public IType FindType(string fullName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IType> FindTypes(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
        {
            return this.types;
        }
    }
}
