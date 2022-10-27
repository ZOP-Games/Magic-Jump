using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameExtensions
{
    public class SpellManager
    {
        public List<IGrouping<SpellType,ISpell>> PlayerSpells { get; }
        public List<IGrouping<SpellType,ISpell>> EnemySpells { get; }
        public static SpellManager Instance { get; } = new();


        public SpellManager()
        {
            PlayerSpells = new List<ISpell>
            {
                new DamageSpell("Thunder Shock - Levle 2", SpellType.Lightning, "fak u", 2, 20, true),
                new EnemyStunSpell("Thunder Shock - lvl 1",SpellType.Lightning,"fak u but lvl 1",1,true),
                new KillSpell("Thunder Shock - levle 3",SpellType.Lightning,"fak u but lvl 3",3,true)

            }.GroupBy(s => s.Type).ToList();
            EnemySpells = new List<ISpell>
            {
                new DamageSpell(" evilThunder Shock - Levle 2", SpellType.Lightning, "fak u", 2, 20, false),
                new EnemyStunSpell("evil Thunder Shock - lvl 1",SpellType.Lightning,"fak u but lvl 1",1,false),
                new KillSpell("evil Thunder Shock - levle 3",SpellType.Lightning,"fak u but lvl 3",3,false)

            }.GroupBy(s => s.Type).ToList();
        }


    }
}
