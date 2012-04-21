using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Commands;

namespace RPGClient
{
    public class CharacterStatus
    {
        public const string IN_STANDBY = "IN_STANDBY";
        public const string IS_TRAVELING = "IS_TRAVELING";
        public const string IS_DEAD = "IS_DEAD";
    }
    public class Character : Creature
    {
        private ulong exp;
        private ulong gold;
        private ulong lastDamage;
        private ulong damage;
        private ulong travelEndTime;
        private string status;

        private Socket client;

        public Character(ulong characterId, TcpClient clientTcp)
        {
            client = clientTcp.Client;
            id = characterId;

            Command command = new Command();
            command.Request(ClientCmd.GET_CHARACTER_DATA);
            command.Add(id.ToString());
            string[] dane = CommandToArguments(command.Apply(client, true));

            if (dane[0] == ServerCmd.CHARACTER_DATA)
            {
                name = dane[1];
                level = uint.Parse(dane[2]);
                exp = uint.Parse(dane[3]);
                gold = uint.Parse(dane[4]);
                strength = uint.Parse(dane[5]);
                stamina = uint.Parse(dane[6]);
                dexterity = uint.Parse(dane[7]);
                luck = uint.Parse(dane[8]);

                status = dane[9];
                lastDamage = UInt64.Parse(dane[10]);
                damage = UInt64.Parse(dane[11]);
                location = uint.Parse(dane[12]);
                travelEndTime = UInt64.Parse(dane[13]);
            }
        }

        public ulong Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;

            }
        }

        public uint Dexterity
        {
            get
            {
                return dexterity;
            }
            set
            {
                dexterity = value;
            }
        }

        public uint Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        public uint Luck
        {
            get
            {
                return luck;
            }
            set
            {
                luck = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public uint Stamina
        {
            get
            {
                return stamina;
            }
            set
            {
                stamina = value;
            }
        }

        public uint Strength
        {
            get
            {
                return strength;
            }
            set
            {
                strength = value;
            }
        }

        public ulong Experience
        {
            get
            {
                return exp;
            }
            set
            {
                exp = value;
            }
        }

        public ulong Gold
        {
            get
            {
                return gold;
            }
            set
            {
                gold = value;
            }
        }

        public int Head
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Shoulders
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Chest
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Hands
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Thighs
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Legs
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Weapon
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Shield
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public ulong LastDamage
        {
            get
            {
                return lastDamage;
            }
            set
            {
                lastDamage = value;
            }
        }

        public ulong Damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }
        }

        public uint Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        public ulong TravelEndTime
        {
            get
            {
                return travelEndTime;
            }
            set
            {
                travelEndTime = value;
            }
        }

        //zamiana stringa cmd na akcję i ciąg argumentów
        private string[] CommandToArguments(string command)
        {
            string[] args = command.Split(';');
            return args;
        }
    }
}
