using System.Windows;
using RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;
using IContentHost = RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core.IContentHost;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example.Components;

public partial class MainShell : Window, IContentHost
{
    private readonly MainShellViewModel _viewModel;

    public MainShell(MainShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        _viewModel = viewModel;
        Loaded += (s, e) =>
        {
            viewModel.OnLoaded();
        };
    }

    public void HookupRegion(IViewsLookup views)
    {
        _viewModel.RequestNavigation += (sender, viewName) =>
        {
            var view = views.GetView(viewName);
            ContentRegion.Content = view;
        };
    }
}
