using EdcHost.Games;
using Xunit;

namespace EdcHost.SlaveServers;

public class PacketFromHostTests
{
    [Fact]
    public void PacketFromHost_CorrectlyInitialized()
    {
        int exp_gameStage = 0;
        int exp_elapsedTime = 12000;
        List<int> exp_heightOfChunks = new List<int> { 1, 2, 3, 6, 7 };
        bool exp_hasBed = true;
        bool exp_hasBedOpponent = true;
        float exp_positionX = 0.4f;
        float exp_positionY = 0.4f;
        float exp_positionOpponentX = 7.4f;
        float exp_positionOpponentY = 7.4f;
        int exp_agility = 0;
        int exp_health = 8;
        int exp_MaxHealth = 8;
        int exp_strength = 4;
        int exp_emeraldCount = 64;
        int exp_woolcount = 64;
        PacketFromHost packetFromHost = new PacketFromHost(
            exp_gameStage, exp_elapsedTime, exp_heightOfChunks, exp_hasBed, exp_hasBedOpponent,
            exp_positionX, exp_positionY, exp_positionOpponentX, exp_positionOpponentY, exp_agility,
            exp_health, exp_MaxHealth, exp_strength, exp_emeraldCount, exp_woolcount);
        Assert.Equal(exp_gameStage, packetFromHost.GameStage);
        Assert.Equal(exp_elapsedTime, packetFromHost.ElapsedTime);
        Assert.Equal(exp_heightOfChunks, packetFromHost.HeightOfChunks);
        Assert.Equal(exp_hasBed, packetFromHost.HasBed);
        Assert.Equal(exp_hasBedOpponent, packetFromHost.HasBedOpponent);
        Assert.Equal(exp_positionX, packetFromHost.PositionX);
        Assert.Equal(exp_positionY, packetFromHost.PositionY);
        Assert.Equal(exp_positionOpponentX, packetFromHost.PositionOpponentX);
        Assert.Equal(exp_positionY, packetFromHost.PositionOpponentY);
        Assert.Equal(exp_agility, packetFromHost.Agility);
        Assert.Equal(exp_health, packetFromHost.Health);
        Assert.Equal(exp_MaxHealth, packetFromHost.MaxHealth);
        Assert.Equal(exp_strength, packetFromHost.Strength);
        Assert.Equal(exp_strength, packetFromHost.Strength);
        Assert.Equal(exp_emeraldCount, packetFromHost.EmeraldCount);
        Assert.Equal(exp_woolcount, packetFromHost.WoolCount);
    }

    [Fact]
    public void ToBytes_CorrectlyCoverted()
    {
        int exp_gameStage = 0;
        int exp_elapsedTime = 12000;
        List<int> exp_heightOfChunks = new List<int> { 1, 2, 3, 6, 7 };
        bool exp_hasBed = true;
        bool exp_hasBedOpponent = true;
        float exp_positionX = 0.4f;
        float exp_positionY = 0.4f;
        float exp_positionOpponentX = 7.4f;
        float exp_positionOpponentY = 7.4f;
        int exp_agility = 0;
        int exp_health = 8;
        int exp_MaxHealth = 8;
        int exp_strength = 4;
        int exp_emeraldCount = 64;
        int exp_woolcount = 64;
        PacketFromHost exp_Packet = new PacketFromHost(
            exp_gameStage, exp_elapsedTime, exp_heightOfChunks, exp_hasBed, exp_hasBedOpponent,
            exp_positionX, exp_positionY, exp_positionOpponentX, exp_positionOpponentY, exp_agility,
            exp_health, exp_MaxHealth, exp_strength, exp_emeraldCount, exp_woolcount);
        byte[] data = new byte[39];
        data = exp_Packet.ToBytes();
        PacketFromHost actualPacket = new PacketFromHost(data);
        Assert.Equal(exp_Packet, actualPacket);
    }

}
