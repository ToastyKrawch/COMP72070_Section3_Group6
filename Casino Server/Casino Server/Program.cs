using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CasinoLibrary;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            TCPServer MMMServer = new TCPServer();
            PlayerInfo player = new PlayerInfo();

            MMMServer.packet = MMMServer.sendPacket(0, "\nHello Casino Client, Please select a game:\n1 for BlackJack\n2 for Roulette\n3 to Quit");

            while (true)
            {
                MMMServer.packet = MMMServer.receivePacket();
                MMMServer.runProtocol();
            }

            MMMServer.shutDown();
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }
}