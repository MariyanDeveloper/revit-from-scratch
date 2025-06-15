namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;

public interface IContentManager
{
    IContent FindContent(string contentName);
}

public delegate void OnViewActivated(object view);

public interface ICanBeNavigated
{
    void OnNavigated(object args);
}

public interface INavigationService
{
    void NavigateTo(string regionName, string viewTypeName, object args = null);
}

public interface IContent
{
    event OnViewActivated OnActivated;
    IReadOnlyList<object> Views { get; }
    object ActiveView { get; }
    void Activate(object view);
    void Add(object view);
}

public interface IContentAdapter<T>
{
    void Adapt(IContent content, T targetContent);
}

public interface IHasViewModel
{
    object ViewModel { get; }
}
