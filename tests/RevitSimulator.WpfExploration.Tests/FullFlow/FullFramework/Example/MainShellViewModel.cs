using RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;
using RevitSimulator.WpfExploration.Tests.RevitImpl.Views;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example;

public class MainShellViewModel : Bindable, INavigationTrigger
{
    private readonly IViewPaging _viewPaging;
    public RelayCommand ComponentACommand { get; }
    public RelayCommand ComponentBCommand { get; }
    public RelayCommand NextCommand { get; }
    public RelayCommand PreviousCommand { get; }
    public bool CanGoBackOrForward => _viewPaging.Count() > 1;

    public event EventHandler<string>? RequestNavigation;

    public MainShellViewModel(IViewPaging viewPaging)
    {
        _viewPaging = viewPaging;

        ComponentACommand = new RelayCommand(() =>
        {
            NavigateTo(ViewNames.ComponentA);
        });

        ComponentBCommand = new RelayCommand(() =>
        {
            NavigateTo(ViewNames.ComponentB);
        });

        NextCommand = new RelayCommand(
            () =>
            {
                var nextView = viewPaging.Next();
                RaiseRequestNavigation(nextView);
            },
            canExecute: (_) => CanGoBackOrForward
        );

        PreviousCommand = new RelayCommand(
            () =>
            {
                var previousView = viewPaging.Previous();
                RaiseRequestNavigation(previousView);
            },
            canExecute: (_) => CanGoBackOrForward
        );
    }

    public void OnLoaded()
    {
        RaiseRequestNavigation(ViewNames.ComponentA);
    }

    private void NavigateTo(string viewName)
    {
        RaiseRequestNavigation(viewName);
        _viewPaging.Add(viewName);
        NextCommand.RaiseCanExecuteChanged();
        PreviousCommand.RaiseCanExecuteChanged();
    }

    private void RaiseRequestNavigation(string viewName)
    {
        RequestNavigation?.Invoke(this, viewName);
    }
}
