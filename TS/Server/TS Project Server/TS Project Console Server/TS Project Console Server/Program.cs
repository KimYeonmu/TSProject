using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class Program
{
    static void Main(string[] args)
    {
        Thread serverThread = new Thread(new ThreadStart(ServerThread));
        Thread inputThread = new Thread(new ThreadStart(InputThread));

        serverThread.Start();
        inputThread.Start();
    }

    public static void ServerThread()
    {
        MainServer server = new MainServer(10000);

        while (true)
        {
            Thread.Sleep(1);
        }
    }

    public static void InputThread()
    {
        while (true)
        {
            string[] input = Console.ReadLine().Split(' ');

            if (input[0].Equals("game-start"))
            {
                UserManager.GetInstance().SendAllUser(string.Format("GAMESTART:{0}",input[1]));
            }
            else if (input[0].Equals("put-card"))
            {
                UserManager.GetInstance().SendAllUser(string.Format("PUT-CARD:{0}:{1}", input[1], input[2]));
            }
        }
    }
}

