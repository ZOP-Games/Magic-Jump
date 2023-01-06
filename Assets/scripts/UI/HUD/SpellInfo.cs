using System;
using GameExtensions.Spells;
using UnityEngine;
using TMPro;
namespace GameExtensions.UI.HUD
{
    public class SpellInfo : MonoBehaviour
    {
        private SpellManager spells;
        private TextMeshProUGUI textBox;
        private void Start()
        {
            textBox = GetComponentInChildren<TextMeshProUGUI>();
            spells = SpellManager.Instance;
            spells.SelectedSpellChanged += () =>
            {
                var activeSpell = spells.SelectedSpell;
                textBox.SetText("Current spell: " + activeSpell.Name);
            };
        }
    }
}