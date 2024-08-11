using System.Runtime.InteropServices;

namespace Navislamia.Game.Network.Packets.Game;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TS_LOGIN_RESULT
{
    public ushort Result;

    public uint Handle;

    public float X;

    public float Y;

    public float Z;

    public byte Layer;

    public float Face;

    public int RegionSize;

    public int HP;

    public int MP;

    public int MaxHP;

    public int MaxMP;

    public int Havoc;

    public int MaxHavoc;

    public int Sex;

    public int Race;

    public int Permission;

    public ulong SkinColor;

    public int FaceId;

    public int HairID;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
    public string Name;

    public int CellSize;

    public int GuildID;
}
