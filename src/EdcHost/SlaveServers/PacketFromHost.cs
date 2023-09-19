namespace EdcHost.SlaveServers;

public class PacketFromHost : IPacket
{
    public int GameStage { get; }
    public int ElapsedTime { get; }
    public List<int> HeightOfChunks { get; } = new List<int>();
    public bool HasBed { get; }
    public float PositionX { get; }
    public float PositionY { get; }
    public float PositionOpponentX { get; }
    public float PositionOpponentY { get; }
    public int Agility { get; }
    public int Health { get; }
    public int MaxHealth { get; }
    public int Strength { get; }
    public int EmeraldCount { get; }
    public int WoolCount { get; }

    public byte[] MakePacket()
    {
        // TODO: add a serializer
        throw new NotImplementedException();
    }

    public void ExtractPacketData(byte[] bytes)
    {
        // TODO: add a deserializer
    }
}
