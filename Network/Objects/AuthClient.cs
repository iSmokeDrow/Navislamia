﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Net;
using System.Net.Sockets;

//using Navislamia.Configuration;
using Utilities;

//using Serilog;

using Navislamia.Network.Enums;
using Navislamia.Network.Packets;
using Navislamia.Network.Packets.Auth;
using static Navislamia.Network.Packets.PacketExtension;

using Notification;
using Network;
using Configuration;

namespace Navislamia.Network.Objects
{
    public class AuthClient : Client
    {
        bool debugPackets = false;

        public AuthClient(Socket socket, int length, IConfigurationService configurationService, INotificationService notificationService, INetworkService networkService) : base(socket, length, configurationService, notificationService, networkService) 
        {
            debugPackets = ConfigurationService.Get<bool>("packet.debug", "Logs", false);
        }

        public override void Send(Packet msg, bool beginReceive = true)
        {
            if (!Socket.Connected)
                return;

            Socket.Send(msg.Data);

            if (ConfigurationService.Get<bool>("packet.debug", "Logs", false))
            {
                NotificationService.WriteDebug($"[orange3]Sending {msg.GetType().Name} ({msg.Data.Length} bytes) to the Auth Server...[/]");
                NotificationService.WriteString((msg).DumpToString());
            }

            Data = new byte[512];

            if (beginReceive)
                Socket.BeginReceive(Data, 0, Data.Length, SocketFlags.None, ReceiveCallback, this);
        }

        public override void Receive()
        {
            if (!Socket.Connected)
                return;

            Socket.BeginReceive(Data, 0, Data.Length, SocketFlags.None, ReceiveCallback, this);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (ConfigurationService.Get<bool>("debug", "Runtime", false))
                NotificationService.WriteDebug("Receiving data from the auth server...");

            Client auth = (Client)ar.AsyncState;

            int readCnt = auth.Socket.EndReceive(ar);

            if (readCnt <= 0)
            {
                NotificationService.WriteMarkup("[bold red]Failed to read data from the Auth server![/]");
                return;
            }

            if (debugPackets)
                NotificationService.WriteDebug($"{readCnt} bytes received from the Auth server!");

            try
            {
                if (auth.MessageID == (uint)AuthPackets.TS_AG_LOGIN_RESULT)
                {
                    Span<byte> data = auth.Data;

                    byte checksum = data.Slice(6, 1).ToArray()[0];

                    TS_AG_LOGIN_RESULT msg = new TS_AG_LOGIN_RESULT(data);

                    if (debugPackets)
                        NotificationService.WriteString(msg.DumpToString());

                    if (checksum != msg.Checksum)
                    {
                        NotificationService.WriteMarkup("[bold red]TS_AG_LOGIN_RESULT bears an invalid checksum![/]");
                        return;
                    }                  
                }
            }
            catch (Exception ex)
            {
                NotificationService.WriteException(ex);
                return;
            }

            Receive();
        }
    }
}
