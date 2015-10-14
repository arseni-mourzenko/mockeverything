namespace MockEverythingTests.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection;
    using MockEverything.Inspection.MonoCecil;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using System.Linq;

    [TestClass]
    public class MonoMethodReplacementTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestReplaceBodyWrongType()
        {
            new Method(this.DemoMethodDefinition).ReplaceBody(new MethodStub("Hello"));
        }

        [TestMethod]
        public void TestReplaceBodyInstructions()
        {
            var source = new MethodBuilder("SayHello")
                .AddInstruction(OpCodes.Nop)
                .Build();

            var destination = new MethodBuilder("SayHello")
                .AddInstruction(OpCodes.Ldstr, "Hello, World!")
                .AddInstruction(OpCodes.Ret)
                .Build();

            new Method(destination).ReplaceBody(new Method(source));

            Assert.AreEqual(1, destination.Body.Instructions.Count);
            Assert.AreEqual(OpCodes.Nop, destination.Body.Instructions[0].OpCode);
        }

        [TestMethod]
        public void TestReplaceBodyVariables()
        {
            var source = this.DemoMethodDefinition;
            var destination = this.DemoMethodDefinition;

            source.Body.Variables.Add(new VariableDefinition(new TypeDiscovery().FindTypeDefinitionOf<int>()));

            destination.Body.Variables.Add(new VariableDefinition(new TypeDiscovery().FindTypeDefinitionOf<Guid>()));
            destination.Body.Variables.Add(new VariableDefinition(new TypeDiscovery().FindTypeDefinitionOf<string>()));

            new Method(destination).ReplaceBody(new Method(source));

            Assert.AreEqual(1, destination.Body.Variables.Count);
            Assert.AreEqual(typeof(int).FullName, destination.Body.Variables[0].VariableType.FullName);
        }

        [TestMethod]
        public void TestReplaceBodyExceptionHandlers()
        {
            var source = this.DemoMethodDefinition;
            var destination = this.DemoMethodDefinition;

            source.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Filter));

            destination.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Catch));
            destination.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally));

            new Method(destination).ReplaceBody(new Method(source));

            Assert.AreEqual(1, destination.Body.ExceptionHandlers.Count);
            Assert.AreEqual(ExceptionHandlerType.Filter, destination.Body.ExceptionHandlers[0].HandlerType);
        }

        [TestMethod]
        public void TestReplaceWithEntry()
        {
            var source = new MethodBuilder("SayHello")
                .AddInstruction(OpCodes.Nop)
                .Build();

            var destination = new MethodBuilder("SayHello")
                .AddInstruction(OpCodes.Ldstr, "Hello, World!")
                .AddInstruction(OpCodes.Ret)
                .Build();

            var entry = new MethodBuilder("SayHello")
                .AddParameter<string>("name")
                .AddParameter<object[]>("args")
                .Build();

            new Method(destination).ReplaceBody(new Method(source), new Method(entry));

            Assert.IsTrue(destination.Body.Instructions.Count > 1);
            Assert.AreEqual(OpCodes.Ldstr, destination.Body.Instructions[0].OpCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryNonStatic()
        {
            var entry = new MethodBuilder("SayHello")
                .MakeInstance()
                .AddParameter<string>("name")
                .AddParameter<object[]>("args")
                .Build();

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryNonVoid()
        {
            var entry = new MethodBuilder("SayHello")
                .Returns<string>()
                .AddParameter<string>("name")
                .AddParameter<object[]>("args")
                .Build();

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryNonPublic()
        {
            var entry = new MethodBuilder("SayHello")
                .MakePrivate()
                .AddParameter<string>("name")
                .AddParameter<object[]>("args")
                .Build();

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryNameMissing()
        {
            var entry = new MethodBuilder("SayHello")
                .AddParameter<Guid>("name")
                .AddParameter<object[]>("args")
                .Build();

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryWrongNumberOfParameters()
        {
            var entry = new MethodBuilder("SayHello")
                .AddParameter<string>("name")
                .Build();

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryWrongSecondType()
        {
            var entry = new MethodBuilder("SayHello")
                .AddParameter<string>("name")
                .AddParameter<string>("args")
                .Build();

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryWrongArray()
        {
            var entry = new MethodBuilder("SayHello")
                .AddParameter<string>("name")
                .AddParameter<int[]>("args")
                .Build();

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryFirstNotIn()
        {
            var entry = new MethodBuilder("SayHello")
                .AddParameter<string>("name", ParameterAttributes.Out)
                .AddParameter<object[]>("args")
                .Build();

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntrySecondNotIn()
        {
            var entry = new MethodBuilder("SayHello")
                .AddParameter<string>("name")
                .AddParameter<object[]>("args", ParameterAttributes.Out)
                .Build();

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        private class TypeDiscovery
        {
            public TypeDefinition FindTypeDefinitionOf<T>()
            {
                Contract.Ensures(Contract.Result<TypeDefinition>() != null);

                return this.FindTypeDefinitionOf(typeof(T));
            }

            public TypeDefinition FindTypeDefinitionOf(System.Type type)
            {
                Contract.Ensures(Contract.Result<TypeDefinition>() != null);

                var result = this.FindTypeReferenceOf(type) as TypeDefinition;
                if (result == null)
                {
                    throw new NotImplementedException("The discovery of the type failed. You can still get a `TypeReference` by calling `FindTypeReferenceOf<T>()` instead.");
                }

                return result;
            }

            public TypeReference FindTypeReferenceOf<T>()
            {
                Contract.Ensures(Contract.Result<TypeDefinition>() != null);

                return this.FindTypeReferenceOf(typeof(T));
            }

            public TypeReference FindTypeReferenceOf(System.Type type)
            {
                Contract.Requires(type != null);
                Contract.Ensures(Contract.Result<TypeDefinition>() != null);

                var module = ModuleDefinition.ReadModule(this.FindAssemblyPathOf(type));

                if (type.IsArray)
                {
                    var inner = this.FindTypeReferenceOf(type.GetElementType());
                    return (TypeReference)new ArrayType(inner);
                }

                var result = module.GetType(type.FullName);
                return result;
            }

            public string FindAssemblyPathOf(System.Type type)
            {
                Contract.Requires(type != null);

                var codeBase = type.Assembly.CodeBase;
                return System.Uri.UnescapeDataString(new System.UriBuilder(codeBase).Path);
            }
        }

        private class MethodBuilder
        {
            private readonly string name;

            private readonly TypeDefinition returnType;

            private readonly MethodAttributes attributes;

            private readonly ICollection<ParameterDefinition> parameters;

            private readonly ICollection<Instruction> instructions;

            public MethodBuilder(string name) : this(name, MethodAttributes.Public | MethodAttributes.Static, null, null, null)
            {
            }

            private MethodBuilder(string name, MethodAttributes attributes, TypeDefinition returnType, ICollection<ParameterDefinition> parameters, ICollection<Instruction> instructions)
            {
                this.name = name;
                this.attributes = attributes;
                this.returnType = returnType ?? new TypeDiscovery().FindTypeDefinitionOf(typeof(void));
                this.parameters = parameters ?? new List<ParameterDefinition>();
                this.instructions = instructions ?? new List<Instruction>();
            }

            public MethodBuilder Returns(TypeDefinition type)
            {
                return new MethodBuilder(this.name, this.attributes, type, this.parameters, this.instructions);
            }

            public MethodBuilder Returns(System.Type type)
            {
                return this.Returns(new TypeDiscovery().FindTypeDefinitionOf(type));
            }

            public MethodBuilder Returns<T>()
            {
                return this.Returns(typeof(T));
            }

            public MethodBuilder MakePrivate()
            {
                return new MethodBuilder(
                    this.name,
                    (this.attributes & ~MethodAttributes.Public) & MethodAttributes.Private,
                    this.returnType,
                    this.parameters,
                    this.instructions);
            }

            public MethodBuilder MakeInstance()
            {
                return new MethodBuilder(
                    this.name,
                    this.attributes & ~MethodAttributes.Static,
                    this.returnType,
                    this.parameters,
                    this.instructions);
            }

            public MethodBuilder AddParameter(ParameterDefinition parameter)
            {
                return new MethodBuilder(this.name, this.attributes, this.returnType, this.parameters.Concat(new[] { parameter }).ToList(), this.instructions);
            }

            public MethodBuilder AddParameter(string name, TypeReference type, ParameterAttributes attributes = ParameterAttributes.None)
            {
                return this.AddParameter(new ParameterDefinition(name, attributes, type));
            }

            public MethodBuilder AddParameter(string name, System.Type type, ParameterAttributes attributes = ParameterAttributes.None)
            {
                return this.AddParameter(name, new TypeDiscovery().FindTypeReferenceOf(type), attributes);
            }

            public MethodBuilder AddParameter<T>(string name, ParameterAttributes attributes = ParameterAttributes.None)
            {
                return this.AddParameter(name, typeof(T), attributes);
            }

            public MethodBuilder AddInstruction(Instruction instruction)
            {
                return new MethodBuilder(this.name, this.attributes, this.returnType, this.parameters, this.instructions.Concat(new[] { instruction }).ToList());
            }

            public MethodBuilder AddInstruction(OpCode opcode)
            {
                return this.AddInstruction(Instruction.Create(opcode));
            }

            public MethodBuilder AddInstruction(OpCode opcode, string value)
            {
                return this.AddInstruction(Instruction.Create(opcode, value));
            }

            public MethodDefinition Build()
            {
                var result = new MethodDefinition(this.name, this.attributes, this.returnType);
                foreach (var parameter in this.parameters)
                {
                    result.Parameters.Add(parameter);
                }

                foreach (var instruction in this.instructions)
                {
                    result.Body.Instructions.Add(instruction);
                }

                result.DeclaringType = new TypeDiscovery().FindTypeDefinitionOf<object>();
                return result;
            }
        }

        private MethodDefinition DemoMethodDefinition
        {
            get
            {
                return new MethodBuilder("SayHello")
                    .AddParameter<string>("name")
                    .AddParameter<object[]>("args")
                    .Build();
            }
        }
    }
}