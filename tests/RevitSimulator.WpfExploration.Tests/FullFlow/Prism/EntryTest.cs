using System.Windows;
using RevitSimulator.WpfExploration.Tests.FullFlow.Prism.Components;
using static RevitSimulator.WpfExploration.Tests.TestHelpers;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.Prism;

public class Bootstrapper : PrismBootstrapper
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterForNavigation<HomeView>(nameof(HomeView));
        containerRegistry.RegisterForNavigation<AboutView>(nameof(AboutView));
    }

    protected override void OnInitialized()
    {
        if (Shell is Window window)
        {
            window.ShowDialog();
        }
    }

    protected override DependencyObject CreateShell()
    {
        return Container.Resolve<MainShell>();
    }
}

public class EntryTest
{
    [Fact]
    public void ShouldFireUpAppUsingPrism()
    {
        ExecuteOnUiThread(() =>
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        });
    }
}
