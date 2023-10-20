namespace EdcHost.SlaveServers;

public class PacketFromSlave : IPacketFromSlave
{
    const int PACKET_LENGTH = 10;
    public int ActionType { get; }
    public int Param { get; }

    public PacketFromSlave(byte[] bytes)
    {
        // TODO: add a deserializer
    }

    public byte[] ToBytes()
    {
        byte[] bytes = new byte[PACKET_LENGTH];
        // TODO: add a serializer
        return bytes;
    }
}
