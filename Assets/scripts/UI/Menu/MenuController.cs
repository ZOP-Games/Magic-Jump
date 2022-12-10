using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable UseNullPropagation //needed for working null checks

public class MenuController : MonoBehaviour
{
    /// <summary>Item1: XP, Item2: Xp threshold, Item3: Level</summary>
    public (int, int, byte) XpInfo { get; private set; }
    [CanBeNull] public MenuScreen ActiveScreen { get; set; }
        private MenuScreen pause;
        public static MenuController Controller {
            get
            {
                var controller = FindObjectsOfType<MenuController>().FirstOrDefault();
                if (controller is null) Debug.LogError("u need a menuController u idoit");
                return controller;
            }
        }

        public void OpenPause()
        {
            var player = Player.Instance;
            XpInfo = (player.Xp, player.XpThreshold, player.Lvl);
            pause.Open();
        }

        public void CloseActive()
        {
            if (ActiveScreen is not null) ActiveScreen.Close();
        }

        public void Start()
        {
            pause = FindObjectOfType<PauseScreen>(true);
            if(pause is null) Debug.LogError("where is pause??");
        }
}

