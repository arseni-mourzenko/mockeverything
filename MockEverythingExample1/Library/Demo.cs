namespace MockEverythingExample1.Library
{
    using System;
    using System.Net;

    public class Demo
    {
        public string FindName()
        {
            var response = new WebClient().DownloadString("http://example.com/");
            var prefix = "Hello, ";
            var suffix = "!";

            if (!response.StartsWith(prefix))
            {
                throw new NotImplementedException("The beginning of the response is invalid.");
            }

            if (!response.EndsWith(suffix))
            {
                throw new NotImplementedException("The ending of the response is invalid.");
            }

            return response.Substring(prefix.Length, response.Length - prefix.Length - suffix.Length);
        }
    }
}
