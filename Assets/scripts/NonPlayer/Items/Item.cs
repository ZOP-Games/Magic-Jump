using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    [Serializable]
    public record Item
    {
        public string Name => name;
        [SerializeField]private string name;
        public string Description => description;
        [SerializeField]private string description;

        public Item(string name, string desc)
        {
            this.name = name;
            description = desc;
        }
    }
}

