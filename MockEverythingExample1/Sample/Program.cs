namespace MockEverythingExample1.Sample
{
    using Exchanger;
    using Library;

    public static class Program
    {
        public static void Main()
        {
            WebClientExchanger.PersonName = "Jeff";
            var actual = new Demo().FindName();
        }
    }
}