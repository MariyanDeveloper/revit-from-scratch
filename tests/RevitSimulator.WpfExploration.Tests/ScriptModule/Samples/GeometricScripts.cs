using Elements.Geometry;
using RevitSimulator.WpfExploration.Tests.ScriptModule.Core;

namespace RevitSimulator.WpfExploration.Tests.ScriptModule.Samples;

public static class GeometricScripts
{
    [Script("Gets center of a line")]
    public static Vector3 Center(Line line)
    {
        return line.PointAt(0.5);
    }
}
