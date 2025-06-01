using Shouldly;

// ReSharper disable once CheckNamespace
namespace RevitSimulator.WpfExploration.Tests.FromChatGPT.StackOverflowExceptionDemo;

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
}

public class StackOverflowExceptionDemo
{
    [Fact]
    public void ShouldCauseStackOverflowException()
    {
        var viewModel = new DummyViewModel();
        var control = new DummyControl();

        viewModel.FirstNameChanged += (_, value) =>
        {
            control.Text = value;
        };

        control.TextChanged += (_, value) =>
        {
            viewModel.FistName = value;
        };

        viewModel.FistName = "Bob";
        control.Text.ShouldBe("Bob");

        control.Text = "Alice";
        viewModel.FistName.ShouldBe("Alice");
    }
}
