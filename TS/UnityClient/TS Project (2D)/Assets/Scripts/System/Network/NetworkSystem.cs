using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


public class NetworkSystem : SingletonBase<NetworkSystem>
{
    private static Socket socketTcp = null;
    private string ipAddress = "192.168.219.150";
    private int port = 10000;
    private byte[] buffer = new byte[4096];
    private int recvlen = 0;

    [HideInInspector]
    public bool IsConnect;

    public void StartConnect()
    {
        DisConnect();

        IPAddress serverIP = IPAddress.Parse(ipAddress);
        socketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socketTcp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
        socketTcp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);

        try
        {
            socketTcp.Connect(new IPEndPoint(serverIP, port));
        }
        catch (Exception)
        {
            IsConnect = false;
            UnityEngine.Debug.Log("connection failed");
        }
        
        StartCoroutine(PacketProc());
    }

    public void DisConnect()
    {
        if (socketTcp != null && socketTcp.Connected)
        {
            socketTcp.Close();
            StopCoroutine(PacketProc());
        }
    }

    private IEnumerator PacketProc()
    {
        byte[] buff = new byte[4096];

        while (true)
        {
            if (socketTcp.Available > 0)
            {
                Array.Clear(buff, 0, 4096);
                int readLen = socketTcp.Receive(buff, socketTcp.Available, 0);

                if (readLen > 0)
                {
                    Buffer.BlockCopy(buff, 0, buffer, recvlen, readLen);
                    recvlen += readLen;

                    while (true)
                    {
                        int length = BitConverter.ToInt16(buffer, 0);

                        if (length > 0 && recvlen >= length)
                        {
                            ServerPacketParse(length);
                            recvlen -= length;
                            Buffer.BlockCopy(buffer, length, buffer, 0, recvlen);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    private void ServerPacketParse(int length)
    {
        string msg = Encoding.UTF8.GetString(buffer, 2, length - 2);
        string[] text = msg.Split(':');

        if (text[0].Equals("CONNECT"))
        {
            UnityEngine.Debug.Log("connect");
        }
        else if (text[0].Equals("GUEST-ID"))
        {
            UnityEngine.Debug.Log("guest-login : "+ text[1]);

            PlayerSystem.GetInstance().MyPlayerId = "Guest"+text[1];
        }
        else if (text[0].Equals("GAMESTART"))
        {
            for (int i = 0; i < text.Length-1; i++)
            {
                if (PlayerSystem.GetInstance().MyPlayerId.Equals(text[i + 1]))
                {
                    for (int j = 0; j < text.Length - 1; j++)
                    {
                        if (i+1 >= text.Length) i -= (text.Length-1);
                        PlayerSystem.GetInstance().AddPlayer(PlayerTag.PLAYER_BOTTOM + j, text[1 + i], false);
                        TurnSystem.GetInstance().AddTurnPlayer(text[1 + i]);
                        i++;
                    }
                    break;
                }
            }

            TurnSystem.GetInstance().SetFirstTurn(text[1]);
            StartCoroutine(SceneSystem.GetInstance().NowScene.NextSceneAnimation());
        }
        else if (text[0].Equals("ADD-CARD"))
        {
            int cardNum = Convert.ToInt32(text[2]);
            PlayerSystem.GetInstance().PlayerAddCardWithDeck(DeckTag.DRAW_DECK, text[1], cardNum);
        }
        else if (text[0].Equals("PUT-CARD"))
        {
            int playerCardIndex = Convert.ToInt32(text[2]);
            PlayerSystem.GetInstance().PlayerPutCard(DeckTag.PUT_DECK,text[1],playerCardIndex, 0.5f);
        }
        else if (text[0].Equals("NEXT-TURN"))
        {
            TurnSystem.GetInstance().NextTurn();
        }
        else if (text[0].Equals("GET-RANDOMSEED"))
        {
            int seed = Convert.ToInt32(text[1]);
            UnityEngine.Random.InitState(seed);
        }
    }

    public void SendServer(string text)
    {
        if (!IsConnect) return;

        try
        {
            if (socketTcp != null && socketTcp.Connected)
            {
                byte[] buff = new byte[4096];

                Buffer.BlockCopy(ConvertUtil.ShortToByte(Encoding.UTF8.GetBytes(text).Length + 2), 0, buff, 0, 2);

                Buffer.BlockCopy(Encoding.UTF8.GetBytes(text), 0, buff, 2, Encoding.UTF8.GetBytes(text).Length);

                socketTcp.Send(buff, Encoding.UTF8.GetBytes(text).Length + 2, 0);
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("Send Error : " + e.Message);
            throw;
        }
    }

    void OnDisable()
    {
        SendServer("DISCONNECT");
        DisConnect();
    }

    void OnDestory()
    {
        SendServer("DISCONNECT");
        DisConnect();
    }
}

