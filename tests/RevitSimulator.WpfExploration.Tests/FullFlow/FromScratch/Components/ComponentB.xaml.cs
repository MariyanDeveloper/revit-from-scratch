using System.Windows.Controls;
using RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Components;

public partial class ComponentB : UserControl, IHasViewModel
{
    public ComponentB(ComponentBViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        ViewModel = viewModel;
    }

    public object ViewModel { get; }
}
