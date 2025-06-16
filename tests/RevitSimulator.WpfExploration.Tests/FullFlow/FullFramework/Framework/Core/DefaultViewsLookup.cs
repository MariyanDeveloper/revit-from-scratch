namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;

public class DefaultViewsLookup(Dictionary<string, Func<object>> viewsMap) : IViewsLookup
{
    public object GetView(string viewName)
    {
        if (!viewsMap.TryGetValue(viewName, out var viewFactory))
        {
            throw new InvalidOperationException($"View {viewName} is not registered");
        }
        return viewFactory();
    }
}
