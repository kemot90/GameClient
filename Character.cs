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
        private CharacterEquipment equip;

        private Socket client;

        public Character(ulong characterId, TcpClient clientTcp)
        {
            client = clientTcp.Client;
            id = characterId;

            Command command = new Command();
            command.Request(ClientCmd.GET_CHARACTER_DATA);
            command.Add(id.ToString());
            string[] dane = command.Apply(client, true);

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
            equip = new CharacterEquipment(id, clientTcp);
            //przykładowa zmiana imienia postaci
            //this.Name = "updateTest";
        }

        //obliczanie pozostałej liczby punktów do rozdania
        public int RemainingPoints()
        {
            return (int)(4 + (this.Level * 4) - (this.Strength + this.Stamina + this.Dexterity + this.Luck));
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character");
                //pola name
                command.Add("dexterity");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character");
                //pola name
                command.Add("level");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character");
                //pola name
                command.Add("luck");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character");
                //pola name
                command.Add("name");
                //na wartość updateTest
                command.Add(value);
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character");
                //pola name
                command.Add("stamina");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character");
                //pola name
                command.Add("strength");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character");
                //pola name
                command.Add("exp");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character");
                //pola name
                command.Add("gold");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_status");
                //pola name
                command.Add("status");
                //na wartość updateTest
                command.Add(value);
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_status");
                //pola name
                command.Add("lastDamage");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_status");
                //pola name
                command.Add("damage");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_status");
                //pola name
                command.Add("location");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
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
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_status");
                //pola name
                command.Add("travelEndTime");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.Id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
            }
        }

        public CharacterEquipment Equipment
        {
            get
            {
                return equip;
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
