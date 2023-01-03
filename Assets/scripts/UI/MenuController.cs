using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable UseNullPropagation //needed for working null checks

namespace GameExtensions.UI
{
    public class MenuController : MonoBehaviour
    {
        /// <summary>Item1: XP, Item2: Xp threshold, Item3: Level</summary>
        public (int, int, byte) XpInfo { get; private set; }
        [CanBeNull] public MenuScreen ActiveScreen { get; set; }
        private MenuScreen pause;
        private MenuScreen spells;
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

        public void OpenSpell()
        {
            spells.Open();
        }

        public void CloseActive()
        {
            if (ActiveScreen is not null && Player.Instance.PInput.currentActionMap.name != "Player") ActiveScreen.Close();
        }

        public void Start()
        {

            Player.PlayerReady += () =>
            {
                var player = Player.Instance;
                player.AddInputAction("Change", OpenSpell);
                player.AddInputAction("Pause", OpenPause);
                player.AddInputAction("Exit", CloseActive);
                pause = FindObjectsOfType<Transform>(true).FirstOrDefault(o => o.name == "Pause")?.GetComponent<MenuScreen>();
                pause!.GetComponentInChildren<Button>().onClick.AddListener(CloseActive);
                spells = FindObjectsOfType<Transform>(true).FirstOrDefault(o => o.name == "Spell Menu")?.GetComponent<MenuScreen>();
            };
        }
    }
}

