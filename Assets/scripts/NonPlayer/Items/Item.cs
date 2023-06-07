using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    [Serializable]
    public struct Item
    {

        public readonly string Name => name;

        public readonly string Description => description;

        [SerializeField] private string name;
        [SerializeField] private string description;

        public Item(string name, string description)
        {
            this.description = description;
            this.name = name;
        }
    }
}

