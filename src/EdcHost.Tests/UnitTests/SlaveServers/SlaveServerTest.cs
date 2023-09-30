using EdcHost.SlaveServers;
using EdcHost.SlaveServers.EventArgs;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.SlaveServers
{
    public class SlaveServerTests
    {
        [Fact]
        public void SlaveServer_WrongPlayerNum_ThrowsCorrectException()
        {
            string[] portNameList1 = new string[] { "COM1" };
            string[] portNameList2 = new string[] { "COM1", "COM2" };
            string[] portNameList3 = new string[] { "COM1", "COM2", "COM3" };
            int[] baudRateList1 = new int[] { 9600 };
            int[] baudRateList2 = new int[] { 9600, 9600 };
            int[] baudRateList3 = new int[] { 9600, 9600, 9600 };
            Assert.Throws<ArgumentException>(() => new SlaveServer(portNameList2, baudRateList1));
            Assert.Throws<ArgumentException>(() => new SlaveServer(portNameList2, baudRateList3));
            Assert.Throws<ArgumentException>(() => new SlaveServer(portNameList1, baudRateList2));
            Assert.Throws<ArgumentException>(() => new SlaveServer(portNameList3, baudRateList2));
        }

        //todo:baudratelist wrongnumber
        [Fact]
        public void Start_Calls_SerialPort_Open_For_Each_Port()
        {
            // Arrange
            string[] portNameList = new string[] { "COM1", "COM2" };
            int[] baudRateList = new int[] { 9600, 9600 };
            var parityList = new Parity[] { Parity.None, Parity.None };
            var dataBits = 8;
            var stopBits = StopBits.One;
            var slaveServer = new SlaveServer(portNameList, baudRateList, parityList, dataBits, stopBits);

            var mockSerialPort1 = new Mock<SerialPortWrapper>();
            var mockSerialPort2 = new Mock<SerialPortWrapper>();
            slaveServer.SetSerialPortWrapper(0, mockSerialPort1.Object);
            slaveServer.SetSerialPortWrapper(1, mockSerialPort2.Object);

            // Act
            slaveServer.Start();

            // Assert
            mockSerialPort1.Verify(s => s.Open(), Times.Once);
            mockSerialPort2.Verify(s => s.Open(), Times.Once);
        }

        [Fact]
        public void Stop_Calls_SerialPort_Close_For_Each_Port()
        {
            // Arrange
            var portNameList = new string[] { "COM1", "COM2" };
            var baudRateList = new int[] { 9600, 9600 };
            var parityList = new Parity[] { Parity.None, Parity.None };
            var dataBits = 8;
            var stopBits = StopBits.One;
            var slaveServer = new SlaveServer(portNameList, baudRateList, parityList, dataBits, stopBits);

            var mockSerialPort1 = new Mock<SerialPortWrapper>();
            var mockSerialPort2 = new Mock<SerialPortWrapper>();
            slaveServer.SetSerialPortWrapper(0, mockSerialPort1.Object);
            slaveServer.SetSerialPortWrapper(1, mockSerialPort2.Object);

            // Act
            slaveServer.Stop();

            // Assert
            mockSerialPort1.Verify(s => s.Close(), Times.Once);
            mockSerialPort2.Verify(s => s.Close(), Times.Once);
        }

        [Fact]
        public void UpdatePacket_Sets_Packet_To_Send()
        {
            // Arrange
            var portNameList = new string[] { "COM1", "COM2" };
            var baudRateList = new int[] { 9600, 9600 };
            var parityList = new Parity[] { Parity.None, Parity.None };
            var dataBits = 8;
            var stopBits = StopBits.One;
            var slaveServer = new SlaveServer(portNameList, baudRateList, parityList, dataBits, stopBits);

            var packetMock = new Mock<IPacket>();
            var id = 0;

            // Act
            slaveServer.UpdatePacket(id, packetMock.Object);

            // Assert
            Assert.Equal(packetMock.Object, slaveServer.GetPacketsToSend()[id]);
        }

        // Add more test methods as needed
    }
}