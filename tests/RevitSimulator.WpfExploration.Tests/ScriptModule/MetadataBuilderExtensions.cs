using HelixToolkit.Wpf;
using RevitSimulator.WpfExploration.Tests.ScriptModule.Core;

namespace RevitSimulator.WpfExploration.Tests.ScriptModule;

public static class MetadataBuilderExtensions
{
    public static MetadataReferenceBuilder AddHelix(this MetadataReferenceBuilder builder)
    {
        return builder.AddAssemblies(
            typeof(HelixViewport3D).Assembly,
            typeof(System.Windows.Media.Color).Assembly,
            typeof(System.Windows.FrameworkElement).Assembly,
            typeof(System.Windows.Point).Assembly
        );
    }

    public static MetadataReferenceBuilder AddCoreGeometryAssemblies(
        this MetadataReferenceBuilder builder
    )
    {
        return builder.AddAssemblies(typeof(Elements.Geometry.Arc).Assembly);
    }
}
