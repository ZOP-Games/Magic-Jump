using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GameExtensions.Debug
{
    public class DebugInputHandler : MonoBehaviour
    {
        public static DebugInputHandler Instance { get; private set; }
        [SerializeField] private InputActionAsset actions;

        public void AddInputCallback(string actionName, UnityAction callback, InputActionPhase type = InputActionPhase.Performed)
        {
            switch (type)
            {
                case InputActionPhase.Started:
                    actions[actionName].started += _ => callback();
                    break;
                case InputActionPhase.Performed:
                    actions[actionName].performed += _ => callback();
                    break;
                case InputActionPhase.Canceled:
                    actions[actionName].canceled += _ => callback();
                    break;
                case InputActionPhase.Disabled:
                case InputActionPhase.Waiting:
                default:
                    DebugConsole.Log("The provided InputActionPhase ("
                                     + type + ") is invalid for adding a new callback.",
                        DebugConsole.WarningColor);
                    break;
            }
        }

        private void Start()
        {
            if (Instance is not null) Destroy(this);
            else Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}