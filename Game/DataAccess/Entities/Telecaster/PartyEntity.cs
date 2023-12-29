using System.Collections.Generic;
using Navislamia.Game.DataAccess.Entities.Enums;

namespace Navislamia.Game.DataAccess.Entities.Telecaster;

public class PartyEntity : Entity
{
    public string Name { get; set; }
    public long LeaderId { get; set; }
    public virtual CharacterEntity Leader { get; set; }

    public PartyItemShareMode ShareMode { get; set; }
    public PartyType PartyType { get; set; }
    
    public long LeadPartyId { get; set; }
    public virtual PartyEntity LeadParty { get; set; } // Attackteams/raids

    public virtual ICollection<CharacterEntity> PartyMembers { get; set; }
    public virtual ICollection<PartyEntity> RaidParties { get; set; }
}