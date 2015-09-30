using System;
namespace MockEverythingTests.Inspection.Demo
{
    public class PropertiesFromInterface : ISampleInterface
    {
        string ISampleInterface.Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
