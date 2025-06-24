using System.IO;
using Elements.Geometry;
using RevitSimulator.WpfExploration.Tests.Scripts.Core;
using Shouldly;

namespace RevitSimulator.WpfExploration.Tests.Scripts;

delegate double CalculateSum(IEnumerable<double> numbers);

delegate Vector3 LineComputationFunction(Line line);

class DummyClass { }

public class ScriptFromFileTests
{
    private string SampleScriptByName(string scriptFileName) =>
        Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "Samples", scriptFileName);

    [Fact]
    public void FromFile_WhenNonDelegateTypeProvided_ThrowsArgumentException()
    {
        var scriptPath = SampleScriptByName("NumbersScripts.cs");

        var createScript = () =>
            Core.Scripts.FromFile<DummyClass>(Files.FromCSharpFile(scriptPath));

        createScript.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void FromFile_WithDefaultReferences_CanLoadSimpleNumericScripts()
    {
        var scriptPath = SampleScriptByName("NumbersScripts.cs");

        var scriptFile = Core.Scripts.FromFile<CalculateSum>(Files.FromCSharpFile(scriptPath));

        scriptFile.Scripts.Count.ShouldBe(2);
    }

    [Fact]
    public void FromFile_WithGeometryReferences_CanLoadGeometricScripts()
    {
        var scriptPath = SampleScriptByName("GeometricScripts.cs");
        var options = ScriptOptionsBuilder.Create(
            metadataReferences: new MetadataReferenceBuilder()
                .AddSystemAssemblies()
                .AddExecutingAssembly()
                .AddCoreGeometryAssemblies()
                .Build()
        );

        var scriptFile = Core.Scripts.FromFile<LineComputationFunction>(
            Files.FromCSharpFile(scriptPath),
            options
        );

        scriptFile.Scripts.Count.ShouldBe(1);
        scriptFile.Scripts.Single().Descriptor.Name.ShouldBe("Gets center of a line");
    }
}
