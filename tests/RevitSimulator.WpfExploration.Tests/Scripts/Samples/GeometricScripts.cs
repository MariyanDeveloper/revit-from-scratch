using Elements.Geometry;
using RevitSimulator.WpfExploration.Tests.Scripts.Core;

namespace RevitSimulator.WpfExploration.Tests.Scripts.Samples;

public static class GeometricScripts
{
    [Script("Gets center of a line")]
    public static Vector3 Center(Line line)
    {
        return line.PointAt(0.5);
    }
}
