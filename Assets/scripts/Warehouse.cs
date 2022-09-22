using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine.InputSystem;

namespace GameExtensions
{
    //this is a central place for all the important data we need
    public class Warehouse
    {
        public Player Player { get; }
        public PlayerInput PInput => Player.PInput;
        public MenuScreen ActiveScreen { get; set; }


        public Warehouse(Player player)
        {
            Player = player;
        }

    }
}


