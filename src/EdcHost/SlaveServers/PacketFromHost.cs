namespace EdcHost.SlaveServers;

public class PacketFromHost : IPacketFromHost
{
    const int PACKET_LENGTH = 100;
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

    public PacketFromHost(
        int gameStage, int elapsedTime, List<int> heightOfChunks, bool hasBed,
        float positionX, float positionY, float positionOpponentX, float positionOpponentY,
        int agility, int health, int maxHealth, int strength,
        int emeraldCount, int woolCount
        )
    {
        GameStage = gameStage;
        ElapsedTime = elapsedTime;
        HeightOfChunks = new(heightOfChunks);
        HasBed = hasBed;
        PositionX = positionX;
        PositionY = positionY;
        PositionOpponentX = positionOpponentX;
        PositionOpponentY = positionOpponentY;
        Agility = agility;
        Health = health;
        MaxHealth = maxHealth;
        Strength = strength;
        EmeraldCount = emeraldCount;
        WoolCount = woolCount;
    }

    public byte[] MakePacket()
    {
        byte[] bytes = new byte[PACKET_LENGTH];
        // TODO: add a serializer
        return bytes;
    }

    public void ExtractPacketData(byte[] bytes)
    {
        // TODO: add a deserializer
    }
}
