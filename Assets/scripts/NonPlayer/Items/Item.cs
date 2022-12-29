using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    public class Item
    {
       public string Name { get; private set; }
       public string Description { get; private set; }

       public Item(string name, string desc)
       {
           Name = name;
           Description = desc;
       }
    }
}

