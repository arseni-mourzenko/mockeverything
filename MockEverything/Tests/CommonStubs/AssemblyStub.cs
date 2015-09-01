namespace MockEverythingTests.CommonStubs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using MockEverything.Inspection;

    public class AssemblyStub : IAssembly
    {
        private readonly string fullName;

        private readonly ICollection<IType> types;

        public AssemblyStub(string fullName, params IType[] types)
        {
            Contract.Requires(types != null);

            this.fullName = fullName;
            this.types = types;
        }

        public string FullName
        {
            get
            {
                return this.fullName;
            }
        }

        public string FilePath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IAssembly))
            {
                return false;
            }

            var other = (IAssembly)obj;
            return other.FullName == this.FullName;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static AssemblyStub FromNames(string fullName, params string[] types)
        {
            Contract.Requires(types != null);

            return new AssemblyStub(fullName, types.Select(t => (IType)new TypeStub(t, "Demo." + t)).ToArray());
        }

        public void AlterVersion(Version version)
        {
            throw new NotImplementedException();
        }

        public IType FindType(string fullName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IType> FindTypes(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
        {
            if (type == MemberType.Static)
            {
                return this.types;
            }

            return Enumerable.Empty<IType>();
        }

        public void ReplacePublicKey(IAssembly model)
        {
            throw new NotImplementedException();
        }

        public void Save(string path)
        {
            throw new NotImplementedException();
        }
    }
}
