using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace GameExtensions.UI
{
    public class ScreenLayout : UIComponent
    {
        [SerializeField] private GameObject firstObj;

        protected void OnEnable()
        {
            var menu = Controller.ActiveScreen;
            if (menu is null) return;
            MenuScreen.RemapNavigation(menu.GetComponentInChildren<Button>(),
                GetComponentsInChildren<Selectable>().LastOrDefault(s => s is not Scrollbar),
                MenuScreen.NavigationDirection.Up);
            ES.SetSelectedGameObject(firstObj);
        }

        protected void Start()
        {
            UnityEngine.Debug.Assert(ES.currentInputModule is InputSystemUIInputModule,
                "Current Input Module is not of type InputSystemUIInputModule.");
        }
    }
}