using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions
{
    public class KillSpell : ISpell
    {
        public string Name { get; }
        public SpellType Type { get; }
        public string Description { get; }
        public byte Level { get; }
        public int Power => int.MaxValue;
        public bool Unlocked { get; }
        public KillSpell(string name,SpellType type,string description,byte lvl,bool isUsedByPlayer)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = lvl;
            Unlocked = isUsedByPlayer;

        }

        public void Use(IEnumerable<Entity> targets,int amount = 1)
        {
          foreach (var target in targets)target.Die();
        }
    }
}