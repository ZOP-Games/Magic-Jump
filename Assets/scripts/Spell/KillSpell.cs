using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameExtensions
{
    public class KillSpell : ISpell
    {
        public float TargetAmount { get; }
        public string Name { get; }
        public SpellType Type { get; }
        public string Description { get; }
        public byte Level { get; }
        public int Power => int.MaxValue;
        public bool Unlocked { get; }
        public KillSpell(string name,SpellType type,string description,byte lvl,bool isUsedByPlayer, float targetAmount)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = lvl;
            Unlocked = isUsedByPlayer;
            TargetAmount = targetAmount;
        }
        public void Use(IEnumerable<Entity> targets)
        {
            if (!Unlocked) throw new SpellNotUnlockedException();
            var tf = this as ISpell;
            var realTargets = tf.GetRealTargets(targets);
            foreach (var target in realTargets) target.Die();
        }
    }
}