using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.Nonplayer.Items
{
    public record Item(string Name, string Description)
    {
        public string Name { get; } = Name;
        public string Description { get; } = Description;
    }
}
