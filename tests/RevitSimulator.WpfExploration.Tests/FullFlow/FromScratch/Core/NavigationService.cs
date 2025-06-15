using Autofac;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;

public class NavigationService(IContentManager contentManager, ILifetimeScope scope)
    : INavigationService
{
    public void NavigateTo(string regionName, string viewTypeName, object args = null)
    {
        var viewType = Type.GetType(viewTypeName);
        if (viewType == null)
        {
            throw new Exception($"View type {viewTypeName} not found");
        }

        var view = scope.Resolve(viewType);
        var content = contentManager.FindContent(regionName);
        content.Activate(view);

        if (view is not IHasViewModel hasViewModel)
        {
            return;
        }

        if (hasViewModel.ViewModel is ICanBeNavigated canBeNavigated)
        {
            canBeNavigated.OnNavigated(args);
        }
    }
}
