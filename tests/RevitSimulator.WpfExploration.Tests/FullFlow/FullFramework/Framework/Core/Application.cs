using System.Windows;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;

public class Application(
    Func<Window> createShell,
    ExecutionOptions executionOptions,
    IViewsLookup viewsLookup
)
{
    public void Run()
    {
        if (executionOptions.RunOnUiThread)
        {
            RunOnUiThread(RunInternal);
            return;
        }
        RunInternal();
    }

    private void RunInternal()
    {
        var shell = createShell();
        if (shell is IContentHost contentHost)
        {
            contentHost.HookupRegion(viewsLookup);
        }

        if (executionOptions.IsModeless)
        {
            shell.Show();
            return;
        }

        shell.ShowDialog();
    }

    private void RunOnUiThread(Action action)
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
