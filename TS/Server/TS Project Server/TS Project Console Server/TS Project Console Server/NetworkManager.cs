using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class NetworkManager : SingletonBase<NetworkManager>
{
    public Dictionary<int,NetworkData> NetworkDataList = new Dictionary<int,NetworkData>();

    public void CreateNetworkData(Socket socket)
    {
        NetworkData networkData = new NetworkData();

        networkData.workSocket = socket;
        networkData.workSocket.BeginReceive(networkData.buffer, networkData.recvlen, NetworkData.BufferSize, 0,
           networkData.ReciveClientData, networkData);
        networkData.UserKey = UserManager.GetInstance().UserKey;

        NetworkDataList.Add(UserManager.GetInstance().UserKey,networkData);
        
    }

    public void RemoveNetworkData(int userKey)
    {
        NetworkDataList.Remove(userKey);
    }

    public void SendClient(int userKey, string text)
    {
        try
        {
            if (NetworkDataList[userKey].workSocket != null && NetworkDataList[userKey].workSocket.Connected)
            {
                byte[] buff = new byte[Encoding.UTF8.GetBytes(text).Length + 2];
                Buffer.BlockCopy(ConvertUtil.ShortToByte(Encoding.UTF8.GetBytes(text).Length + 2),
                    0, buff, 0, 2);
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(text), 0, buff, 2,
                    Encoding.UTF8.GetBytes(text).Length);
                NetworkDataList[userKey].workSocket.Send(buff, Encoding.UTF8.GetBytes(text).Length + 2, 0);
            }
        }
        catch (Exception ex)
        {
            NetworkDataList[userKey].IsConnect = false;
        }
    }

    public bool GetUserConnect(int userKey)
    {
        return NetworkDataList[userKey].IsConnect;
    }
}

