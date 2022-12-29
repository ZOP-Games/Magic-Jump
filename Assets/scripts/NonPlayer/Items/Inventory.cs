//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace GameExtensions.Nonplayer.Items
//{
//    public class Inventory : MonoBehaviour
//    {
//        public static Inventory Instance => FindObjectOfType<Inventory>();

//        private static readonly HashSet<Item> AllItems = new()
//        {
//            new Item("item1","The first item."),
//            new Item("wand1","The first wand"),
//            new Item("fak","very has")
//        };

//        public HashSet<Item> Items
//        {
//            get
//            {
//                var itemStrings = PlayerPrefs.GetString("items").Split(';').ToList();
//                var itemSet = new HashSet<Item>();
//                foreach (var items in AllItems.Intersect(Items))
//                {
//                    itemSet.Add(new Item())
//                }
//                return itemSet;
//            };
//        }

//        public void SaveItems()
//        {
//            var itemStrings = Items.Select(x => x.ToString());
//            var itemString = itemStrings.Aggregate((current, item) => current + item);
//            PlayerPrefs.SetString("items",itemString);
//        }
//    }
//}
