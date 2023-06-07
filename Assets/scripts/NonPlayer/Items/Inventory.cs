using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    [System.Serializable]
    public class Inventory : ISaveable
    {
        private Inventory()
        {
            (this as ISaveable).AddToList();
        }
        public static Inventory Instance => new();

        public static readonly HashSet<Item> AllItems = new()
        {
            new Item("item1", "The first item."),
            new Item("wand1", "The first wand"),
            new Item("fak", "very has")
        };

        public List<Item> Items => items;

        private byte id;
        [SerializeField]private List<Item> items = new();

        public void AddItem(Item item)
        {
            if (!AllItems.Contains(item)) return;
            Items.Add(item);
        }

        byte ISaveable.Id
        {
            get => id;
            set => id = value;
        }
    }
}
