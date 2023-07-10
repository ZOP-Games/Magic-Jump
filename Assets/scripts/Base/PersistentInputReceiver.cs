using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GameExtensions
{
    /// <summary>
    ///     Stores input callbacks that need to work without the player (e.g. saving).
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PersistentInputReceiver : MonoBehaviour, IInputHandler
    {
        // Start is called before the first frame update
        private void Start()
        {
            PInput = GetComponent<PlayerInput>();
            var ih = this as IInputHandler;
            ih.AddInputAction("Save", SaveManager.SaveAll,IInputHandler.ActionType.Canceled);
            ih.AddInputAction("Load", SaveManager.LoadAll,IInputHandler.ActionType.Canceled);
        }

        public PlayerInput PInput { get; set; }
    }
}