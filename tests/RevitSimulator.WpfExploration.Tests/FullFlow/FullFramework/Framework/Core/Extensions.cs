namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;

public static class Extensions
{
    public static Lazy<T> Lazy<T>(this Func<T> func) => new Lazy<T>(func);
}
