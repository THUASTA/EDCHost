namespace EdcHost.SlaveServers;

public class PacketFromSlave : IPacketFromSlave
{
    const int PACKET_LENGTH = 10;
    public int ActionType { get; private set; }
    public int Param { get; private set; }

    public PacketFromSlave(byte[] bytes)
    {
        // TODO: add a deserializer
    }

    public byte[] ToBytes()
    {
        int datalength = 2;
        byte[] data = new byte[datalength];

        int currentIndex = 0;
        data[currentIndex++] = Convert.ToByte(ActionType);
        data[currentIndex] = Convert.ToByte(Param);

        //add header
        byte[] header = IPacket.GeneratePacketHeader(data);
        byte[] bytes = new byte[header.Length + data.Length];
        header.CopyTo(bytes, 0);
        data.CopyTo(bytes, header.Length);
        return bytes;
    }
}
