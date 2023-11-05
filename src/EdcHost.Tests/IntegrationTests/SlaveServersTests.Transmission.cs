using System.Text;

using EdcHost.SlaveServers;

using Xunit;

namespace EdcHost.Tests.IntegrationTests;
public partial class SlaveServersTests
{
    [Theory]
    [InlineData("COM1", 9600)]
    public void Transmission(string portName, int baudRate)
    {
        SerialPortHubMock serialPortHubMock = new()
        {
            SerialPorts = {
                { portName, new SerialPortWrapperMock(portName) }
            }
        };
        var slaveServer = new SlaveServer(serialPortHubMock);

        slaveServer.Start();
        slaveServer.OpenPort(portName, baudRate);

        var serialPort = (SerialPortWrapperMock)serialPortHubMock.Get(portName, baudRate);

        int gameStage = 1;
        int elapsedTime = 100;
        List<int> heightOfChunks = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
        bool hasBed = true;
        bool hasBedOpponent = true;
        double positionX = 1.0;
        double positionY = 1.0;
        double positionOpponentX = 1.0;
        double positionOpponentY = 1.0;
        int agility = 1;
        int health = 2;
        int maxHealth = 3;
        int strength = 4;
        int emeraldCount = 5;
        int woolCount = 6;

        PacketFromHost packet = new PacketFromHost(
            gameStage, elapsedTime,
            heightOfChunks,
            hasBed, hasBedOpponent,
            (float)positionX, (float)positionY,
            (float)positionOpponentX, (float)positionOpponentY,
            agility, health, maxHealth, strength,
            emeraldCount, woolCount);

        byte[] bytes = packet.ToBytes();

        slaveServer.Publish(
            portName,
            gameStage, elapsedTime,
            heightOfChunks,
            hasBed, hasBedOpponent,
            positionX, positionY, positionOpponentX, positionOpponentY,
            agility, health, maxHealth, strength,
            emeraldCount, woolCount);

        byte[] bytesToSend = serialPort.WriteBuffer.ToArray();

        //Assertion
        Assert.Equal(bytes, bytesToSend);
    }
}
