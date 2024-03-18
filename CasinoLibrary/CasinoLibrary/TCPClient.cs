using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CasinoLibrary
{
    public class TCPClient
    {
        TcpClient client;

        NetworkStream stream;
        byte[] RxBytes;
        byte[] TxBytes;
        int bytesRead;

        public CasinoPacket packet;
        public string dataPayloadString;
        byte[] dataPayload;

        public TCPClient()
        {
            //Create a TcpClient
            client = new TcpClient("127.0.0.1", 27000);

            //Setup stream and buffers
            stream = client.GetStream();
            RxBytes = new byte[4096];
            bytesRead = 0;
            dataPayloadString = "";

            //Setup the initial hello packet
            dataPayload = Encoding.UTF8.GetBytes("Hello, Casino Server!");
            packet = new CasinoPacket(27000, 27000, 0, dataPayload);

            //Serialize the packet then send
            TxBytes = packet.Serialize();
            stream.Write(TxBytes, 0, TxBytes.Length);
        }

        public CasinoPacket receivePacket()
        {
            //Setup packet
            bytesRead = stream.Read(RxBytes, 0, RxBytes.Length);
            packet = CasinoPacket.Deserialize(RxBytes[..bytesRead]);

            // Process packet
            Console.WriteLine($"Received packet: SourcePort={packet.SourcePort}, DestinationPort={packet.DestinationPort}, Timestamp={packet.Timestamp}");
            dataPayloadString = Encoding.UTF8.GetString(packet.DataPayload);
            Console.WriteLine($"DataPayload: {dataPayloadString}");

            return packet;
        }

        public CasinoPacket sendPacket(byte type, string message)
        {
            //Setup TxBytes
            byte[] dataPayload = Encoding.UTF8.GetBytes(message);

            //Setup packet
            packet.setPacket(27000, 27000, type, dataPayload);

            // Serialize and send the packet
            TxBytes = packet.Serialize();
            stream.Write(TxBytes, 0, TxBytes.Length);

            return packet;
        }

        public void shutDown()
        {
            stream.Close();
            client.Close();
        }
    }
}
