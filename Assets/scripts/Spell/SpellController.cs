using UnityEngine;
using GameExtensions;

namespace GameExtensions.Spells
{
    public class SpellController : MonoBehaviour
    {
        private  readonly SpellManager spells = SpellManager.Instance;
        private Player player;

        private void Start()
        {
            Player.PlayerReady += () =>
            {
                player = Player.Instance;
                player.AddInputAction("Spell", UseSpell);
            };

        }

        /// <summary>
        /// Event handler for using spells.
        /// </summary>
        private void UseSpell()
        {
            //play animation
            var spell = spells.SelectedSpell;
            spell.Use(player.Get<EnemyBase>());
            Debug.Log("Use Spell: " + spell);
        }
    }
}