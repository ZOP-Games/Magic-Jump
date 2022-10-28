using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExtensions
{
    public class EnemyStunSpell : ISpell
    {
        public string Name { get; }
        public SpellType Type {get; }
        public string Description { get; }
        public byte Level { get; }
        public int Power => 0;
        public bool Unlocked { get; }

        public EnemyStunSpell(string name, SpellType type, string description, byte level, bool isUsedByPlayer)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = level;
            Unlocked = isUsedByPlayer;
        }

        public  void Use(IEnumerable<Entity> targets, int amount = 1)
        {
            var targetList = targets.ToList();
            switch (Unlocked)
            {
                case true:
                    targetList = targetList.Where(e => e is EnemyBase).ToList();
                    foreach (var entity in targetList.Skip(Random.Range(0,targetList.Count-(amount+1))).Take(amount)) entity.Stun();
                    break;
                case false:
                    var player = targetList.FirstOrDefault(e => e is Player);
                    if (player != null) player.Stun();
                    break;
            }
            
        }
    }
}