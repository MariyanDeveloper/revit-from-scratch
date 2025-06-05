using Shouldly;
using Xunit.Abstractions;

// ReSharper disable once CheckNamespace
namespace RevitSimulator.WpfExploration.Tests.Communication.Callbacks;

public record Room(double Area);

public delegate void OnRoomAreaComputed(Room room, double factor, double area);

public class TheDependency
{
    public double ComputeAreas(
        IEnumerable<Room> rooms,
        OnRoomAreaComputed? onRoomAreaComputedCallback
    )
    {
        var businessLogicFactor = 1.2;
        return rooms.Sum(room =>
        {
            var area = room.Area * businessLogicFactor;
            onRoomAreaComputedCallback?.Invoke(room, businessLogicFactor, area);
            return area;
        });
    }
}

public record Progress(string Action, double PercentageDone)
{
    public static Progress Initial(string action) => new Progress(action, 0);

    public static Progress FromCompletionRatio(
        string action,
        double totalCount,
        double completedCount
    ) => new Progress(action, completedCount / totalCount * 100);
};

public class Callbacks
{
    [Fact]
    public void ShouldTrackProgressViaCallbacks()
    {
        var dependency = new TheDependency();
        var rooms = new List<Room>() { new Room(10), new Room(15) };

        var progresses = new List<Progress>();
        var totalCount = rooms.Count;
        var completedCount = 0;

        progresses.Add(Progress.Initial(action: "Beginning area calculation for all rooms"));

        var sum = dependency.ComputeAreas(
            rooms: rooms,
            onRoomAreaComputedCallback: (room, factor, computedArea) =>
            {
                completedCount++;
                var currentProgress = Progress.FromCompletionRatio(
                    action: $"Processing room {completedCount}: Area {room.Area} → {computedArea:F2} (factor: {factor})",
                    totalCount,
                    completedCount
                );
                progresses.Add(currentProgress);
            }
        );

        var expectedResult = """
            [
              {
                "action": "Beginning area calculation for all rooms",
                "percentageDone": 0.0
              },
              {
                "action": "Processing room 1: Area 10 → 12.00 (factor: 1.2)",
                "percentageDone": 50.0
              },
              {
                "action": "Processing room 2: Area 15 → 18.00 (factor: 1.2)",
                "percentageDone": 100.0
              }
            ]
            """;

        sum.ShouldBe(30);

        progresses.ShouldMatchInlineSnapshot(expectedResult);
    }
}
