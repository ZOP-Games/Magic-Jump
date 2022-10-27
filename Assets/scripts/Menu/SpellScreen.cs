using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameExtensions;
using JetBrains.Annotations;
using UnityEngine.UI;
using TMPro;

public class SpellScreen : MenuScreen
{
    private readonly SpellManager spells = SpellManager.Instance;
    private GameObject button;
    [CanBeNull] public ISpell SelectedSpell { get; private set; }

    private void Start()
    {
        button = GetComponentInChildren<Button>().gameObject;
        foreach (var s in spells.PlayerSpells.SelectMany(type => type))
        {
            void SelectSpell()
            {
                SelectedSpell = s;
            }
            var obj = Instantiate(button,GetComponentInChildren<GridLayoutGroup>().transform);
            obj.GetComponent<Button>().onClick.AddListener(SelectSpell); //todo:find a way to select a spell
            obj.GetComponent<LayoutElement>().minWidth = 50;
            obj.GetComponentInChildren<TextMeshProUGUI>().SetText(s.Name);
        }
    }
}
