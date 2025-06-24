using HelixToolkit.Wpf;
using Microsoft.CodeAnalysis;
using RevitSimulator.WpfExploration.Tests.Scripts.Core;

namespace RevitSimulator.WpfExploration.Tests.Scripts;

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
