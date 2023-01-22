using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    [SaveToFile]
    public class Inventory
    {
        private Inventory() {}
        public static Inventory Instance => new();

        public static readonly HashSet<Item> AllItems = new()
        {
            new Item("item1", "The first item."),
            new Item("wand1", "The first wand"),
            new Item("fak", "very has")
        };

        public List<Item> Items => new();

        public void AddItem(Pickup pickup)
        {
            if (!AllItems.Contains(pickup.BaseItem)) return;
            Items.Add(pickup.BaseItem);
        }
    }
}
