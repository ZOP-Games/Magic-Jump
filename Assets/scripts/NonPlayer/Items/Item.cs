using System;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    [Serializable]
    public struct Item
    {
        [SerializeField] private string name;
        [SerializeField] private string description;

        public Item(string name, string description)
        {
            this.description = description;
            this.name = name;
        }

        public readonly string Name => name;

        public readonly string Description => description;
    }
}