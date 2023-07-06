using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GameExtensions.UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UIComponent : MonoBehaviour
    {
        protected GameObject GObj => gameObject;
        protected MenuController Controller => MenuController.Controller;
        protected EventSystem ES => EventSystem.current;
        [CanBeNull] protected PlayerInput PInput => PlayerInput.GetPlayerByIndex(0);
    }
}