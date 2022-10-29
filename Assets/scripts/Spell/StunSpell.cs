using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExtensions
{
    public class EnemyStunSpell : ISpell
    {
        public float TargetAmount { get; }
        public string Name { get; }
        public SpellType Type {get; }
        public string Description { get; }
        public byte Level { get; }
        public int Power => 0;
        public bool Unlocked { get; }

        public EnemyStunSpell(string name, SpellType type, string description, byte level, bool isUsedByPlayer,float targetAmount)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = level;
            Unlocked = isUsedByPlayer;
            TargetAmount = targetAmount;
        }
        public void Use(IEnumerable<Entity> targets)
        {
            if (!Unlocked) throw new SpellNotUnlockedException();
            var tf = this as ISpell;
            var realTargets = tf.GetRealTargets(targets);
            foreach (var target in realTargets) target.Stun();
        }
    }
}