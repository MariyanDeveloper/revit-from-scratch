namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;

public class Content : IContent
{
    private readonly List<object> _views = new();
    public object ActiveView { get; private set; }

    public event OnViewActivated? OnActivated;
    public IReadOnlyList<object> Views => _views;

    public void Activate(object view)
    {
        ActiveView = view;
        OnActivated?.Invoke(view);
    }

    public void Add(object view)
    {
        _views.Add(view);
    }
}
