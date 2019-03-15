using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class UserManager : SingletonBase<UserManager>
{
    public Dictionary<int,User> Users = new Dictionary<int,User>();
    public int GuestUsers = 0;
    public int UserKey = 0;

    public void AddUser(Socket socket)
    {
        User user = new User(socket, UserKey);

        Users.Add(UserKey, user);
        UserKey++;
    }

    public void RemoveUser(int userKey)
    {
        if (Users[userKey].IsGuest == true)
            GuestUsers--;

        if (Users[userKey].IsJoinRoom == true)
            RoomManager.GetInstance().RemoveUser(GetRoomIndex(userKey), userKey);

        Console.WriteLine("{0} is out.", Users[userKey].UserName);

        Users.Remove(userKey);

        NetworkManager.GetInstance().RemoveNetworkData(userKey);
    }

    public void SendAllUser(string text)
    {
        for (int i = 0; i < Users.Count; i++)
        {
            NetworkManager.GetInstance().SendClient(Users.Keys.ToList()[i], text);
        }
    }

    public void SetRoomIndex(int userKey, int roomIndex)
    {
        Users[userKey].RoomIndex = roomIndex;
    }

    public void SetUserName(int userKey, string name)
    {
        Users[userKey].UserName = name;
    }

    public void IsGuestUser(int userKey, bool isGuest)
    {
        Users[userKey].IsGuest = isGuest;
    }

    public void IsUserJoinRoom(int userKey, bool isJoin)
    {
        Users[userKey].IsJoinRoom = isJoin;
    }

    public int GetUserId(int userKey)
    {
        return Users[userKey].UserId;
    }

    public int GetRoomIndex(int userKey)
    {
        return Users[userKey].RoomIndex;
    }

    public string GetUserName(int userIndex)
    {
        return Users[userIndex].UserName;
    }
}