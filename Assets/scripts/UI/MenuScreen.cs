using GameExtensions.Debug;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable UseNullPropagation
namespace GameExtensions.UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class MenuScreen : UIComponent
    {
        public enum NavigationDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        [CanBeNull]
        protected virtual MenuScreen Parent => parentScreen;
        private MenuScreen parentScreen;

        public virtual void Open()
        {
            Controller.ActiveScreen = this;
            GObj.SetActive(true);
            StartCoroutine(FindParentMenu(transform));
            if (Parent is null && PInput is not null) PInput.SwitchCurrentActionMap("UI");
            var firstSelectable = GetComponentInChildren<Selectable>();
            if (firstSelectable is not null) ES.SetSelectedGameObject(firstSelectable.gameObject);
            else
                DebugConsole.Log("There is no Selectable on this MenuScreen so EventSystem will not focus on it.",
                    DebugConsole.WarningColor);
        }

        public virtual void Close()
        {
            Controller.ActiveScreen = Parent;
            if (Parent is not null) Parent.Open();
            GObj.SetActive(false);
            if (Parent is null && PInput is not null) PInput.SwitchCurrentActionMap("Player");
        }

        public static void RemapNavigation(Selectable target, Selectable obj, NavigationDirection dir)
        {
            var nav = target.navigation;
            nav.mode = Navigation.Mode.Explicit;
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


        // ReSharper disable once FunctionRecursiveOnAllPaths
        protected IEnumerator FindParentMenu(Transform tf)
        {
            tf = tf.parent;
            yield return new WaitForEndOfFrame();
            if (tf.TryGetComponent(out parentScreen) || tf.parent is null)
            {
                StopCoroutine(FindParentMenu(tf));
            }
            else StartCoroutine(FindParentMenu(tf));
        }
    }
}