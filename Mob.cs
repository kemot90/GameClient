namespace RPGClient
{
    class Mob : Creature
    {
        private ulong bonusHP;
        private uint goldDrop;

        //konstruktor
        public Mob(
            ulong _id,
            string _name,
            uint _level,
            ulong _bonusHP,
            uint _strength,
            uint _luck,
            uint _dexterity,
            uint _stamina,
            uint _glodDrop
            )
        {
            id = _id;
            name = _name;
            level = _level;
            bonusHP = _bonusHP;
            strength = _strength;
            luck = _luck;
            dexterity = _dexterity;
            stamina = _stamina;
            goldDrop = _glodDrop;
        }

        #region AKCESORY
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

        public uint GoldDrop
        {
            get
            {
                return goldDrop;
            }
            set
            {
                goldDrop = value;
            }
        }

        public ulong BonusHP
        {
            get
            {
                return bonusHP;
            }
            set
            {
                bonusHP = value;
            }
        }

        #endregion
        
        //obliczanie maksymalnej wartości HP
        public ulong GetMaxHP()
        {
            return strength + (10 * stamina);
        }

    }
}
