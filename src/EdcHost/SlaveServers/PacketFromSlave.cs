using System.Data.SqlTypes;

namespace EdcHost.SlaveServers;

public class PacketFromSlave : IPacketFromSlave
{
    const int PACKET_LENGTH = 10;
    public int ActionType { get;private set; }
    public int Param { get;private set; }               

    public byte[] MakePacket()
    {
        byte[] bytes = new byte[PACKET_LENGTH];
        // TODO: add a serializer
        bytes[0]=Convert.ToByte(ActionType);
        bytes[1]=Convert.ToByte(Param);

        return bytes;
    }
    public void ExtractPacketData(byte[] bytes)
    {
        //TODO: add a deserializer
        ActionType=Convert.ToInt32(bytes[0]);
        Param=Convert.ToInt32(bytes[1]);
    }
}
