using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Globalization;
using Ultima_5_Cheat_Engine.Model;
using System.ComponentModel;
using System.Windows.Data;
using Ultima_5_Cheat_Engine.Converter;
using System.Linq;

namespace Ultima_5_Cheat_Engine.ViewModel
{
    /// <summary>
    /// MainViewMOdel
    /// 
    /// View model of the only view, contains all the commands, variables, and properties necessary for the view to function
    /// Code is divided into regions. If viewing in Visual Studio 2017, use the expand options on the left side of the screen near the code line number
    /// to expand the code
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Variables
        //Variables for the command properties
        private ICommand _fileButtonCommand;
        private ICommand _mouseDoubleClick;
        private ICommand _savePlayerCommand;
        private ICommand _keyCommand;
        private ICommand _skullKeyCommand;
        private ICommand _gemCommand;
        private ICommand _bBadgeCommand;
        private ICommand _magicCarpetCommand;
        private ICommand _magicAxeCommand;
        //Variables for the other properites
        private string _fileName;
        private BindingList<Character> _characters;
        private int _gold;

        #endregion

        #region Properties
        //List of characters
        public List<Character> characters;
        //Import attributes for the character class
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int HitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int Experience { get; set; }
        //Chosen character (current character currently selected)
        public Character ChosenCharacter { get; set; }
        //Two booleans determining whether the user can use the specific buttons
        public bool SelectButtonOn { get; set; }
        public bool SaveButtonOn { get; set; }

