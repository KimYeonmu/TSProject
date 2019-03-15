using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public struct UserData
{
    public int Gold;
    public int Chip;
    public int Wins;
    public int Loses;
}

public class User
{
    public string UserName;

    public int RoomIndex;
    public int UserKey;
    public int UserId;

    public bool IsGuest = false;
    public bool IsJoinRoom = false;

    public UserData UserData = new UserData();
    

    public User(Socket socket, int userKey)
    {
        NetworkManager.GetInstance().CreateNetworkData(socket);
        NetworkManager.GetInstance().SendClient(userKey, "CONNECT");
    }
}

