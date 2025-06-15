using System.Windows.Controls;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;

public class ContentControlAdapter : IContentAdapter<ContentControl>
{
    public void Adapt(IContent content, ContentControl targetContent)
    {
        content.OnActivated += (view) =>
        {
            targetContent.Content = view;
        };
    }
}
