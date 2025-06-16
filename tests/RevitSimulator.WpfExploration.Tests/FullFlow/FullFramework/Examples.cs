using RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example;
using RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Example.Components;
using RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;
using Application = RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core.Application;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework;

public class Examples
{
    [Fact]
    public void Application_ShouldCreateAndRunShell_WithManualConfiguration()
    {
        var application = new Application(
            createShell: () =>
            {
                var viewPaging = new ViewPaging();
                var mainShellViewModel = new MainShellViewModel(viewPaging);
                var mainShell = new MainShell(mainShellViewModel);
                return mainShell;
            },
            executionOptions: new ExecutionOptions() { IsModeless = false, RunOnUiThread = true },
            viewsLookup: new DefaultViewsLookup(
                new Dictionary<string, Func<object>>()
                {
                    [ViewNames.ComponentA] = () =>
                        new ComponentA(
                            new ComponentAViewModel() { Text = "Hello from ComponentA" }
                        ),
                    [ViewNames.ComponentB] = () =>
                        new ComponentB(
                            new ComponentBViewModel() { Text = "Hello from ComponentB" }
                        ),
                }
            )
        );

        application.Run();
    }

    [Fact]
    public void ApplicationBuilder_ShouldCreateAndRunShell_WithFluentConfiguration()
    {
        var applicationBuilder = new ApplicationBuilder();

        var application = applicationBuilder
            .RegisterView(
                ViewNames.ComponentA,
                () => new ComponentA(new ComponentAViewModel() { Text = "Hello from Component A" })
            )
            .RegisterView(
                ViewNames.ComponentB,
                () => new ComponentB(new ComponentBViewModel() { Text = "Hello from Component B" })
            )
            .RegisterShell(() =>
            {
                var viewPaging = new ViewPaging();
                var mainShellViewModel = new MainShellViewModel(viewPaging);
                var mainShell = new MainShell(mainShellViewModel);
                return mainShell;
            })
            .Build(options: new ExecutionOptions() { IsModeless = false, RunOnUiThread = true });

        application.Run();
    }
}
