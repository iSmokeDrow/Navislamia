using System.Runtime.InteropServices;

namespace Navislamia.Game.Network.Packets.Game;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TS_SC_URL_LIST
{
    public ushort ListLength;

    public TS_SC_URL_LIST(ushort length)
    {
        ListLength = length;
    }
}
