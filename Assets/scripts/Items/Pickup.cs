using System;
using System.Collections;
using System.Collections.Generic;
using GameExtensions;
using UnityEngine;

namespace GameExtensions.Items
{
    /// <summary>
    /// An item a player can pickup (like wands)
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        public string Name { get; }
        public string Description { get; }
        private bool isNearby = false;

        private void OnCollisionStay(Collision collisionInfo)
        {
            isNearby = true;
        }
    }
}
