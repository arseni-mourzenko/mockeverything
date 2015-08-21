# MockEverything

This library, currently in development, consists of a tampering mechanism which makes it possible to rewrite methods, getters, setters and constructors of third-party libraries used by the tested code.

Everyone has to deal with untested legacy code which was made with no architecture or design in mind. Such code has no abstractions whatsoever, and heavily depends on specific libraries or parts of the framework. It is impossible to unit test such code without refactoring it first. But, refactoring requires regression testing, so this is a common chicken or the egg problem: you can't test without refactoring, and you can't refactor without tests.

An elementary example would be a method which, somewhere deep in its business logic, makes a call to [`File.ReadAllText(string) : string`][1] method. In order to test it, one needs to write contents to the specific file, which is not really the spirit of unit tests.

In the same way, many web applications expect `HttpRequest`, assume they run in a web context, and won't work if called by a unit test. And then, there is SharePoint. If you have seen legacy code of SharePoint web apps, you know how terrible they could be, and how are they impossible to test. When most methods do requests such as `SPContext.Current.Site.Url`, things become particularly hairy when it comes to unit testing.

Unfortunately, as I explained in [a related article](http://blog.pelicandd.com/article/91/tampering-sharepoint-assemblies-part-1), there are currently no Microsoft or third-party products which make it possible to unit test such code. There was Microsoft Fakes coupled with SharePoint.Emulators, but for .NET Framework 3.5 only. Also, the common techniques such as proxying or AOP are unable to solve this problem.

## Design

The system has two parts which are called by a consumer: the discovery and the tampering.

A consumer could be a build task or a simple console application, or a system test. A consumer starts by interacting with the discovery in order to get the pairs which combine the proxy assemblies with the target ones. Then, for every pair, the tampering is called.

The tampering is done in four steps:

 1. ILMerge is called to merge the proxy and the target assemblies.

 1. The actual tampering is done, replacing the code within the target methods by the code of the corresponding proxy methods.

 1. If needed, the version of the resulting assembly is modified. This is especially useful when the target assembly comes from GAC: with [`bindingRedirect`][3], the local assembly can be used instead of the one in the GAC.

 1. The resulting assembly public key is altered. This step is necessary when dealing with signed assemblies. Although private key of the target assembly remains unknown, only the public key is needed, since its validation is not done when running code in full trust.

To do its job, tampering relies on the [browsers](https://github.com/MainMa/mockeverything/tree/master/MockEverything/Source/Engine/Browsers): one for the types, another one for the methods. Those browsers search the proxy assembly or type for possible proxies, and match them with the corresponding types or methods from the target assembly: the matching is done by [matching classes](https://github.com/MainMa/mockeverything/tree/master/MockEverything/Source/Engine/Matching).

In turn, browsers use the [inspection facade](https://github.com/MainMa/mockeverything/tree/master/MockEverything/Source/Inspection) to get the information they need from the assemblies. Currently, the only inspection facade uses Mono.Cecil. A similar facade can be created for System.Reflection.

The changing of the version and the public key is done directly by the inspection facade. In the same way, the facade handles the replacement of the method body by another.

## Status

The library being in development, it will be released soon. Currently, [a working prototype](http://source.pelicandd.com/codebase/tampering/prototypes/TamperingForTests/) exists, and was successfully used on a legacy SharePoint project by making it possible to unit test a few methods. The approach used in this prototype is explained in [an article](http://blog.pelicandd.com/article/92/tampering-sharepoint-assemblies-part-2). Still, the protoype is not expected to be used, its only goal being to show the technical feasibility of the solution.

## Limitations

The system contains the following limitations:

 1. [Type parameters covariance][2] is ignored when doing the matching between proxies and their targets. I've never seen it being used in .NET Framework or third-party libraries; therefore, I assume that it is unnecessary to make the code more difficult for such edge case.

 2. The access to the assemblies is done through .NET Framework's `System.IO` methods, which means the terrible [259-characters paths](http://stackoverflow.com/q/5188527/240613) limitation.

## Contributions

Contributions are welcome and can be made through the standard GitHub procedure. If you need additional information, please contact me at arseni.mourzenko@gmail.com.

[1]: https://msdn.microsoft.com/en-us/library/ms143368(v=vs.110).aspx
[2]: https://msdn.microsoft.com/en-us/library/vstudio/dd469487(v=vs.100).aspx
[3]: https://msdn.microsoft.com/en-us/library/eftw1fys(v=vs.110).aspx