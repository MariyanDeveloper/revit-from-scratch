using System.Windows;
using Autofac;
using RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Components;
using RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;
using RevitSimulator.WpfExploration.Tests.RevitImpl.Views;
using static RevitSimulator.WpfExploration.Tests.TestHelpers;
using ComponentA = RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example.Components.ComponentA;
using ComponentB = RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example.Components.ComponentB;
using MainShell = RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example.Components.MainShell;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch;

public class MainShellViewModel : Bindable
{
    private readonly INavigationService _navigationService;
    public RelayCommand ComponentACommand { get; }
    public RelayCommand ComponentBCommand { get; }

    public MainShellViewModel(INavigationService navigationService)
    {
        _navigationService =
            navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        ComponentACommand = new RelayCommand(() =>
        {
            _navigationService.NavigateTo("SomeContent", typeof(ComponentA).FullName);
        });
        ComponentBCommand = new RelayCommand(() =>
        {
            _navigationService.NavigateTo("SomeContent", typeof(ComponentB).FullName);
        });
    }
}

public class ComponentAViewModel : Bindable, ICanBeNavigated
{
    public void OnNavigated(object args) { }
}

public class ComponentBViewModel : Bindable { }

public class SampleAppBootstrapper : Bootstrapper
{
    protected override void RegisterTypes(ContainerBuilder builder)
    {
        builder.RegisterType<MainShellViewModel>().SingleInstance();
        builder.RegisterType<MainShell>().AsSelf().SingleInstance();
        builder.RegisterType<ComponentAViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<ComponentBViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<ComponentA>().AsSelf().SingleInstance();
        builder.RegisterType<ComponentB>().AsSelf().SingleInstance();
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainShell>();
    }
}

public class EntryTest
{
    [Fact]
    public void ShouldFireUpAppUsingCustomFramework()
    {
        ExecuteOnUiThread(() =>
        {
            var customBootstrapper = new SampleAppBootstrapper();
            customBootstrapper.Run();
        });
    }
}
