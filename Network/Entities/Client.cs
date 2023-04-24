using System;
using System.Net;
using System.Net.Sockets;
using Configuration;
using Network;
using Navislamia.Network.Packets;
using Navislamia.Network.Packets.Actions.Interfaces;
using Microsoft.Extensions.Options;
using Navislamia.Configuration.Options;
using Navislamia.Network.Enums;
using Navislamia.Notification;

namespace Navislamia.Network.Entities
{
    public class Client
    {
        byte[] messageBuffer;

        public bool Ready = false;

        public INotificationService notificationSVC;
        public INetworkService networkSVC;

        public MessageQueue MessageQueue;

        public uint MsgVersion = 0x070300;

        public Tag ClientInfo = new Tag();

        private readonly NetworkOptions _networkOptions;
        private readonly LogOptions _logOptions;

        public Client(Socket socket, int length, INotificationService notificationService,
            INetworkService networkService, IAuthActionService authActionService,
            IUploadActionService uploadActionService, IGameActionService gameActionService,
            IOptions<NetworkOptions> networkOptions, IOptions<LogOptions> logOptions)
        {
            Socket = socket;
            BufferLen = length;
            notificationSVC = notificationService;
            networkSVC = networkService;
            _networkOptions = networkOptions.Value;
            _logOptions = logOptions.Value;

            messageBuffer = new byte[BufferLen];

            MessageQueue = new MessageQueue(notificationSVC, authActionService, gameActionService, uploadActionService,
                networkOptions, logOptions);
        }

        public int DataLength => Data?.Length ?? -1;

        public int BufferLen = -1;

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

                    return ep?.Address.ToString();
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
                    IPEndPoint ep = null;

                    ep = (this is AuthClient || this is UploadClient) ? Socket.RemoteEndPoint as IPEndPoint : Socket.LocalEndPoint as IPEndPoint;

                    return (ep is null) ? (short)-1 : (short)ep.Port;
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
                notificationSVC.WriteError($"An error occured while attempting to connect to remote endpoint!");
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

        public void Send(byte[] data)
        {
            try
            {
                Socket.BeginSend(data, 0, data.Length, SocketFlags.None, sendCallback, this);
            }
            catch (Exception ex)
            {
                notificationSVC.WriteError($"An error occured while attempting to send data to connection! {IP}:{Port}");
                notificationSVC.WriteException(ex);
                return;
            }
        }

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

            try
            {
                Socket.BeginReceive(messageBuffer, 0, messageBuffer.Length, SocketFlags.None, listenCallback, this);
            }
            catch (Exception ex)
            {
                notificationSVC.WriteError($"An error occured while attempting to read listen for data from connection! {IP}:{Port}");
                notificationSVC.WriteException(ex);
                return;
            }
        }

        private void listenCallback(IAsyncResult ar)
        {
            Client client = (Client)ar.AsyncState;

            if (!Socket.Connected)
            {
                notificationSVC.WriteError($"Read attempted for closed connection! {client.IP}:{client.Port}");
                return;
            }

            try
            {
                int availableBytes = client.Socket.EndReceive(ar);

                if (availableBytes == 0)
                    Listen();

                if (client is GameClient)
                    MessageQueue.LoadEncryptedBuffer(this, messageBuffer, availableBytes);
                else
                    MessageQueue.LoadPlainBuffer(this, messageBuffer, availableBytes);
            }
            catch (Exception ex)
            {
                notificationSVC.WriteError($"An error occured while attempting to read data from connection! {client.IP}:{client.Port}");
                notificationSVC.WriteException(ex);
                return;
            }        
        }
    }
}
