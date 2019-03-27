using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ClientPackets
{
    CHelloServer = 1,
}

class DataSender
{
    public static void SendHelloServer()
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((int)ClientPackets.CHelloServer);
        buffer.WriteString("Client connected.");
        ClientTCP.SendData(buffer.ToArray());
        buffer.Dispose();
    }
}

