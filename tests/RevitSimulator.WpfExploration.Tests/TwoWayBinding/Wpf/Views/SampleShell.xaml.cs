using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RevitSimulator.WpfExploration.Tests.TwoWayBinding.Wpf.Views;

public interface ISampleShellViewModel
{
    string FirstName { get; set; }
}

public class NotNotifiableSampleShellViewModel : ISampleShellViewModel
{
    public string FirstName { get; set; }
}

public class NotifiableSampleShellViewModel : ISampleShellViewModel, INotifyPropertyChanged
{
    private string _firstName;

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public partial class SampleShell : Window
{
    public SampleShell(ISampleShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        var binding = new Binding("FirstName")
        {
            Source = viewModel,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        FirstNameTextBox.SetBinding(TextBox.TextProperty, binding);
    }
}
