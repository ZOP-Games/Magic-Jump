using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

namespace GameExtensions
{
    public class DamageSpell : ISpell
    {
        

        public float TargetAmount { get; }
        public string Name { get; }
        public SpellType Type { get; }
        public string Description { get; }
        public byte Level { get; }
        public int Power { get; }
        public bool Unlocked { get;}

        public DamageSpell(string name, SpellType type, string description, byte lvl, int power, bool isUsedByPlayer,float targetAmount)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = lvl;
            Power = power;
            Unlocked = isUsedByPlayer;
            TargetAmount = targetAmount;
        }

        public void Use(IEnumerable<Entity> targets)
        {
            if (!Unlocked) throw new SpellNotUnlockedException();
            var tf = this as ISpell;
            var realTargets = tf.GetRealTargets(targets);
            foreach (var target in realTargets) target.TakeDamage(Power);
        }

        
    }

    internal class SpellNotUnlockedException : Exception
    {
        
    }
}