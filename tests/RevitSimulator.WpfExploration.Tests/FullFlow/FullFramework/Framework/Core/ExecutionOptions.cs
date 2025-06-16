namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;

public record ExecutionOptions
{
    public bool IsModeless { get; init; } = false;
    public bool RunOnUiThread { get; set; } = false;
};
