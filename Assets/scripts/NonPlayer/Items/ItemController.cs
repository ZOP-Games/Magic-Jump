using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    public class ItemController : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            Inventory.Instance.Start();
        }
    }
}
