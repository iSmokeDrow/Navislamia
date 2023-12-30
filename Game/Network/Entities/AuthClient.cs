using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Navislamia.Game.Network.Entities.Actions;
using Navislamia.Game.Network.Packets;
using Navislamia.Game.Network.Packets.Auth;
using Navislamia.Game.Network.Packets.Enums;
using Serilog;

namespace Navislamia.Game.Network.Entities;

public class AuthClient : Client
{
    private readonly ILogger _logger = Log.ForContext<AuthClient>();
    
    public bool Ready { get; set; }
    
    public AuthClient(NetworkService networkService) : base(networkService, ClientType.Auth)
    {        
        CreateClientConnection(networkService.NetworkOptions.Auth.Ip, networkService.NetworkOptions.Auth.Port);
    }
    
    private void CreateClientConnection(string ip, int port)
    {
        if (!IPAddress.TryParse(ip, out var ipParsed))
        {
            _logger.Error("Failed to parse ip {ip} for auth", ip);
            return;
        }
        
        var authEndPoint = new IPEndPoint(ipParsed, port);
        var authSock = new Socket(authEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        authSock.Connect(authEndPoint.Address, authEndPoint.Port);

        Connection = new Connection(authSock);
        Connection.OnDataSent = OnDataSent;
        Connection.OnDataReceived = OnDataReceived;
        Connection.OnDisconnected = OnDisconnect;
        Connection.Start();
    }

    public override void OnDataReceived(int bytesReceived)
    {
        var remainingData = bytesReceived;

        while (remainingData > Marshal.SizeOf<Header>())
        {
            var header = new Header(Connection.Peek(Marshal.SizeOf<Header>()));
            var isValidMsg = header.Checksum == Header.CalculateChecksum(header);

            if (!isValidMsg)
            {
                Connection.Disconnect();
                throw new Exception($"Invalid Message received from {ClientTag} !!!");
            }

            var msgBuffer = Connection.Read((int)header.Length);

            remainingData -= msgBuffer.Length;

            // Check for packets that haven't been defined yet (development)
            if (!Enum.IsDefined(typeof(AuthPackets), header.ID))
            {
                _logger.Debug("Undefined packet ID: {id} Length: {length}) received from {clientTag}", header.ID,
                    header.Length, ClientTag);
                continue;
            }

            IPacket msg = header.ID switch
            {
                (ushort)AuthPackets.TS_AG_LOGIN_RESULT => new Packet<TS_AG_LOGIN_RESULT>(msgBuffer),
                (ushort)AuthPackets.TS_AG_CLIENT_LOGIN => new Packet<TS_AG_CLIENT_LOGIN>(msgBuffer),
                // (ushort)AuthPackets.TM_AG_KICK_CLIENT => new Packet<TM_AG_KICK_CLIENT>(msgBuffer),
                
                _ => throw new Exception("Unknown Packet Type")
            };

            _logger.Debug("{name}({id}) Length: {length} received from {clientTag}", msg.StructName, msg.ID,
                msg.Length, ClientTag);

            Actions.Execute(this, msg);
        }
        
    }
}