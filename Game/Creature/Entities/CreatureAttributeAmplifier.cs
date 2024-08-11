using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navislamia.Game.Creature.Entities
{
    // TODO: all values * 10000
    public struct CreatureAttributeAmplifier
    {
        public int Critical { get; set; }

        public int CriticalPower { get; set; }

        public int AttackPointRight { get; set; }

        public int AttackPointLeft { get; set; }

        public int Defense { get; set; }

        public int BlockDefense { get; set; }

        public int MagicAttack { get; set; }

        public int MagicDefense { get; set; }

        public int AccuracyRight { get; set; }

        public int AccuracyLeft { get; set; }

        public int MagicAccuracy { get; set; }

        public int Evade { get; set; }

        public int MagicEvade { get; set; }

        public int BlockChance { get; set; }

        public int MoveSpeed { get; set; }

        public int AttackSpeed { get; set; }

        public int AttackRange { get; set; }

        public int MaxWeight { get; set; }

        public int CastingSpeed { get; set; }

        public int CooldownSpeed { get; set; }

        public int ItemChance { get; set; }

        public int HPRegenPercentage { get; set; }

        public int HPRegenPoint { get; set; }

        public int MPRegenPercentage { get; set; }

        public int MPRegenPoint { get; set; }

        public int PerfectBlock { get; set; }

        public int PhysicalDefense { get; set; }

        public int PhysicalDefenseIgnore { get; set; }

        public int PhysicalDefenseIgnoreRatio { get; set; }

        public int MagicalDefenseIgnore { get; set; }

        public int MagicialDefenseIgnoreRaatio { get; set; }

        public int PhysicalPenetration { get; set; }

        public int PhysicalPenetrationRatio { get; set; }

        public int MagicialPenetration { get; set; }

        public int MagicialPenetrationRatio { get; set; }

        public int AttackSpeedRight { get; set; }

        public int AttackSpeedLeft { get; set; }

        public int DoubleAttackRatio { get; set; }

        public int StunResistance { get; set; }

        public int MoveSpeedDecreaseResist { get; set; }

        public int HPAdd { get; set; }

        public int MPAdd { get; set; }

        public int HPAddByItem { get; set; }

        public int MPAddByItem { get; set; }

    }
}
