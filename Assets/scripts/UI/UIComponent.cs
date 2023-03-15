using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GameExtensions.UI
{
    public abstract class UIComponent : MonoBehaviour
    {
        protected GameObject GObj => gameObject;
        protected static MenuController Controller => MenuController.Controller;
        protected static EventSystem ES => EventSystem.current;
        [CanBeNull] protected static PlayerInput PInput => PlayerInput.GetPlayerByIndex(0);
    }
}