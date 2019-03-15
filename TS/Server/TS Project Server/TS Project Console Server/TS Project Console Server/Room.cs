using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class Room
{
    public List<int> UserIndexs = new List<int>();
    public User HostUser = null;

    public bool IsStart = false;
    public bool IsFinish = false;

    public int MaxUser = 2;
    public int RoomIndex = 0;

    private Timer _connectionTimer;

    public Room(int maxUser)
    {
        MaxUser = maxUser;

        _connectionTimer = new Timer(CheckUserConnection);
        _connectionTimer.Change(0, 1000);
    }

    public void AddUser(int userIndex)
    {
        UserIndexs.Add(userIndex);

        UserManager.GetInstance().SetRoomIndex(userIndex, RoomIndex);
        UserManager.GetInstance().IsUserJoinRoom(userIndex, true);

        if (UserIndexs.Count == MaxUser)
        {
            string str = "GAMESTART";

            for (int i = 0; i < UserIndexs.Count; i++)
            {
                str += ":" + UserManager.GetInstance().GetUserName(UserIndexs[i]);
            }

            IsStart = true;

            RoomUserSendClient(str);
            RoomUserSendClient(string.Format("GET-RANDOMSEED:{0}",EtcUtil.RandomUtil.Next(0,100)));
        }
    }

    public void RemoveUser(int userIndex)
    {
        for (int i = 0; i < UserIndexs.Count; i++)
        {
            if (UserIndexs[i] == userIndex)
            {
                UserIndexs.RemoveAt(i);
                break;
            }
        }

        if (UserIndexs.Count == 0)
        {
            _connectionTimer.Dispose();
            RoomManager.GetInstance().RemoveRoom(RoomIndex);
        }
    }

    public void CheckUserConnection(object state)
    {
        for (int i = 0; i < UserIndexs.Count; i++)
        {
            if (NetworkManager.GetInstance().GetUserConnect(UserIndexs[i]) == false)
            {
                int userId = UserManager.GetInstance().GetUserId(UserIndexs[i]);

                UserManager.GetInstance().RemoveUser(UserIndexs[i]);

                UserIndexs.RemoveAt(i);

                RoomUserSendClient(string.Format("OUT:" + userId));
            }
        }
    }

    public void RoomUserSendClient(string text)
    {
        for (int i = 0; i < UserIndexs.Count; i++)
        {
            NetworkManager.GetInstance().SendClient(UserIndexs[i], text);
        }
    }
}