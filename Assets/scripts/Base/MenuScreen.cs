using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// ReSharper disable UseNullPropagation
namespace GameExtensions 
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class MenuScreen : MonoBehaviour
    {
        protected GameObject GObj => gameObject;
        protected static MenuController Controller => MenuController.Controller;
        [CanBeNull] protected virtual MenuScreen Parent => transform.parent.GetComponent<MenuScreen>();
        protected static EventSystem ES => EventSystem.current; //todo:add null checking
        protected static PlayerInput PInput => PlayerInput.GetPlayerByIndex(0);

        public virtual void Open()
        {
            Controller.ActiveScreen = this;
            GObj.SetActive(true);
            if(Parent is null) PInput.SwitchCurrentActionMap("UI");
            var firstButton = GetComponentInChildren<Button>();
            if(firstButton is not null) ES.SetSelectedGameObject(firstButton.gameObject);
        }
        public virtual void Close()
        {
            Controller.ActiveScreen = Parent;
            if(Parent is not null) Parent.Open();
            GObj.SetActive(false);
            if(Parent is null) PInput.SwitchCurrentActionMap("Player");
        }

    }
}
