using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Commands;

namespace RPGClient
{
    public class CharacterEquipment
    {
        public enum Part { Head, Chest, Legs, Weapon, Shield };
        private ulong id;
        private uint head;
        private uint chest;
        private uint legs;
        private uint weapon;
        private uint shield;

        private Socket client;

        public CharacterEquipment(ulong characterId, TcpClient clientTcp)
        {
            id = characterId;
            client = clientTcp.Client;

            Command command = new Command();
            command.Request(ClientCmd.GET_CHARACTER_EQUIPMENT);
            command.Add(id.ToString());
            string[] dane = command.Apply(client, true);

            if (dane[0] == ServerCmd.CHARACTER_EQUIPMENT)
            {
                head = uint.Parse(dane[1]);
                chest = uint.Parse(dane[2]);
                legs = uint.Parse(dane[3]);
                weapon = uint.Parse(dane[4]);
                shield = uint.Parse(dane[5]);
            }
        }
        public uint Head
        {
            get
            {
                return head;
            }
            set
            {
                head = value;
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_equipment");
                //pola name
                command.Add("head");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
            }
        }

        public uint Chest
        {
            get
            {
                return chest;
            }
            set
            {
                chest = value;
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_equipment");
                //pola name
                command.Add("chest");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
            }
        }

        public uint Legs
        {
            get
            {
                return legs;
            }
            set
            {
                legs = value;
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_equipment");
                //pola name
                command.Add("legs");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
            }
        }

        public uint Weapon
        {
            get
            {
                return weapon;
            }
            set
            {
                weapon = value;
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_equipment");
                //pola name
                command.Add("weapon");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
            }
        }

        public uint Shield
        {
            get
            {
                return shield;
            }
            set
            {
                shield = value;
                Command command = new Command();
                //ustawienie żądania uaktualnienia bazy danych
                command.Request(ClientCmd.UPDATE_DATA_BASE);
                //w tabeli character
                command.Add("character_equipment");
                //pola name
                command.Add("shield");
                //na wartość updateTest
                command.Add(value.ToString());
                //gdzie wartość pola id
                command.Add("id");
                //jest równa identyfikatorowi gracza
                command.Add(this.id.ToString());
                //uaktualnij i nie czekaj na odpowiedź
                command.Apply(client, false);
            }
        }
    }
}
