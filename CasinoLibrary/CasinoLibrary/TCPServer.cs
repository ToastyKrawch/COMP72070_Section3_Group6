using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CasinoLibrary
{
    public class TCPServer
    {
        public TcpListener server;
        public TcpClient client;
        
        public NetworkStream stream;
        public byte[] RxBytes;
        public byte[] TxBytes;
        int bytesRead;

        public CasinoPacket packet;
        public string dataPayloadString;

        public TCPServer()
        {
            InitializeNonNetworkResources();
            SetupNetwork();
        }

        protected TCPServer(bool setupNetwork)
        {
            if (setupNetwork)
            {
                SetupNetwork();
            }
            InitializeNonNetworkResources();
        }

        protected void InitializeNonNetworkResources()
        {
            // Setup buffers and any other non-network resources here
            RxBytes = new byte[4096];
            TxBytes = new byte[4096];
            dataPayloadString = "";
            byte[] dataPayload = Encoding.UTF8.GetBytes("Mock");
            packet = new CasinoPacket(27000, 27000, 100, dataPayload);
        }

        private void SetupNetwork()
        {
            //Setup Server
            server = new TcpListener(IPAddress.Any, 27000);
            server.Start();
            Console.WriteLine("Server started. Waiting for a connection...");

            //Setup Client
            client = server.AcceptTcpClient();
            Console.WriteLine("Connected!");

            //Setup stream
            stream = client.GetStream();

            //Receive the initial hello packet
            packet = receivePacket();
        }

        public virtual CasinoPacket receivePacket()
        {
            bytesRead = stream.Read(RxBytes, 0, RxBytes.Length);

            //Setup packet
            packet = CasinoPacket.Deserialize(RxBytes[..bytesRead]);

            // Process packet
            Console.WriteLine($"Received packet: SourcePort={packet.SourcePort}, DestinationPort={packet.DestinationPort}, Timestamp={packet.Timestamp}");
            dataPayloadString = Encoding.UTF8.GetString(packet.DataPayload);
            Console.WriteLine($"DataPayload: {dataPayloadString}");

            return packet;
        }

        public virtual CasinoPacket sendPacket(byte type, string message)
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
            // Close everything
            client.Close();
            server.Stop();
        }

        public void runProtocol(PlayerInfo p)
        {
            switch (packet.PacketType) 
            {
                //Client has disconnected
                case 0:
                    shutDown();
                    break;
                case 1:
                    Console.WriteLine("Start BlackJack Game request received. Initializing a new game...");

                    string[] data = dataPayloadString.Split(',');
                    p.bet = Int32.Parse(data[0]);
                    p.balance = Int32.Parse(data[1]);
                    
                    BlackjackGame BJG = new BlackjackGame(this, p);
                    BJG.StartGame();
                    break;
                case 2:
                    Console.WriteLine("Player has joined the roulette table. Initializing a new game...");

                    RouletteGame RG = new RouletteGame(this, p);
                    RG.listen();
                    break;
            }
        }
    }
}
