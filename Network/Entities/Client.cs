﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Net;
using System.Net.Sockets;
using Notification;
using Configuration;
using Network;
using Navislamia.Network.Packets;
using System.Collections.Concurrent;
using Network.Security;
using Navislamia.Network.Packets.Actions;
using Navislamia.Network.Packets.Actions.Interfaces;
using System.Threading;

namespace Navislamia.Network.Entities
{
    public class Client
    {
        byte[] messageBuffer;

        public bool Ready = false;

        public IConfigurationService configSVC;
        public INotificationService notificationSVC;
        public INetworkService networkSVC;

        public MessageQueue MessageQueue;

        public uint MsgVersion = 0x070300;

        public Tag ClientInfo = new Tag();

        public Client(Socket socket, int length, IConfigurationService configurationService, INotificationService notificationService, INetworkService networkService, IAuthActionService authActionService, IUploadActionService uploadActionService, IGameActionService gameActionService)
        {
            Socket = socket;
            BufferLen = length;
            configSVC = configurationService;
            notificationSVC = notificationService;
            networkSVC = networkService;

            messageBuffer = new byte[BufferLen];

            DebugPackets = configurationService.Get<bool>("packet.debug", "Logs", false);

            MessageQueue = new MessageQueue(this.configSVC, this.notificationSVC, authActionService, gameActionService, uploadActionService);
        }

        public int DataLength => Data?.Length ?? -1;

        public int BufferLen = -1;

        public bool DebugPackets = false;

        public byte[] Data;

        public int DataOffset;

        public Socket Socket = null;

        public bool Connected => Socket?.Connected ?? false;

        public string IP
        {
            get
            {
                if (Socket is not null)
                {
                    IPEndPoint ep = Socket.RemoteEndPoint as IPEndPoint;

                    return ep.Address.ToString();
                }

                return null;
            }
        }

        public short Port
        {
            get
            {
                if (Socket is not null)
                {
                    IPEndPoint ep = Socket.LocalEndPoint as IPEndPoint;

                    return (short)ep.Port;
                }

                return -1;
            }
        }

        public int Connect(IPEndPoint ep)
        {
            try
            {
                Socket.Connect(ep);
            }
            catch (Exception ex)
            {
                notificationSVC.WriteException(ex);

                return 1;
            }

            return 0;
        }

        public virtual void PendMessage(ISerializablePacket msg) 
        {
            MessageQueue.PendSend(this, msg);

            MessageQueue.Finalize(QueueType.Send);
        }

        public void Send(byte[] data) =>
            Socket.BeginSend(data, 0, data.Length, SocketFlags.None, sendCallback, this);

        private void sendCallback(IAsyncResult ar)
        {
            Client client = (Client)ar.AsyncState;

            int bytesSent = client.Socket.EndSend(ar);

            Listen();
        }

        public void Listen()
        {
            if (!Socket.Connected)
                return;

            Socket.BeginReceive(messageBuffer, 0, messageBuffer.Length, SocketFlags.None, listenCallback, this);
        }

        private void listenCallback(IAsyncResult ar)
        {
            Client client = (Client)ar.AsyncState;

            string clientTag = "Auth Server Client";

            if (client is UploadClient)
                clientTag = "Upload Server Client";
            else if (client is GameClient)
                clientTag = "Game Client";

            if (!Socket.Connected)
            {
                notificationSVC.WriteError($"Read attempted for closed {clientTag}!");
                return;
            }

            int availableBytes = 0;

            try
            {
                availableBytes = client.Socket.EndReceive(ar);
            }
            catch (Exception ex)
            {
                notificationSVC.WriteError($"An error occured while attempting to read from client {client.IP}:{client.Port}");
                notificationSVC.WriteException(ex);
                return;
            }

            if (availableBytes == 0)
                Listen();

            if (DebugPackets)
                notificationSVC.WriteDebug($"Receiving {availableBytes} bytes from a game client {client.IP}:{client.Port}@{client.ClientInfo.AccountName.String}");

            if (client is GameClient)
                MessageQueue.LoadEncryptedBuffer(this, messageBuffer, availableBytes);
            else
                MessageQueue.LoadPlainBuffer(this, messageBuffer, availableBytes);

            Listen();
        }
    }
}
