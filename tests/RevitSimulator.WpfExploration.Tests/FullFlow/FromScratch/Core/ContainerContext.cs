using Autofac;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;

public static class ContainerContext
{
    private static IContainer? _container;

    public static IContainer Container
    {
        get => _container ?? throw new Exception("Container not set");
        private set => _container = value;
    }

    public static void SetContainer(IContainer container)
    {
        Container = container;
    }
}
