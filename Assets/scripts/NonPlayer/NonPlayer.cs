using GameExtensions.UI;
using System.Linq;
using UnityEngine;

namespace GameExtensions.Nonplayer
{
    /// <summary>
    ///     An object reprsenting an NPC (Non-Player Character).
    /// </summary>
    public class NonPlayer : MonoBehaviour, IInteractable
    {
        [SerializeField] private string ownName;

        /// <summary>
        ///     The first <see cref="IContinuable" /> the character will show when interacted with.
        /// </summary>
        private MenuScreen firstDialogBox;

        private void Start()
        {
            firstDialogBox = GetComponentInChildren<IContinuable>(true) as MenuScreen;
            Player.PlayerReady += () =>
            {
                Player.Instance.AddInputAction("Attack", CheckForInteraction);
                Player.Instance.AddInputAction("Interact", ContinueDialog);
            };
        }

        /// <summary>
        ///     The name of the character.
        /// </summary>
        public string OwnName => ownName;

        /// <summary>
        ///     Starts an interaction with the NPC and opens the first dialog box.
        /// </summary>
        public void Interact()
        {
            firstDialogBox.Open();
        }

        private static void CheckForInteraction()
        {
            var nonPlayer = Player.Instance.GetNearby<NonPlayer>().FirstOrDefault();
            // ReSharper disable once UseNullPropagation
            if (nonPlayer is null) return;
            nonPlayer.Interact();
        }

        private static void ContinueDialog()
        {
            if (MenuController.Controller.ActiveScreen is not InGameDialogBox box) return;
            Debug.Log("found " + box.name + ", I'm doing continues");
            box.Continue();
        }
    }
}