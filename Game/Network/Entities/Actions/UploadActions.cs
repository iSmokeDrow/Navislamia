using System;
using System.Collections.Generic;
using Navislamia.Game.Network.Packets;
using Serilog;

using Navislamia.Game.Network.Interfaces;

namespace Navislamia.Game.Network.Entities.Actions;

public class UploadActions : IActions
{
    private readonly ILogger _logger = Log.ForContext<UploadActions>();
    private readonly Dictionary<ushort, Action<UploadClient, IPacket>> _actions = new();
    private readonly NetworkService _networkService;

    public UploadActions(NetworkService networkService)
    {
        _networkService = networkService;

        _actions[(ushort)UploadPackets.TS_US_LOGIN_RESULT] = OnLoginResult;
    }
    
    private void OnLoginResult(UploadClient client, IPacket packet)
    {
        var msg = packet.GetDataStruct<TS_US_LOGIN_RESULT>();

        if (msg.Result > 0)
        {
            _logger.Error("Failed to register to the Upload Server!");
            throw new Exception();

        }

        client.Ready = true;

        _logger.Debug("Successfully registered to the Upload Server!");
    }
    
    public void Execute(Client client, IPacket packet)
    {
        if (!_actions.TryGetValue(packet.ID, out var action))
        {
            return;
        }
            
        action?.Invoke(client as UploadClient, packet);
    }
}