using System.Windows.Controls;
using RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Components;

public partial class ComponentA : UserControl, IHasViewModel
{
    public ComponentA(ComponentAViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        ViewModel = viewModel;
    }

    public object ViewModel { get; }
}
