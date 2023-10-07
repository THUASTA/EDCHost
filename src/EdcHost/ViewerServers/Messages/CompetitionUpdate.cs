using System.ComponentModel.Design.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EdcHost.ViewerServers.Messages;

public class CompetitionUpdate : ICompetitionUpdate
{
    [JsonPropertyName("messageType")]
    public string MessageType { get; }

    [JsonPropertyName("cameras")]
    public List<object> Cameras { get; }

    [JsonPropertyName("chunks")]
    public List<object> Chunks { get; }

    [JsonPropertyName("events")]
    public List<object> Events { get; }

    [JsonPropertyName("info")]
    public object Info { get; set; }

    [JsonPropertyName("mines")]
    public List<object> Mines { get; }

    [JsonPropertyName("players")]
    public List<object> Players { get; }

    public CompetitionUpdate()
    {
        MessageType = "COMPETITION_UPDATE";
        Cameras = new List<object>();
        Chunks = new List<object>();
        Events = new List<object>();
        Info = new object();
        Mines = new List<object>();
        Players = new List<object>();
    }

    [JsonConstructor]
    public CompetitionUpdate(
        string messageType,
        List<object> cameras,
        List<object> chunks,
        List<object> events,
        object info,
        List<object> mines,
        List<object> players
    ) => (MessageType, Cameras, Chunks, Events, Info, Mines, Players)
        = (messageType, cameras, chunks, events, info, mines, players);

    public byte[] SerializeToUtf8Bytes() => JsonSerializer.SerializeToUtf8Bytes(this);

    public string SerializeToString() => JsonSerializer.Serialize(this);
}
