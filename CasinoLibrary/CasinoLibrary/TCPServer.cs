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
            RxBytes = new byte[131072];
            TxBytes = new byte[131072];
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

        public virtual CasinoPacket receiveImagePacket()
        {
            // It's assumed that the caller knows that the next packet will be an image.
            bytesRead = stream.Read(RxBytes, 0, RxBytes.Length);

            // Here, we are not converting the data to a string since it's binary data for the image
            packet = CasinoPacket.Deserialize(RxBytes[..bytesRead]);

            // Log receipt of the packet but don't attempt to process it as a string
            Console.WriteLine($"Received image packet: SourcePort={packet.SourcePort}, DestinationPort={packet.DestinationPort}, Timestamp={packet.Timestamp}");

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

        public virtual CasinoPacket sendImagePacket(byte type, string imagePath)
        {
            // Read the image into a byte array
            byte[] dataPayload = File.ReadAllBytes(imagePath);

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
                case 3:
                    Console.WriteLine("Request to change profile picture, saving image...");
                    packet = receiveImagePacket();
                    SaveImage(packet.DataPayload);
                    break;
                case 4:
                    Console.WriteLine("Request for profile picture, sending image...");
                    packet = sendImagePacket(0, "../../../Saved Images/ProfilePic.jpg");
                    break;
            }
        }

        private void SaveImage(byte[] imageData)
        {
            // Determine the path where you want to save the image
            string imagePath = "../../../Saved Images/ProfilePic.jpg";

            // Write the binary data to a file
            File.WriteAllBytes(imagePath, imageData);
            Console.WriteLine($"Image saved to {imagePath}");
        }
    }
}
