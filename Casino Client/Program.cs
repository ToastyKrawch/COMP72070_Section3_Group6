using System.Net.Sockets;
using System.Net;
using System.Text;
using CasinoLibrary;

namespace Casino_Client
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                
                TCPClient MMMClient = new TCPClient();
                PlayerInfo player = new PlayerInfo();
                Image pfp;

                MMMClient.packet = MMMClient.receivePacket();

                //Request server for pfp
                MMMClient.packet = MMMClient.sendPacket(4, "Requesting profile picture");
                MMMClient.packet = MMMClient.receiveImagePacket();

                using (var ms = new MemoryStream(MMMClient.packet.DataPayload))
                {
                    pfp = Image.FromStream(ms);
                }

                ApplicationConfiguration.Initialize();
                Application.Run(new MainMenu(player, MMMClient, pfp));

                MMMClient.shutDown();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}