using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class RoomManager
{
    public Dictionary<int,Room> Rooms = new Dictionary<int,Room>();
    public int RoomKey = 0;
    private static RoomManager instance = null;

    public static RoomManager GetInstance()
    {
        if(instance == null)
            instance = new RoomManager();

        return instance;
    }

    public bool FindRoom(int userIndex)
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            if (Rooms.Values.ToList()[i].IsStart == false)
            {
                if (Rooms.Values.ToList()[i].UserIndexs.Count < Rooms.Values.ToList()[i].MaxUser)
                {
                    Rooms.Values.ToList()[i].AddUser(userIndex);
                    return true;
                }
            }
        }

        CreateRoom(userIndex,2);
        return false;
    }

    public void CreateRoom(int userIndex, int maxUser)
    {
        Room room = new Room(maxUser);
        room.RoomIndex = RoomKey;
        room.AddUser(userIndex);
        Rooms.Add(RoomKey,room);

        RoomKey++;
    }

    public void RemoveRoom(int roomIndex)
    {
        Rooms.Remove(roomIndex);
    }

    public void RemoveUser(int roomIndex, int userIndex)
    {
        Rooms[roomIndex].RemoveUser(userIndex);
    }
}