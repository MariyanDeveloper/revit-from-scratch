using System.Windows.Controls;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example.Components;

public partial class ComponentB : UserControl
{
    public ComponentB(ComponentBViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
