using System.Collections.Generic;
using System.Linq;
using RevitSimulator.WpfExploration.Tests.Scripts.Core;

namespace RevitSimulator.WpfExploration.Tests.Scripts.Samples;

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
