using Shouldly;

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

    public static T ShouldMatchType<T>(this object any)
    {
        any.ShouldBeOfType<T>();
        return (T)any;
    }

    public static T UnsafeCast<T>(this object any) => (T)any;
}
