using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum ServerPackets
{
    SWelcomeMessage = 1,
}

class DataReceiver
{
    public static void HandleWelcomeMsg(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetID = buffer.ReadInteger();
        string msg = buffer.ReadString();
        buffer.Dispose();

        Debug.Log(msg);
        DataSender.SendHelloServer();
    }
}