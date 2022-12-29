using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    [RequireComponent(typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        public string OwnName => ownName;
        [SerializeField] private string ownName;
        public string Description => desc;
        [SerializeField]private string desc;
        public InGameDialogBox DescBox { get; private set; }
        public Item BaseItem { get; }

        public void ObtainItem()
        {

        }

        private void Start()
        {
            DescBox = GetComponent<InGameDialogBox>();
        }

        
    }
}
