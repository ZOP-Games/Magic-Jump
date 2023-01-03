using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    public class Inventory
    {
        public static Inventory Instance => new();

        public static readonly HashSet<Item> AllItems = new()
        {
            new Item("item1", "The first item."),
            new Item("wand1", "The first wand"),
            new Item("fak", "very has")
        };

        //public HashSet<Item> Items { get; private set; }  //just adding a new set because there is no save feature yet
        public HashSet<Item> Items => new();

        public void SaveItems()
        {
            var itemStrings = Items.Select(x => x.ToString());
            var itemString = itemStrings.Aggregate((current, item) => current + item);
            PlayerPrefs.SetString("items", itemString);
        }

        private HashSet<Item> FetchItems()
        {
            var itemStrings = PlayerPrefs.GetString("items").Split(';').Intersect(AllItems.Select(i => i.Name)).ToList();
            var itemSet = new HashSet<Item>();
            foreach (var item in itemStrings)
            {
                itemSet.Add(AllItems.First(i => i.Name == item));
            } 
            return itemSet;

        }

        public void AddItem(Pickup pickup)
        {
            if(!AllItems.Contains(pickup.BaseItem)) return;
            Items.Add(pickup.BaseItem);
        }

        public void Start()
        {
            //Items = FetchItems();
        }
    }
}
