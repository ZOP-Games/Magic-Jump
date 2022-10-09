using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameExtensions
{
    public abstract class Spell
    {
        public string Name { get; protected set; }
        public SpellType Type { get; protected set; }
        public string Description { get; protected set; }
        public byte Level { get; protected set; }
        public int? Power { get; protected set; }

        public abstract void Use(Entity[] targets);
    }

    public enum SpellType
    {
        Ice = 0,
        Lightning = 1,
        Fire = 2,
        Wind = 3,
        Grass = 4,
        Earth = 5,
        Dark = 6,
        Other = 7
    }
}
