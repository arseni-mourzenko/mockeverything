namespace MockEverythingTests.Inspection.Demo
{
    public class ClassWithIndexers
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

        public string this[string i]
        {
            get
            {
                return i;
            }

            set
            {
            }
        }
    }
}
