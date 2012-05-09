using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using Commands;

namespace RPGClient
{
    class Skill
    {
        private uint id;
        private uint accessLevel;
        private uint strength;
        private uint stamina;
        private uint dexterity;
        private uint luck;

        public Skill(uint _id, uint _accessLevel, uint _strength, uint _stamina, uint _dexterity, uint _luck)
        {
            id = _id;
            accessLevel = _accessLevel;
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

        public uint AccessLevel
        {
            get
            {
                return accessLevel;
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

    class Skills
    {
        private List<Skill> skillList;
        private Socket socket;

        public List<Skill> SkillList
        {
            get
            {
                return skillList;
            }
        }

        public Skills(TcpClient client)
        {
            skillList = new List<Skill>();
            socket = client.Client;
            string[] result;

            Command request = new Command();
            request.Request(ClientCmd.GET_SKILLS);
            result = request.Apply(socket, true);

            if (result[0] == ServerCmd.SKILLS)
            {
                for (int i = 1; i < result.Length; i += 6)
                {
                    Skill skill = new Skill(
                        uint.Parse(result[i]),
                        uint.Parse(result[i+1]),
                        uint.Parse(result[i+2]),
                        uint.Parse(result[i+3]),
                        uint.Parse(result[i+4]),
                        uint.Parse(result[i+5])
                        );
                    skillList.Add(skill);
                }
            }
        }
    }
}
