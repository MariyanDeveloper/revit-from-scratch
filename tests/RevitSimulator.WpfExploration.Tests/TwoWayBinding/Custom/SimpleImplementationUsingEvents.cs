using Shouldly;

// ReSharper disable once CheckNamespace
namespace RevitSimulator.WpfExploration.Tests.FromChatGPT.SimpleImplementation;

public delegate void StringPropertyChangedEventHandler(object sender, string value);

public class DummyViewModel
{
    private string _firstName = string.Empty;

    public string FistName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            FirstNameChanged?.Invoke(this, _firstName);
        }
    }

    public event StringPropertyChangedEventHandler? FirstNameChanged;
}

public class DummyControl
{
    public event StringPropertyChangedEventHandler? TextChanged;
    private string _text = string.Empty;

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            TextChanged?.Invoke(this, value);
        }
    }

    public void SetTextBinding(DummyViewModel viewModel)
    {
        viewModel.FirstNameChanged += (_, value) =>
        {
            if (Text == value)
            {
                return;
            }
            Text = value;
        };

        TextChanged += (_, value) =>
        {
            if (viewModel.FistName == value)
            {
                return;
            }
            viewModel.FistName = value;
        };
    }
}

public class SimpleImplementationUsingEvents
{
    [Fact]
    public void ShouldShowSimpleBindingBehavior()
    {
        var viewModel = new DummyViewModel();
        var control = new DummyControl();

        control.SetTextBinding(viewModel);
        viewModel.FistName = "Bob";
        control.Text.ShouldBe("Bob");

        control.Text = "Alice";
        viewModel.FistName.ShouldBe("Alice");
    }
}
