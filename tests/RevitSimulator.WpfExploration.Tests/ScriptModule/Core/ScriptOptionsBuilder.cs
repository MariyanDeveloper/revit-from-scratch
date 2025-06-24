using Microsoft.CodeAnalysis;

namespace RevitSimulator.WpfExploration.Tests.ScriptModule.Core;

public class ScriptOptionsBuilder
{
    private static readonly string DefaultAssemblyName = "DefaultAssemblyName";

    public static ScriptOptions Default()
    {
        var references = new MetadataReferenceBuilder()
            .AddExecutingAssembly()
            .AddSystemAssemblies()
            .Build();

        return new ScriptOptions(DefaultAssemblyName, references);
    }

    public static ScriptOptions Create(
        string? assemblyName = null,
        IReadOnlyList<MetadataReference>? metadataReferences = null
    )
    {
        return new ScriptOptions(assemblyName ?? DefaultAssemblyName, metadataReferences ?? []);
    }
}
