using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace RevitSimulator.WpfExploration.Tests.Communication;

public record Position(double X, double Y, double Z);

public record WallMoved(Position NewPosition) : INotification;

public record ViewChanged(string NewViewId) : INotification;

public class WallMovedHandler : INotificationHandler<WallMoved>
{
    public Task Handle(WallMoved notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class AddStatisticsWallMovedHandler : INotificationHandler<WallMoved>
{
    public Task Handle(WallMoved notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public record View();

public interface IViewSource
{
    View GeyById(string id);
}

public class MockViewSource : IViewSource
{
    public View GeyById(string id)
    {
        return new View();
    }
}

public class ViewChangedHandler : INotificationHandler<ViewChanged>
{
    private readonly IViewSource _viewSource;

    public ViewChangedHandler(IViewSource viewSource)
    {
        _viewSource = viewSource ?? throw new ArgumentNullException(nameof(viewSource));
    }

    public Task Handle(ViewChanged notification, CancellationToken cancellationToken)
    {
        var view = _viewSource.GeyById(notification.NewViewId);
        return Task.CompletedTask;
    }
}

public class CommunicationUsingMediatR
{
    [Fact]
    public async Task ShouldShowCommunicationUsingMediatR()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddTransient<IViewSource, MockViewSource>();
        serviceCollection.AddTransient<INotificationHandler<WallMoved>, WallMovedHandler>();
        serviceCollection.AddTransient<
            INotificationHandler<WallMoved>,
            AddStatisticsWallMovedHandler
        >();
        serviceCollection.AddTransient<INotificationHandler<ViewChanged>, ViewChangedHandler>();
        var provider = serviceCollection.BuildServiceProvider();
        var mediator = new Mediator(provider);

        await mediator.Publish(new WallMoved(new Position(2, 3, 2)));
        await mediator.Publish(new ViewChanged("123"));
    }
}
