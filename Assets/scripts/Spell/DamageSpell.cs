using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExtensions
{
    public class DamageSpell : ISpell
    {
        public string Name { get; }
        public SpellType Type { get; }
        public string Description { get; }
        public byte Level { get; }
        public int Power { get; }
        public bool IsUsedByPlayer { get;}

        public DamageSpell(string name, SpellType type, string description, byte lvl, int power, bool isUsedByPlayer)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = lvl;
            Power = power;
            IsUsedByPlayer = isUsedByPlayer;
        }

        public void Use(IEnumerable<Entity> targets,int amount = 1)
        {
            var targetList = targets.ToList();
            switch (IsUsedByPlayer)
            {
                case true:
                    targetList = targetList.Where(e => e is EnemyBase).ToList();
                    foreach (var entity in targetList.Skip(Random.Range(0,targetList.Count-(amount+1))).Take(amount)) entity.TakeDamage(Power);
                    break;
                case false:
                    var player = targetList.FirstOrDefault(e => e is Player);
                    if (player != null) player.TakeDamage(Power);
                    break;
            }
        }
    }
}