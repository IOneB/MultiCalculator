using System;
using System.Collections.Generic;
using System.Text;

namespace ClientLibraryTests
{
    class TestData
    {
        public const string name = "name";
        public const string value = "value";

        public const string Url = "http://localhost/";
        public const string ApiUrl = "/api/calculator";

        public const string queryParameters = "?name=Ivan";
        public const string jsonString = "{\"" + name + "\":\"Ivan\",\"" + value + "\":50}";

        public static Dictionary<string, string> Parameters { get; } = new Dictionary<string, string>
        {
            ["name"] = "name",
            ["value"] = "value",
            ["formulaString"] = "default"
        };
    }
}
