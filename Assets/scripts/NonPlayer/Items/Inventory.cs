using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    [System.Serializable]
    public class Inventory : ISaveable
    {
        private Inventory()
        {
            Player.PlayerReady += () =>
            {
                var isa = this as ISaveable;
                isa.AddToList();
            };
        }
        public static Inventory Instance => new();

        public static readonly HashSet<Item> AllItems = new()
        {
            new Item("item1", "The first item."),
            new Item("wand1", "The first wand"),
            new Item("fak", "very has")
        };

        public List<Item> Items => items;
        [SerializeField] private List<Item> items = new();
        private byte id;

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
