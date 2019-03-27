using System;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class ClientTCP
{
    private static TcpClient clientSocket;
    private static NetworkStream myStream;
    private static byte[] recBuffer;

    public static void InitializingNetworking()
    {
        clientSocket = new TcpClient();
        clientSocket.ReceiveBufferSize = 4096;
        clientSocket.SendBufferSize = 4096;
        recBuffer = new byte[4096 * 2];
        clientSocket.BeginConnect("127.0.0.1", 5557, new AsyncCallback(ClientConnectionCallback), clientSocket);
    }

    private static void ClientConnectionCallback(IAsyncResult result)
    {
        clientSocket.EndConnect(result);
        if (clientSocket.Connected == false)
        {
            return;
        }
        else
        {
            clientSocket.NoDelay = true;
            myStream = clientSocket.GetStream();
            myStream.BeginRead(recBuffer, 0, 2096 * 2, ReceiveCallback, null);
        }
    }

    private static void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int length = myStream.EndRead(result);
            if (length <= 0)
            {
                return;
            }

            byte[] newBytes = new byte[length];
            Array.Copy(recBuffer, newBytes, length);
            UnityThread.executeInFixedUpdate(() =>
            {
                ClientHandleData.HandleData(newBytes);
            });
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static void SendData(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
        buffer.WriteBytes(data);
        myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
        buffer.Dispose();
    }

    public static void Disconnect()
    {
        clientSocket.Close();
    }
}

