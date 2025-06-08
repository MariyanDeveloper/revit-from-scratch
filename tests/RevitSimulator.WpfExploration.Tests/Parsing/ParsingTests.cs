using System.Windows.Controls;
using System.Xaml;
using System.Xaml.Schema;
using RevitSimulator.WpfExploration.Tests.Parsing.Components;
using Shouldly;
using static RevitSimulator.WpfExploration.Tests.TestHelpers;
using XamlParseException = System.Windows.Markup.XamlParseException;
using XamlReader = System.Windows.Markup.XamlReader;

namespace RevitSimulator.WpfExploration.Tests.Parsing;

public class ParsingTests
{
    [Fact]
    public void ShouldMapCorrectNamespaceToButton()
    {
        ExecuteOnUiThread(() =>
        {
            var buttonXaml = """
            <Button 
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            Content="Click me"
            Background="LightGray"/>
            """;

            var button = XamlReader.Parse(buttonXaml).UnsafeCast<Button>();

            button.ShouldNotBeNull();
            button.Content.ShouldBe("Click me");
        });
    }

    [Fact]
    public void ShouldFailWithIncorrectNamespace()
    {
        ExecuteOnUiThread(() =>
        {
            var buttonXaml = """
            <Button 
            xmlns="http://incorrect.namespace/presentation"
            Content="Click me"/>
            """;

            Should.Throw<XamlParseException>(() => XamlReader.Parse(buttonXaml));
        });
    }

    [Fact]
    public void ShouldResolveTypeWithXamlSchemaContext()
    {
        ExecuteOnUiThread(() =>
        {
            var context = new XamlSchemaContext();
            var wpfNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

            var buttonType = context.GetXamlType(new XamlTypeName(wpfNamespace, "Button"));

            buttonType.ShouldNotBeNull();
            buttonType.UnderlyingType.ShouldBe(typeof(Button));
        });
    }

    [Fact]
    public void ShouldResolveCustomNamespace()
    {
        ExecuteOnUiThread(() =>
        {
            var xaml = """
            <local:SomeComponent 
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            xmlns:local="clr-namespace:RevitSimulator.WpfExploration.Tests.Parsing.Components;assembly=RevitSimulator.WpfExploration.Tests"/>
            """;

            var control = XamlReader.Parse(xaml);

            control.ShouldNotBeNull();
            control.ShouldMatchType<RevitImpl.Views.SomeControl>();
        });
    }

    [Fact]
    public void ResolveFullComponent()
    {
        ExecuteOnUiThread(() =>
        {
            var userControlXaml = """
            <UserControl
                         xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                         xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                         xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
                         xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
                         xmlns:local="clr-namespace:RevitSimulator.WpfExploration.Tests.Parsing.Components;assembly=RevitSimulator.WpfExploration.Tests"
                         mc:Ignorable="d"
                         d:DesignHeight="300" d:DesignWidth="300">
                <Grid>
                    <local:SomeComponent/>
                </Grid>
            </UserControl>
            """;

            var userControl = XamlReader.Parse(userControlXaml).ShouldMatchType<UserControl>();
            var grid = userControl.Content.ShouldMatchType<Grid>();
            grid.Children.OfType<SomeComponent>().ShouldHaveSingleItem();
        });
    }
}
