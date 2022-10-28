using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameExtensions;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SpellScreen : MenuScreen
{
    private readonly SpellManager spells = SpellManager.Instance;
    private GameObject button;
    

    private void Start()
    {
        button = GetComponentInChildren<Button>().gameObject;
        foreach (var s in spells.PlayerSpells.SelectMany(type => type))
        {

            var obj = Instantiate(button, GetComponentInChildren<GridLayoutGroup>().transform);
            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                spells.SelectedSpell = s;
                Debug.Log("Selected spell: " + spells.SelectedSpell);
            }); //todo:find a way to select a spell
            obj.GetComponent<LayoutElement>().minWidth = 50;
            obj.GetComponentInChildren<TextMeshProUGUI>().SetText(s.Name);
        }
    }
}
