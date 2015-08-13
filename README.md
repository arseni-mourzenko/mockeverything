# MockEverything

This library, currently in development, consists of a tampering mechanism which makes it possible to rewrite methods, getters, setters and constructors of third-party libraries used by the tested code.

Everyone has to deal with untested legacy code which was made with no architecture or design in mind. Such code has no abstractions whatsoever, and heavily depends on specific libraries or parts of the framework. It is impossible to unit test such code without refactoring it first. But, refactoring requires regression testing, so this is a common chicken or the egg problem: you can't test without refactoring, and you can't refactor without tests.

An elementary example would be a method which, somewhere deep in its business logic, makes a call to [`File.ReadAllText(string) : string`](https://msdn.microsoft.com/en-us/library/ms143368(v=vs.110).aspx) method. In order to test it, one needs to write contents to the specific file, which is not really the spirit of unit tests.

In the same way, many web applications expect `HttpRequest`, assume they run in a web context, and won't work if called by a unit test. And then, there is SharePoint. If you have seen legacy code of SharePoint web apps, you know how terrible they could be, and how are they impossible to test. When most methods do requests such as `SPContext.Current.Site.Url`, things become particularly hairy when it comes to unit testing.

Unfortunately, as I explained in [a related article](http://blog.pelicandd.com/article/91/tampering-sharepoint-assemblies-part-1), there are currently no Microsoft or third-party products which make it possible to unit test such code. There was Microsoft Fakes coupled with SharePoint.Emulators, but for .NET Framework 3.5 only. Also, the common techniques such as proxying or AOP are unable to solve this problem.

## Status

The library being in development, it will be released soon. Currently, [a working prototype](http://source.pelicandd.com/codebase/tampering/prototypes/TamperingForTests/) exists, and was successfully used on a legacy SharePoint project by making it possible to unit test a few methods. The approach used in this prototype is explained in [an article](http://blog.pelicandd.com/article/92/tampering-sharepoint-assemblies-part-2). Still, the protoype is not expected to be used, its only goal being to show the technical feasibility of the solution.

## Contributions

Contributions are welcome and can be made through the standard GitHub procedure. If you need additional information, please contact me at arseni.mourzenko@gmail.com.