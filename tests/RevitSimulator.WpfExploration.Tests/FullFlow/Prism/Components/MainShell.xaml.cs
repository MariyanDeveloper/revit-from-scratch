using System.Windows;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.Prism.Components;

public partial class MainShell : Window
{
    private readonly IRegionManager _regionManager;

    public MainShell(IRegionManager regionManager)
    {
        _regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
        InitializeComponent();
    }

    private void NavigateHome(object sender, RoutedEventArgs e)
    {
        _regionManager.RequestNavigate("MainRegion", nameof(HomeView));
    }

    private void NavigateAbout(object sender, RoutedEventArgs e)
    {
        _regionManager.RequestNavigate("MainRegion", nameof(AboutView));
    }
}
