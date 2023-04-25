using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace GameExtensions.UI.Layouts
{
    public class ScreenLayout : UIComponent
    {
        [SerializeField] private GameObject firstObj;

        protected virtual void OnEnable()
        {
            var menu = Controller.ActiveScreen;
            if (menu is null) return;
            MenuScreen.RemapNavigation(menu.GetComponentInChildren<Button>(),
                GetComponentsInChildren<Selectable>().LastOrDefault(s => s is not Scrollbar),
                MenuScreen.NavigationDirection.Up);
            ES.SetSelectedGameObject(firstObj); //todo: fix object selection
        }

        private void Start()
        {
            UnityEngine.Debug.Assert(ES.currentInputModule is InputSystemUIInputModule,
                "Current Input Module is not of type InputSystemUIInputModule.");
            var input = ES.currentInputModule as InputSystemUIInputModule;
        }
    }
}