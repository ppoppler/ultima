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
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Variables
        private ICommand _fileButtonCommand;
        private ICommand _mouseDoubleClick;
        private ICommand _savePlayerCommand;
        private ICommand _keyCommand;
        private ICommand _skullKeyCommand;
        private ICommand _gemCommand;
        private ICommand _bBadgeCommand;
        private ICommand _magicCarpetCommand;
        private ICommand _magicAxeCommand;
        private string _fileName;
        private BindingList<Character> _characters;
        private int _gold;

        #endregion

        #region Properties
        public List<Character> characters;
        public string Name { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int HitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int Experience { get; set; }
        public Character ChosenCharacter { get; set; }

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

        }

        #region Command Properties

        public ICommand FileButtonCommand
        {
            get
            {
                _fileButtonCommand = new RelayCommand(FileButtonAction);
                return _fileButtonCommand;
            }
        }

        public ICommand MouseDoubleClick
        {
            get
            {
                _mouseDoubleClick = new RelayCommand<object>(MouseDoubleClickAction);
                return _mouseDoubleClick;
            }
        }

        public ICommand SavePlayerCommand
        {
            get
            {
                _savePlayerCommand = new RelayCommand<object>(SavePlayerAction);
                return _savePlayerCommand;
            }
        }

        public ICommand KeyCommand
        {
            get
            {
                _keyCommand = new RelayCommand(KeyAction);
                return _keyCommand;
            }
        }

        public ICommand SkullKeyCommand
        {
            get
            {
                _skullKeyCommand = new RelayCommand(SkullKeyAction);
                return _skullKeyCommand;
            }
        }

        public ICommand GemCommand
        {
            get
            {
                _gemCommand = new RelayCommand(GemAction);
                return _gemCommand;
            }
        }

        public ICommand BlackBadgeCommand
        {
            get
            {
                _bBadgeCommand = new RelayCommand(BlackBadgeAction);
                return _bBadgeCommand;
            }
        }

        public ICommand MagicCarpetCommand
        {
            get
            {
                _magicCarpetCommand = new RelayCommand(MagicCarpetAction);
                return _magicCarpetCommand;
            }
        }

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

        private void FileButtonAction()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".GAM";
            //dialog.Filter = "Game Files (*.GAM)|All files (*.)";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                FileName = dialog.FileName;
                Ultima5Utils.ReadGameFile(FileName, out characters, out _gold);
                Characters = new BindingList<Character>(characters);
                RaisePropertyChanged("FileName");
                RaisePropertyChanged("Characters");
                RaisePropertyChanged("Gold");
            }
        }

        private void MouseDoubleClickAction(object o)
        {
            ChosenCharacter = (Character)o;
            Name = ChosenCharacter.Name;
            Strength = ChosenCharacter.Strength;
            Dexterity = ChosenCharacter.Dexterity;
            Intelligence = ChosenCharacter.Intelligence;
            HitPoints = ChosenCharacter.HitPoints;
            MaxHitPoints = ChosenCharacter.MaxHitPoints;
            Experience = ChosenCharacter.Experience;

            //RaisePropertyChanged("Name");
            RaisePropertyChanged("Strength");
            RaisePropertyChanged("Dexterity");
            RaisePropertyChanged("Intelligence");
            RaisePropertyChanged("HitPoints");
            RaisePropertyChanged("MaxHitPoints");
            RaisePropertyChanged("Experience");

        }

        private void SavePlayerAction(object o)
        {
            var values = (object[])o;
            ChosenCharacter.Strength = Convert.ToInt32(values[0]);
            ChosenCharacter.Dexterity = Convert.ToInt32(values[1]);
            ChosenCharacter.Intelligence = Convert.ToInt32(values[2]);
            ChosenCharacter.HitPoints = Convert.ToInt32(values[3]);
            ChosenCharacter.MaxHitPoints = Convert.ToInt32(values[4]);
            ChosenCharacter.Experience = Convert.ToInt32(values[5]);
            Gold = Convert.ToInt32(values[6]);
            //byte[] intBytes = BitConverter.GetBytes(Gold)

            //characters.Find(m => m.Name.Equals(ChosenCharacter.Name));
            characters[characters.FindIndex(m => m.Name.Equals(ChosenCharacter.Name))] = ChosenCharacter;
            Characters = new BindingList<Character>(characters);
            RaisePropertyChanged("Characters");

            Ultima5Utils.WriteGameFile(FileName, ChosenCharacter, Gold);
        }

        public void KeyAction()
        {
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.KEYS, 99);
        }

        public void SkullKeyAction()
        {
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.SKULL_KEYS, 99);
        }

        public void GemAction()
        {
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.BLACK_BADGE, 99);
        }

        public void BlackBadgeAction()
        {
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.BLACK_BADGE, 255);
        }

        public void MagicCarpetAction()
        {
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.MAGIC_CARPETS, 2);
        }

        public void MagicAxeAction()
        {
            Ultima5Utils.AddItems(FileName, (int)Ultima5Items.MAGIC_AXES, 10);
        }

        #endregion

    }


    public static class Ultima5Utils
    {
        public static void ReadGameFile(string path, out List<Character> characterList, out int Gold)
        {
            byte[] bytes = File.ReadAllBytes(path);
            List<byte> bytelist = new List<byte>(bytes);

            Console.WriteLine(bytelist[3].ToString("X"));
            List<List<byte>> characters = new List<List<byte>>();
            List<byte> character = new List<byte>();
            for (int i = 0; i < bytelist.Count; i++)
            {
                character.Add(bytelist[i]);

                if (character.Count == 32)
                {
                    characters.Add(character);
                    character = new List<byte>();
                }

            }

            foreach (List<byte> byts in characters)
            {
                foreach (byte b in byts)
                {
                    Console.Write(b.ToString("X2"));
                }
                Console.Write("\n");
            }

            characterList = new List<Character>();
            for (int i = 0; i <= 15; i++)
            {
                Character c = ParseCharacter(characters[i]);
                characterList.Add(c);
                Console.WriteLine(c);
            }

            Gold = ParseGold(characters[16]);

        }

        public static void WriteGameFile(string path, Character c, int Gold)
        {
            //Write Character into the file
            //Get characters name in bytes
            byte[] bytename = (Encoding.ASCII.GetBytes(c.Name));

            byte[] bytes = File.ReadAllBytes(path);
            List<byte> bytelist = new List<byte>(bytes);

            List<List<byte>> characters = new List<List<byte>>();
            List<byte> character = new List<byte>();
            for (int i = 0; i < bytelist.Count; i++)
            {
                character.Add(bytelist[i]);

                if (character.Count == 32)
                {
                    characters.Add(character);
                    character = new List<byte>();
                }

            }

            int index = characters.FindIndex(m => Enumerable.SequenceEqual(m.GetRange(2, bytename.Length).ToArray(), (bytename)));
            //If name is found within the file...
            List<byte> replacement;
            if (index != 1)
            {
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


                int startIndex = index * 32;
                for (int i = startIndex; i < 32; i++)
                {
                    bytelist[i] = replacement[i];
                }
                bytelist[516] = (byte)(Gold);
                bytelist[517] = (byte)(Gold >> 8);

                File.WriteAllBytes(path, bytelist.ToArray());
            }
        }

        public static void AddItems(string path, int item, int amount)
        {
            byte[] bytes = File.ReadAllBytes(path);
            List<byte> bytelist = new List<byte>(bytes);

            bytelist[item] = (byte)amount;
            File.WriteAllBytes(path, bytelist.ToArray());
        }

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
            //Console.WriteLine(Class);

            int Strength = Convert.ToInt32(characterbytes[14]);
            int Dexterity = Convert.ToInt32(characterbytes[15]);
            int Intelligence = Convert.ToInt32(characterbytes[16]);
            int CurrentHP = Convert.ToInt32(characterbytes[18] + characterbytes[19]);
            int MaxHP = Convert.ToInt32(characterbytes[20] + characterbytes[21]);
            int Experience = Convert.ToInt32(characterbytes[22] + characterbytes[23]);
            //Console.WriteLine("STR: " + Strength + ", DEX: " + Dexterity + ", INT: " + Intelligence);
            //Console.WriteLine("HP: " + CurrentHP + ", MAX_HP: " + MaxHP + ", EXP: " + Experience);

            return new Character(Name, Gender, Class, Strength, Dexterity, Intelligence,
                CurrentHP, MaxHP, Experience);
        }

        public static int ParseGold(List<byte> characterbytes)
        {
            int Gold = Convert.ToInt32(characterbytes[5] << 8 | characterbytes[4]);
            return Gold;
        }
    }

    public enum Ultima5Items {BLACK_BADGE=536,KEYS=518,GEMS=519,MAGIC_CARPETS=522,
    SKULL_KEYS=523, MAGIC_AXES=576};


}