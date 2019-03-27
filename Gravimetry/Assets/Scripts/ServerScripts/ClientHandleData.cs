using System;
using System.Collections.Generic;
using System.Linq;

class ClientHandleData
{
    private static ByteBuffer playerBuffer;
    public delegate void Packet(byte[] data);
    public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

    public static void InitializePackets()
    {
        packets.Add((int)ServerPackets.SWelcomeMessage, DataReceiver.HandleWelcomeMsg);
    }

    public static void HandleData(byte[] data)
    {
        byte[] buffer = (byte[])data.Clone();
        int pLength = 0;

        if (playerBuffer == null)
            playerBuffer = new ByteBuffer();

        playerBuffer.WriteBytes(buffer);
        if (playerBuffer.Count() == 0)
        {
            playerBuffer.Clear();
            return;
        }

        if (playerBuffer.Length() >= 4)
        {
            pLength = playerBuffer.ReadInteger(false);
            if (pLength <= 0)
            {
                playerBuffer.Clear();
                return;
            }
        }

        while (pLength > 0 & pLength <= playerBuffer.Length() - 4)
        {
            if (pLength <= playerBuffer.Length() - 4)
            {
                playerBuffer.ReadInteger();
                data = playerBuffer.ReadBytes(pLength);
                HandleDataPackets(data);
            }

            pLength = 0;
            if (playerBuffer.Length() >= 4)
            {
                pLength = playerBuffer.ReadInteger(false);
                if (pLength <= 0)
                {
                    playerBuffer.Clear();
                    return;
                }
            }
        }

        if (pLength <= 1)
        {
            playerBuffer.Clear();
        }
    }

    private static void HandleDataPackets(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteBytes(data);
        int packetID = buffer.ReadInteger();
        buffer.Dispose();
        if (packets.TryGetValue(packetID, out Packet packet))
        {
            packet.Invoke(data);
        }
    }
}