        /// <summary>
        /// Property for Characters binding list
        /// </summary>
        public BindingList<Character> Characters
        {
            get
            {
                return _characters;
            }
            set
            {
                _characters = value;
            }

        }


      
        /// <summary>
        /// Property for File Name string
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;
            }

        }

        /// <summary>
        /// Property for Gold int
        /// </summary>
        public int Gold
        {
            get
            {
                return _gold;
            }
            set
            {
                _gold = value;
            }
        }
        #endregion


        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            SelectButtonOn = false;
            SaveButtonOn = false;

        }

        #region Command Properties

        /// <summary>
        /// File Button Command Property
        /// </summary>
        public ICommand FileButtonCommand
        {
            get
            {
                _fileButtonCommand = new RelayCommand(FileButtonAction);
                return _fileButtonCommand;
            }
        }

        /// <summary>
        /// Select Button Command Property
        /// </summary>
        public ICommand MouseDoubleClick
        {
            get
            {
                _mouseDoubleClick = new RelayCommand<object>(MouseDoubleClickAction);
                return _mouseDoubleClick;
            }
        }

        /// <summary>
        /// Save Player Button Property
        /// </summary>
        public ICommand SavePlayerCommand
        {
            get
            {
                _savePlayerCommand = new RelayCommand<object>(SavePlayerAction);
                return _savePlayerCommand;
            }
        }

        /// <summary>
        /// 100 Keys Button Command
        /// </summary>
        public ICommand KeyCommand
        {
            get
            {
                _keyCommand = new RelayCommand(KeyAction);
                return _keyCommand;
            }
        }

        /// <summary>
        /// 100 Skull Keys Button Command
        /// </summary>
        public ICommand SkullKeyCommand
        {
            get
            {
                _skullKeyCommand = new RelayCommand(SkullKeyAction);
                return _skullKeyCommand;
            }
        }

        /// <summary>
        /// 100 Gems Button Command
        /// </summary>
        public ICommand GemCommand
        {
            get
            {
                _gemCommand = new RelayCommand(GemAction);
                return _gemCommand;
            }
        }

        /// <summary>
        /// Black Badge Button Command
        /// </summary>
        public ICommand BlackBadgeCommand
        {
            get
            {
                _bBadgeCommand = new RelayCommand(BlackBadgeAction);
                return _bBadgeCommand;
            }
        }

        /// <summary>
        /// 2 Magic Carpet Button Command
        /// </summary>
        public ICommand MagicCarpetCommand
        {
            get
            {
                _magicCarpetCommand = new RelayCommand(MagicCarpetAction);
                return _magicCarpetCommand;
            }
        }

        /// <summary>
        /// 10 Magic Axes Button Command
        /// </summary>
        public ICommand MagicAxeCommand
        {
            get
            {
                _magicAxeCommand = new RelayCommand(MagicAxeAction);
                return _magicAxeCommand;
            }
        }

        #endregion


        #region Command Actions

        /// <summary>
        /// File Button Action, called whenever the File Button is pressed (FileButtonCommand)
        /// </summary>
        private void FileButtonAction()
        {
            //Open up a windows dialog
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            //Default extension
            dialog.DefaultExt = ".GAM";
            //dialog.Filter = "Game Files (*.GAM)|All files (*.)";

            //Display dialog
            Nullable<bool> result = dialog.ShowDialog();

            //If the result is true, then you received the file; therefore continue:
            if (result == true)
            {
                //Set filename property
                FileName = dialog.FileName;
                //Read the game file through the ultima5 utility class
                //Two outputs for this function: characters and gold! 
                Ultima5Utils.ReadGameFile(FileName, out characters, out _gold);
                //characters now updated, save it to Characters, BindingList property.
                //BindingList is special 
                Characters = new BindingList<Character>(characters);
                //Change the button enables
                SelectButtonOn = true;
                SaveButtonOn = false;

                //Notify the view with all these properties
                RaisePropertyChanged("SaveButtonOn");
                RaisePropertyChanged("SelectButtonOn");
                RaisePropertyChanged("FileName");
                RaisePropertyChanged("Characters");
                RaisePropertyChanged("Gold");
            }
        }

        /// <summary>
        /// Select Button action that runs when the select button is pressed
        /// </summary>
        /// <param name="o">The current character object selected in the listbox</param>
        private void MouseDoubleClickAction(object o)
        {
            //Take the character the user chose and save it.
            ChosenCharacter = (Character)o;
            //Save all the properties of ChosenCharacter so they can be displayed
            //To the user once they select the character
            Name = ChosenCharacter.Name;
            Strength = ChosenCharacter.Strength;
            Dexterity = ChosenCharacter.Dexterity;
            Intelligence = ChosenCharacter.Intelligence;
            HitPoints = ChosenCharacter.HitPoints;
            MaxHitPoints = ChosenCharacter.MaxHitPoints;
            Experience = ChosenCharacter.Experience;

            //Change Savebutton to enabled (user can now save the information)
            SaveButtonOn = true;

            //Raise all the property values to the view
            RaisePropertyChanged("SaveButtonOn");
            RaisePropertyChanged("Strength");
            RaisePropertyChanged("Dexterity");
            RaisePropertyChanged("Intelligence");
            RaisePropertyChanged("HitPoints");
            RaisePropertyChanged("MaxHitPoints");
            RaisePropertyChanged("Experience");

        }

        /// <summary>
        /// Save Player Action that occurs when the save button is pressed
        /// </summary>
        /// <param name="o">An anonymous list of objects (o) which contain all the attributes</param>
        private void SavePlayerAction(object o)
        {
            //Save the list into a variable "values"
            var values = (object[])o;
            //Parse the information into the respective Chosen character
            ChosenCharacter.Strength = Convert.ToInt32(values[0]);
            ChosenCharacter.Dexterity = Convert.ToInt32(values[1]);
            ChosenCharacter.Intelligence = Convert.ToInt32(values[2]);
            ChosenCharacter.HitPoints = Convert.ToInt32(values[3]);
            ChosenCharacter.MaxHitPoints = Convert.ToInt32(values[4]);
            ChosenCharacter.Experience = Convert.ToInt32(values[5]);
            Gold = Convert.ToInt32(values[6]);
            //Now all the information is saved into this character object

            //LINQ single query statement:
            //The function within the index tries to find a character within the characters list
            //where the name is equal to the chosen character's name
            //Essentially the program tries to find where the chosen character is within the list of characters
            characters[characters.FindIndex(m => m.Name.Equals(ChosenCharacter.Name))] = ChosenCharacter;
            //Update the Characters BindingList property
            Characters = new BindingList<Character>(characters);
            //Set save button to false now
            SaveButtonOn = false;

            //Raise property changes
            RaisePropertyChanged("SaveButtonOn");
            RaisePropertyChanged("Characters");
            //Write the information to file
            Ultima5Utils.WriteGameFile(FileName, ChosenCharacter, Gold);
            //No longer choosing a character
            ChosenCharacter = null;
        }

        /// <summary>
        /// KeyAction which occurs when the Key button is pressed
        /// </summary>
        public void KeyAction()
        {
            //Change the file for 100 keys
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.KEYS, 99);
        }

        /// <summary>
        /// SkullKeyAction which occurs when the Skull Key button is pressed
        /// </summary>
        public void SkullKeyAction()
        {
            //Change the file for 100 skull keys
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.SKULL_KEYS, 99);
        }

        /// <summary>
        /// GemAction which occurs when the Gem button is pressed
        /// </summary>
        public void GemAction()
        {
            //Change the file for 100 gems 
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.GEMS, 99);
        }

        /// <summary>
        /// BlackBadgeAction which occurs when the BlackBadge Button is pressed
        /// </summary>
        public void BlackBadgeAction()
        {
            //Changes the file for 1 black badge
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.BLACK_BADGE, 255);
        }

        /// <summary>
        /// MagicCarpetAction which occurs when the MagicCarpet button is pressed
        /// </summary>
        public void MagicCarpetAction()
        {
            //Changes the file for 2 Magic Carpets
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.MAGIC_CARPETS, 2);
        }

        /// <summary>
        /// MagicAxeAction which occurs when the MagicAxe button is pressed
        /// </summary>
        public void MagicAxeAction()
        {
            //Changes the file for 10 Magic Axes
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.MAGIC_AXES, 10);
        }

        #endregion

    }

    /// <summary>
    /// Ultima5Utils Class
    /// 
    /// A special class for functions regarding the parsing of information from the Ultima 5 SAVED.GAM file
    /// </summary>
    public static class Ultima5Utils
    {

        /// <summary>
        /// ReadGameFile
        /// 
        /// Reads SAVED.GAM file path and parses the document into a list of characters and also reads the amount of Gold
        /// </summary>
        /// <param name="path">File path to be read from in bytes</param>
        /// <param name="characterList">Output for the binding list of characters</param>
        /// <param name="Gold"></param>
        public static void ReadGameFile(string path, out List<Character> characterList, out int Gold)
        {
            //Read all the bytes in the file and save them to an array
            byte[] bytes = File.ReadAllBytes(path);
            //Convert array to a list
            List<byte> bytelist = new List<byte>(bytes);

            //Create a list of lists of bytes, known as characters
            List<List<byte>> characters = new List<List<byte>>();
            //Each character will be represented as a list of 32 bytes
            List<byte> character = new List<byte>();
            //Iterate through the entire lsit 
            for (int i = 0; i < bytelist.Count; i++)
            {
                //Add the bytes to the character
                character.Add(bytelist[i]);

                //If its the 32nd byte, go to the next line
                if (character.Count == 32)
                {
                    //add the character
                    characters.Add(character);
                    //reset the character
                    character = new List<byte>();
                }

            }

            //For each list of bytes in characters (list of 32 bytes)
            foreach (List<byte> byts in characters)
            {
                //Printing information...
                foreach (byte b in byts)
                {
                    Console.Write(b.ToString("X2"));
                }
                Console.Write("\n");
            }

            //Create a characterList and prepare to parse characters into them
            characterList = new List<Character>();
            //For the 15 characters in the saved game file...
            for (int i = 0; i <= 15; i++)
            {
                //Parse the character and save it
                Character c = ParseCharacter(characters[i]);
                //Add to the character list
                characterList.Add(c);
                Console.WriteLine(c);
            }
            //Also parse the gold (once) given a specific row of the file of bytes
            Gold = ParseGold(characters[16]);

        }


        /// <summary>
        /// WriteGameFile
        /// 
        /// Replaces the gamefile with a modified version with a new character modification or gold modification
        /// </summary>
        /// <param name="path">The filename to be modified</param>
        /// <param name="c">The character to be written into the game file</param>
        /// <param name="Gold">The gold value to be written into the file</param>
        public static void WriteGameFile(string path, Character c, int Gold)
        {
            //Write Character into the file
            //Get characters name in bytes
            byte[] bytename = (Encoding.ASCII.GetBytes(c.Name));

            //Read the file in bytes
            byte[] bytes = File.ReadAllBytes(path);
            //Get a list of all the bytes
            List<byte> bytelist = new List<byte>(bytes);
            //Create a list of lists of bytes to represent the characters
            List<List<byte>> characters = new List<List<byte>>();
            //Create a specific character List 
            List<byte> character = new List<byte>();
            //For each byte in the lsit
            for (int i = 0; i < bytelist.Count; i++)
            {
                //Add it to the character
                character.Add(bytelist[i]);

                //When the character has 32 bytes, then that's the end of the character
                if (character.Count == 32)
                {
                    //Add to the lsit of lists of bytes
                    characters.Add(character);
                    //Then reset the character 
                    character = new List<byte>();
                }

            }

            //LINQ Single Statement to find the index
            //The index is equal to an index within characters where the bytename and a specific range within the specific character are equal
            //For example if bytename is Shamino (in bytes) then it must find Shamino (in bytes) within a specified range within the character
            int index = characters.FindIndex(m => Enumerable.SequenceEqual(m.GetRange(2, bytename.Length).ToArray(), (bytename)));
            //If name is found within the file... create a replacement list of 32 bytes called replacement
            List<byte> replacement;
            if (index != -1)
            {

                //Parse all the information to bytes from the input Character c
                replacement = new List<byte>(characters[index]);
                replacement[14] = (byte)(c.Strength);
                replacement[15] = (byte)(c.Dexterity);
                replacement[16] = (byte)(c.Intelligence);
                replacement[18] = (byte)(c.HitPoints);
                replacement[19] = (byte)(c.HitPoints >> 8);
                replacement[20] = (byte)(c.MaxHitPoints);
                replacement[21] = (byte)(c.MaxHitPoints >> 8);
                replacement[22] = (byte)(c.Experience);
                replacement[23] = (byte)(c.Experience >> 8);

                //Starting index is the index found above * 32 bits (size of character)
                int startIndex = index * 32;
                //j is another counter variable that increments to change the values within bytelist
                int j = startIndex;
                for (int i = 0; i < 32; i++)
                {
                    bytelist[j] = replacement[i];
                    j++;
                }

                //Parse the gold too
                bytelist[516] = (byte)(Gold);
                bytelist[517] = (byte)(Gold >> 8);

                //Write all the bytes into the file (Replaces the file with a modified version)
                File.WriteAllBytes(path, bytelist.ToArray());
            }
        }

        /// <summary>
        /// Adds a specific item and writes it to the file
        /// </summary>
        /// <param name="path">File to be modified</param>
        /// <param name="item">The specific item (an index within the bytelist) to be added or changed</param>
        /// <param name="amount">Amount of the item or the value of the item</param>
        public static void AddItems(string path, int item, int amount)
        {
            try
            {
                //Read all the bytes and make the byetlist
                byte[] bytes = File.ReadAllBytes(path);
                List<byte> bytelist = new List<byte>(bytes);

                //Add the item and the specified value or amount
                bytelist[item] = (byte)amount;

                //Write to file 
                File.WriteAllBytes(path, bytelist.ToArray());
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("ArgumentNullException: Change button for items pressed when no file was selected");
            }
        }

        /// <summary>
        /// ParseCharacter
        /// 
        /// Takes a character (list of 32 bytes)
        /// and parses the information into a Character object to be easier read into the program
        /// </summary>
        /// <param name="characterbytes">List of 32 bytes representing a character in Ultima 5</param>
        /// <returns>a Character object representing the bytes</returns>
        public static Character ParseCharacter(List<byte> characterbytes)
        {

            //Determine Name (Bytes 0 - 10)
            string Name = Encoding.ASCII.GetString(characterbytes.GetRange(0, 10).ToArray());
            //Console.WriteLine(Name);

            //Determine Gender (11th byte)
            string Gender = "";
            if (characterbytes[11] == 11)
            {
                Gender = "Male";
            }
            else if (characterbytes[11] == 12)
            {
                Gender = "Female";
            }

            //Console.WriteLine(Gender);

            string Class = "";
            if (characterbytes[12] == 0x41)
            {
                Class = "Avatar";
            }
            else if (characterbytes[12] == 0x46)
            {
                Class = "Fighter";
            }
            else if (characterbytes[12] == 0x42)
            {
                Class = "Bard";
            }
            else if (characterbytes[12] == 0x4d)
            {
                Class = "Mage";
            }
 
            //Convert all the values
            int Strength = Convert.ToInt32(characterbytes[14]);
            int Dexterity = Convert.ToInt32(characterbytes[15]);
            int Intelligence = Convert.ToInt32(characterbytes[16]);
            int CurrentHP = Convert.ToInt32(characterbytes[18] + characterbytes[19]);
            int MaxHP = Convert.ToInt32(characterbytes[20] + characterbytes[21]);
            int Experience = Convert.ToInt32(characterbytes[22] + characterbytes[23]);
            //Console.WriteLine("STR: " + Strength + ", DEX: " + Dexterity + ", INT: " + Intelligence);
            //Console.WriteLine("HP: " + CurrentHP + ", MAX_HP: " + MaxHP + ", EXP: " + Experience);

            //Return the character
            return new Character(Name, Gender, Class, Strength, Dexterity, Intelligence,
                CurrentHP, MaxHP, Experience);
        }

        /// <summary>
        /// ParseGold
        /// 
        /// Function that takes a specific characterobject (specifically not a character, but a
        /// row in the program which contains the gold attribute) and parses the information
        /// </summary>
        /// <param name="characterbytes">The line/row to be evaluated</param>
        /// <returns>Integer value of the gold</returns>
        public static int ParseGold(List<byte> characterbytes)
        {
            //Convert the gold via little endian
            int Gold = Convert.ToInt32(characterbytes[5] << 8 | characterbytes[4]);
            return Gold;
        }
    }

    /// <summary>
    /// Enum which contains all the index locations within bytelist which represent the specific items
    /// </summary>
    public enum Ultima5Items {BLACK_BADGE=536,KEYS=518,GEMS=519,MAGIC_CARPETS=522,
    SKULL_KEYS=523, MAGIC_AXES=576};


}