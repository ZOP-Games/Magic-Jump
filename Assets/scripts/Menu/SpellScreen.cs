using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameExtensions;
using UnityEngine.UI;
using TMPro;

public class SpellScreen : MenuScreen
{
    private readonly SpellManager spells = SpellManager.Instance;
    private GameObject button;
    private const int MinWidth = 50;

    private void Start()
    {
        button = GetComponentInChildren<Button>(true).gameObject;
        foreach (var s in spells.PlayerSpells.SelectMany(type => type))
        {

            var obj = Instantiate(button, GetComponentInChildren<GridLayoutGroup>().transform);
            obj.SetActive(true);
            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                spells.SelectedSpell = s;
                Debug.Log("Selected spell: " + spells.SelectedSpell);
            });
            obj.GetComponent<Button>().navigation = Navigation.defaultNavigation;
            obj.GetComponent<LayoutElement>().minWidth = MinWidth;
            obj.GetComponentInChildren<TextMeshProUGUI>().SetText(s.Name);
        }
        
    }
}
