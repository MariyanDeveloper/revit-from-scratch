using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using RevitSimulator.WpfExploration.Tests.TwoWayBinding.Wpf.Views;
using Shouldly;
using static RevitSimulator.WpfExploration.Tests.TestHelpers;

namespace RevitSimulator.WpfExploration.Tests.TwoWayBinding.Wpf;

public class TwoWayBindingWpf
{
    [Fact]
    public void TextBox_UpdatesViewModel_WithoutRequiringNotifyPropertyChanged()
    {
        ExecuteOnUiThread(() =>
        {
            var viewModel = new NotNotifiableSampleShellViewModel() { FirstName = "Bob" };
            var shell = new SampleShell(viewModel);
            shell.FirstNameTextBox.Text.ShouldBe("Bob");
            shell.FirstNameTextBox.Text = "Alice";
            viewModel.FirstName.ShouldBe("Alice");

            viewModel.FirstName = "Martin";

            shell.FirstNameTextBox.Text.ShouldBe(
                "Alice",
                "Should still be Alice because we don't notify from the view model"
            );
        });
    }

    [Fact]
    public void NotifiableViewModel_SupportsFullTwoWayBinding_WithShell()
    {
        ExecuteOnUiThread(() =>
        {
            var viewModel = new NotifiableSampleShellViewModel() { FirstName = "Bob" };
            var shell = new SampleShell(viewModel);
            viewModel.FirstName = "Alice";
            shell.FirstNameTextBox.Text.ShouldBe("Alice");

            shell.FirstNameTextBox.Text = "Martin";
            viewModel.FirstName.ShouldBe("Martin");
        });
    }

    [Fact]
    public void TextBox_SupportsFullTwoWayBinding_WithNotifiableViewModel()
    {
        ExecuteOnUiThread(() =>
        {
            var viewModel = new NotifiableSampleShellViewModel();
            var control = new TextBox();
            var binding = new Binding("FirstName")
            {
                Source = viewModel,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };

            control.SetBinding(TextBox.TextProperty, binding);

            control.Text = "Bob";
            viewModel.FirstName.ShouldBe("Bob");

            viewModel.FirstName = "Alice";
            control.Text.ShouldBe("Alice");
        });
    }
}
