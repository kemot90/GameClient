using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGClient
{
    public abstract class Creature
    {
        protected string name;
        protected ulong id;
        protected uint level;
        protected uint strength;
        protected uint stamina;
        protected uint dexterity;
        protected uint luck;
        protected uint location;
    }
}
