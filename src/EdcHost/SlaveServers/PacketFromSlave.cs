namespace EdcHost.SlaveServers;

public class PacketFromSlave : IPacketFromSlave
{
    public int ActionType { get; }
    public int Param { get; }

    public byte[] MakePacket()
    {
        //TODO: add a serializer
        throw new NotImplementedException();
    }
    public void ExtractPacketData(byte[] bytes)
    {
        //TODO: add a deserializer
    }
}
