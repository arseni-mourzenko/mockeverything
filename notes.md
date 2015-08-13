# Notes

 - Prototype uses Reflection. Instead, use Mono.Cecil to do everything: this will make processing faster.

 - If the proxy haven't changed, there is no need to tamper the corresponding assembly. This will make unit tests much faster. Beware of the case where the MockEverything library itself changed.

 - Use specific names to identify proxy assemblies. `.Proxies.dll` suffix seems a good candidate (for instance, `Microsoft.SharePoint.Proxies.dll`).

 - One problem with the prototype is the usage of `this`. Since after tampering, `this` refers to a proxy from within the tampered code, this means that we can't, for instance, call private methods, but when writing code of the proxies, nothing indicates that we will encounter this error during runtime. I suggest to allow static methods only to be proxies, and decorate instance methods with specific attribute.

 - Merges should be completely independent, making it possible to do them in parallel (is it even possible?)

 - Log to a file. Output logging is too limited.

 - If the method is flagged as proxy but has no match in target assembly, consider this an error. It *is* an actual, very serious error, because it indicates that the author of the proxy have missed something (there is no way for a sane person to intentionnally proxy a method which doesn't exist).

 - A good way to determine that two methods are the same is that we can't put two of them in the same class. For example, methods which take different parameters are different, but methods which differ only by their visibility are the same.

# Examples to use

 - A library which calls `File.ReadAllText`.

 - SharePoint.

 - Some third-party library (to have an example of a library which is not in GAC).