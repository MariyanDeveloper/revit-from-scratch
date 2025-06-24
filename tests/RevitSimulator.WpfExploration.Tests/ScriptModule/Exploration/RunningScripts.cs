using System.Reflection;
using DynamicExecution;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Shouldly;

namespace RevitSimulator.WpfExploration.Tests.ScriptModule.Exploration;

public class Application
{
    public bool IsOpened { get; private set; } = false;

    public void OpenFile()
    {
        IsOpened = true;
    }
}

public enum ExecutionResult
{
    Success,
    Failure,
}

public record ApiContext(Application Application);

public class RunningScripts
{
    private static ScriptOptions CreateDefaultScriptOptions() =>
        ScriptOptions
            .Default.WithImports(
                "System",
                "RevitSimulator.WpfExploration.Tests.ScriptModule.Exploration"
            )
            .WithReferences(Assembly.GetExecutingAssembly());

    [Fact]
    public async Task ExecuteScriptFromRawText_ShouldReturnSuccessAndOpenFile()
    {
        var apiContext = new ApiContext(new Application());

        var rawScript = """
            Application.OpenFile();
            return ExecutionResult.Success;
            """;

        apiContext.Application.IsOpened.ShouldBeFalse();

        var result = await ExecuteScript(rawScript, apiContext);

        result.ShouldBe(ExecutionResult.Success);
        apiContext.Application.IsOpened.ShouldBeTrue();
    }

    [Fact]
    public async Task ExtractAndExecuteScriptFromFile_ShouldReturnSuccessAndOpenFile()
    {
        var cSharpScriptFileContent = """
            public class SomeScripts
            {
                public static double ExecutePlugin(Application application)
                {
                    Application.OpenFile();
                    return ExecutionResult.Success;
                }
            }
            """;

        var apiContext = new ApiContext(new Application());

        apiContext.Application.IsOpened.ShouldBeFalse();

        var functionBody =
            CSharpSyntaxTree
                .ParseText(cSharpScriptFileContent)
                .GetCompilationUnitRoot()
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Single()
                .Body?.ToString() ?? throw new InvalidOperationException();

        var result = await ExecuteScript(functionBody, apiContext);

        result.ShouldBe(ExecutionResult.Success);
        apiContext.Application.IsOpened.ShouldBeTrue();
    }

    private async Task<ExecutionResult> ExecuteScript(string scriptCode, ApiContext context)
    {
        var script = CSharpScript.Create<ExecutionResult>(
            code: scriptCode,
            options: CreateDefaultScriptOptions(),
            globalsType: typeof(ApiContext)
        );
        script.Compile();
        var scriptState = await script.RunAsync(context);
        return scriptState.ReturnValue;
    }
}
