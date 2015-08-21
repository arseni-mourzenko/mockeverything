namespace MockEverythingTests.Inspection
{
    using System;
    using CommonStubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MockEverything.Inspection.MonoCecil;
    using Demo;
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
            var source = this.DemoMethodDefinition;
            var destination = this.DemoMethodDefinition;

            var sourceProcessor = source.Body.GetILProcessor();
            source.Body.Instructions.Add(sourceProcessor.Create(OpCodes.Nop));

            var destionationProcessor = destination.Body.GetILProcessor();
            destination.Body.Instructions.Add(destionationProcessor.Create(OpCodes.Ldstr, "Hello, World!"));
            destination.Body.Instructions.Add(destionationProcessor.Create(OpCodes.Ret));

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

        private MethodDefinition DemoMethodDefinition
        {
            get
            {
                return new MethodDefinition("SayHello", MethodAttributes.Public, this.FindTypeDefinitionOf<string>());
            }
        }

        private TypeDefinition FindTypeDefinitionOf<T>()
        {
            var module = ModuleDefinition.ReadModule(this.FindAssemblyPathOf<int>());
            return module.GetType(typeof(T).FullName);
        }

        private string FindAssemblyPathOf<T>()
        {
                var codeBase = typeof(T).Assembly.CodeBase;
                return System.Uri.UnescapeDataString(new System.UriBuilder(codeBase).Path);
        }
    }
}
