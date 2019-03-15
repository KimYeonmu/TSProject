using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class MainServer
{
    public ManualResetEvent MenualEvent = new ManualResetEvent(false);

    public MainServer(int port)
    {
        RoomManager.GetInstance().Rooms.Clear();
        StartListening(port);
    }

    public void StartListening(int port)
    {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.NoDelay = true;
        listener.LingerState = new LingerOption(true, 0);
        listener.SendBufferSize = 81920;
        listener.ReceiveBufferSize = 81920;

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(100);

            Console.WriteLine("Start TS server");

            while (true)
            {
                MenualEvent.Reset();
                listener.BeginAccept(new AsyncCallback(AcceptUser), listener);
                MenualEvent.WaitOne();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress Any Key");
        Console.Read();
    }

    public void AcceptUser(IAsyncResult ar)
    {
        MenualEvent.Set();

        Socket listener = (Socket) ar.AsyncState;
        Socket handler = listener.EndAccept(ar);
        handler.NoDelay = true;
        handler.LingerState = new LingerOption(true, 0);
        handler.SendBufferSize = 81920;
        handler.ReceiveBufferSize = 81920;

        UserManager.GetInstance().AddUser(handler);
    }
}