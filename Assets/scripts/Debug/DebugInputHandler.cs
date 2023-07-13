using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GameExtensions.Debug
{
    public class DebugInputHandler : MonoBehaviour
    {
        public static DebugInputHandler Instance { get; private set; }
        [SerializeField] private InputActionAsset actions;

        public void AddInputCallback(string actionName, UnityAction callback, CallbackType type = CallbackType.Performed)
        {
            switch (type)
            {
                case CallbackType.Started:
                    actions[actionName].started += _ => callback();
                    break;
                case CallbackType.Performed:
                    actions[actionName].performed += _ => callback();
                    break;
                case CallbackType.Canceled:
                    actions[actionName].canceled += _ => callback();
                    break;
                default:
                    DebugConsole.Log("The provided CallbackType ("
                                     + type + ") is invalid for adding a new callback.",
                        DebugConsole.WarningColor);
                    break;
            }
        }

        internal void DisableDebugActions()
        {
            var debugActions = actions.Where(a => a.name.Contains("[Debug]")).ToList();
            foreach (var action in debugActions)
            {
                action.Disable();
            }
            DebugConsole.Log("Debug features disabled!", DebugConsole.ErrorColor);
        }

        internal void EnableDebugActions()
        {
            var debugActions = actions.Where(a => a.name.Contains("[Debug]")).ToList();
            foreach (var action in debugActions)
            {
                action.Enable();
            }
            DebugConsole.Log("Debug features enabled!", DebugConsole.SuccessColor);
        }

        private void Start()
        {
            if (Instance is not null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AddInputCallback("[Debug] Toggle Console", DebugConsole.ToggleConsole);
            AddInputCallback("Debug enable", DebugManager.ToggleDebug, CallbackType.Canceled);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public enum CallbackType
        {
            Started,
            Performed,
            Canceled
        }
    }
}