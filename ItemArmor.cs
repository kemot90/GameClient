using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Commands;

namespace RPGClient
{
    class ItemArmor
    {
        private uint id;
        private string type;
        private uint price;
        private string name;
        private string part;
        private uint armor;
        private uint strength;
        private uint stamina;
        private uint dexterity;
        private uint luck;

        public ItemArmor(uint _id, string _type, uint _price, string _name, string _part, uint _armor, uint _strength, uint _stamina, uint _dexterity, uint _luck)
        {
            id = _id;
            type = _type;
            price = _price;
            name = _name;
            part = _part;
            armor = _armor;
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

        public string Part
        {
            get
            {
                return part;
            }
        }

        public uint Armor
        {
            get
            {
                return armor;
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

    class ItemsArmor
    {
        private List<ItemArmor> itemAList;
        private Socket socket;

        public List<ItemArmor> ItemAList
        {
            get
            {
                return itemAList;
            }
        }

        public ItemsArmor(TcpClient client)
        {
            itemAList = new List<ItemArmor>();
            socket = client.Client;
            string[] result;

            Command request = new Command();
            request.Request(ClientCmd.GET_ITEMS_ARMORS);
            result = request.Send(socket, true);

            if (result[0] == ServerCmd.ARMORS)
            {
                for (int i = 1; i < result.Length; i += 10)
                {
                    ItemArmor armor = new ItemArmor(
                        uint.Parse(result[i]),
                        result[i+1],
                        uint.Parse(result[i+2]),
                        result[i+3],
                        result[i+4],
                        uint.Parse(result[i+5]),
                        uint.Parse(result[i+6]),
                        uint.Parse(result[i+7]),
                        uint.Parse(result[i+8]),
                        uint.Parse(result[i+9])
                        );
                    itemAList.Add(armor);
                }
            }
        }
    }
}



  
