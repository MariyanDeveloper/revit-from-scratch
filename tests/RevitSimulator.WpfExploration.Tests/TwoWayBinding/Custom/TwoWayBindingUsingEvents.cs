using Shouldly;

// ReSharper disable once CheckNamespace
namespace RevitSimulator.WpfExploration.Tests.TwoWayBinding.ImplementationFromScratch.TwoWayBindingUsingEvents;

public record PropertyChangedEventArgs(string PropertyName);

public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs eventArgs);

public interface INotifiable
{
    event PropertyChangedEventHandler? PropertyChanged;
}

public record Binding(string PropertyName, object Source);

public class DummyViewModel : INotifiable
{
    private string _firstName = string.Empty;

    public string FistName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FistName)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}

public class DummyTextControl
{
    private string _text = "";

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            TextChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? TextChanged;

    public void SetBinding(string controlPropertyName, Binding binding)
    {
        var controlType = GetType();

        var controlProperty =
            controlType.GetProperty(controlPropertyName)
            ?? throw new ArgumentException($"Property {controlPropertyName} not found on control");

        var viewModelProperty =
            binding.Source.GetType().GetProperty(binding.PropertyName)
            ?? throw new ArgumentException(
                $"Property {binding.PropertyName} not found on ViewModel"
            );

        var initialValue = viewModelProperty.GetValue(binding.Source);
        controlProperty.SetValue(this, initialValue);

        if (binding.Source is INotifiable notifier)
        {
            notifier.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName != binding.PropertyName)
                {
                    return;
                }
                var newValue = viewModelProperty.GetValue(binding.Source);

                if (controlProperty.GetValue(this) == newValue)
                {
                    return;
                }

                controlProperty.SetValue(this, newValue);
            };
        }

        var eventInfo = controlType.GetEvent($"{controlPropertyName}Changed");

        if (eventInfo?.EventHandlerType != typeof(EventHandler))
        {
            return;
        }

        var bindViewModelHandler = (object? sender, EventArgs? args) =>
        {
            var newControlValue = controlProperty.GetValue(this);
            if (viewModelProperty.GetValue(binding.Source) == newControlValue)
            {
                return;
            }
            viewModelProperty.SetValue(binding.Source, newControlValue);
        };

        eventInfo.AddEventHandler(this, new EventHandler(bindViewModelHandler));
    }
}

public class TwoWayBindingUsingEvents
{
    [Fact]
    public void ShouldSetDefaultValueDuringBinding()
    {
        var dummyViewModel = new DummyViewModel() { FistName = "Bob" };

        var dummyControl = new DummyTextControl();

        var binding = new Binding(
            PropertyName: nameof(DummyViewModel.FistName),
            Source: dummyViewModel
        );

        dummyControl.SetBinding(
            controlPropertyName: nameof(DummyTextControl.Text),
            binding: binding
        );

        dummyControl.Text.ShouldBe(
            "Bob",
            "Should be Bob since it's set a default value in the binding"
        );

        dummyViewModel.FistName = "Alice";
        dummyControl.Text.ShouldBe("Alice");
    }

    [Fact]
    public void ShouldUpdateControlWhenViewModelChanges()
    {
        var dummyViewModel = new DummyViewModel() { FistName = "Bob" };

        var dummyControl = new DummyTextControl();

        var binding = new Binding(
            PropertyName: nameof(DummyViewModel.FistName),
            Source: dummyViewModel
        );

        dummyControl.SetBinding(
            controlPropertyName: nameof(DummyTextControl.Text),
            binding: binding
        );

        dummyViewModel.FistName = "Alice";
        dummyControl.Text.ShouldBe("Alice");
    }

    [Fact]
    public void ShouldUpdateViewModelWhenControlChanges()
    {
        var dummyViewModel = new DummyViewModel() { FistName = "Bob" };

        var dummyControl = new DummyTextControl();

        var binding = new Binding(
            PropertyName: nameof(DummyViewModel.FistName),
            Source: dummyViewModel
        );

        dummyControl.SetBinding(
            controlPropertyName: nameof(DummyTextControl.Text),
            binding: binding
        );

        dummyControl.Text = "Alice";
        dummyViewModel.FistName.ShouldBe("Alice");
    }

    [Fact]
    public void ShouldSupportFullTwoWayBindingTwoWayBinding()
    {
        var dummyViewModel = new DummyViewModel() { FistName = "Bob" };

        var dummyControl = new DummyTextControl();

        var binding = new Binding(
            PropertyName: nameof(DummyViewModel.FistName),
            Source: dummyViewModel
        );

        dummyControl.SetBinding(
            controlPropertyName: nameof(DummyTextControl.Text),
            binding: binding
        );

        dummyControl.Text = "Alice";
        dummyViewModel.FistName.ShouldBe("Alice");

        dummyViewModel.FistName = "Martin";
        dummyControl.Text.ShouldBe("Martin");
    }
}
