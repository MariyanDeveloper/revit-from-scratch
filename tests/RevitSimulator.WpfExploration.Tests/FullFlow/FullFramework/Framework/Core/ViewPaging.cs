using System.Collections;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;

public class ViewPaging : IViewPaging
{
    private readonly Paging<string> _viewPaging = new();

    public void Add(string viewName)
    {
        _viewPaging.Add(viewName);
    }

    public string Next()
    {
        return _viewPaging.Next();
    }

    public string Previous()
    {
        return _viewPaging.Previous();
    }

    public IEnumerator<string> GetEnumerator()
    {
        return _viewPaging.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
