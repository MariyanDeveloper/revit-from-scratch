using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RevitSimulator.WpfExploration.Tests.Scripts.Core;

public static class Scripts
{
    public static ScriptFile<TScriptFunction> FromFile<TScriptFunction>(
        File file,
        ScriptOptions? scriptOptions = null
    )
    {
        if (!typeof(TScriptFunction).IsSubclassOf(typeof(Delegate)))
        {
            throw new ArgumentException("TScriptFunction must be a delegate type");
        }
        scriptOptions ??= ScriptOptionsBuilder.Default();

        var rawScript = System.IO.File.ReadAllText(file.Path);

        var syntaxTree = CSharpSyntaxTree.ParseText(
            rawScript,
            new CSharpParseOptions(LanguageVersion.Latest)
        );

        var compilation = CSharpCompilation.Create(
            assemblyName: scriptOptions.AssemblyName,
            syntaxTrees: [syntaxTree],
            references: scriptOptions.MetadataReferences,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using var memoryStream = new MemoryStream();
        var result = compilation.Emit(memoryStream);

        if (!result.Success)
        {
            throw new InvalidOperationException("Compilation failed");
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(memoryStream.ToArray());

        var scripts = assembly
            .GetTypes()
            .SelectMany(type => type.GetMethods(BindingFlags.Static | BindingFlags.Public))
            .Select(method => (method, attribute: method.GetCustomAttribute<ScriptAttribute>()))
            .Where(methodAndAttribute => methodAndAttribute.attribute is not null)
            .Select(methodAndAttribute =>
            {
                var (method, attribute) = methodAndAttribute;
                var function = Delegate
                    .CreateDelegate(typeof(TScriptFunction), method)
                    .UnsafeCast<TScriptFunction>();

                return new Script<TScriptFunction>(function, new ScriptDescriptor(attribute!.Name));
            })
            .ToList();

        return new ScriptFile<TScriptFunction>(assembly, scripts);
    }
}
