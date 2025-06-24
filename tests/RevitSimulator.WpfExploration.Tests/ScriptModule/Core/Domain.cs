using System.Reflection;
using Microsoft.CodeAnalysis;

namespace RevitSimulator.WpfExploration.Tests.ScriptModule.Core;

public class ScriptAttribute(string Name) : Attribute
{
    public string Name { get; } = Name;
};

public record File(string Path)
{
    public static implicit operator string(File file) => file.Path;
};

public record ScriptDescriptor(string Name);

public record Script<TScriptFunction>(TScriptFunction Function, ScriptDescriptor Descriptor);

public record ScriptFile<TScriptFunction>(
    Assembly ContainingAssembly,
    IReadOnlyList<Script<TScriptFunction>> Scripts
);

public record ScriptOptions(
    string AssemblyName,
    IReadOnlyList<MetadataReference> MetadataReferences
);
