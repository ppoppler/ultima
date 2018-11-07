using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ultima_5_Cheat_Engine.Model
{
    public class Character
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public string Status { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int MagicPoints { get; set; }
        public int HitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }

        public Character(string Name, string Gender, string Class,
            int Strength, int Dexterity, int Intelligence, int HitPoints,
            int MaxHitPoints, int Experience)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            
            this.Name = rgx.Replace(Name, "").Replace("-","").Replace("/","");
            this.Gender = Gender;
            this.Class = Class;
            this.Strength = Strength;
            this.Dexterity = Dexterity;
            this.Intelligence = Intelligence;
            this.HitPoints = HitPoints;
            this.MaxHitPoints = MaxHitPoints;
            this.Experience = Experience;
        }

        public string Info()
        {
            return this.Name + " Gender: " + Gender + " Class: " + Class + "\n" +
                "STR: " + Strength + ", DEX: " + Dexterity + ", INT: " + Intelligence + "\n" +
                "HP: " + HitPoints + ", MAX_HP: " + MaxHitPoints + ", EXP: " + Experience; 
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
