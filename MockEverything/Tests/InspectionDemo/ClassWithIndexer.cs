namespace MockEverythingTests.Inspection.Demo
{
    public class ClassWithIndexer
    {
        public string this[int i]
        {
            get
            {
                return i.ToString();
            }

            set
            {
            }
        }
    }
}
