using System.Data.SqlTypes;

namespace EdcHost.SlaveServers;

public class PacketFromSlave : IPacketFromSlave
{
    const int PACKET_LENGTH = 10;
    public int ActionType { get;private set; }
    public int Param { get;private set; }               

    public byte[] MakePacket()
    {
        int datalength = 2;
        byte[] data = new byte[datalength];

        int currentIndex = 0;
        data[currentIndex++]=Convert.ToByte(ActionType);
        data[currentIndex]=Convert.ToByte(Param);

        //add header
        byte[] header = IPacket.GeneratePacketHeader(data);
        byte[] bytes = new byte[header.Length + data.Length];
        header.CopyTo(bytes, 0);
        data.CopyTo(bytes, header.Length);
        return bytes;
    }
    public void ExtractPacketData(byte[] bytes)
    {
        byte[] data=IPacket.GetPacketData(bytes);
        int currentIndex = 0;
        ActionType=Convert.ToInt32(data[currentIndex++]);
        Param=Convert.ToInt32(data[currentIndex]);
    }
}
