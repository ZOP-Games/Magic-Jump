using System.Collections.Generic;
using GameExtensions.Debug;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable UseNullPropagation
namespace GameExtensions.UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class MenuScreen : UIComponent
    {
        [CanBeNull] protected virtual MenuScreen Parent => transform.parent.GetComponent<MenuScreen>();

        public virtual void Open()
        {
            Controller.ActiveScreen = this;
            GObj.SetActive(true);
            if(Parent is null && PInput is not null) PInput.SwitchCurrentActionMap("UI");
            var firstButton = GetComponentInChildren<Button>();
            if(firstButton is not null) ES.SetSelectedGameObject(firstButton.gameObject);
            else DebugConsole.Log("There is no button on this MenuScreen so EventSystem will not focus on it.",Color.yellow);
        }
        public virtual void Close()
        {
            Controller.ActiveScreen = Parent;
            if(Parent is not null) Parent.Open();
            GObj.SetActive(false);
            if(Parent is null && PInput is not null) PInput.SwitchCurrentActionMap("Player");
        }

        public static void RemapNavigation(Selectable target,Selectable obj,NavigationDirection dir)
        {
            var nav = target.navigation;
            switch (dir)
            {
                case NavigationDirection.Up:
                    nav.selectOnUp = obj;
                    break;
                case NavigationDirection.Down:
                    nav.selectOnDown = obj;
                    break;
                case NavigationDirection.Left:
                    nav.selectOnLeft = obj;
                    break;
                case NavigationDirection.Right:
                    nav.selectOnRight = obj;
                    break;
                default:
                    DebugConsole.LogError("Unknown navigation direction");
                    break;
            }

            target.navigation = nav;
        }

        public enum NavigationDirection
        {
            Left,
            Right,
            Up,
            Down
        }
    }
}
