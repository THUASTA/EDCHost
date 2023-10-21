using EdcHost.Games;

namespace EdcHost.SlaveServers;

public class PacketFromHost : IPacketFromHost
{
    const int Chunk_MaxHeight = 8;
    const int Chunk_MinHeight = 0;
    const int PACKET_LENGTH = 100;
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
    public static bool PositionIsInvalid(float PositionT)
    {
        return (PositionT > 7 || PositionT < 0);
    }

    public byte[] ToBytes()
    {
        int datalength = (
           1 +                  //GameStage
           4 +                  //ElapsedTime
           1 * 64 +               //HeightOfChunk
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
        if (ElapsedTime >= 10000 || ElapsedTime < 0) throw new ArgumentException("The ElapsedTime is incorrect");
        data[currentIndex++] = Convert.ToByte(ElapsedTime % 10);         //*1
        data[currentIndex++] = Convert.ToByte(ElapsedTime % 100 / 10);     //*10
        data[currentIndex++] = Convert.ToByte(ElapsedTime % 1000 / 100);   //*100
        data[currentIndex++] = Convert.ToByte(ElapsedTime / 1000);       //*1000

        //HeightOfChunks
        for (int i = 0; i < HeightOfChunks.Count(); i++)
        {
            if (HeightOfChunks[i] > Chunk_MaxHeight || HeightOfChunks[i] < Chunk_MinHeight) throw new ArgumentException("The HeightOfChunks is incorrect");
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
        if (PositionIsInvalid(PositionX)) throw new ArgumentException("The PositionX is incorrect");
        byte[] temp = BitConverter.GetBytes(PositionX);    //convert float to 4 bytes
        for (int i = 0; i < temp.Length; i++)
        {
            data[currentIndex] = temp[i];
            currentIndex++;
        }
        if (PositionIsInvalid(PositionY)) throw new ArgumentException("The PositionY is incorrect");
        temp = BitConverter.GetBytes(PositionY);
        for (int i = 0; i < 4; i++)
        {
            data[currentIndex] = temp[i];
            currentIndex++;
        }
        if (PositionIsInvalid(PositionOpponentX)) throw new ArgumentException("The PositionOpponentX is incorrect");
        temp = BitConverter.GetBytes(PositionOpponentX);
        for (int i = 0; i < 4; i++)
        {
            data[currentIndex] = temp[i];
            currentIndex++;
        }
        if (PositionIsInvalid(PositionOpponentY)) throw new ArgumentException("The PositionOpponentY is incorrect");
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
