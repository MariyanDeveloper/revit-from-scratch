using System.Collections;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;

public class Paging<T> : IEnumerable<T>
{
    private readonly List<T> _items = [];

    private int _currentViewIndex = -1;

    public void Add(T item)
    {
        _items.Add(item);
        _currentViewIndex++;
    }

    public T Next()
    {
        var nextIndex = _currentViewIndex + 1;
        if (nextIndex >= _items.Count)
        {
            nextIndex = 0;
        }
        _currentViewIndex = nextIndex;
        return _items[nextIndex];
    }

    public T Previous()
    {
        var previousIndex = _currentViewIndex - 1;
        if (previousIndex < 0)
        {
            previousIndex = _items.Count - 1;
        }
        _currentViewIndex = previousIndex;
        return _items[previousIndex];
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
