namespace MockEverythingTests.Engine.Browsers.Stubs
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
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

            return new AssemblyStub(types.Select(t => (IType)new TypeStub(t)).ToArray());
        }

        public IEnumerable<IType> FindTypes(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
        {
            return this.types;
        }
    }
}
