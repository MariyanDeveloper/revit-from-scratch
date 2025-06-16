using System.Windows;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;

public class ApplicationBuilder
{
    private readonly Dictionary<string, Func<object>> _views = [];
    private Func<Window>? _createShell;

    public ApplicationBuilder RegisterView(string name, Func<object> createUiElement)
    {
        var lazyFactory = createUiElement.Lazy();
        _views.TryAdd(name, () => lazyFactory.Value);
        return this;
    }

    public ApplicationBuilder RegisterShell(Func<Window> createShell)
    {
        _createShell = createShell;
        return this;
    }

    public Application Build(ExecutionOptions? options = null)
    {
        var viewsLookup = new DefaultViewsLookup(_views);
        options ??= new ExecutionOptions();
        if (_createShell is null)
        {
            throw new InvalidOperationException("Shell has not been initialized.");
        }

        return new Application(_createShell, options, viewsLookup);
    }
}
