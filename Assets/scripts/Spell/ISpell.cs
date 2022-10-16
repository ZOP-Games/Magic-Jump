using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameExtensions
{
    public interface ISpell
    {
        public string Name { get; }
        public SpellType Type { get;}
        public string Description { get; }
        public byte Level { get; }
        public int Power { get; }
        public bool IsUsedByPlayer { get; }
        public void Use(IEnumerable<Entity> targets,int amount = 1);
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
