using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GameExtensions
{
    public interface IInputHandler
    {
        /// <summary>
        /// The player's <see cref="PlayerInput"/> component.
        /// </summary>
        PlayerInput PInput { get; }

        /// <summary>
        /// Adds an <see cref="UnityAction"/> to the input action list.
        /// </summary>
        /// <param name="actionName">The name of the <see cref="InputAction"/>.</param>
        /// <param name="action">The action to be invoked.</param>
        /// <param name="type">When you want to listen to the event.</param>
        void AddInputAction(string actionName, UnityAction action, ActionType type = ActionType.Performed)
        {
            switch (type)
            {
                case ActionType.Started:
                    PInput.actions[actionName].started += context =>
                    {
                        if (context.started) action.Invoke();
                    };
                    break;
                case ActionType.Performed:
                    PInput.actions[actionName].performed += context =>
                    {
                        if (context.performed) action.Invoke();
                    };
                    break;
                case ActionType.Canceled:
                    PInput.actions[actionName].canceled += context =>
                    {
                        if (context.canceled) action.Invoke();
                    };
                    break;
                default:
                    Debug.LogError("bad ActionType found");
                    break;
            }
        }

        /// <summary>
        /// Defines which event the added action will add to.
        /// </summary>
        public enum ActionType
        {
            Started,
            Performed,
            Canceled
        }
    }
}