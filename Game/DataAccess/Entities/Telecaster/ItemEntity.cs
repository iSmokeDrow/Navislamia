using System;
using Navislamia.Game.DataAccess.Entities.Enums;

namespace Navislamia.Game.DataAccess.Entities.Telecaster;

public class ItemEntity : Entity
{
    public long? CharacterId { get; set; }
    public virtual CharacterEntity Character { get; set; }
    
    // relation not possible since you can't create a realation cross multiple contexts
    // (TelecasterContext => AuthContext)
    public int? AccountId { get; set; } 
    
    public int? EquippedBySummonId { get; set; }
    public virtual SummonEntity Summon { get; set; }
    
    public int? AuctionId { get; set; } 
    public virtual AuctionEntity Auction { get; set; }
    public virtual AuctionEntity RelatedAuction { get; set; }
    
    public int? StorageId { get; set; }  // relation required
    public virtual ItemStorageEntity ItemStorage { get; set; }
    
    public int Idx { get; set; } // probably for sorting e.g. Item place in warehouse rename to slot/uiSlot?
    public long ItemResourceId { get; set; } 
    public long Amount { get; set; }
    public uint Level { get; set; }
    public uint Enhance { get; set; }
    public int EtherealDurability { get; set; }
    public int Endurance { get; set; }
    public ItemFlag Flag { get; set; }
    public ItemGenerateSource GenerateBySource { get; set; }
    public ItemWearType WearInfo { get; set; }
    public long[] SocketItemIds { get; set; }
    public int RemainingTime { get; set; } // could be refactored into "ExpiresAt" -> using Datetime 
    public ElementalType ElementalEffectType { get; set; }
    public DateTime? ElementalEffectExpireTime { get; set; }
    public int ElementalEffectAttackPoint { get; set; }
    public int ElementalEffectMagicPoint { get; set; }
    
    public virtual PetEntity PetItem { get; set; }
    public virtual ItemStorageEntity RelatedItemStorage { get; set; }
}