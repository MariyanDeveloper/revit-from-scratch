﻿using System.Windows;
using Autofac;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FromScratch.Core;

public class ContentManager : IContentManager
{
    private static readonly Dictionary<string, IContent> Contents = new();

    public static readonly DependencyProperty RegionNameProperty =
        DependencyProperty.RegisterAttached(
            "RegionName",
            typeof(string),
            typeof(ContentManager),
            new PropertyMetadata(null, OnRegionNameChanged)
        );

    private static void OnRegionNameChanged(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs e
    )
    {
        var content = new Content();
        var adapterType = typeof(IContentAdapter<>).MakeGenericType(dependencyObject.GetType());

        var adapter = ContainerContext.Container.ResolveOptional(adapterType);

        if (adapter is not null)
        {
            var adaptMethod = adapterType.GetMethod(nameof(IContentAdapter<object>.Adapt));
            adaptMethod?.Invoke(adapter, [content, dependencyObject]);
        }

        var regionName = e.NewValue.UnsafeCast<string>();

        if (!Contents.TryAdd(regionName, content))
        {
            throw new Exception($"Region {regionName} already exists");
        }
    }

    public static void SetRegionName(DependencyObject element, string value)
    {
        element.SetValue(RegionNameProperty, value);
    }

    public static string GetRegionName(DependencyObject element)
    {
        return (string)element.GetValue(RegionNameProperty);
    }

    public IContent FindContent(string regionName)
    {
        if (!Contents.TryGetValue(regionName, out var region))
        {
            throw new Exception($"Region {regionName} not found");
        }

        return region;
    }
}
