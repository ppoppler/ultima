using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ultima_5_Cheat_Engine.Model
{

    /// <summary>
    /// Character Class
    /// 
    /// Represents a single character (32 bytes)
    /// within Ultima 5 SAVED.GAM file
    /// </summary>
    public class Character
    {

        ///Properties
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

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Gender"></param>
        /// <param name="Class"></param>
        /// <param name="Strength"></param>
        /// <param name="Dexterity"></param>
        /// <param name="Intelligence"></param>
        /// <param name="HitPoints"></param>
        /// <param name="MaxHitPoints"></param>
        /// <param name="Experience"></param>
        public Character(string Name, string Gender, string Class,
            int Strength, int Dexterity, int Intelligence, int HitPoints,
            int MaxHitPoints, int Experience)
        {
            //Replace any strange characters to increase accuracy
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

        /// <summary>
        /// Displays information about the character
        /// </summary>
        /// <returns></returns>
        public string Info()
        {
            return this.Name + " Gender: " + Gender + " Class: " + Class + "\n" +
                "STR: " + Strength + ", DEX: " + Dexterity + ", INT: " + Intelligence + "\n" +
                "HP: " + HitPoints + ", MAX_HP: " + MaxHitPoints + ", EXP: " + Experience; 
        }

        /// <summary>
        /// ToString Override
        /// 
        /// Overrides ToString to display the name (important for the ListBox of all the characters in MainView.xaml)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

    }
}
