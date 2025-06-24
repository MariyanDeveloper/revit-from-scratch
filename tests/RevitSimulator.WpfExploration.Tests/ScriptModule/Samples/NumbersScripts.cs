using RevitSimulator.WpfExploration.Tests.ScriptModule.Core;

namespace RevitSimulator.WpfExploration.Tests.ScriptModule.Samples;

public class NumbersScripts
{
    [Script("Gets the sum of the numbers")]
    public static double Sum(IEnumerable<double> numbers)
    {
        return numbers.Sum();
    }

    [Script("Gets largest number in the numbers")]
    public static double LargestValue(IEnumerable<double> numbers)
    {
        return numbers.OrderDescending().First();
    }

    public static double DummyFunction() => 2;
}
