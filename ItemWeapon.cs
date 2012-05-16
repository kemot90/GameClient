﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Commands;

namespace RPGClient
{
    class ItemWeapon
    {
        private uint id;
        private string type;
        private uint price;
        private string name;
        private uint min_attack;
        private uint max_attack;
        private uint strength;
        private uint stamina;
        private uint dexterity;
        private uint luck;

        public ItemWeapon(uint _id, string _type, uint _price, string _name, uint _min_attack, uint _max_attack, uint _strength, uint _stamina, uint _dexterity, uint _luck)
        {
            id = _id;
            type = _type;
            price = _price;
            name = _name;
            min_attack = _min_attack;
            max_attack = _max_attack;
            strength = _strength;
            stamina = _stamina;
            dexterity = _dexterity;
            luck = _luck;
        }

        public uint Id
        {
            get
            {
                return id;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }
        }

        public uint Price
        {
            get
            {
                return price;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public uint Min_attack
        {
            get
            {
                return min_attack;
            }
        }

        public uint Max_attack
        {
            get
            {
                return max_attack;
            }
        }

        public uint Strength
        {
            get
            {
                return strength;
            }
        }

        public uint Stamina
        {
            get
            {
                return stamina;
            }
        }

        public uint Dexterity
        {
            get
            {
                return dexterity;
            }
        }

        public uint Luck
        {
            get
            {
                return luck;
            }
        }

    }

    class ItemsWeapon
    {
        private List<ItemWeapon> itemWList;
        private Socket socket;

        public List<ItemWeapon> ItemWList
        {
            get
            {
                return itemWList;
            }
        }

        public ItemsWeapon(TcpClient client)
        {
            itemWList = new List<ItemWeapon>();
            socket = client.Client;
            string[] result;

            Command request = new Command();
            request.Request(ClientCmd.GET_ITEMS_WEAPONS);
            result = request.Send(socket, true);

            if (result[0] == ServerCmd.WEAPONS)
            {
                for (int i = 1; i < result.Length; i += 10)
                {
                    ItemWeapon weapon = new ItemWeapon(
                        uint.Parse(result[i]),
                        result[i+1],
                        uint.Parse(result[i+2]),
                        result[i+3],
                        uint.Parse(result[i+4]),
                        uint.Parse(result[i+5]),
                        uint.Parse(result[i+6]),
                        uint.Parse(result[i+7]),
                        uint.Parse(result[i+8]),
                        uint.Parse(result[i+9])
                        );
                    itemWList.Add(weapon);
                }
            }
        }
    }
}
