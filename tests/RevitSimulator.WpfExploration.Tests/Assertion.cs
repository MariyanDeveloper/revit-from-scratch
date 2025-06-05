using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shouldly;

namespace RevitSimulator.WpfExploration.Tests;

public static class Assertion
{
    public static void ShouldMatchInlineSnapshot<T>(this T actual, string expected)
    {
        var json = JsonConvert.SerializeObject(
            actual,
            Formatting.Indented,
            new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            }
        );

        json.ShouldBe(expected);
    }
}
