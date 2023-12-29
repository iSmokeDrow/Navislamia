using Navislamia.Game.DataAccess.Entities.Enums;

namespace Navislamia.Game.DataAccess.Entities.Telecaster;

public class StarterItemsEntity : Entity
{
    public Race Race { get; set; }
    public long ItemId { get; set; }
    public uint Level { get; set; }
    public uint Enhancement { get; set; }
    public uint Amount { get; set; }
    public int ValidForSeconds { get; set; } // 60 * 60 = 1 hour, 60 * 60 * 24 = 1 day etc.
}