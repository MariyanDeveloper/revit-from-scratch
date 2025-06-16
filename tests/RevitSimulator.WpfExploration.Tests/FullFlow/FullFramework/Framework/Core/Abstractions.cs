namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;

public interface IContentHost
{
    void HookupRegion(IViewsLookup views);
}

public interface INavigationTrigger
{
    event EventHandler<string>? RequestNavigation;
}

public interface IViewsLookup
{
    object GetView(string viewName);
}

public interface IViewPaging : IEnumerable<string>
{
    void Add(string viewName);
    string Next();
    string Previous();
}
