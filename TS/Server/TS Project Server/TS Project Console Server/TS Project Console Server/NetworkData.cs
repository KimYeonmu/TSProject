using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class NetworkData
{
    public static int BufferSize = 3276;
    public Socket workSocket = null;
    public byte[] buffer = new byte[BufferSize];
    public int recvlen = 0;
    public int UserKey = 0;
    public bool IsConnect = true;

    public string[] GetSplitbufferOfUTF8(int length, string saparator)
    {
        string msg = Encoding.UTF8.GetString(buffer, 2, length - 2);

        return msg.Split(saparator.ToCharArray());
    }

    public void ReciveClientData(IAsyncResult ar)
    {
        try
        {
            Socket handler = workSocket;
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                recvlen += bytesRead;

                while (true)
                {
                    short length;
                    ConvertUtil.GetShort(buffer, 0, out length);

                    if (length > 0 && recvlen >= length)
                    {
                        UserPacketParse(length);
                        recvlen -= length;

                        if (recvlen > 0)
                        {
                            Buffer.BlockCopy(buffer, length, buffer, 0, recvlen);
                        }
                        else
                        {
                            handler.BeginReceive(buffer, recvlen, NetworkData.BufferSize, 0,
                                ReciveClientData, this);
                            break;
                        }
                    }
                    else
                    {
                        handler.BeginReceive(buffer, recvlen, BufferSize, 0,
                            ReciveClientData, this);
                        break;
                    }
                }
            }
            else
            {
                handler.BeginReceive(buffer, recvlen, BufferSize, 0,
                    ReciveClientData, this);
            }
        }
        catch (Exception e)
        {
            IsConnect = false;
            Console.WriteLine(e.ToString());
        }
    }

    private void UserPacketParse(int length)
    {
        string[] text = GetSplitbufferOfUTF8(length, ":");

        if (text[0].Equals("CONNECT"))
        {
            UserManager.GetInstance().Users[UserKey].UserName = text[1];
            Console.WriteLine("{0} 님이 접속했습니다.",text[1]);

            //SendClient(string.Format("USER:{0}:{1}:{2}:{3}", UserData.Gold, UserData.Chip, UserData.Wins, UserData.Loses));
        }
        else if (text[0].Equals("GUEST-LOGIN"))
        {
            int guestNum = UserManager.GetInstance().GuestUsers;

            Console.WriteLine("Guest{0} is connected",guestNum);

            UserManager.GetInstance().SetUserName(UserKey, "Guest"+guestNum);
            UserManager.GetInstance().IsGuestUser(UserKey, true);

            NetworkManager.GetInstance().SendClient(UserKey, string.Format("GUEST-ID:{0}", guestNum));

            UserManager.GetInstance().GuestUsers++;
        }
        else if (text[0].Equals("FIND-ROOM"))
        {
            Console.WriteLine("{0} is find a room", UserManager.GetInstance().GetUserName(UserKey));

            RoomManager.GetInstance().FindRoom(UserKey);
        }
        else if (text[0].Equals("PUT-CARD"))
        {
            int roomIndex = UserManager.GetInstance().GetRoomIndex(UserKey);

            RoomManager.GetInstance().Rooms[roomIndex].RoomUserSendClient(string.Format("PUT-CARD:{0}:{1}",text[1],text[2]));
        }
        else if (text[0].Equals("ADD-CARD"))
        {
            int roomIndex = UserManager.GetInstance().GetRoomIndex(UserKey);

            RoomManager.GetInstance().Rooms[roomIndex].RoomUserSendClient(string.Format("ADD-CARD:{0}:{1}",text[1],text[2]));
        }
        else if (text[0].Equals("NEXT-TURN"))
        {
            int roomIndex = UserManager.GetInstance().GetRoomIndex(UserKey);

            RoomManager.GetInstance().Rooms[roomIndex].RoomUserSendClient("NEXT-TURN");
        }
        else if (text[0].Equals("DISCONNECT"))
        {
            UserManager.GetInstance().RemoveUser(UserKey);
        }
    }
}

