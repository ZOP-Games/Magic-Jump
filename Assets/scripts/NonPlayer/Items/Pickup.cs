using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameExtensions.UI;

namespace GameExtensions.Nonplayer.Items
{
    [RequireComponent(typeof(Collider))]
    public class Pickup : MonoBehaviour, IInteractable
    {
        /// <summary>
        /// The name of the item.
        /// </summary>
        public string OwnName => $"You got {ownName}!";
        [SerializeField] private string ownName;
        /// <summary>
        /// The item's description.
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// The <see cref="Item"/> this pickup is.
        /// </summary>
        public Item BaseItem { get; private set; }
        /// <summary>
        /// The item's <see cref="InGameDialogBox"/>.
        /// </summary>
        private InGameDialogBox descBox;
        /// <summary>
        /// Reference to the current <see cref="Inventory"/> instance.
        /// </summary>
        private Inventory inventory;

        /// <summary>
        /// Stores if the player interact with this pickup.
        /// </summary>
        private bool canInteract;

        public void ObtainItem()
        {
            inventory.AddItem(this);
        }


        public void Interact()
        {
            Debug.Log("henlo, u can " + (canInteract ? "" : "not") + "get me");
            if(!canInteract) return;
            descBox.Open();
            ObtainItem();
        }

        private void OnCollisionEnter(Collision collisionInfo)
        {
            if(!collisionInfo.gameObject.CompareTag("Player")) return;
            canInteract = true;
        }

        private void OnCollisionExit()
        {
            canInteract = false;
        }

        
        private void Start()
        {
            Player.PlayerReady += () => Player.Instance.AddInputAction("Attack",Interact,Player.ActionType.Canceled);
            inventory = Inventory.Instance;
            BaseItem = Inventory.AllItems.FirstOrDefault(i => i.Name == ownName);
            Description = BaseItem!.Description;
            descBox = GetComponentInChildren<InGameDialogBox>(true);
            descBox.Text = Description;
        }
    }
}
