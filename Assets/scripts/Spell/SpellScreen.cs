using GameExtensions.UI;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameExtensions.Spells
{
    public class SpellScreen : MenuScreen
    {
        private const int MinWidth = 50;
        private readonly SpellManager spells = SpellManager.Instance;
        private GameObject button;

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
                    spells.FinishChange();
                });
                obj.GetComponent<Button>().navigation = Navigation.defaultNavigation;
                obj.GetComponent<LayoutElement>().minWidth = MinWidth;
                obj.GetComponentInChildren<TextMeshProUGUI>().SetText(s.Name);
                var firstButton = GetComponentInChildren<Button>();
                if (firstButton is not null) ES.SetSelectedGameObject(firstButton.gameObject);
            }
        }
    }
}