using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GameExtensions
{
    /// <summary>
    /// Stores input callbacks that need to work without the player (e.g. saving).
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PersistentInputReceiver : MonoBehaviour, IInputHandler
    {
        public PlayerInput PInput { get; set; }

        public void AddInputAction(string actionName, UnityAction action, IInputHandler.ActionType type = IInputHandler.ActionType.Performed)
        {
            switch (type)
            {
                case IInputHandler.ActionType.Started:
                    PInput.actions[actionName].started += context =>
                    {
                        if (context.started) action.Invoke();
                    };
                    break;
                case IInputHandler.ActionType.Performed:
                    PInput.actions[actionName].performed += context =>
                    {
                        if (context.performed) action.Invoke();
                    };
                    break;
                case IInputHandler.ActionType.Canceled:
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

        // Start is called before the first frame update
        private void Start()
        {
            PInput = GetComponent<PlayerInput>();
            AddInputAction("Save",SaveManager.SaveAll,IInputHandler.ActionType.Canceled);
            AddInputAction("Load",SaveManager.LoadAll,IInputHandler.ActionType.Canceled);

        }
    }
}
