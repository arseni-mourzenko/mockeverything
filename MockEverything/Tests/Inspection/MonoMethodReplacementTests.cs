namespace MockEverythingTests.Inspection
{
    using System;
    using System.Diagnostics.Contracts;
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection;
    using MockEverything.Inspection.MonoCecil;
    using Mono.Cecil;
    using Mono.Cecil.Cil;

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
            var source = this.GenerateMethod(
                Instruction.Create(OpCodes.Nop));

            var destination = this.GenerateMethod(
                Instruction.Create(OpCodes.Ldstr, "Hello, World!"),
                Instruction.Create(OpCodes.Ret));

            new Method(destination).ReplaceBody(new Method(source));

            Assert.AreEqual(1, destination.Body.Instructions.Count);
            Assert.AreEqual(OpCodes.Nop, destination.Body.Instructions[0].OpCode);
        }

        [TestMethod]
        public void TestReplaceBodyVariables()
        {
            var source = this.DemoMethodDefinition;
            var destination = this.DemoMethodDefinition;

            source.Body.Variables.Add(new VariableDefinition(this.FindTypeDefinitionOf<int>()));

            destination.Body.Variables.Add(new VariableDefinition(this.FindTypeDefinitionOf<Guid>()));
            destination.Body.Variables.Add(new VariableDefinition(this.FindTypeDefinitionOf<string>()));

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
            var source = this.GenerateMethod(
                Instruction.Create(OpCodes.Nop));

            var destination = this.GenerateMethod(
                Instruction.Create(OpCodes.Ldstr, "Hello, World!"),
                Instruction.Create(OpCodes.Ret));

            var entry = this.DemoMethodDefinition;
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.In, this.FindTypeReferenceOf<string>()));
            entry.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.In, this.FindTypeReferenceOf<object[]>()));

            new Method(destination).ReplaceBody(new Method(source), new Method(entry));

            Assert.IsTrue(destination.Body.Instructions.Count > 1);
            Assert.AreEqual(OpCodes.Ldstr, destination.Body.Instructions[0].OpCode);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryNonStatic()
        {
            var entry = this.DemoMethodDefinition;
            entry.Attributes = MethodAttributes.Public;
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.In, this.FindTypeReferenceOf<string>()));
            entry.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.In, this.FindTypeReferenceOf<object[]>()));

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryNonVoid()
        {
            var entry = this.DemoMethodDefinition;
            entry.ReturnType = this.FindTypeDefinitionOf<string>();
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.In, this.FindTypeReferenceOf<string>()));
            entry.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.In, this.FindTypeReferenceOf<object[]>()));

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryNonPublic()
        {
            var entry = this.DemoMethodDefinition;
            entry.Attributes = MethodAttributes.Private | MethodAttributes.Static;
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.In, this.FindTypeReferenceOf<string>()));
            entry.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.In, this.FindTypeReferenceOf<object[]>()));

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryNameMissing()
        {
            var entry = this.DemoMethodDefinition;
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.In, this.FindTypeReferenceOf<Guid>()));
            entry.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.In, this.FindTypeReferenceOf<object[]>()));

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryWrongNumberOfParameters()
        {
            var entry = this.DemoMethodDefinition;
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.In, this.FindTypeReferenceOf<string>()));

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryWrongSecondType()
        {
            var entry = this.DemoMethodDefinition;
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.In, this.FindTypeReferenceOf<string>()));
            entry.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.In, this.FindTypeReferenceOf<string>()));

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryWrongArray()
        {
            var entry = this.DemoMethodDefinition;
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.In, this.FindTypeReferenceOf<string>()));
            entry.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.In, this.FindTypeReferenceOf<int[]>()));

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntryFirstNotIn()
        {
            var entry = this.DemoMethodDefinition;
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.Out, this.FindTypeReferenceOf<string>()));
            entry.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.In, this.FindTypeReferenceOf<object[]>()));

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEntryException))]
        public void TestReplaceWithEntrySecondNotIn()
        {
            var entry = this.DemoMethodDefinition;
            entry.Parameters.Add(new ParameterDefinition("name", ParameterAttributes.In, this.FindTypeReferenceOf<string>()));
            entry.Parameters.Add(new ParameterDefinition("args", ParameterAttributes.Out, this.FindTypeReferenceOf<object[]>()));

            new Method(this.DemoMethodDefinition).ReplaceBody(new Method(this.DemoMethodDefinition), new Method(entry));
        }

        private MethodDefinition GenerateMethod(params Instruction[] instructions)
        {
            Contract.Requires(instructions != null);
            Contract.Ensures(Contract.Result<MethodDefinition>() != null);

            var method = this.DemoMethodDefinition;
            foreach (var instruction in instructions)
            {
                method.Body.Instructions.Add(instruction);
            }

            return method;
        }

        private MethodDefinition DemoMethodDefinition
        {
            get
            {
                var method = new MethodDefinition("SayHello", MethodAttributes.Public | MethodAttributes.Static, this.FindTypeDefinitionOf(typeof(void)));
                method.DeclaringType = this.FindTypeDefinitionOf<object>();
                return method;
            }
        }

        private TypeDefinition FindTypeDefinitionOf<T>()
        {
            Contract.Ensures(Contract.Result<TypeDefinition>() != null);

            return this.FindTypeDefinitionOf(typeof(T));
        }

        private TypeDefinition FindTypeDefinitionOf(System.Type type)
        {
            Contract.Ensures(Contract.Result<TypeDefinition>() != null);

            var result = this.FindTypeReferenceOf(type) as TypeDefinition;
            if (result == null)
            {
                throw new NotImplementedException("The discovery of the type failed. You can still get a `TypeReference` by calling `FindTypeReferenceOf<T>()` instead.");
            }

            return result;
        }

        private TypeReference FindTypeReferenceOf<T>()
        {
            Contract.Ensures(Contract.Result<TypeDefinition>() != null);

            return this.FindTypeReferenceOf(typeof(T));
        }

        private TypeReference FindTypeReferenceOf(System.Type type)
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

        private string FindAssemblyPathOf(System.Type type)
        {
            Contract.Requires(type != null);

            var codeBase = type.Assembly.CodeBase;
            return System.Uri.UnescapeDataString(new System.UriBuilder(codeBase).Path);
        }
    }
}
