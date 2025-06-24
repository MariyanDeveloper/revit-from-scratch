using System.Reflection;

namespace RevitSimulator.WpfExploration.Tests.Scripts.Core;

public static class AssemblyLookupExtensions
{
    public static IEnumerable<Assembly> SystemAssemblies(this AppDomain domain)
    {
        var knownPrefixes = new[]
        {
            "System.",
            "Microsoft.",
            "netstandard",
            "WindowsBase",
            "PresentationCore",
        };
        return domain
            .AllNotDynamicAssemblies()
            .Where(x =>
                knownPrefixes.Any(knowPrefix =>
                {
                    var name = x.GetName().Name;
                    return name is not null && name.StartsWith(knowPrefix);
                })
            );
    }

    public static IEnumerable<Assembly> AllNotDynamicAssemblies(this AppDomain domain) =>
        domain.GetAssemblies().Where(NotDynamicWithLocation);

    public static IEnumerable<Assembly> TransitiveAssemblies(this Assembly assembly) =>
        assembly.GetReferencedAssemblies().Select(Assembly.Load).Where(NotDynamicWithLocation);

    public static bool NotDynamicWithLocation(this Assembly assembly) =>
        !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location);
}