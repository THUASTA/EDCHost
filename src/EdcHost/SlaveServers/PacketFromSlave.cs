namespace EdcHost.SlaveServers;

public class PacketFromSlave : IPacketFromSlave
{
    public int ActionType { get; private set; }
    public int Param { get; private set; }

    public PacketFromSlave(byte[] bytes)
    {
        // Validate the byte array
        if (bytes.Length < 5)
        {
            throw new Exception("The header of the packet is broken.");
        }
        if (bytes[0] != 0x55 || bytes[1] != 0xAA)
        {
            throw new Exception("The header of the packet is broken.");
        }
        short dataLength = BitConverter.ToInt16(bytes, 2);
        byte checksum = bytes[4];
        if (bytes.Length < dataLength + 5)
        {
            throw new Exception("The data length of the packet is incorrect.");
        }
        if (checksum != IPacket.CalculateChecksum(bytes[5..(dataLength + 5)]))
        {
            throw new Exception("The checksum of the packet is incorrect.");
        }

        int currentIndex = 5;
        ActionType = Convert.ToInt32(bytes[currentIndex++]);
        Param = Convert.ToInt32(bytes[currentIndex]);
    }

    public PacketFromSlave(int actionType, int param)
    {
        ActionType = actionType;
        Param = param;
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
