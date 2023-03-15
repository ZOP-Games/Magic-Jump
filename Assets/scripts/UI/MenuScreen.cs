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
    }
}
