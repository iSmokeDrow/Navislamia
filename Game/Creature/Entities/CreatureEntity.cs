using Navislamia.Game.Creature.Enums;
using Npgsql.PostgresTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navislamia.Game.Creature.Entities
{
    public class CreatureEntity
    {
        public int Race { get; set; }

        public int CriticalCount { get; set; }

        public int Havoc { get; set; }

        public int MaxHavoc { get; set; }

        public int HP { get; set; }

        public int MinHP { get; set; }

        public int MaxHP { get; set; }

        // TODO: value * factor (10000)
        public int FixedMinHP { get; set; } 

        public float MaxHPAmplifier { get; set; }

        // TODO: used in hp add/regen calculations
        public int HpDecPart { get; set; }

        public int MP {  get; set; }

        public int MaxMP { get; set; }

        public float MaxMPAmplifier { get; set; }

        // TODO: used in mp add/regen calculations
        public int MPDecPart { get; set; }

        // TODO: no skills are implemented which use 'Energy'

        public float HealRatio { get; set; }

        public float MpHealRatio { get; set; }

        public float HealByItemRatio { get; set; }

        public float HealMPByItemRatio { get; set; }

        public float HealByRestRatio { get; set; }

        public float HealMPByRestRatio { get; set; }

        public int AdditionalHeal {  get; set; }

        public int AdditiionalMPHeal { get; set; }

        public int AdditionalByItemHeal { get; set; }

        public int AdditionalMPByItemHeal { get; set; }

        public int AdditionalByRestHeal { get; set; }

        public int AdditionalMPByRestHeal { get; set; }

        // TODO: Skill cost modifier

        public float HateRatio { get; set; }

        public float CastKeep {  get; set; }

        public int Level { get; set; }

        public int MaxLevel { get; set; }

        public float BattleLevel { get; set; }

        // TODO: relevant to creature unit card
        public int UnitExpertLevel { get; set; }

        public long EXP {  get; set; }

        public long LastExpLost { get; set; }

        // TODO: Experience struct

        public float EXPModifier { get; set; }

        public CreatureStatEntity Stat { get; set; }

        public CreatureStatEntity StatByState { get; set; }

        public CreatureAttributeAmplifier Attribute {  get; set; }

        public CreatureAttributeAmplifier AttributeByState { get; set; }



        public CreatureStatus Status { get; set; }
    }
}
