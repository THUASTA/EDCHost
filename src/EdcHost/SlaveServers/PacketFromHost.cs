using EdcHost.Games;

namespace EdcHost.SlaveServers;

public class PacketFromHost : IPacketFromHost
{
    public int GameStage { get; private set; }
    public int ElapsedTime { get; private set; }
    public List<int> HeightOfChunks { get; private set; } = new List<int>();
    public bool HasBed { get; private set; }
    public bool HasBedOpponent { get; private set; }
    public float PositionX { get; private set; }
    public float PositionY { get; private set; }
    public float PositionOpponentX { get; private set; }
    public float PositionOpponentY { get; private set; }
    public int Agility { get; private set; }
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public int Strength { get; private set; }
    public int EmeraldCount { get; private set; }
    public int WoolCount { get; private set; }

    public PacketFromHost(byte[] bytes)
    {
        int currentIndex = 0;
        GameStage = Convert.ToInt32(bytes[currentIndex++]);
        ElapsedTime = BitConverter.ToInt32(bytes, currentIndex);
        currentIndex += 4;
        for (int i = 0; i < 64; i++)
        {
            HeightOfChunks.Add(Convert.ToInt32(bytes[currentIndex++]));
        }
        HasBed = Convert.ToBoolean(bytes[currentIndex++]);
        HasBedOpponent = Convert.ToBoolean(bytes[currentIndex++]);
        PositionX = BitConverter.ToSingle(bytes, currentIndex);
        currentIndex += 4;
        PositionY = BitConverter.ToSingle(bytes, currentIndex);
        currentIndex += 4;
        PositionOpponentX = BitConverter.ToSingle(bytes, currentIndex);
        currentIndex += 4;
        PositionOpponentY = BitConverter.ToSingle(bytes, currentIndex);
        currentIndex += 4;
        Agility = Convert.ToInt32(bytes[currentIndex++]);
        Health = Convert.ToInt32(bytes[currentIndex++]);
        MaxHealth = Convert.ToInt32(bytes[currentIndex++]);
        Strength = Convert.ToInt32(bytes[currentIndex++]);
        EmeraldCount = Convert.ToInt32(bytes[currentIndex++]);
        WoolCount = Convert.ToInt32(bytes[currentIndex++]);
    }

    public PacketFromHost(
        int gameStage, int elapsedTime, List<int> heightOfChunks, bool hasBed, bool hasBedOpponent,
        float positionX, float positionY, float positionOpponentX, float positionOpponentY,
        int agility, int health, int maxHealth, int strength,
        int emeraldCount, int woolCount
        )
    {
        GameStage = gameStage;
        ElapsedTime = elapsedTime;
        HeightOfChunks = new(heightOfChunks);
        HasBed = hasBed;
        HasBedOpponent = hasBedOpponent;
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

    public byte[] ToBytes()
    {
        int datalength = (
           1 +                  //GameStage
           4 +                  //ElapsedTime
           1 * HeightOfChunks.Count + //HeightOfChunk
           1 +                  //HasBed
           1 +                  //HasBedOpponet
           4 * 4 +                // Position 
           1 * 6                 //agility health maxHealth strength emeraldCount woolCount
       );
        byte[] data = new byte[datalength];

        int currentIndex = 0;
        //GameStage
        data[currentIndex++] = Convert.ToByte(GameStage);

        //ElapsedTime
        BitConverter.GetBytes(ElapsedTime).CopyTo(data, currentIndex);
        currentIndex += 4;

        //HeightOfChunks
        for (int i = 0; i < HeightOfChunks.Count(); i++)
        {
            data[currentIndex] = Convert.ToByte(HeightOfChunks[i]);
            currentIndex++;
        }

        //HasBed
        data[currentIndex] = Convert.ToByte(HasBed);
        currentIndex++;

        //HasBedOpponent
        data[currentIndex] = Convert.ToByte(HasBedOpponent);
        currentIndex++;

        //Position
        byte[] temp = BitConverter.GetBytes((float)PositionX);    //convert float to 4 bytes
        for (int i = 0; i < temp.Length; i++)
        {
            data[currentIndex] = temp[i];
            currentIndex++;
        }
        temp = BitConverter.GetBytes(PositionY);
        for (int i = 0; i < 4; i++)
        {
            data[currentIndex] = temp[i];
            currentIndex++;
        }
        temp = BitConverter.GetBytes(PositionOpponentX);
        for (int i = 0; i < 4; i++)
        {
            data[currentIndex] = temp[i];
            currentIndex++;
        }
        temp = BitConverter.GetBytes(PositionOpponentY);
        for (int i = 0; i < 4; i++)
        {
            data[currentIndex] = temp[i];
            currentIndex++;
        }

        //1 byte factors
        data[currentIndex++] = Convert.ToByte(Agility);
        data[currentIndex++] = Convert.ToByte(Health);
        data[currentIndex++] = Convert.ToByte(MaxHealth);
        data[currentIndex++] = Convert.ToByte(Strength);
        data[currentIndex++] = Convert.ToByte(EmeraldCount);
        data[currentIndex++] = Convert.ToByte(WoolCount);

        //add header
        byte[] header = IPacket.GeneratePacketHeader(data);
        byte[] bytes = new byte[header.Length + data.Length];
        header.CopyTo(bytes, 0);
        data.CopyTo(bytes, header.Length);
        return bytes;
    }
}
