using System.Linq.Expressions;
using System.Windows;
using System.Windows.Data;

namespace RevitSimulator.WpfExploration.Tests;

public static class ControlHelpers
{
    public static TControl Bind<TControl, TViewModel, TProperty>(
        this TControl control,
        DependencyProperty targetProperty,
        TViewModel viewModel,
        Expression<Func<TViewModel, TProperty>> propertySelector
    )
        where TControl : FrameworkElement
    {
        var propertyName = propertySelector.Name();
        var binding = new Binding(propertyName)
        {
            Source = viewModel,
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };

        control.SetBinding(targetProperty, binding);

        return control;
    }

    public static Binding Binding(
        this FrameworkElement frameworkElement,
        DependencyProperty targetProperty
    ) =>
        frameworkElement.GetBindingExpression(targetProperty)?.ParentBinding
        ?? throw new Exception("Binding not found");

    private static string Name<TViewModel, TProperty>(
        this Expression<Func<TViewModel, TProperty>> propertySelector
    )
    {
        if (propertySelector.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new ArgumentException(
            "Expression must be a member access expression",
            nameof(propertySelector)
        );
    }
}
