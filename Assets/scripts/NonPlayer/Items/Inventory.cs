using System;
using System.Collections.Generic;
using UnityEngine;
using GameExtensions;
namespace GameExtensions.Nonplayer.Items
{
    [Serializable]
    public class Inventory : ISaveable
    {
        public static readonly HashSet<Item> AllItems = new()
        {
            new Item("item1", "The first item."),
            new Item("wand1", "The first wand"),
            new Item("fak", "very has")
        };

        [SerializeField] private List<Item> items = new();

        private byte id;

        private Inventory()
        {
            Player.PlayerReady += () => (this as ISaveable).AddToList();
        }

        public static Inventory Instance => new();

        public List<Item> Items => items;

        byte ISaveable.Id
        {
            get => id;
            set => id = value;
        }

        public void AddItem(Item item)
        {
            if (!AllItems.Contains(item)) return;
            Items.Add(item);
        }
    }
}