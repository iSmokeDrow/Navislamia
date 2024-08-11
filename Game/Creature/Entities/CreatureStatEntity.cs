using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navislamia.Game.Creature.Entities
{
    // TODO: All values * 10000
    public struct CreatureStatEntity
    {
        public static CreatureStatEntity operator +(CreatureStatEntity lh, CreatureStatEntity rh)
        {
            lh.Strength += rh.Strength;
            lh.Vitality += rh.Vitality;
            lh.Dexterity += rh.Dexterity;
            lh.Agility += rh.Agility;
            lh.Intelligence += rh.Intelligence;
            lh.Wisdom += rh.Wisdom;
            lh.Luck += rh.Luck;

            return lh;
        }

        public short StatID { get; set; }

        public int Strength { get; set; }

        public int Vitality { get; set; }

        public int Dexterity { get; set; }

        public int Agility { get; set; }

        public int Intelligence { get; set; }

        public int Wisdom { get; set; }

        public int Luck { get; set; }
    }
}
