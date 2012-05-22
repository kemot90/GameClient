using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Commands;

namespace RPGClient
{
    public class Armor
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

        public Armor(uint _id, string _type, uint _price, string _name, string _part, uint _armor, uint _strength, uint _stamina, uint _dexterity, uint _luck)
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

        public uint ArmorPoints
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
        private List<Armor> armorsList;
        private Socket socket;

        public List<Armor> ArmorsList
        {
            get
            {
                return armorsList;
            }
        }

        public Armor GetArmorById(uint id)
        {
            //pobranie broni o podanym id
            IEnumerable<Armor> armors =
                from armorsListResult in armorsList
                where armorsListResult.Id == id
                select armorsListResult;
            foreach (Armor armor in armors)
            {
                return armor;
            }
            return null;
        }

        public ItemsArmor(TcpClient client)
        {
            armorsList = new List<Armor>();
            socket = client.Client;
            string[] result;

            Command request = new Command();
            request.Request(ClientCmd.GET_ITEMS_ARMORS);
            result = request.Send(socket, true);

            if (result[0] == ServerCmd.ARMORS)
            {
                for (int i = 1; i < result.Length; i += 10)
                {
                    Armor armor = new Armor(
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
                    armorsList.Add(armor);
                }
            }
        }
    }
}



  
