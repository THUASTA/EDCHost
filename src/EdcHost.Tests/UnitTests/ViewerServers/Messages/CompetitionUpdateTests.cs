using System.Text.Json;
using EdcHost.ViewerServers.Messages;
using Xunit;

namespace EdcHost.Tests.UnitTests.ViewerServers.Messages;
public class CompetitionUpdateTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        // // Arrange
        // const string expectedMessageType = "COMPETITION_UPDATE";
        // var expectedCameras = new List<object>();
        // var expectedChunks = new List<object>();
        // var expectedEvents = new List<object>();
        // var expectedInfo = new List<object>();
        // var expectedMines = new List<object>();
        // var expectedPlayers = new List<object>();

        // // Act
        // var competitionUpdate = new CompetitionUpdate(
        //     expectedMessageType,
        //     expectedCameras,
        //     expectedChunks,
        //     expectedEvents,
        //     expectedInfo,
        //     expectedMines,
        //     expectedPlayers
        // );

        // Assert
        // Assert.Equal(expectedMessageType, competitionUpdate.MessageType);
        // Assert.Same(expectedCameras, competitionUpdate.Cameras);
        // Assert.Same(expectedChunks, competitionUpdate.Chunks);
        // Assert.Same(expectedEvents, competitionUpdate.Events);
        // Assert.Same(expectedInfo, competitionUpdate.Info);
        // Assert.Same(expectedMines, competitionUpdate.Mines);
        // Assert.Same(expectedPlayers, competitionUpdate.Players);
    }

    [Fact]
    public void SerializeToUtf8Bytes_Roundtrip()
    {
        // Arrange
        var originalUpdate = new CompetitionUpdate();
        string expectedJson = JsonSerializer.Serialize(originalUpdate);

        // Act
        byte[] bytes = originalUpdate.SerializeToUtf8Bytes();
        CompetitionUpdate? deserializedUpdate = JsonSerializer.Deserialize<CompetitionUpdate>(bytes);
        Assert.NotNull(deserializedUpdate);
        string deserializedJson = deserializedUpdate.SerializeToString();

        // Assert
        Assert.Equal(expectedJson, deserializedJson);
    }

    [Fact]
    public void SerializeToString_Roundtrip()
    {
        // Arrange
        var originalUpdate = new CompetitionUpdate();
        string expectedJson = JsonSerializer.Serialize(originalUpdate);

        // Act
        string jsonString = originalUpdate.SerializeToString();
        CompetitionUpdate? deserializedUpdate = JsonSerializer.Deserialize<CompetitionUpdate>(jsonString);
        Assert.NotNull(deserializedUpdate);
        string deserializedJson = deserializedUpdate.SerializeToString();

        // Assert
        Assert.Equal(expectedJson, deserializedJson);
    }
}
