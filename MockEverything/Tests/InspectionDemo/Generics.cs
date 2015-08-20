namespace MockEverythingTests.Inspection.Demo
{
    public class SimpleGeneric<T1>
    {
    }

    public class SimpleGeneric<T1, T2>
    {
    }

    public class GenericsWithConstraints<T1> where T1 : System.IComparable
    {
    }

    public class GenericsWithNewConstraint<T1> where T1 : System.IComparable, new()
    {
    }
}
