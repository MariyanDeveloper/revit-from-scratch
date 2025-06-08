using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using RevitSimulator.WpfExploration.Tests.ControlPlayground.Components;
using Shouldly;
using static RevitSimulator.WpfExploration.Tests.TestHelpers;

namespace RevitSimulator.WpfExploration.Tests.ControlPlayground;

public class TestItem : Bindable
{
    public string Name { get; set; }
}

public class TestViewModel : Bindable
{
    public ObservableCollection<TestItem> Items { get; set; } = [];
}

public class TextMockViewModel
{
    public string Name { get; set; }
}

public class BindingTests
{
    [Fact]
    public void ShouldBindItemSource()
    {
        ExecuteOnUiThread(() =>
        {
            var itemsControl = new ItemsControl();
            var viewModel = new TestViewModel();
            itemsControl.Bind(ItemsControl.ItemsSourceProperty, viewModel, (model) => model.Items);

            viewModel.Items.Add(new TestItem() { Name = "TestItem" });

            var itemsSource = itemsControl.ItemsSource.ShouldMatchType<
                ObservableCollection<TestItem>
            >();

            itemsSource.ShouldHaveSingleItem();
        });
    }

    [Fact]
    public void ShouldCorrectlyBindingItemSourceAndDataTemplate()
    {
        ExecuteOnUiThread(() =>
        {
            var itemsControl = new ItemsControl();
            var viewModel = new TestViewModel();

            itemsControl.Bind(ItemsControl.ItemsSourceProperty, viewModel, (model) => model.Items);

            var dataTemplateXaml = """
            <DataTemplate xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
                <TextBlock Text="{Binding Name}"/>
            </DataTemplate>
            """;

            var template = XamlReader.Parse(dataTemplateXaml).ShouldMatchType<DataTemplate>();
            itemsControl.ItemTemplate = template;

            viewModel.Items.Add(new TestItem() { Name = "SomeValue" });

            var itemsSource = itemsControl.ItemsSource.ShouldMatchType<
                ObservableCollection<TestItem>
            >();

            itemsSource.ShouldHaveSingleItem();
            var testItem = itemsControl.Items.Cast<TestItem>().ShouldHaveSingleItem();
            testItem.Name.ShouldBe("SomeValue");

            itemsControl.DataContext.ShouldBeNull();

            var testHost = new TestShell() { Content = itemsControl };
            testHost.Show();

            var itemContentPresenter = itemsControl
                .ItemContainerGenerator.ContainerFromItem(testItem)
                .ShouldMatchType<ContentPresenter>();

            var textBlock = VisualTreeHelper
                .GetChild(itemContentPresenter, 0)
                .ShouldMatchType<TextBlock>();

            var textBinding = textBlock.Binding(TextBlock.TextProperty);
            textBinding.Source.ShouldBeNull();
            textBinding.Path.Path.ShouldBe("Name");
            textBlock.DataContext.ShouldMatchType<TestItem>();
            textBlock.DataContext.ShouldBe(testItem);
        });
    }

    [Fact]
    public void ShouldBindCorrectly_WithExplicitSource()
    {
        ExecuteOnUiThread(() =>
        {
            var textBlock = new TextBlock();
            var viewModel = new TextMockViewModel() { Name = "SomeValue" };
            textBlock.Bind(TextBlock.TextProperty, viewModel, (model) => model.Name);

            textBlock.Text.ShouldBe("SomeValue");

            var binding = textBlock.Binding(TextBlock.TextProperty);
            binding.Path.Path.ShouldBe("Name");
            binding.Source.ShouldMatchType<TextMockViewModel>();
            binding.Source.ShouldBe(viewModel);
        });
    }

    [Fact]
    public void ShouldBindCorrectly_WithDataContextFallback()
    {
        ExecuteOnUiThread(() =>
        {
            var viewModel = new TextMockViewModel() { Name = "SomeValue" };
            var textBlock = new TextBlock() { DataContext = viewModel };
            textBlock.SetBinding(TextBlock.TextProperty, new Binding("Name"));
            textBlock.Text.ShouldBe("SomeValue");

            var binding = textBlock.Binding(TextBlock.TextProperty);
            binding.Path.Path.ShouldBe("Name");
            binding.Source.ShouldBeNull();
        });
    }
}
