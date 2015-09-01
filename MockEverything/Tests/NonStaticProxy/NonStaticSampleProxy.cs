namespace NonStaticProxy
{
    using MockEverything.Attributes;

    [ProxyOf(typeof(string))]
    public class NonStaticSampleProxy
    {
    }
}