using System.Linq;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    [RequireComponent(typeof(Collider))]
    public class Pickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private string ownName;

        /// <summary>
        ///     Stores if the player interact with this pickup.
        /// </summary>
        private bool canInteract;

        /// <summary>
        ///     The item's <see cref="InGameDialogBox" />.
        /// </summary>
        private InGameDialogBox descBox;

        /// <summary>
        ///     Reference to the current <see cref="Inventory" /> instance.
        /// </summary>
        private Inventory inventory;

        /// <summary>
        ///     The item's description.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        ///     The <see cref="Item" /> this pickup is.
        /// </summary>
        public Item BaseItem { get; private set; }


        private void Start()
        {
            Player.PlayerReady += () =>
                Player.Instance.AddInputAction("Attack", Interact, IInputHandler.ActionType.Canceled);
            inventory = Inventory.Instance;
            BaseItem = Inventory.AllItems.FirstOrDefault(i => i.Name == ownName);
            Description = BaseItem!.Description;
            descBox = GetComponentInChildren<InGameDialogBox>(true);
            descBox.Text = Description;
        }

        private void OnCollisionEnter(Collision collisionInfo)
        {
            if (!collisionInfo.gameObject.CompareTag("Player")) return;
            canInteract = true;
        }

        private void OnCollisionExit()
        {
            canInteract = false;
        }

        /// <summary>
        ///     The name of the item.
        /// </summary>
        public string OwnName => $"You got {ownName}!";


        public void Interact()
        {
            if (!canInteract) return;
            descBox.Open();
            ObtainItem();
        }

        public void ObtainItem()
        {
            inventory.AddItem(BaseItem);
        }
    }
}