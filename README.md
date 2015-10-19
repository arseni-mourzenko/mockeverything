# MockEverything

The project is also [available on NuGet](https://www.nuget.org/packages/MockEverything/).

This library consists of a tampering mechanism which makes it possible to rewrite methods, getters, setters and constructors of third-party libraries used by the tested code.

Everyone has to deal with untested legacy code which was made with no architecture or design in mind. Such code has no abstractions whatsoever, and heavily depends on specific libraries or parts of the framework. It is impossible to unit test such code without refactoring it first. But, refactoring requires regression testing, so this is a common chicken or the egg problem: you can't test without refactoring, and you can't refactor without tests.

An elementary example would be a method which, somewhere deep in its business logic, makes a call to [`WebClient.DownloadString(Uri) : string`](https://msdn.microsoft.com/en-us/library/ms144200.aspx) method. In order to test it, one needs to rely on the actual web resources, which is not really the spirit of unit tests.

And then, there is SharePoint. If you have seen legacy code of SharePoint web apps, you know how terrible they could be, and how are they impossible to test. When most methods do requests such as `SPContext.Current.Site.Url`, things become particularly hairy when it comes to unit testing.

Unfortunately, as I explained in [a related article](http://blog.pelicandd.com/article/91/tampering-sharepoint-assemblies-part-1), there are currently no Microsoft or third-party products which make it possible to unit test such code. There was Microsoft Fakes coupled with SharePoint.Emulators, but for .NET Framework 3.5 only. Also, the common techniques such as proxying or AOP are unable to solve this problem.

## Terms

 - **Target**: the third-party library which will be subject to tampering. This could be either the assemblies of .NET Framework or any third-party assemblies that you can't alter directly by contributing to their source code.

 - **Proxy**: the assembly which contains the code to inject into the target assembly during the tampering.

 - **Exchanger**: the assembly which makes it possible to communicate between the proxy and the assembly which is calling the tampered target.

## Quick start

The following steps describe how MockEverything can be used to mock third-party code in a test project. [The sample](MockEverythingExample1) is available in this repository.

1. Download the library, compile and copy the binaries to a directory, for instance /externs/MockEverything.

1. Create the proxy project. The DLL should end by “.Proxies.dll” in order for it to be discovered. In order to change the name of the DLL, go to project properties, *Application* tab, and set *Assembly name* to end by “.Proxies”.

1. Create the exchanger project. It will be used to exchange information between the application which calls the third-party library, and the proxy. Since the code from a proxy is moved to the target library, the ability for a caller to communicate directly with the proxy is lost; for instance, setting a value of a public static property which belongs to the proxy won't do anything to the corresponding public static property of the tampered target. Instead, referencing a common exchanger from both the caller and the proxy makes it possible to share state and, in general, customize the behavior of the proxy code.

1. Create the sample library which will be used as a code which should be tested without being changed. We assume that this is some legacy code which should be tested. This library references the target assembly and uses it in a way which makes it difficult to test. For instance, it can download a file from internet, or interact with the environment in any other intrusive way. This library is unaware of the fact that the underlying target assembly will be tampered.

1. Create the test project.

1. From the proxy project, reference the target assembly, the exchanger and `MockEverything.Attributes.dll`.

1. From the test project, reference the sample library, the exchanger and the proxies.

1. Open test project .csproj file and add the following code to the end of the file, inside `<Project/>`:

    ```xml
    <UsingTask AssemblyFile="...\MockEverything.BuildTask.dll"
               TaskName="MockEverything.BuildTask.TamperingTask" />
    <Target Name="AfterBuild">
        <TamperingTask ProxiesPath="$(TargetDir)"
                       DestinationPath="$(TargetDir)"
                       CustomVersion="1.2.3.4" />
    </Target>
    ```

1. Replace ellipsis by the path which leads to the corresponding assembly.
  
If you are tampering an assembly which is available locally only, and is not deployed in the GAC:

1. Remove `CustomVersion` attribute.

If you are tampering an assembly which is installed in the GAC:

1. Replace 1.2.3.4 by an arbitrary version which is not the actual version of the target assembly.

1. Add App.config file to the test project. The file should contain this:

    ```xml
    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <runtime>
        <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
            <dependentAssembly>
            <assemblyIdentity name="..."
                              publicKeyToken="..."
                              culture="..." />
            <bindingRedirect oldVersion="1.0.0.0-1.1.0.0"
                             newVersion="1.2.3.4"/>
            </dependentAssembly>
        </assemblyBinding>
        </runtime>
    </configuration>
    ```

  Replace `name`, `publicKeyToken` and `culture` by the actual metadata of the target assembly. Change `oldVersion` so it contains the version of the target assembly, and set `newVersion` to the value you have set in the previous step.

## Entry and exit hooks

Sometimes, you may need to track the calls to the methods within the proxies. You can do it by using hooks. Those hooks consist of specific methods which are called either before invoking a proxy method (entry hooks), or before returning to the caller (exit hooks). Those hooks are declared through attributes `[Entry]` and `[Exit]`. They should have specific signatures to work.

The entry hook looks like this:

```csharp
[Entry]
public static void HookNameGoesHere(
    string className, string methodName, string signature, object[] args)
{
    ...
}
```

The method is called before executing the proxy method. It will receive the following arguments:

  - The full name of the class, that is the namespace and the class name. Note that **the name corresponds to the target, not the proxy**.

    Example: `Company.Project.Something`

  - The short name of the target method.

    Example: `FindProductName`

  - The signature of the method, which contains additional information about the method, such as the return value or the types of arguments. This can be used, for instance, in logs to determine which overload was actually called.

    Example: `System.String Company.Project.Something::FindProductName(System.Int32)`

  - The arguments passed to the method.

    Example: `123`

The exit hook looks like this:

```csharp
[Exit]
public static void HookNameGoesHere(
    string className, string methodName, string signature, object value)
{
    ...
}
```

The method is called before executing the proxy method. It will receive the following arguments:

  - The full name of the class, that is the namespace and the class name. Note that this information is about the target, not the proxy.
  - The short name of the target method.
  - The signature of the method, which contains additional information about the method, such as the return value or the types of arguments. This can be used, for instance, in logs to determine which overload was actually called.
  - The value returned by this method. If the method is `void`, the value will be `null`.

Note that since proxies are necessarily static and static classes don't support inheritance, hooks should be implemented on every proxy class. If the hooks are identical for several proxies, make sure you put the actual implementation of those hooks in a static class, called from the actual hooks within each proxy, in order to reduce code duplication.

## Exchangers

One issue I encountered when using MockEverything is when it comes to customizing a proxy from a given test. Let's imagine I'm testing business logic which loads a number from a database. MockEverything is used to tamper the data access layer by replacing the method which loads the actual number: its proxy just returns a constant. This works well if I need to test a single case (for example when the returned number is always equal to 1), but what if I want to test the business logic when the number is 0, or when it's 500?

Proxies are static. This means that whoever references an assembly containing a proxy can invoke methods within this proxy, which also means calling getters and setters. What if the unit test could simply use the proxy? Consider the following diagram:

![](MockEverything/Illustrations/1.png?raw=true)

here, we need to test `Lib.ShoppingCart.RefreshPrice()` which, unfortunately, relies on `Lib.ProductsStore.FindProduct()` which in turn interacts with the database. The solution is to create a proxy which overwrites the actual implementation of `Lib.ProductsStore.FindProduct()` by either returning a sample product every time the method is invoked with specific product IDs, or throws an exception when other IDs are passed to the method (simulating a case where the product doesn't exist).

**Test cases:**

  - Test `Lib.ShoppingCart.RefreshPrice()` when all products exist; we expect a price to correspond to the sum of the prices of the sample products sent by the proxy.
  
  - Test `Lib.ShoppingCart.RefreshPrice()` when some products in cart are reported as missing; we expect the exception to be handled by the shopping cart in a specific way.

During the tampering, the proxy is merged with the third-party assembly (the target).

![](MockEverything/Illustrations/1b.png?raw=true)

Now, when `FindProduct` refers to `existingIds`, does it refer to `existingIds` within the tampered `Lib.ProductsStore`? Absolutely not: `Lib.ProductsStore` doesn't contain any `existingIds`, which still resides in `Proxy.ProducsStore`. **This is exactly why all members of proxies are necessarily public:** otherwise, a proxy could compile, but fail at runtime when the methods copied to the target try to access private members of the proxy. Thus, the previous illustration is wrong.

Let's get back to the original problem of changing the data used by the proxies from within the tests. Now, we put the sequence of IDs not as a private field within the proxy class itself, but as a public property of a public class within the same assembly as the proxy class. Since everything is public, there will be no visibility issues after the tampering.

![](MockEverything/Illustrations/2.png?raw=true)

After the tampering:

![](MockEverything/Illustrations/2b.png?raw=true)

What if the unit test fills the sequence with specific values during its initialization, and then calls `RefreshPrice`? This won't work either. In fact, what happens is that the unit test assembly references the proxy assembly; what is actually used by the shopping cart class is the code within the tampered assembly. This mismatch means that the unit test will modify a property of a class within one assembly, while, through the third-party library, it's a different assembly which will be used. As a result, the changes to the sequence made directly by the unit test won't appear when accessing it through the tampered code.

This is better illustrated with the component diagram:

![](MockEverything/Illustrations/2c.png?raw=true)

There is no link between the proxy and the tampered third party; thus, when the unit test changes the values of the properties within the proxy, the third party is not affected by the changes.

The solution is to use a separate assembly to exchange information between the unit test and the tampered target:

![](MockEverything/Illustrations/3.png?raw=true)

After the tampering:

![](MockEverything/Illustrations/3b.png?raw=true)

What happens here is that when the code from the proxy `FindProduct` is copied to the target, both still reference the exchanger assembly—the same assembly in both cases. Affecting `ExistingIds` property from amywhere, would it be from the unit test or the proxy moved to the target, will lead to the change which will be global to all consumers. The component diagram shows the new relations:

![](MockEverything/Illustrations/3c.png?raw=true)

## Design

The system has two parts which are called by a consumer: the discovery and the tampering.

A consumer could be a build task or a simple console application, or a system test. A consumer starts by interacting with the discovery in order to get the pairs which combine the proxy assemblies with the target ones. Then, for every pair, the tampering is called.

The tampering is done in four steps:

 1. ILMerge is called to merge the proxy and the target assemblies.

 1. If needed, the version of the resulting assembly is modified. This is especially useful when the target assembly comes from GAC: with [`bindingRedirect`](https://msdn.microsoft.com/en-us/library/eftw1fys.aspx), the local assembly can be used instead of the one in the GAC.

 1. The resulting assembly public key is altered. This step is necessary when dealing with signed assemblies. Although private key of the target assembly remains unknown, only the public key is needed, since its validation is not done when running code in full trust.

 1. The actual tampering is done, replacing the code within the target methods by the code of the corresponding proxy methods.

To do its job, tampering relies on the [browsers](https://github.com/MainMa/mockeverything/tree/master/MockEverything/Source/Engine/Browsers): one for the types, another one for the methods. Those browsers search the proxy assembly or type for possible proxies, and match them with the corresponding types or methods from the target assembly: the matching is done by [matching classes](https://github.com/MainMa/mockeverything/tree/master/MockEverything/Source/Engine/Matching).

In turn, browsers use the [inspection facade](https://github.com/MainMa/mockeverything/tree/master/MockEverything/Source/Inspection) to get the information they need from the assemblies. Currently, the only inspection facade uses Mono.Cecil. A similar facade can be created for System.Reflection.

The changing of the version and the public key is done directly by the inspection facade. In the same way, the facade handles the replacement of the method body by another.

## Status

The library is currently in development. In can already be used for third-party projects, but lacks documentation and behaves badly when it comes to handling edge cases.

## Limitations

The system contains the following limitations:

 1. `mscorlib.dll` cannot be tampered because of the issue within [ILRepack](https://github.com/gluck/il-repack/issues/53). The [attempt](https://github.com/MainMa/il-repack) was made to modify ILRepack, but I encountered too many problems with the library for now.

 1. [Type parameters covariance](https://msdn.microsoft.com/en-us/library/vstudio/dd469487.aspx) is ignored when doing the matching between proxies and their targets. I've never seen it being used in .NET Framework or third-party libraries; therefore, I assume that it is unnecessary to make the code more difficult for such edge case.

 1. The access to the assemblies is done through .NET Framework's `System.IO` methods, which means the terrible [259-characters paths](http://stackoverflow.com/q/5188527/240613) limitation.

 1. Both proxy and target assemblies should be class libraries. Given that the primary goal of this project is to tamper third-party libraries, this limitation seems fair.

 1. For a given build task, the override version is set globally; every tampered assembly will have the same version.

 1. Proxies should not contain lambda expressions, since they are transformed into private methods by the compiler.

## See also

 - [The original article](http://blog.pelicandd.com/article/91/tampering-sharepoint-assemblies-part-1) where I explain that there are currently no Microsoft or third-party products which make it possible to do what this library does; this includes Microsoft Fakes with SharePoint.Emulators, proxying and AOP.

 - [A working prototype](http://source.pelicandd.com/codebase/tampering/prototypes/TamperingForTests/) was used to study the technical feasability of the approach. It was successfully used on a legacy SharePoint project by making it possible to unit test a few methods. The approach used in the prototype is explained in [an article](http://blog.pelicandd.com/article/92/tampering-sharepoint-assemblies-part-2).

 - [Why do my interceptors not called when using the proxy?](http://stackoverflow.com/q/31826983/240613) question on Stack Overflow showing why Castle DynamicProxy cannot be used as an alternative.

 - [How to do regression testing of code which relies heavily on SP... classes?](http://sharepoint.stackexchange.com/q/151289/40736) question on SharePoint.SE posted to see if there are actual alternatives I have missed.

 - [How do I ignore the assembly reference mismatch in unit tests?](http://stackoverflow.com/q/31900069/240613) question on Stack Overflow related to public key tokens in a context of a signed assembly tampering.

 - [When tampering an assembly, why can't I remove original instructions?](http://stackoverflow.com/q/31969111/240613) question on Stack Overflow I asked when working on the prototype, related to a problem with try/catch blocks within the tampered methods.

## Contributions

Contributions are welcome and can be made through GitHub forks. If you need additional information, please contact me at arseni.mourzenko@gmail.com.