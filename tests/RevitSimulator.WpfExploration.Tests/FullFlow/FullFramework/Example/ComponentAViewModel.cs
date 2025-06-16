namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example;

public class ComponentAViewModel : Bindable
{
    private string _text;

    public string Text
    {
        get => _text;
        set
        {
            if (value == _text)
            {
                return;
            }
            _text = value;
            OnPropertyChanged();
        }
    }
}
