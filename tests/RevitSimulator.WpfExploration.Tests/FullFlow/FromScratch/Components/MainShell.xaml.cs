using System.Windows;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Components;

public partial class MainShell : Window
{
    public MainShell(MainShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
