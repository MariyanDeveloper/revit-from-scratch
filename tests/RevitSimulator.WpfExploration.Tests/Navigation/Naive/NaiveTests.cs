using RevitSimulator.WpfExploration.Tests.Navigation.Naive.Components;
using static RevitSimulator.WpfExploration.Tests.TestHelpers;

namespace RevitSimulator.WpfExploration.Tests.Navigation.Naive;

public class NaiveTests
{
    [Fact]
    public void ShowSimplestImplementationWithTransientComponents()
    {
        ExecuteOnUiThread(() =>
        {
            var shell = new MainShell();
            var componentAButton = shell.ComponentAButton!;
            var componentBButton = shell.ComponentBButton!;
            var contentRegion = shell.ContentRegion!;

            componentAButton.Click += (sender, args) =>
            {
                contentRegion.Content = new ComponentA();
            };
            componentBButton.Click += (sender, args) =>
            {
                contentRegion.Content = new ComponentB();
            };

            shell.ShowDialog();
        });
    }

    [Fact]
    public void ShowSimplestImplementationWithSingletonComponents()
    {
        ExecuteOnUiThread(() =>
        {
            var shell = new MainShell();
            var componentAButton = shell.ComponentAButton!;
            var componentBButton = shell.ComponentBButton!;
            var contentRegion = shell.ContentRegion!;

            var componentA = new ComponentA();
            var componentB = new ComponentB();

            componentAButton.Click += (sender, args) =>
            {
                contentRegion.Content = componentA;
            };
            componentBButton.Click += (sender, args) =>
            {
                contentRegion.Content = componentB;
            };

            shell.ShowDialog();
        });
    }
}
