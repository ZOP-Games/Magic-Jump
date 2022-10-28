using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameExtensions
{
    public class DamageSpell : ISpell
    {
        public string Name { get; }
        public SpellType Type { get; }
        public string Description { get; }
        public byte Level { get; }
        public int Power { get; }
        public bool Unlocked { get;}

        public DamageSpell(string name, SpellType type, string description, byte lvl, int power, bool isUsedByPlayer)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = lvl;
            Power = power;
            Unlocked = isUsedByPlayer;
        }

        public void Use(IEnumerable<Entity> targets,int amount = 1)
        {
            var targetList = targets.ToList();
            Debug.Log("got " + targetList.Count + " enemies");
            switch (Unlocked)
            {
                case true:
                    targetList = targetList.Where(e => e is EnemyBase).ToList();
                    foreach (var entity in targetList.Skip(Random.Range(0,targetList.Count-(amount+1))).Take(amount)) entity.TakeDamage(Power);
                    break;
                case false:
                    Debug.LogException(new SpellNotUnlockedException());
                    break;
            }
        }
    }

    internal class SpellNotUnlockedException : Exception
    {
        
    }
}