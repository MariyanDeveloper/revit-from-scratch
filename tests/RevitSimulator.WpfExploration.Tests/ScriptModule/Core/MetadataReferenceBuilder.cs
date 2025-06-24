using System.Reflection;
using Microsoft.CodeAnalysis;

namespace RevitSimulator.WpfExploration.Tests.ScriptModule.Core;

public class MetadataReferenceBuilder
{
    private readonly List<MetadataReference> _references = new();

    public MetadataReferenceBuilder AddReferences(params MetadataReference[] reference)
    {
        _references.AddRange(reference);
        return this;
    }

    public MetadataReferenceBuilder AddAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            _references.Add(MetadataReference.CreateFromFile(assembly.Location));
        }

        return this;
    }

    public MetadataReferenceBuilder AddSystemAssemblies()
    {
        AddAssemblies(AppDomain.CurrentDomain.SystemAssemblies().ToArray());
        return this;
    }

    public MetadataReferenceBuilder AddExecutingAssembly()
    {
        AddAssemblies(Assembly.GetExecutingAssembly());
        return this;
    }

    public IReadOnlyList<MetadataReference> Build() =>
        _references.DistinctBy(reference => reference.Display).ToArray();
}
