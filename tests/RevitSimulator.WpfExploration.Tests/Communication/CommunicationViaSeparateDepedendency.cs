using Shouldly;

namespace RevitSimulator.WpfExploration.Tests.Communication;

public class Events
{
    private readonly Dictionary<string, List<Action<object>>> _eventsMap = new();

    public void Subscribe<TArgs>(string name, Action<TArgs> action)
    {
        Action<object> castedAction = (obj) => action((TArgs)obj);
        if (_eventsMap.TryGetValue(name, out var actions))
        {
            actions.Add(castedAction);
            return;
        }
        _eventsMap.Add(name, [castedAction]);
    }

    public void Notify<TArgs>(string name, TArgs args)
    {
        if (!_eventsMap.TryGetValue(name, out var actions))
        {
            return;
        }
        foreach (var action in actions)
        {
            action(args);
        }
    }
}

public record NameChanged();

public static class EventKeys
{
    public const string OnFirstNameChanged = "OnFirstNameChanged";
}

public class ComponentA
{
    private readonly Events _events;
    public string SomeValue { get; set; }

    public ComponentA(Events events)
    {
        _events = events ?? throw new ArgumentNullException(nameof(events));
        _events.Subscribe<NameChanged>(
            EventKeys.OnFirstNameChanged,
            (args) =>
            {
                SomeValue = "Bob";
            }
        );
    }
}

public class ComponentB
{
    private readonly Events _events;

    public ComponentB(Events events)
    {
        _events = events ?? throw new ArgumentNullException(nameof(events));
    }

    public void SomeOperation()
    {
        _events.Notify(EventKeys.OnFirstNameChanged, new NameChanged());
    }
}

public class CommunicationViaSeparateDepedendency
{
    [Fact]
    public void ShouldShowCommunicationViaSeparateDependency()
    {
        var events = new Events();
        var componentA = new ComponentA(events);
        var componentB = new ComponentB(events);

        componentB.SomeOperation();

        componentA.SomeValue.ShouldBe("Bob");
    }
}
