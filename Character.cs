using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Commands;
using System.Windows;

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
        private ulong lastFatigue;
        private ulong fatigue;
        private ulong travelEndTime;
        private uint travelDestination;
        private string status;
        private uint pointRegenerationTime;
        private uint fullRegenerationTime;
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
                lastFatigue = UInt64.Parse(dane[12]);
                fatigue = UInt64.Parse(dane[13]);
                
                location = uint.Parse(dane[14]);
                travelEndTime = UInt64.Parse(dane[15]);
                travelDestination = UInt32.Parse(dane[16]);
            }
            equip = new CharacterEquipment(id, clientTcp);
            pointRegenerationTime = 30;
            fullRegenerationTime = 15 * 60;
            //przykładowa zmiana imienia postaci
            //this.Name = "updateTest";
        }

        //obliczanie pozostałej liczby punktów do rozdania
        public int RemainingPoints()
        {
            return (int)(4 + (this.Level * 4) - (this.Strength + this.Stamina + this.Dexterity + this.Luck));
        }

        //obliczanie maksymalnej wartości HP
        public ulong GetMaxHP()
        {
            return strength + (10 * stamina);
        }

        //obliczanie aktualnie posiadanego HP
        public ulong GetHP(long currentTime)
        {
            if (this.Damage != 0)
            {
                ulong passed = (ulong)((ulong)currentTime - lastDamage);
                ulong currentHP = GetMaxHP() - damage + Convert.ToUInt64(Math.Floor((double)passed / (double)pointRegenerationTime));
                if (currentHP >= GetMaxHP())
                {
                    this.LastDamage = 0;
                    this.Damage = 0;
                    return GetMaxHP();
                }
                else
                {
                    return currentHP;
                }
            }
            else
            {
                return GetMaxHP();
            }
        }

        //obliczanie maksyamlanej wartości kondycji
        public ulong GetMaxStamina()
        {
            return (10 * strength) + (10 * stamina);
        }

        //obliczanie aktualnie posiadanych punktów kondycji
        public ulong GetStamina(long currentTime)
        {
            if (this.Fatigue != 0)
            {
                ulong passed = (ulong)((ulong)currentTime - lastFatigue);
                ulong currentStamina = GetMaxStamina() - fatigue + Convert.ToUInt64(Math.Floor(((double)passed * (double)GetMaxStamina()) / (double)fullRegenerationTime));
                if (currentStamina >= GetMaxStamina())
                {
                    this.LastFatigue = 0;
                    this.Fatigue = 0;
                    return GetMaxStamina();
                }
                else
                {
                    return currentStamina;
                }
            }
            else
            {
                return GetMaxStamina();
            }
        }

        //obliczanie punktów doświadczenia potrzebnych do osiągnięcia następnego poziomu
        public ulong ExpToNextLevel()
        {
            ulong n1 = 0;
            ulong n2 = 100;
            ulong need = n2 + n1;

            for (int i = 2; i <= level + 1; i++)
            {
                need = (n1 / 2) + n2;
                n1 = n2;
                n2 = need;
            }

            return need;
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

        public ulong LastFatigue
        {
            get
            {
                return lastFatigue;
            }
            set
            {
                lastFatigue = value;
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_status");
                //pola name
                command.Add("lastFatigue");
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

        public ulong Fatigue
        {
            get
            {
                return fatigue;
            }
            set
            {
                fatigue = value;
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_status");
                //pola name
                command.Add("fatigue");
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

        public uint TravelDestination
        {
            get
            {
                return travelDestination;
            }
            set
            {
                travelDestination = value;
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_status");
                //pola name
                command.Add("travelDestination");
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
