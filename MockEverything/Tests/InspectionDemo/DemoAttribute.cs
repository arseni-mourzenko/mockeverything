namespace MockEverythingTests.Inspection.Demo
{
    using System;

    public sealed class DemoAttribute : Attribute
    {
        public DemoAttribute(string text = null)
        {
            this.Text = text;
        }

        public string Text { get; private set; }
    }
}
