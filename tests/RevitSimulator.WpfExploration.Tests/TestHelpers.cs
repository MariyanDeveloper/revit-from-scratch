namespace RevitSimulator.WpfExploration.Tests;

public static class TestHelpers
{
    public static void ExecuteOnUiThread(Action action)
    {
        var thread = new Thread(() =>
        {
            action();
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
    }
}
