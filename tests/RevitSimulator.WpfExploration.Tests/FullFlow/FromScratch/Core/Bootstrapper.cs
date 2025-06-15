using System.Windows;
using System.Windows.Controls;
using Autofac;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;

public abstract class Bootstrapper
{
    protected abstract void RegisterTypes(ContainerBuilder builder);

    protected abstract Window CreateShell();
    protected IContainer Container { get; set; }

    public void Run()
    {
        var containerBuilder = new ContainerBuilder();
        RegisterDefaults(containerBuilder);
        RegisterTypes(containerBuilder);
        var container = containerBuilder.Build();
        Container = container;
        ContainerContext.SetContainer(container);
        var shell = CreateShell();
        shell.ShowDialog();
    }

    private void RegisterDefaults(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<ContentManager>().As<IContentManager>().SingleInstance();

        containerBuilder
            .RegisterType<NavigationService>()
            .As<INavigationService>()
            .SingleInstance();

        containerBuilder
            .RegisterType<ContentControlAdapter>()
            .As<IContentAdapter<ContentControl>>()
            .SingleInstance();
    }
}
