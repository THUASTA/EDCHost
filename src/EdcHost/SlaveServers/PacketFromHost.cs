namespace EdcHost.SlaveServers;

public class PacketFromHost : IPacketFromHost
{
    const int PACKET_LENGTH = 100;
    public int GameStage { get;private set; }
    public int ElapsedTime { get;private set;  }
    public List<int> HeightOfChunks { get;private set;  } = new List<int>();
    public bool HasBed { get;private set;  }
    public float PositionX { get;private set;  }
    public float PositionY { get;private set;  }
    public float PositionOpponentX { get;private set;  }
    public float PositionOpponentY { get;private set;  }
    public int Agility { get;private set;  } 
    public int Health { get;private set;  }
    public int MaxHealth { get;private set;  }
    public int Strength { get; private set; }
    public int EmeraldCount { get;private set;  }
    public int WoolCount { get;private set;  }

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
        bytes[0]=Convert.ToByte(GameStage);
        
        bytes[1]=Convert.ToByte(ElapsedTime%10);         //*1
        bytes[2]=Convert.ToByte(ElapsedTime%100/10);     //*10
        bytes[3]=Convert.ToByte(ElapsedTime%1000/100);   //*100
        bytes[4]=Convert.ToByte(ElapsedTime/1000);       //*1000
        
        for(int i=0;i<HeightOfChunks.Count();i++){
        bytes[5+i]=Convert.ToByte(HeightOfChunks);
        }
        bytes[69]=Convert.ToByte(HasBed);

        byte[] temp=BitConverter.GetBytes(PositionX);
        for(int i = 0;i<4;i++){
            bytes[70+i]=temp[i];
        }
        temp=BitConverter.GetBytes(PositionY);
         for(int i = 0;i<4;i++){
            bytes[74+i]=temp[i];
        }
        temp=BitConverter.GetBytes(PositionOpponentX);
         for(int i = 0;i<4;i++){
            bytes[78+i]=temp[i];
        }
        temp=BitConverter.GetBytes(PositionOpponentY);
         for(int i = 0;i<4;i++){
            bytes[82+i]=temp[i];
        }

        bytes[86]=Convert.ToByte(Agility);
        bytes[87]=Convert.ToByte(Health);
        bytes[88]=Convert.ToByte(MaxHealth);
        bytes[89]=Convert.ToByte(Strength);
        bytes[90]=Convert.ToByte(EmeraldCount);
        bytes[91]=Convert.ToByte(WoolCount);
        return bytes;
    }

    public void ExtractPacketData(byte[] bytes)
    {
        // TODO: add a deserializer
        GameStage=Convert.ToInt32(bytes[0]);

        ElapsedTime=0;
        for(int i = 0;i<4;i++){
            ElapsedTime+=Convert.ToInt32(bytes[1+i])*(int)Math.Pow(10,i);
        }

        for(int i=0;i<HeightOfChunks.Count();i++){
        HeightOfChunks[i]=Convert.ToInt32(bytes[5+i]);
        }
        HasBed=Convert.ToBoolean(bytes[69]);

        byte[] temp=new byte[4];
        for(int i = 0;i<4;i++){
            temp[i]=bytes[70+i];
        }
        PositionX=BitConverter.ToSingle(temp);
        for(int i = 0;i<4;i++){
            temp[i]=bytes[74+i];
        }
        PositionY=BitConverter.ToSingle(temp);
        for(int i = 0;i<4;i++){
            temp[i]=bytes[78+i];
        }
        PositionOpponentX=BitConverter.ToSingle(temp);
         for(int i = 0;i<4;i++){
            temp[i]=bytes[82+i];
        }
        PositionOpponentY=BitConverter.ToSingle(temp);

        Agility=Convert.ToInt32(bytes[86]);
        Health=Convert.ToInt32(bytes[87]);
        MaxHealth=Convert.ToInt32(bytes[88]);
        Strength=Convert.ToInt32(bytes[89]);
        EmeraldCount=Convert.ToInt32(bytes[90]);
        WoolCount=Convert.ToInt32(bytes[91]);
    }
}
