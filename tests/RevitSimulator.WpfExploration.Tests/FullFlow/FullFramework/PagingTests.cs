using RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework.Framework.Core;
using Shouldly;

namespace RevitSimulator.WpfExploration.Tests.FullFlow.FullFramework;

public class PagingTests
{
    [Fact]
    public void Add_ShouldIncreaseItemCount()
    {
        var paging = new Paging<string>();
        paging.Add("item1");
        paging.Count().ShouldBe(1);
    }

    [Fact]
    public void Next_ShouldThrowException_WhenPagingIsEmpty()
    {
        var paging = new Paging<string>();
        var nextFunction = () => paging.Next();
        nextFunction.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Navigation_ShouldCycleThrough_AllAddedItems()
    {
        var paging = new Paging<string>();
        paging.Add("item1");
        paging.Add("item2");

        var first = paging.Next();
        first.ShouldNotBeNull();
        first.ShouldBe("item1");

        var second = paging.Next();
        second.ShouldNotBeNull();
        second.ShouldBe("item2");

        var previous = paging.Previous();
        previous.ShouldNotBeNull();
        previous.ShouldBe("item1");
    }

    [Fact]
    public void Next_ShouldWrapAround_WhenReachingEnd()
    {
        var paging = new Paging<string>();
        paging.Add("item1");
        paging.Add("item2");

        paging.Next();
        paging.Next();

        var wrappedItem = paging.Next();
        wrappedItem.ShouldBe("item1");
    }

    [Fact]
    public void Previous_ShouldWrapAround_WhenReachingBeginning()
    {
        var paging = new Paging<string>();
        paging.Add("item1");
        paging.Add("item2");

        paging.Next(); // item1

        var wrappedItem = paging.Previous();
        wrappedItem.ShouldBe("item2");
    }
}
