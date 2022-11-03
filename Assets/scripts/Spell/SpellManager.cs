using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameExtensions
{
    public class SpellManager
    {
        public HashSet<IGrouping<SpellType, Spell>> PlayerSpells { get; }
        public HashSet<IGrouping<SpellType, Spell>> EnemySpells { get; }
        public static SpellManager Instance { get; } = new();
        public Spell SelectedSpell { get; set; }

        public SpellManager()
        {
            PlayerSpells = new List<Spell>
            {
                new EnemyStunSpell("Thunder Shock - lvl 1",SpellType.Lightning,"fak u but lvl 1",1,true,1),
                new EnemyStunSpell("Thunder Shock - lvl 2",SpellType.Lightning,"fak u but lvl 2",2,true,0.5f),
                new KillSpell("Thunder Shock - levle 3",SpellType.Lightning,"fak u but lvl 3",3,true,0.5f)

            }.GroupBy(s => s.Type).ToHashSet();
            //EnemySpells = new List<Spell>    todo:do something w/ enemy spells
            /*{
                new DamageSpell(" evilThunder Shock - Levle 2", SpellType.Lightning, "fak u", 2, 20, false),
                new EnemyStunSpell("evil Thunder Shock - lvl 1",SpellType.Lightning,"fak u but lvl 1",1,false),
                new KillSpell("evil Thunder Shock - levle 3",SpellType.Lightning,"fak u but lvl 3",3,false)

            }.GroupBy(s => s.Type).ToList();*/
            SelectedSpell = PlayerSpells.First().First();
        }


    }
}
