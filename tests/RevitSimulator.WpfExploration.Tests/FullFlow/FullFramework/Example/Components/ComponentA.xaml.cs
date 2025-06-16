using System.Windows.Controls;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example.Components;

public partial class ComponentA : UserControl
{
    public ComponentA(ComponentAViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
